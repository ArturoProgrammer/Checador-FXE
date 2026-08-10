using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Inputs;
using FlowControls.Utils;
using System.Globalization;

namespace Checador_FXE
{
    public partial class frmCrudRelacionHorarios : Form
    {
        /// <summary>
        /// Arreglo de las columnas actualmente cargadas, no la vista actual.
        /// </summary>
        List<DataGridViewRow> actualView = new List<DataGridViewRow>();
        RelacionHorarios actualSelected = new RelacionHorarios();

        public frmCrudRelacionHorarios()
        {
            InitializeComponent();

            //Properties.Settings.Default.TURNOS_HORARIOS = @"{""1"":{""titulo"":""Turno corrido"",""primer_horario"":{""entrada"":800,""salida"":1500},""segundo_horario"":{""entrada"":0,""salida"":0},""tiempo_extra"":{""entrada"":0,""salida"":0}},""2"":{""titulo"":""Turno completo con comida"",""primer_horario"":{""entrada"":800,""salida"":1300},""segundo_horario"":{""entrada"":1500,""salida"":1700},""tiempo_extra"":{""entrada"":0,""salida"":0}},""3"":{""titulo"":""Media tarde"",""primer_horario"":{""entrada"":1500,""salida"":1700},""segundo_horario"":{""entrada"":0,""salida"":0},""tiempo_extra"":{""entrada"":0,""salida"":0}}}";
            //Properties.Settings.Default.Save();

            // Cargamos los elementos
            this.cboxParametroLimitacion.Items.AddRange(Enum.GetValues<LimitationParam>()
                                                            .Cast<LimitationParam>()
                                                            .Select(l => l.GetText())
                                                            .ToArray());
            this.cboxParametroLimitacion.SelectedIndex = 0;
            this.txtValorLimitacion.Text = "";
            this.dgvRelacionDeHorarios.SetGridStyle(DataGridStylesGallery.BlueStyle);
        }

        void WriteStatus(bool status, string message)
        {
            this.lblStatus.Text = status ? "Listo" : "Error";
            this.lblStatus.ForeColor = status ? Color.DarkGreen : Color.IndianRed;
            this.lblMessage.Text = message;
        }

        private void frmCrudRelacionHorarios_Load(object sender, EventArgs e)
        {
            WriteStatus(true, "Inicializacion exitosa");

            this.cboxMonth.SelectedIndex = DateTime.Now.Month - 1;
            this.txtYear.Text = DateTime.Now.Year.ToString();

            this.btnIrAMes.PerformClick();
            this.dgvRelacionDeHorarios.MouseHoverEffectEnabled = true;
        }

        /// <summary>
        /// Vista con objetos filtrados
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        void LoadLimitedView(LimitationParam param, string value)
        {
            try
            {
                this.dgvRelacionDeHorarios.Rows.Clear();
                foreach (DataGridViewRow r in actualView)
                {
                    switch (param)
                    {
                        case LimitationParam.TODO:
                            // Mostramos todo
                            this.dgvRelacionDeHorarios.Rows.Add(r);
                            break;
                        case LimitationParam.NOMBRE:
                            // Mostramos vista filtrada por el nombre de empleado
                            if (r.Cells[RelacionHorariosGridCells.NOMBRE_COMP.GetIndex()].Value.ToString().Contains(value, StringComparison.OrdinalIgnoreCase))
                                this.dgvRelacionDeHorarios.Rows.Add(r);
                            break;
                        case LimitationParam.NUM_EMP:
                            // Mostramos vista filtrada por el numero de empleado
                            if (r.Cells[RelacionHorariosGridCells.NO_EMP.GetIndex()].Value.ToString().Contains(value, StringComparison.OrdinalIgnoreCase))
                                this.dgvRelacionDeHorarios.Rows.Add(r);
                            break;
                    }
                }
                this.dgvRelacionDeHorarios.Invalidate();

                WriteStatus(true, $"Ambito limitado a '{param.GetText()}' : '{value}'");
            }
            catch (Exception ex)
            {
                WriteStatus(false, $"Ocurio un error inesperado. {ex.Message}");
            }

        }

        void LoadView(int month, int year, string localidad = "Sufragio")
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                actualSelected = RelacionHorarios.Get(new RelacionHorarioID(DateTimeFormatInfo.CurrentInfo.GetMonthName(month), year),
                                                                        ShowObjectLog: false).Object ?? throw new NullReferenceException("Ocurrio un error en el proceso de obtencion de la relacion de horarios!");
                Response loadViewProcess = actualSelected.LoadCrudBaseView(this.dgvRelacionDeHorarios, month, year, localidad);

                if (!loadViewProcess.Success)
                    throw new Exception("No se cargo correctamente la vista de la grilla de datos");

                actualView.Clear();
                foreach (DataGridViewRow r in this.dgvRelacionDeHorarios.Rows)
                    actualView.Add(r);

                WriteStatus(true, $"Visualizacion de {cboxMonth.Text}-{txtYear.Text}!");
            }
            catch (Exception ex)
            {
                WriteStatus(false, $"Error al cargar la vista: {ex.Message}");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //
            // BOTON DE GUARDADO
            //
            Response _resp = actualSelected.UpdateByGrid(this.dgvRelacionDeHorarios.Rows.Cast<DataGridViewRow>().ToArray())
                                            .Save(ShowObjectLog: false);
            WriteStatus(_resp.Success, _resp.Message);

            if (_resp.Success is false)
                MessageBox.Show(_resp.GetBuildedLog(), "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("Funcion proxima a implementar!");
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e) => Program.DefaultRowSelectionChanged(this.dgvRelacionDeHorarios, e);
        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e) => Program.DefaultRowValidating(this.dgvRelacionDeHorarios, e);

        private void dgvAjustesEmpleados_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex <= RelacionHorariosGridCells.NOMBRE_COMP.GetIndex())
            {
                WriteStatus(false, "No se puede editar esta celda!");
                return;
            }

            var grid = this.dgvRelacionDeHorarios;

            string oldValue = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()!;
            string newValue = e.FormattedValue!.ToString()!.Trim();

            // Limpiar antes de validar
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ErrorText = string.Empty;

            // Toma lo que el usuario está intentando dejar en la celda
            int input = -1;

            if (String.IsNullOrEmpty(newValue))
                return;

            if (!int.TryParse(newValue, out input))
            {
                e.Cancel = true;
                grid.Rows[e.RowIndex]
                    .Cells[e.ColumnIndex].ErrorText = "Debe ingresar un número entero válido.";
                return;
            }

            // Valida que el ID exista en la lista de horarios
            if (!Utils.GetHorariosIDs().Contains(input))
            {
                List<string> turns = new List<string>();
                foreach (Turno i in Turno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS))
                    turns.Add($"* {i.ID} ({i.Nombre})");

                MessageBox.Show($"Turno invalido. Escribe una de la lista disponible.\n\n{String.Join("\n", turns)}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;   // No permite salir de la celda
                return;
            }

            // Limpia error si es valido
            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;

            // Guardamos la nueva informacion
            actualView[e.RowIndex].Cells[e.ColumnIndex].Value = cell.Value;

            WriteStatus(true, $"Valor de celda actualizado de '{oldValue}' -> '{newValue}'");
        }

        private void dgvAjustesEmpleados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //var grid = (flExtendedDataGridView)sender;
            //grid.Rows[e.RowIndex].Cells[e.ColumnIndex].;
        }

        private void dgvRelacionDeHorarios_KeyDown(object sender, KeyEventArgs e)
        {
            // Agregar que solamente se puedan teclear numeros
        }

        enum Fields
        {
            [ControlValidateAttrib("txtYear", ControlField.GENERIC)]
            [ValidationRuleAttrib(ValidationParams.CUSTOM_ACTION)]
            Year,
            [ControlValidateAttrib("cboxMonth", ControlField.GENERIC)]
            [ValidationRuleAttrib(ValidationParams.CUSTOM_ACTION)]
            Month,
            [ControlValidateAttrib("dgvRelacionDeHorarios", ControlField.GENERIC)]
            Dgv
        }

        /// <summary>
        /// Funcion helper para los procesos de validacion de los controles
        /// </summary>
        /// <returns></returns>
        Control[] getLocalsControls()
        {
            List<Control> lsControls = new List<Control>();

            foreach (Control c in this.Controls)
                lsControls.Add(c);

            // Conversiones dinamicas de prueba
            ComboBox cboxMonth_Dynamic = new ComboBox() { Name = this.cboxMonth.Name };
            TextBox txtYear_Dynamic = new TextBox() { Name = this.txtYear.Name };

            lsControls.AddRange(new Control[] { cboxMonth_Dynamic, txtYear_Dynamic });

            return lsControls.ToArray();
        }

        bool ValidateFields(Fields f)
        {
            bool flag;

            Multivalidator mv = new Multivalidator(this, getLocalsControls());

            switch (f)
            {
                case Fields.Month:
                    flag = mv.Validate<Fields>(f, invalidValues: null, customValidation: () =>
                    {
                        return true;    // Por defecto, solo de manera momentanea
                    }).Success;
                    break;
                case Fields.Year:
                    flag = mv.Validate<Fields>(f, invalidValues: null, customValidation: () =>
                    {
                        return (String.IsNullOrEmpty(this.txtYear.Text.Trim()) || !int.TryParse(this.txtYear.Text.Trim(), out _));
                    }).Success;
                    break;
                case Fields.Dgv:
                    flag = mv.Validate<Fields>(f, invalidValues: null, customValidation: () =>
                    {
                        return true;
                    }, ValidationParams.CUSTOM_ACTION).Success;
                    break;
                default:
                    flag = true;
                    break;
            }

            return flag;
        }


        private void txtYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Cargamos la nueva vista de horario
                this.btnIrAMes.PerformClick();
                return;
            }

            // Permitir números del teclado principal (0–9)
            bool isNumberKey = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9;

            // Permitir números del teclado numérico
            bool isNumpadKey = e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9;

            // Permitir teclas de control
            bool isControlKey =
                e.KeyCode == Keys.Back ||
                e.KeyCode == Keys.Delete ||
                e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Tab;

            if (!isNumberKey && !isNumpadKey && !isControlKey)
            {
                e.SuppressKeyPress = true; // Bloquea el input
                e.Handled = true;
            }
        }

        private void btnIrAMes_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(Fields.Year))
            {
                WriteStatus(false, "Año invalido!");
                return;
            }

            DateTime dt = DateTime.Parse($"01-{this.cboxMonth.Text.Trim()}-{this.txtYear.Text.Trim()}");
            LoadView(dt.Month, dt.Year);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            LoadLimitedView(LimitationParamExtensions.Parse(this.cboxParametroLimitacion.Text),
                            this.txtValorLimitacion.Text);
        }

        private void cboxParametroLimitacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboxParametroLimitacion.SelectedText == "Todo")
            {
                this.txtValorLimitacion.Enabled = false;
                return;
            }
        }

        private void txtValorLimitacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnLimitarAmbito.PerformClick();
        }

        private void toolStrpBtn_NuevoTurno_Click(object sender, EventArgs e)
        {
            frmConfiguraciones frm = new frmConfiguraciones("tabAjustesHorario");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // En caso de que se haya eliminado un turno, eliminamos las referencias de ese turno
                #region
                throw new NotImplementedException();
                #endregion
            }
        }

        private void lblLocalidadSeleccionada_Click(object sender, EventArgs e)
        {
            // Abrimos el selector de localidad para hacer el cambio
            var dlgResp = flComboBoxInput.Show("Relacion de Horarios", "Localidad:", Utils.GetLocalidadesDisponibles(), Program.StandardFormStyle);
            if (dlgResp.DialogResult != DialogResult.OK)
                return;

            // Mensaje opcional para guardar en caso de haberse realizado cambios

            string localidadSeleccionada = dlgResp.Response;

            if (!ValidateFields(Fields.Year))
            {
                WriteStatus(false, "Año invalido!");
                return;
            }

            DateTime dt = DateTime.Parse($"01-{this.cboxMonth.Text.Trim()}-{this.txtYear.Text.Trim()}");
            LoadView(dt.Month, dt.Year, localidadSeleccionada);
        }
    }
}
