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

                for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
                {
                    _row.Cells.Add(new DataGridViewTextBoxCell());
                }
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
            if (e.ColumnIndex == GridCells.LOCALIDAD.GetIndex())
            {
                var grid = (flExtendedDataGridView)sender;

                // Toma lo que el usuario está intentando dejar en la celda
                string input = e.FormattedValue?.ToString() ?? string.Empty;

                // Normaliza espacios
                input = input.Trim();

                // Valida contra la lista, ignorando mayúsculas/minúsculas
                bool ok = Utils.GetLocalidadesDisponibles()
                               .Contains(input, StringComparer.OrdinalIgnoreCase);
                if (!ok)
                {
                    List<string> localidades = new List<string>();
                    foreach (string i in Utils.GetLocalidadesDisponibles())
                        localidades.Add($"* {i}");

                    MessageBox.Show($"Localidad inválida. Escribe una de la lista disponible.\n\n{String.Join("\n", localidades)}");
                    e.Cancel = true;   // No permite salir de la celda
                    return;
                }

                // Limpia error si es válido
                grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
            }
        }

        private void dgvAjustesEmpleados_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (flExtendedDataGridView)sender;
            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;

            var input = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (input is null)
                return;

            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Utils.GetHorarios().Cast<int>()
                                                                                    .FirstOrDefault(t => input == t.ToString());
        }

        private void dgvAjustesEmpleados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //var grid = (flExtendedDataGridView)sender;
            //grid.Rows[e.RowIndex].Cells[e.ColumnIndex].;
        }
    }
}
