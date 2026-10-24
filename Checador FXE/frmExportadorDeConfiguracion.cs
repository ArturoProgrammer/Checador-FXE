using Checador_FXE.Plantillas;
using ClosedXML.Excel;
using FlowControls.Inputs;
using FlowControls.Utils;

namespace Checador_FXE
{
    internal partial class frmExportadorDeConfiguracion : Form
    {
        Empleado[] empleados { get; set; }
        string localidadOrigen { get; set; }
        RelacionHorarioID relacHorarioId { get; set; }

        #region SETTERS
        internal void SetEmpleadosData(Empleado[] emps) => this.empleados = emps;
        internal void SetLocalidadOrgien(string site) => this.localidadOrigen = site;

        #endregion

        Program.EXPORT_CONFIG_SESSION SESSION { get; set; }

        public frmExportadorDeConfiguracion(Program.EXPORT_CONFIG_SESSION aSession)
        {
            InitializeComponent();
            this.SESSION = aSession;

            this.lblPerfilSeleccionado.InfoLabelText = aSession switch 
            {
                Program.EXPORT_CONFIG_SESSION.LISTADO_EMPLEADOS => "Listado de empleados",
                Program.EXPORT_CONFIG_SESSION.AJUSTES_HORARIOS => "Ajustes de turnos del mes",
                Program.EXPORT_CONFIG_SESSION.AJUSTES_TURNOS => "Ajustes de horarios de los turnos",
                _ => "N/A"
            };
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecciona la plantilla de configuracion...";
                ofd.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx, *.xls)|*.xlsx;*.xls";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Extension == ".xls")
                    {
                        flMessageBox.Show("El archivo proporcionado es formato '*.xls' por lo que se debe convertir a '*.xlsx'. Abre el archivo .xls en Excel y guardalo en formato .xlsx para posteriormente abrirlo en este programa.", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.txtRutaIngreso.Value = ofd.FileName;
                    this.txtRutaDestino.Value = $"{ofd.FileName.Replace(".xlsx", "_updated.xlsx")}";
                }
            }
            ValidateClauses();
        }

        private void btnExaminarDestino_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx)|*.xlsx";
                dialog.InitialDirectory = CafProjFile.DefaultProjFilePath;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                this.txtRutaDestino.Value = dialog.FileName;
            }
            ValidateClauses();
        }

        enum Fields
        {
            [ControlValidateAttrib("txtRutaIngreso", ControlField.FLTEXTBOXLABELJOINT)]
            Origen,
            [ControlValidateAttrib("txtRutaDestino", ControlField.FLTEXTBOXLABELJOINT)]
            Destino
        }

        void ValidateClauses()
        {
            Multivalidator mv = new Multivalidator(this);

            int fails = 0;

            foreach (Fields field in Enum.GetValues<Fields>())
                fails += mv.Validate<Fields>(field, 
                                            invalidValues: null, 
                                            customValidation: null, 
                                            ValidationParams.NOT_EMPTY_ENTRY).Success ? 0 : 1;

            this.btnAceptar.Enabled = fails == 0;
        }

        enum WriteMode
        {
            OVERWRITE_ALL,
            UPDATE_AND_ALL,
            ADD_INEXISTENTS
        }

        WriteMode getWriteMode() => this.rbtnlistModoDeEscritura.SelectedIndex switch
        {
            0 => WriteMode.OVERWRITE_ALL,   // ELIMINA TODO EL CONTENIDO Y ESCRIBE DEL NUEVO
            1 => WriteMode.UPDATE_AND_ALL, // ACTUALIZA LOS EXISTENTES Y AÑADE LOS NUEVOS
            2 => WriteMode.ADD_INEXISTENTS,   // AÑADE SOLAMENTE LOS INEXISTENTES
            _ => throw new IndexOutOfRangeException()
        };

        string getWriteModeDescription(WriteMode mode) => mode switch
        {
            WriteMode.OVERWRITE_ALL => "Sobrescribir todo el contenido de la plantilla y escribir la nueva informacion (RECOMENDADO PARA PRIMERA VEZ)",
            WriteMode.UPDATE_AND_ALL => "Actualiza la informacion existentes en la plantilla y añade la nueva informacion que no se encuentren en la plantilla",
            WriteMode.ADD_INEXISTENTS => "Añadir solamente la informacion que no se encuentre en la plantilla (RECOMENDADO PARA ACTUALIZACIONES POSTERIORES)",
            _ => throw new IndexOutOfRangeException()
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.txtRutaIngreso.Value))
                {
                    flMessageBox.Show($"No se encontro el archivo '{this.txtRutaIngreso.Value}' en la ruta esperada. Verifica su existencia y vuelve a intentar",
                                      "Archivo no Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.WriteStatus(false, $"El archivo no existe en la ruta esperada '{this.txtRutaIngreso.Value}'");
                    return;
                }
                
                this.Cursor = Cursors.WaitCursor;

                // Creamos una copia de seguridad antes de manipular el archivo
                string targetBackupPath = this.txtRutaDestino.Value.Replace(".xls", $"_backup-{DateTime.Now.ToString("d HH:mm:ss")}.xls");
                File.Copy(this.txtRutaIngreso.Value, targetBackupPath);
                Program.WriteStatus(true, $"Archivo Backup generado con exito en '{targetBackupPath}'", ReportOnGui: false);

                bool OPERATION_FLAG = SESSION switch
                {
                    Program.EXPORT_CONFIG_SESSION.LISTADO_EMPLEADOS => WriteListadoEmpleados(),
                    Program.EXPORT_CONFIG_SESSION.AJUSTES_HORARIOS  => WriteAjustesHorarios(),
                    Program.EXPORT_CONFIG_SESSION.AJUSTES_TURNOS    => WriteAjustesTurnos(),
                    _ => throw new NotImplementedException($"No existe una implementacion para '{SESSION}'")
                };

                flMessageBox.Show(OPERATION_FLAG ? "Operacion finalizada con exito!" : "Operacion finalizada");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                flMessageBox.Show($"Ocurrio un error al procesar el archivo de plantilla. {ex.Message}\n\nAsegurate de que el archivo no se encuentre abierto por otro programa!\n\n\n{ex}",
                                "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Escribe o actualiza los datos de empleados en la hoja de Excel 'Ajuste de Turnos' según el modo de escritura
        /// seleccionado.
        /// </summary>
        /// <remarks>Utiliza diferentes modos para sobrescribir, actualizar o agregar empleados en la hoja
        /// de Excel. Guarda los cambios en la ruta de destino especificada.</remarks>
        /// <returns>true si la operación se realizó correctamente; de lo contrario, false.</returns>
        bool WriteAjustesHorarios()
        {
            #region
            try
            {
                bool OPERATION_FLAG = false;

                using (XLWorkbook wb = new XLWorkbook(this.txtRutaIngreso.Value))
                {
                    var ws = wb.Worksheet("Ajuste de Horarios");

                    // Columnas según la configuración
                    (string ID, string PH_IN, string PH_OUT, string SH_IN, string SH_OUT) COLS = ("A", "B", "C", "D", "E");
                    int FIRST_ROW = 6;
                    int LIMIT_ROW = 29;

                    switch (getWriteMode())
                    {
                        case WriteMode.OVERWRITE_ALL:
                            #region
                            {
                                int ACTUAL_ROW = FIRST_ROW;
                                bool emptyRowFound = false;

                                while (!emptyRowFound)
                                {
                                    // Eliminamos el contenido de las filas
                                    var cellValue = ws.Cell(ACTUAL_ROW, COLS.PH_IN).GetValue<string>().Trim();
                                    if (String.IsNullOrWhiteSpace(cellValue))
                                    {
                                        emptyRowFound = true;
                                        break;
                                    }

                                    ws.Cell(ACTUAL_ROW, COLS.PH_IN).Clear(XLClearOptions.Contents);
                                    ws.Cell(ACTUAL_ROW, COLS.PH_OUT).Clear(XLClearOptions.Contents);
                                    ws.Cell(ACTUAL_ROW, COLS.SH_IN).Clear(XLClearOptions.Contents);
                                    ws.Cell(ACTUAL_ROW, COLS.SH_OUT).Clear(XLClearOptions.Contents);
                                    ACTUAL_ROW++;
                                }

                                // Buscamos la relacion de horarios actuales
                                Turno[] serv_resp = Turno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS);
                                
                                // Escribimos el contenido nuevo
                                ACTUAL_ROW = FIRST_ROW;
                                while (true)
                                {
                                    int _rowId = int.Parse(ws.Cell(ACTUAL_ROW, COLS.ID).Value.ToString());

                                    if (!serv_resp.Any(t => t.ID == _rowId))
                                        break;

                                    Turno i = serv_resp.FirstOrDefault(t => t.ID == _rowId);
                                    ws.Cell(ACTUAL_ROW, COLS.PH_IN).Value = i.PrimerHorario.Entrada.ToString("HH:mm");
                                    ws.Cell(ACTUAL_ROW, COLS.PH_OUT).Value = i.PrimerHorario.Salida.ToString("HH:mm");
                                    ws.Cell(ACTUAL_ROW, COLS.SH_IN).Value = i.SegundoHorario.Entrada.ToString("HH:mm");
                                    ws.Cell(ACTUAL_ROW, COLS.SH_OUT).Value = i.SegundoHorario.Salida.ToString("HH:mm");
                                    ACTUAL_ROW++;
                                }

                                OPERATION_FLAG = true;
                            }
                            #endregion
                            break;
                        case WriteMode.UPDATE_AND_ALL:
                            #region
                            {

                            }
                            #endregion
                            break;
                        case WriteMode.ADD_INEXISTENTS:
                            #region
                            {

                            }
                            #endregion
                            break;

                        default:
                            throw new IndexOutOfRangeException("El modo de escritura seleccionado no es valido.");
                    }

                    if (OPERATION_FLAG)
                    {
                        // Guardamos los cambios en el archivo destino especificado
                        wb.SaveAs(this.txtRutaDestino.Value);
                    }
                }
            }
            catch { return false; }

            return true;
            #endregion
        }

        bool WriteAjustesTurnos()
        {
            
        }

        /// <summary>
        /// Listado de empleados. No escribe los turnos asignados para el mes
        /// </summary>
        /// <returns></returns>
        bool WriteListadoEmpleados()
        {
            #region
            try
            {
                bool OPERATION_FLAG = false;

                using (XLWorkbook wb = new XLWorkbook(this.txtRutaIngreso.Value))
                {
                    var ws = wb.Worksheet("Ajuste de Turnos");

                    // Columnas según la configuración
                    (string NumEmp, string Nombre, string Area) COLS = ("A", "B", "C");
                    int FIRST_ROW = 6;

                    // Declaraciones globales
                    bool emptyRowFound = false;
                    int ACTUAL_ROW = 0;

                    switch (getWriteMode())
                    {
                        case WriteMode.OVERWRITE_ALL:
                            #region
                            if (flMessageBox.Show("Este modo ELIMINARA TODOS los usuarios para escribir los nuevos. ¿Estas seguro que deseas continuar?",
                                                  "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                                break;

                            // Eliminamos todas las filas con información existente
                            ACTUAL_ROW = FIRST_ROW;
                            while (!emptyRowFound)
                            {
                                var cellValue = ws.Cell(ACTUAL_ROW, COLS.NumEmp).GetValue<string>().Trim();
                                if (String.IsNullOrWhiteSpace(cellValue))
                                {
                                    emptyRowFound = true;
                                    break;
                                }

                                ws.Cell(ACTUAL_ROW, COLS.NumEmp).Clear(XLClearOptions.Contents);
                                ws.Cell(ACTUAL_ROW, COLS.Nombre).Clear(XLClearOptions.Contents);
                                ws.Cell(ACTUAL_ROW, COLS.Area).Clear(XLClearOptions.Contents);
                                ACTUAL_ROW++;
                            }

                            ACTUAL_ROW = FIRST_ROW;
                            // Escribimos todo el contenido nuevo
                            foreach (Empleado _e in empleados)
                            {
                                // Escribimos la sincronizacion de los empleados
                                ws.Cell(ACTUAL_ROW, COLS.NumEmp).Value = _e.NoEmp;
                                ws.Cell(ACTUAL_ROW, COLS.Nombre).Value = _e.Nombres;
                                ws.Cell(ACTUAL_ROW, COLS.Area).Value = _e.Area;
                                ACTUAL_ROW++;
                            }

                            OPERATION_FLAG = true;
                            #endregion
                            break;

                        case WriteMode.UPDATE_AND_ALL:
                            #region
                            List<(Empleado Emp, bool Listo)> ArrayEmpleados = new List<(Empleado Emp, bool Listo)>();
                            foreach (Empleado _e in empleados)
                                ArrayEmpleados.Add((_e, false));

                            // Actualizamos primero los existentes
                            ACTUAL_ROW = FIRST_ROW;
                            while (!emptyRowFound)
                            {
                                string numEmpInCell = ws.Cell(ACTUAL_ROW, COLS.NumEmp).GetValue<string>().Trim();
                                if (String.IsNullOrWhiteSpace(numEmpInCell))
                                {
                                    emptyRowFound = true;
                                    break;
                                }

                                Empleado empToUpdate = ArrayEmpleados.Where(e => e.Emp.NoEmp.Equals(numEmpInCell)).Select(e => e.Emp).FirstOrDefault();
                                if (empToUpdate != null)
                                {
                                    ws.Cell(ACTUAL_ROW, COLS.Nombre).Value = empToUpdate.Nombres;
                                    ws.Cell(ACTUAL_ROW, COLS.Area).Value = empToUpdate.Area;

                                    // Marcamos el empleado como actualizado
                                    int index = ArrayEmpleados.FindIndex(e => e.Emp.NoEmp.Equals(numEmpInCell));
                                    ArrayEmpleados[index] = (ArrayEmpleados[index].Emp, true);
                                }
                                ACTUAL_ROW++;
                            }

                            // Añadimos los faltantes al final
                            foreach ((Empleado Emp, bool Listo) _e in ArrayEmpleados)
                            {
                                if (_e.Listo)
                                    continue;

                                ws.Cell(ACTUAL_ROW, COLS.NumEmp).Value = _e.Emp.NoEmp;
                                ws.Cell(ACTUAL_ROW, COLS.Nombre).Value = _e.Emp.Nombres;
                                ws.Cell(ACTUAL_ROW, COLS.Area).Value = _e.Emp.Area;
                                ACTUAL_ROW++;
                            }

                            OPERATION_FLAG = true;
                            #endregion
                            break;

                        case WriteMode.ADD_INEXISTENTS:
                            #region
                            // Recopilamos todos los números de empleado que ya existen en la hoja de Excel
                            List<string> empleadosEnHoja = new List<string>();
                            ACTUAL_ROW = FIRST_ROW;
                            while (!emptyRowFound)
                            {
                                string numEmpInCell = ws.Cell(ACTUAL_ROW, COLS.NumEmp).GetValue<string>().Trim();
                                if (String.IsNullOrWhiteSpace(numEmpInCell))
                                {
                                    emptyRowFound = true;
                                    break;
                                }
                                empleadosEnHoja.Add(numEmpInCell);
                                ACTUAL_ROW++;
                            }

                            // Identificamos cuáles de la lista 'empleados' no están en la plantilla
                            List<Empleado> faltantes = empleados.Where(e => !empleadosEnHoja.Contains(e.NoEmp)).ToList();

                            // Escribimos los faltantes a partir de la primera fila vacía encontrada (ACTUAL_ROW)
                            int ROW_TO_WRITE = ACTUAL_ROW;
                            foreach (Empleado actEmp in faltantes)
                            {
                                ws.Cell(ROW_TO_WRITE, COLS.NumEmp).Value = actEmp.NoEmp;
                                ws.Cell(ROW_TO_WRITE, COLS.Nombre).Value = actEmp.Nombres;
                                ws.Cell(ROW_TO_WRITE, COLS.Area).Value = actEmp.Area;
                                ROW_TO_WRITE++;
                            }

                            OPERATION_FLAG = true;
                            #endregion
                            break;

                        default:
                            throw new IndexOutOfRangeException("El modo de escritura seleccionado no es valido.");
                    }

                    if (OPERATION_FLAG)
                    {
                        // Guardamos los cambios en el archivo destino especificado
                        wb.SaveAs(this.txtRutaDestino.Value);
                    }
                }
            } catch { return false; }

            return true;
            #endregion
        }

        private void frmExportadorDeConfiguracion_Load(object sender, EventArgs e)
        {
            this.rtxtExplicacionDelModo.Text = getWriteModeDescription(getWriteMode());

            //flMessageBox.Show("Funcion inhabiltada temporalmente por bugs en funcionalidad", "Funcion Inhabilitada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //this.Close();
        }

        private void rbtnlistModoDeEscritura_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.rtxtExplicacionDelModo.Text = getWriteModeDescription(getWriteMode());
        }
    }
}
