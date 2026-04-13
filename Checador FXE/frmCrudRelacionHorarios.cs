using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using System.CodeDom;
using System.Data;

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

        private void frmCrudRelacionHorarios_Load(object sender, EventArgs e)
        {
            LoadView(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {

        }

        void LoadView(int month, int year, string localidad = "Sufragio")
        {
            #region CODIGO
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
                this.dgvRelacionDeHorarios.SelectedRows[0].Cells[GridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenSelected;
        }

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Establecemos el icono de no seleccionado
            if (this.dgvRelacionDeHorarios.Rows.Count > 0)
                this.dgvRelacionDeHorarios.Rows[e.RowIndex].Cells[GridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenUnselected;
        }

        private void dgvAjustesEmpleados_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex <= 2)
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
    }
}
