using Checador_FXE.Plantillas;
using FlowControls.Utils;
using SpreadsheetLight;

namespace Checador_FXE
{
    public partial class frmExportadorDeConfiguracion : Form
    {
        Empleado[] empleados;

        public frmExportadorDeConfiguracion(Empleado[] data, string localidadOrigen)
        {
            InitializeComponent();
            empleados = data;
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
                        MessageBox.Show("El archivo proporcionado es formato '*.xls' por lo que se debe convertir a '*.xlsx'. Abre el archivo .xls en Excel y guardalo en formato .xlsx para posteriormente abrirlo en este programa.", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            WriteMode.UPDATE_AND_ALL => "Actualizar los empleados existentes en la plantilla y añadir los nuevos empleados que no se encuentren en la plantilla",
            WriteMode.ADD_INEXISTENTS => "Añadir solamente los empleados que no se encuentren en la plantilla (RECOMENDADO PARA ACTUALIZACIONES POSTERIORES)",
            _ => throw new IndexOutOfRangeException()
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Iniciamos el proceso de llenado del archivo de configuracion
            bool OPERATION_FLAG = false;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                string sheet = "Ajuste de Turnos";
                
                using (SLDocument sl = new SLDocument(this.txtRutaIngreso.Value, sheet))
                {
                    MessageBox.Show("aqui 1");
                    MessageBox.Show(sl.GetCellValueAsString(6,2));
                    MessageBox.Show("aqui 2");
                }

                using (SLDocument sl = new SLDocument(this.txtRutaIngreso.Value, sheet))
                {
                    (char NumEmp, char Nombre, char Area) columns = ('A', 'B', 'C');
                    int FIRST_ROW = 6;

                    //
                    // TODO: Ticket de tarea ##100197##; MODOS DE ESCRITURA DE EXPORTACION
                    //

                    // Declaraciones "globales"
                    bool emptyRowFound = false;
                    int ACTUAL_ROW = 0;

                    switch (getWriteMode())
                    {
                        case WriteMode.OVERWRITE_ALL:
                            #region
                            if (MessageBox.Show("Este modo ELIMINARA TODOS los usuarios para escribir los nuevos. ¿Estas seguro que deseas continuar?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                                break;

                            // Eliminamos todas las filas con informacion
                            ACTUAL_ROW = FIRST_ROW;
                            while (!emptyRowFound)
                            {
                                if (String.IsNullOrWhiteSpace(sl.GetCellValueAsString(ACTUAL_ROW, Utils.GetColumnInt(columns.NumEmp.ToString())).Trim()))
                                {
                                    emptyRowFound = true;
                                    break;
                                }

                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.NumEmp.ToString()), "");
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Nombre.ToString()), "");
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Area.ToString()), "");
                                ACTUAL_ROW++;
                            }

                            ACTUAL_ROW = FIRST_ROW;
                            // Escribimos todo el contenido nuevo
                            foreach (Empleado _e in empleados)
                            {
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.NumEmp.ToString()), _e.NoEmp);
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Nombre.ToString()), _e.Nombres);
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Area.ToString()), _e.Area);
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
                                string numEmpInCell = sl.GetCellValueAsString(ACTUAL_ROW, Utils.GetColumnInt(columns.NumEmp.ToString())).Trim();
                                if (String.IsNullOrWhiteSpace(numEmpInCell))
                                {
                                    emptyRowFound = true;
                                    break;
                                }

                                Empleado empToUpdate = ArrayEmpleados.Where(e => e.Emp.NoEmp.Equals(numEmpInCell)).Select(e => e.Emp).FirstOrDefault();
                                if (empToUpdate != null)
                                {
                                    sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Nombre.ToString()), empToUpdate.Nombres);
                                    sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Area.ToString()), empToUpdate.Area);
                                    // Marcamos el empleado como actualizado para no añadirlo despues
                                    int index = ArrayEmpleados.FindIndex(e => e.Emp.NoEmp.Equals(numEmpInCell));
                                    ArrayEmpleados[index] = (ArrayEmpleados[index].Emp, true);
                                }
                                ACTUAL_ROW++;
                            }

                            // Añadimos los faltantes
                            foreach ((Empleado Emp, bool Listo) _e in ArrayEmpleados)
                            {
                                if (_e.Listo)
                                    continue;

                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.NumEmp.ToString()), _e.Emp.NoEmp);
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Nombre.ToString()), _e.Emp.Nombres);
                                sl.SetCellValue(ACTUAL_ROW, Utils.GetColumnInt(columns.Area.ToString()), _e.Emp.Area);
                                ACTUAL_ROW++;
                            }

                            OPERATION_FLAG = true;
                            #endregion
                            break;
                        case WriteMode.ADD_INEXISTENTS:
                            #region
                            List<int> faltantes = new List<int>();  // Indices en el array de los empleados que no se encuentran en la plantilla

                            // Validamos primero los empleados que ya estan
                            for (int i = 0; i < empleados.Length; i++)
                            {
                                Empleado actEmp = empleados[i];
                                int actRow = FIRST_ROW + i;

                                if (empleados.Cast<Empleado>().Any(e => e.NoEmp.Equals(sl.GetCellValueAsString(actRow, Utils.GetColumnInt(columns.NumEmp.ToString())))))
                                    continue;
                                faltantes.Add(i);
                            }

                            // Detectamos la ultima fila con datos para no sobreescribir informacion
                            int EMPTY_ROW = FIRST_ROW;
                            while (!emptyRowFound)
                            {
                                if (String.IsNullOrWhiteSpace(sl.GetCellValueAsString(EMPTY_ROW, Utils.GetColumnInt(columns.NumEmp.ToString())).Trim()))
                                {
                                    emptyRowFound = true;
                                    break;
                                }

                                EMPTY_ROW++;
                            }

                            // Ingresamos los faltantes apartir del lugar inexistente
                            int ROW_TO_WRITE = EMPTY_ROW;
                            foreach (int i in faltantes)
                            {
                                Empleado actEmp = empleados[i];
                                sl.SetCellValue(ROW_TO_WRITE, Utils.GetColumnInt(columns.NumEmp.ToString()), actEmp.NoEmp); // Numero de empleado
                                sl.SetCellValue(ROW_TO_WRITE, Utils.GetColumnInt(columns.Nombre.ToString()), actEmp.Nombres); // Nombre
                                sl.SetCellValue(ROW_TO_WRITE, Utils.GetColumnInt(columns.Area.ToString()), actEmp.Area); // Area
                                ROW_TO_WRITE++;
                            }

                            OPERATION_FLAG = true;
                            #endregion
                            break;
                        default:
                            throw new IndexOutOfRangeException("El modo de escritura seleccionado no es valido.");
                    }


                    if (OPERATION_FLAG)
                        sl.SaveAs(this.txtRutaDestino.Value);  // Guardamos el documento editado una vez finalizado el proceso
                }

                MessageBox.Show(OPERATION_FLAG ? "Operacion finalizada con exito!" : "Operacion finalizada");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al procesar el archivo de plantilla. {ex.Message}\n\nAsegurate de que el archivo no se encuentre abierto por otro programa!\n\n\n{ex}",
                                "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void frmExportadorDeConfiguracion_Load(object sender, EventArgs e)
        {
            this.rtxtExplicacionDelModo.Text = getWriteModeDescription(getWriteMode());

            MessageBox.Show("Funcion inhabiltada temporalmente por bugs en funcionalidad", "Funcion Inhabilitada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        private void rbtnlistModoDeEscritura_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.rtxtExplicacionDelModo.Text = getWriteModeDescription(getWriteMode());
        }
    }
}
