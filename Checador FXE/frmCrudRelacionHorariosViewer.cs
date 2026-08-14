using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Utils;

namespace Checador_FXE
{
    public partial class frmCrudRelacionHorariosViewer : Form
    {
        /// <summary>
        /// Arreglo de las columnas actualmente cargadas, no la vista actual.
        /// </summary>
        List<DataGridViewRow> actualView = new List<DataGridViewRow>();

        internal Response<DataGridViewRow[]> Response { get; private set; } = new Response<DataGridViewRow[]>(false, "-1", null);

        public frmCrudRelacionHorariosViewer(DataGridViewColumn[] cols, DataGridViewRow[] rows)
        {
            InitializeComponent();

            // Cargamos los elementos
            this.cboxParametroLimitacion.Items.AddRange(Enum.GetValues<LimitationParam>()
                                                            .Cast<LimitationParam>()
                                                            .Select(l => l.GetText())
                                                            .ToArray());
            this.cboxParametroLimitacion.SelectedIndex = 0;
            this.txtValorLimitacion.Text = "";

            this.dgvRelacionDeHorarios.Columns.Clear();
            foreach (DataGridViewColumn col in cols)
                this.dgvRelacionDeHorarios.Columns.Add(col);

            this.dgvRelacionDeHorarios.SetGridStyle(Program.StandardGridStyle);
            actualView = rows.ToList();
        }

        void WriteStatus(bool status, string message)
        {
            this.lblStatus.Text = status ? "Listo" : "Error";
            this.lblStatus.ForeColor = status ? Color.DarkGreen : Color.IndianRed;
            this.lblMessage.Text = message;
        }

        private void frmCrudRelacionHorarios_Load(object sender, EventArgs e)
        {
            this.dgvRelacionDeHorarios.MouseHoverEffectEnabled = true;
            this.dgvRelacionDeHorarios.SetLockedColumns(2);
            this.dgvRelacionDeHorarios.AllowUserToResizeRows = false;
            LoadView();

            WriteStatus(true, "Inicializacion exitosa");
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


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

        void LoadView()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                this.dgvRelacionDeHorarios.Rows.Clear();
                foreach (DataGridViewRow r in actualView)
                    this.dgvRelacionDeHorarios.Rows.Add(r);

                WriteStatus(true, $"Visualizacion cargada con exito!");
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

        DataGridViewRow[] _CloneRows()
        {
            var clones = new List<DataGridViewRow>();

            foreach (DataGridViewRow src in this.dgvRelacionDeHorarios.Rows)
            {
                // Clonar la estructura de la fila
                var newRow = (DataGridViewRow)src.Clone();

                // Asegurar que existan las celdas clonadas
                for (int i = 0; i < src.Cells.Count; i++)
                {
                    // Copiar valor
                    newRow.Cells[i].Value = src.Cells[i].Value;

                    // Copiar estilo visual básico
                    newRow.Cells[i].Style = src.Cells[i].Style;
                    newRow.Cells[i].ToolTipText = src.Cells[i].ToolTipText;

                    // Copiar Tag si existe (por seguridad, no referencia a controles)
                    try
                    {
                        newRow.Cells[i].Tag = src.Cells[i].Tag;
                    }
                    catch { }
                }

                // Copiar propiedades de fila
                newRow.HeaderCell.Value = src.HeaderCell.Value;
                try { newRow.Tag = src.Tag; } catch { }

                clones.Add(newRow);
            }

            return clones.ToArray();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //
            // BOTON DE GUARDADO
            //
            this.Response = new Response<DataGridViewRow[]>(true, "Filas de la visualizacion actual cargada!", _CloneRows());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e) => 
            Program.DefaultRowSelectionChanged(this.dgvRelacionDeHorarios, e);

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e) => 
            Program.DefaultRowValidating(this.dgvRelacionDeHorarios, e);

        private void dgvAjustesEmpleados_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) =>
            Program.DefaultCellValidating(this.dgvRelacionDeHorarios, actualView, e);

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

    }
}
