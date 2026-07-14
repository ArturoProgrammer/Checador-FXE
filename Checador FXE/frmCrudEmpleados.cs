using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Inputs;
using System.Data;

namespace Checador_FXE
{
    public partial class frmCrudEmpleados : Form
    {
        public frmCrudEmpleados()
        {
            InitializeComponent();
        }

        private void frmCrudEmpleados_Load(object sender, EventArgs e)
        {
            LoadLocalidades();
        }

        void LoadLocalidades()
        {
            //
            // CARGA DE LOCALIDADES Y SELECCION LA ACTUAL POR DEFAULT
            //
            this.cboxLocalidadSeleccionada.Items.Clear();
            this.cboxLocalidadSeleccionada.Items.AddRange(Utils.GetLocalidadesDisponibles());
            this.cboxLocalidadSeleccionada.SelectedItem = Properties.Settings.Default.LOCALIDAD_DEFAULT;
            this.lblLocalidadDefaultActualmente.Text = Properties.Settings.Default.LOCALIDAD_DEFAULT;
        }

        private void cboxLocalidadSeleccionada_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Traemos los empleados de la localidad seleccionada
            if (this.cboxLocalidadSeleccionada.SelectedItem == null)
                return;

            LoadView(this.cboxLocalidadSeleccionada.SelectedItem.ToString()!);
        }

        private void btnEstablecerSeleccion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea establecer la localidad seleccionada como la localidad por default?", "Confirmar selección", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            Properties.Settings.Default.LOCALIDAD_DEFAULT = this.cboxLocalidadSeleccionada.SelectedItem!.ToString();
            Properties.Settings.Default.Save();

            this.lblLocalidadDefaultActualmente.Text = Properties.Settings.Default.LOCALIDAD_DEFAULT;
            MessageBox.Show("Localidad por default actualizada correctamente.", "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {
            var input = new flDataGridInputBox().SetLimitRow(1)
                                                .SetRowGridValidation((row) =>
                                                {
                                                    // Validar no existencia de duplicados de numeros de empleados
                                                    var _empleados = Empleado.GetAll(actualLocalidad);

                                                    if (_empleados.Success)
                                                        if (_empleados.Object!.Any(emp => emp.NoEmp == row.Cells[EmpleadosGridCells.NO_EMP.GetIndex()].Value?.ToString()))
                                                            return false;

                                                    // Validar el turno por defecto asignado a los empleados   
                                                    if (row.Cells[EmpleadosGridCells.TURNO_DEFAULT.GetIndex()].Value == null || 
                                                        String.IsNullOrWhiteSpace(row.Cells[EmpleadosGridCells.TURNO_DEFAULT.GetIndex()].Value.ToString().Trim()) ||
                                                        !Utils.GetHorariosIDs().Contains(int.Parse(row.Cells[EmpleadosGridCells.TURNO_DEFAULT.GetIndex()].Value.ToString().Trim())))
                                                        return false;

                                                    return true;
                                                })
                                                .;
            flDialogResult<DataGridViewRow[]> resp = input.Show(
                "Nuevo empleado",
                new string[] { "No. Emp.", "Nombres", "Apellidos", "Puesto", "Region", "Division", "Localidad", "Turno Default" }
            );

            if (resp.DialogResult != DialogResult.OK)
                return;

            // Agregar el empleado correspondiente
            this.dgvAjustesEmpleados.Rows.Add(resp.Response);
            this.dgvAjustesEmpleados.Rows[this.dgvAjustesEmpleados.Rows.Count - 1].Selected = true;
            this.dgvAjustesEmpleados.CurrentCell = this.dgvAjustesEmpleados.Rows[this.dgvAjustesEmpleados.Rows.Count - 1].Cells[1];

            /*
             * PENDIENTE MODIFICAR
             * 
            DataGridViewRow _row = new DataGridViewRow();
            _row.Cells.Add(new DataGridViewImageCell()
            {
                Value = IconGallery.Size64.NeutralObjectGreenUnselected,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // No. Emp.
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Nombre
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Apellidos
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Puesto
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Pacifico" }); // Region
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Hermosillo" }); // Division
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = $"{Properties.Settings.Default.LOCALIDAD_DEFAULT}" }); // Localidad
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "1" });   // Turno Default
            */
        }

        private string actualLocalidad = "-1";

        void LoadView(string localidad)
        {
            actualLocalidad = localidad;
            #region CODIGO
            Response<Empleado[]> _SERV_RESP = Empleado.GetAll(actualLocalidad, ShowObjectLog: false);
            this.dgvAjustesEmpleados.Rows.Clear();

            if (!_SERV_RESP.Success)
            {
                MessageBox.Show(_SERV_RESP.Message);
                return;
            }

            foreach (Empleado i in _SERV_RESP.Object!)
            {
                DataGridViewRow _row = new DataGridViewRow();
                _row.Cells.Add(new DataGridViewImageCell()
                {
                    Value = IconGallery.Size64.NeutralObjectGreenUnselected,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                });
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.NoEmp }); // No. Emp.
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Nombres }); // Nombre
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Apellidos }); // Apellidos
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Puesto }); // Puesto
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Region }); // Region
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Division }); // Division
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.Localidad }); // Localidad
                _row.Cells.Add(new DataGridViewTextBoxCell() { Value = i.TurnoDefault });   // Turno Default

                this.dgvAjustesEmpleados.Rows.Add(_row);
            }
            #endregion
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dgvAjustesEmpleados.Rows)
            {
                Empleado emp = new Empleado()
                {
                    NoEmp = r.Cells[EmpleadosGridCells.NO_EMP.GetIndex()].Value?.ToString() ?? "",
                    Nombres = r.Cells[EmpleadosGridCells.NOMBRE.GetIndex()].Value?.ToString() ?? "",
                    Apellidos = r.Cells[EmpleadosGridCells.APELLIDOS.GetIndex()].Value?.ToString() ?? "",
                    Puesto = r.Cells[EmpleadosGridCells.PUESTO.GetIndex()].Value?.ToString() ?? "",
                    Region = r.Cells[EmpleadosGridCells.REGION.GetIndex()].Value?.ToString() ?? "",
                    Division = r.Cells[EmpleadosGridCells.DIVISION.GetIndex()].Value?.ToString() ?? "",
                    Localidad = r.Cells[EmpleadosGridCells.LOCALIDAD.GetIndex()].Value?.ToString() ?? "",
                    Area = "UdA",
                    TurnoDefault = int.TryParse(r.Cells[EmpleadosGridCells.TURNO_DEFAULT.GetIndex()].Value?.ToString(), out int turno) ? turno : 1
                };

                emp.Save(ShowObjectLog: false);
            }

            // Recargamos la vista para sincronizarla con los valores de la DB
            LoadView(this.cboxLocalidadSeleccionada.SelectedItem!.ToString()!);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e)
        {
            // Establecemos el icono de seleccionado
            if (this.dgvAjustesEmpleados.Rows.Count > 0)
                this.dgvAjustesEmpleados.SelectedRows[0].Cells[EmpleadosGridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenSelected;
        }

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Establecemos el icono de no seleccionado
            if (this.dgvAjustesEmpleados.Rows.Count > 0)
                this.dgvAjustesEmpleados.Rows[e.RowIndex].Cells[EmpleadosGridCells.ICON.GetIndex()].Value = IconGallery.Size64.NeutralObjectGreenUnselected;
        }

        private void exportarParaConfiguracionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Tarea en ticket ##100183##
            string loc = this.cboxLocalidadSeleccionada.SelectedItem!.ToString()!;
            frmExportadorDeConfiguracion frm = new frmExportadorDeConfiguracion(Empleado.GetAll(loc).Object, loc);
            frm.ShowDialog();
        }

        private void cboxLocalidadSeleccionada_Click(object sender, EventArgs e)
        {

        }

        private void dgvAjustesEmpleados_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == EmpleadosGridCells.LOCALIDAD.GetIndex())
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

        // Limpia el texto de error al terminar la edición (visual)
        private void dgvAjustesEmpleados_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (flExtendedDataGridView)sender;
            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;

            string input = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()!;
            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Utils.GetLocalidadesDisponibles().Cast<string>()
                                                                                                .FirstOrDefault(t => t.Equals(input, StringComparison.OrdinalIgnoreCase));
        }

        private void dgvAjustesEmpleados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //var grid = (flExtendedDataGridView)sender;
            //grid.Rows[e.RowIndex].Cells[e.ColumnIndex].;
        }
    }
}
