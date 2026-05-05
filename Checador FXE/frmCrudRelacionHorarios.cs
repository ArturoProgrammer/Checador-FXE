using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Utils;
using System.CodeDom;
using System.Diagnostics;
using System.Globalization;

namespace Checador_FXE
{
    public partial class frmCrudRelacionHorarios : Form
    {
        DataGridViewColumn[] colBaseTemplate =
        {
            new DataGridViewImageColumn() {
                Name = "colIcon",
                HeaderText = "",
                ReadOnly = true,
                Width = 32,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            }, new DataGridViewTextBoxColumn() {
                Name = "colNumEmp",
                HeaderText = "No. Emp.",
                ReadOnly = true,
                Width = 60,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            }, new DataGridViewTextBoxColumn() {
                Name = "colNombre",
                HeaderText = "Nombre",
                ReadOnly = true,
                Width = 250,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            }
        };

        public frmCrudRelacionHorarios()
        {
            InitializeComponent();

            //Properties.Settings.Default.TURNOS_HORARIOS = @"{""1"":{""titulo"":""Turno corrido"",""primer_horario"":{""entrada"":800,""salida"":1500},""segundo_horario"":{""entrada"":0,""salida"":0},""tiempo_extra"":{""entrada"":0,""salida"":0}},""2"":{""titulo"":""Turno completo con comida"",""primer_horario"":{""entrada"":800,""salida"":1300},""segundo_horario"":{""entrada"":1500,""salida"":1700},""tiempo_extra"":{""entrada"":0,""salida"":0}},""3"":{""titulo"":""Media tarde"",""primer_horario"":{""entrada"":1500,""salida"":1700},""segundo_horario"":{""entrada"":0,""salida"":0},""tiempo_extra"":{""entrada"":0,""salida"":0}}}";
            //Properties.Settings.Default.Save();
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
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {

        }

        void LoadView(int month, int year, string localidad = "Sufragio")
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                #region CARGA DE LA UI
                Response<Empleado[]> _SERV_RESP = Empleado.GetAll(localidad, ShowObjectLog: false);

                this.dgvRelacionDeHorarios.Rows.Clear();
                this.dgvRelacionDeHorarios.Columns.Clear();

                if (!_SERV_RESP.Success)
                {
                    MessageBox.Show(_SERV_RESP.Message);
                    return;
                }

                // Preparamos primero las columnas del DGV
                this.dgvRelacionDeHorarios.Columns.AddRange(colBaseTemplate);
                for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                {
                    this.dgvRelacionDeHorarios.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = i.ToString(),
                        Name = $"colDay{i.ToString()}",
                        Width = 32,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                        Resizable = DataGridViewTriState.False,
                    });
                }

                // Cargamos las filas
                foreach (Empleado j in _SERV_RESP.Object!)
                {
                    DataGridViewRow _row = new DataGridViewRow();
                    _row.Cells.AddRange(
                        new DataGridViewImageCell()
                        {
                            Value = IconGallery.Size64.NeutralObjectGreenUnselected,
                            ImageLayout = DataGridViewImageCellLayout.Zoom,
                        },
                        new DataGridViewTextBoxCell() { Value = j.NoEmp },
                        new DataGridViewTextBoxCell() { Value = $"{j.Nombres} {j.Apellidos}" }
                    );

                    // Agregamos las celdas de los dias
                    for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                        _row.Cells.Add(new DataGridViewTextBoxCell());

                    this.dgvRelacionDeHorarios.Rows.Add(_row);
                }
                #endregion

                #region CARGA DE DATOS EN LA UI
                // Recorremos todas las filas para ir llenando los turnos asignados a esos dias
                RelacionHorarios _actualRelacion = RelacionHorarios.Get(new RelacionHorarioID(DateTimeFormatInfo.CurrentInfo.GetMonthName(month), year), 
                                                                        ShowObjectLog: true).Object ?? throw new NullReferenceException("Ocurrio un error en el proceso de obtencion de la relacion de horarios!");
                foreach (var i in _actualRelacion.Relacion.Items)
                {
                    foreach (DataGridViewRow r in this.dgvRelacionDeHorarios.Rows)
                    {
                        if (r.Cells[RelacionHorariosGridCells.NO_EMP.GetIndex()].Value.ToString() != i.NoEmp.ToString())
                            continue;
                        
                        for (int d_i = RelacionHorariosGridCells.DAYS_START.GetIndex(); d_i < r.Cells.Count; d_i++)
                            r.Cells[d_i].Value = i.Turno;   // Escribimos el turno asignado

                        break;
                    }
                }
                #endregion

                WriteStatus(true, $"Visualizacion de {cboxMonth.Text}-{txtYear.Text}!");
            }
            catch (Exception ex)
            {
                WriteStatus(false, $"Error al cargar la vista: {ex.Message}");
            }

            this.Cursor = Cursors.Default;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //
            // BOTON DE GUARDADO
            //
            throw new NotImplementedException();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e)
        {
            // Establecemos el icono de seleccionado
            if (this.dgvRelacionDeHorarios.Rows.Count > 0)
                this.dgvRelacionDeHorarios.SelectedRows[0].Cells[RelacionHorariosGridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenSelected;
        }

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Establecemos el icono de no seleccionado
            if (this.dgvRelacionDeHorarios.Rows.Count > 0)
                this.dgvRelacionDeHorarios.Rows[e.RowIndex].Cells[RelacionHorariosGridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenUnselected;
        }

        private void dgvAjustesEmpleados_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex <= RelacionHorariosGridCells.NOMBRE_COMP.GetIndex())
                return;

            var grid = (flExtendedDataGridView)sender;

            // Limpiar antes de validar
            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ErrorText = string.Empty;

            // Toma lo que el usuario está intentando dejar en la celda
            int input = -1;

            if (e.FormattedValue?.ToString()?.Trim() == String.Empty)
                return;

            if (!int.TryParse(e.FormattedValue?.ToString(), out input))
            {
                e.Cancel = true;
                grid.Rows[e.RowIndex]
                    .Cells[e.ColumnIndex].ErrorText = "Debe ingresar un número entero válido.";
                return;
            }

            // Valida que el ID exista en la lista de horarios
            bool ok = Utils.GetHorariosIDs().Contains(input);

            if (!ok)
            {
                List<string> turns = new List<string>();
                foreach (Turno i in Turno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS))
                    turns.Add($"* {i.ID} ({i.Nombre})");

                MessageBox.Show($"Turno invalido. Escribe una de la lista disponible.\n\n{String.Join("\n", turns)}");
                e.Cancel = true;   // No permite salir de la celda
                return;
            }

            // Limpia error si es valido
            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
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
    }
}
