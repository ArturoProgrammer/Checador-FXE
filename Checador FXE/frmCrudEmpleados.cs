using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Inputs;
using System.ComponentModel;
using System.Data;

namespace Checador_FXE
{
    public partial class frmCrudEmpleados : Form
    {
        public frmCrudEmpleados()
        {
            InitializeComponent();
            this.dgvAjustesEmpleados.SetGridStyle(Program.StandardGridStyle);
            this.dgvAjustesEmpleados.RowTemplate.Height = Program.DefaultRowHeight;
            this.dgvAjustesEmpleados.AllowUserToResizeRows = false;
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
            if (flMessageBox.Show("¿Desea establecer la localidad seleccionada como la localidad por default?", 
                                  "Confirmar selección", 
                                  MessageBoxButtons.YesNo, 
                                  MessageBoxIcon.Question) == DialogResult.No)
                return;

            Properties.Settings.Default.LOCALIDAD_DEFAULT = this.cboxLocalidadSeleccionada.SelectedItem!.ToString();
            Properties.Settings.Default.Save();

            this.lblLocalidadDefaultActualmente.Text = Properties.Settings.Default.LOCALIDAD_DEFAULT;
            flMessageBox.Show("Localidad por default actualizada correctamente.", 
                              "Operación exitosa", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Information);
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {
            bool rowIsValid = false;

            var input = new flDataGridInputBox()
                .SetLimitRow(1)
                .SetRowGridValidation((row) =>
                {
                    var _empleados = Empleado.GetAll(actualLocalidad);
                    int _turnoDefIndex = EmpleadosGridCells.TURNO_DEFAULT.GetIndex() - 1;
                    int _noEmpDefIndex = EmpleadosGridCells.NO_EMP.GetIndex() - 1;
                    List<string> fails = new List<string>();

                    // Validar campos no vacios
                    int index = 1;
                    foreach (DataGridViewCell c in row.Cells)
                    {
                        if (c.Value is null || String.IsNullOrEmpty(c.Value.ToString()!.Trim()))
                            fails.Add($"* No se pueden dejar celdas con informacion vacia (Col.: {index})");

                        index++;
                    }

                    // Validar no existencia de duplicados de numeros de empleados
                    if (_empleados.Success)
                        if (_empleados.Object!.Any(emp => emp.NoEmp == row.Cells[_noEmpDefIndex].Value?.ToString()))
                            fails.Add($"* Numero de empleado duplicado ({_empleados.Object!.FirstOrDefault(e => e.NoEmp == row.Cells[_noEmpDefIndex].Value.ToString())})");

                    // Validar el turno por defecto asignado a los empleados   
                    if (row.Cells[_turnoDefIndex].Value == null ||
                        String.IsNullOrWhiteSpace(row.Cells[_turnoDefIndex].Value.ToString().Trim()) ||
                        !Utils.GetHorariosIDs().Contains(int.Parse(row.Cells[_turnoDefIndex].Value.ToString().Trim())))
                        fails.Add("El turno por defecto asignado al empleado no es valido");

                    rowIsValid = fails.Count == 0; // Asignamos una identificacion que indique que la fila es valida

                    if (!rowIsValid)
                        flMessageBox.Show($"La fila no es valida por los siguientes motivos:\n\n{String.Join('\n', fails)}", "Validacion de Fila Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return rowIsValid;
                })
                .SetDefaultValues(new flDefaultColumnValue[]
                {
                    new (4, "PACIFICO"),
                    new (5, "HERMOSILLO"),
                    new (6, actualLocalidad),
                    new (7, Properties.Settings.Default.TURNO_DEFECTO)
                })
                .SetCloseEvenIfFails(true)
                .SetGridStyle(Program.StandardGridStyle.SetAutoSizeColumnsMode(DataGridViewAutoSizeColumnsMode.AllCells));

            flDialogResult<DataGridViewRow[]> resp = input.Show(
                "Nuevo empleado",
                new[] { "No. Emp.", "Nombres", "Apellidos", "Puesto", "Region", "Division", "Localidad", "Turno Default" }
            );

            if (resp.DialogResult != DialogResult.OK || rowIsValid is false)
                return;

            // Agregamos el icono en la celda inicial
            DataGridViewRow _row = new DataGridViewRow();
            _row.Cells.AddRange(
                new DataGridViewImageCell()
                {
                    Value = IconGallery.NeutralObjectGreenUnselected.Render(IconSize.S_64),
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                },
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[0].Value },  // No. Emp.
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[1].Value },  // Nombre
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[2].Value },  // Apellidos
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[3].Value },  // Puesto
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[4].Value },  // Region
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[5].Value },  // Division
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[6].Value },  // Localidad
                new DataGridViewTextBoxCell() { Value = resp.Response[0].Cells[7].Value }   // Turno Default
            );

            // Agregar el empleado correspondiente
            this.dgvAjustesEmpleados.Rows.Add(_row);
            this.dgvAjustesEmpleados.Rows[this.dgvAjustesEmpleados.Rows.Count - 1].Selected = true;
            this.dgvAjustesEmpleados.CurrentCell = this.dgvAjustesEmpleados.Rows[this.dgvAjustesEmpleados.Rows.Count - 1].Cells[1];
        }

        /// <summary>
        /// Localidad actualmente seleccionada. Si no se encuentra ninguna localidad seleccionada, el valor es igual al por defecto (-1)
        /// </summary>
        private string actualLocalidad = "-1";

        void LoadView(string localidad)
        {
            actualLocalidad = localidad;
            #region CODIGO
            Response<Empleado[]> _SERV_RESP = Empleado.GetAll(actualLocalidad, ShowObjectLog: false);
            this.dgvAjustesEmpleados.Rows.Clear();

            if (!_SERV_RESP.Success)
            {
                flMessageBox.Show(_SERV_RESP.Message);
                return;
            }

            foreach (Empleado i in _SERV_RESP.Object!)
            {
                DataGridViewRow _row = new DataGridViewRow();
                _row.Cells.Add(new DataGridViewImageCell()
                {
                    Value = IconGallery.NeutralObjectGreenUnselected.Render(IconSize.S_64),
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
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
            catch (Exception ex)
            {
                _commonFailMessage(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void _commonFailMessage(string ex) => 
            flMessageBox.Show($"Ocurrio un error inesperado. {ex}",
                "Excepcion inesperada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                Program.StandardFormStyle);

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e) => Program.DefaultRowSelectionChanged(this.dgvAjustesEmpleados, e);

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e) => Program.DefaultRowValidating(this.dgvAjustesEmpleados, e);

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
                try
                {
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

                        flMessageBox.Show($"Localidad invalida. Escribe una de la lista disponible.\n\n{String.Join("\n", localidades)}",
                                          "Localidad Invalida",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning,
                                          Program.StandardFormStyle);
                        e.Cancel = true;   // No permite salir de la celda
                        return;
                    }

                    // Limpia error si es válido
                    this.dgvAjustesEmpleados.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
                }
                catch (Exception ex)
                {
                    _commonFailMessage(ex.Message);
                }
                return;
            }

            if (e.ColumnIndex == EmpleadosGridCells.TURNO_DEFAULT.GetIndex())
            {
                Program.DefaultCellValidating(Program.WriteStatus, this.dgvAjustesEmpleados, null!, e);
                return;
            }
        }

        // Limpia el texto de error al terminar la edición (visual)
        private void dgvAjustesEmpleados_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var grid = this.dgvAjustesEmpleados;

                if (e.ColumnIndex == EmpleadosGridCells.LOCALIDAD.GetIndex())
                {

                    grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;

                    string input = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()!;
                    grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Utils.GetLocalidadesDisponibles()
                                                                            .Cast<string>()
                                                                            .FirstOrDefault(t => t.Equals(input, StringComparison.OrdinalIgnoreCase));
                    return;
                }

                if (e.ColumnIndex == EmpleadosGridCells.TURNO_DEFAULT.GetIndex())
                {
                    // Validacion del turno ingresado
                    return;
                }
            }
            catch (Exception ex)
            {
                _commonFailMessage(ex.Message);
            }
        }

        private void dgvAjustesEmpleados_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //var grid = (flExtendedDataGridView)sender;
            //grid.Rows[e.RowIndex].Cells[e.ColumnIndex].;
        }

        private void dgvAjustesEmpleados_OnRemoveClick(object sender, CancelEventArgs e)
        {
            if (flMessageBox.Show("¿estas seguro que deseas eliminar el elemento seleccionado?",
                                    "Confirmacion",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) is DialogResult.No)
            {
                // Cancelamos la eliminacion de la fila
                e.Cancel = true;
            }
        }
    }
}
