using Checador_FXE.Plantillas;
using FlowCommonWorkcore.SqlUtils.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            #region CODIGO
            MySqlDataReader _query = new Server.GeneralQuery(new ConnectionsData(
                Properties.Settings.Default.SERVER_HOSTNAME,
                Properties.Settings.Default.SERVER_USER,
                Properties.Settings.Default.SERVER_PASS,
                Int32.Parse(Properties.Settings.Default.SERVER_PORT),
                Empleado.TABLE_NAME,
                Empleado.DATABASE_NAME
            )).ExecuteQuery(
                $"SELECT * FROM {Empleado.DATABASE_NAME}.{Empleado.TABLE_NAME};",
                new (string, object)[] { }
            );

            try
            {
                while (_query.Read())
                {
                    DataGridViewRow _row = new DataGridViewRow();
                    _row.Cells.Add(new DataGridViewImageCell()
                    {
                        Value = Properties.Resources.neutral_object1_unselected_64,
                        ImageLayout = DataGridViewImageCellLayout.Zoom,
                    });
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(0) }); // No. Emp.
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(1) }); // Nombre
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(2) }); // Apellidos
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(3) }); // Puesto
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(4) }); // Region
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(5) }); // Division
                    _row.Cells.Add(new DataGridViewTextBoxCell() { Value = _query.GetString(6) }); // Localidad

                    this.dgvAjustesHorarios.Rows.Add(_row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar los empleados de la localidad seleccionada.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _query.Close();
            }
            #endregion
        }

        private void btnEstablecerSeleccion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea establecer la localidad seleccionada como la localidad por default?", "Confirmar selección", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            Properties.Settings.Default.LOCALIDAD_DEFAULT = this.cboxLocalidadSeleccionada.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            this.lblLocalidadDefaultActualmente.Text = Properties.Settings.Default.LOCALIDAD_DEFAULT;
            MessageBox.Show("Localidad por default actualizada correctamente.", "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {
            DataGridViewRow _row = new DataGridViewRow();
            _row.Cells.Add(new DataGridViewImageCell()
            {
                Value = Properties.Resources.neutral_object1_unselected_64,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // No. Emp.
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Nombre
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Apellidos
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "" }); // Puesto
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Pacifico" }); // Region
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Hermosillo" }); // Division
            _row.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sufragio" }); // Localidad

            this.dgvAjustesHorarios.Rows.Add(_row);
            this.dgvAjustesHorarios.Rows[this.dgvAjustesHorarios.Rows.Count - 1].Selected = true;
            this.dgvAjustesHorarios.CurrentCell = this.dgvAjustesHorarios.Rows[this.dgvAjustesHorarios.Rows.Count - 1].Cells[1];
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e)
        {
            // Establecemos el icono de seleccionado
            this.dgvAjustesHorarios.SelectedRows[0].Cells[0].Value = Properties.Resources.neutral_object_64;
        }

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Establecemos el icono de no seleccionado
            this.dgvAjustesHorarios.Rows[e.RowIndex].Cells[0].Value = Properties.Resources.neutral_object1_unselected_64;
        }

        private void exportarParaConfiguracionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
