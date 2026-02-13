using Checador_FXE.MdiForms;
using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Checador_FXE
{
    public partial class MainDesktop : Form
    {
        mdiQuincenaView? actualView = null;

        public MainDesktop()
        {
            InitializeComponent();
            //this.IsMdiContainer = true;
            Program.lblStatus = this.lblStatusText;
            Program.lblOperation = this.lblOperationText;
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* 
             * Crea un nuevo proyecto
             * */
            frmNuevoProyecto frm_n = new frmNuevoProyecto();
            frm_n.ShowDialog();

            if (frm_n.DialogResult != DialogResult.OK)
            {
                Program.WriteStatus(false, "Operacion cancelada por el usuario!");
                return;
            }

            ReporteAsistencias report = new ReporteAsistencias(frm_n.Response.Path, frm_n.Response.Device);

            // Early Return para validar que no este abierto actualmente
            string targetName = $"{report.ReportPeriod.Start:d} - {report.ReportPeriod.End:d}";
            string[] nodesTexts = this.treeViewProyectosQuincenas.Nodes.Cast<TreeNode>().Select(n => n.Text.Trim()).ToArray();
            if (nodesTexts.Contains(targetName))
            {
                Program.WriteStatus(false, $"Ya se encuentra abierto el reporte '{targetName}'");
                return;
            }

            // Abrimos el proyecto
            mdiQuincenaView frm = new mdiQuincenaView(frm_n.Response.Titulo, report, this);
            frm.TopLevel = false;
            frm.Tag = UtilityFunctions.HASHGenerator();
            frm.Dock = DockStyle.Fill;
            MDI_PANEL.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();

            // Agregamos el nodo correspondiente
            this.treeViewProyectosQuincenas.Nodes.Add(new TreeNode()
            {
                Text = targetName,
                Tag = Int32.Parse(frm.Tag.ToString()!)
            });
        }

        private void MainDesktop_Load(object sender, EventArgs e)
        {
            Program.WriteStatus(true, "Inizializacion de programa exitosa!");
        }

        private void MDI_PANEL_ControlAdded(object sender, ControlEventArgs e)
        {
            this.lblBienvenido.Visible = this.MDI_PANEL.Controls.Cast<Control>()
                                                    .Any(c => c is not Label);
        }

        private void MDI_PANEL_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.lblBienvenido.Visible = this.MDI_PANEL.Controls.Cast<Control>()
                                                    .Any(c => c is not Label);
        }

        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfiguraciones frm = new frmConfiguraciones();
            frm.ShowDialog();
        }

        private void treeViewProyectosQuincenas_DoubleClick(object sender, EventArgs e)
        {
            // Traemos el mdi del nodo seleccionado al frente
            List<Form> frm = new List<Form>();
            foreach (Control c in this.MDI_PANEL.Controls)
            {
                if (c is Form && (c as Form).Tag.ToString() == this.treeViewProyectosQuincenas.SelectedNode.Tag.ToString())
                    frm.Add(c as Form);
            }

            if (frm.Count <= 0)
                Program.WriteStatus(false, "No se encontró el proyecto seleccionado!");

            frm[0].BringToFront();
            Program.WriteStatus(true, $"Formulario {this.treeViewProyectosQuincenas.SelectedNode.Text} traido al frente");
        }

        private void toolStrpBtnNuevo_Click(object sender, EventArgs e)
        {
            nuevoToolStripMenuItem.PerformClick();
        }

        private void toolStrpBtnAbrir_Click(object sender, EventArgs e)
        {
            abrirToolStripMenuItem.PerformClick();
        }

        private void editorDePersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abrimos el editor de personal
            frmCrudEmpleados frm = new frmCrudEmpleados();
            frm.ShowDialog();
        }


        /// <summary>
        /// Metodo que lanza el dialogo de guardado del sistema y retorna la ruta seleccionada
        /// </summary>
        /// <returns></returns>
        string _CommonSaveDialog()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = $"{CafProjFile.FileExtensionName} (*.{CafProjFile.FileExtension}) | *.{CafProjFile.FileExtension}";
                dialog.InitialDirectory = CafProjFile.DefaultProjFilePath;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return "-1";

                return dialog.FileName;
            }
        }

        /// <summary>
        /// Metodo de guardado del archivo de proyecto
        /// </summary>
        /// <param name="path"></param>
        void _CommonSaveMethod(string path)
        {
            Response funcResp = actualView!.ActualCafProject.Save(path);

            string resultText = funcResp.Success ? $"Proyecto guardado en '{funcResp.Tag}'!" : funcResp.Message;
            Program.WriteStatus(funcResp.Success, resultText);
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Tarea en ticket ##100187##
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = $"{CafProjFile.FileExtensionName} (*.{CafProjFile.FileExtension}) | *.{CafProjFile.FileExtension}";
                dialog.InitialDirectory = CafProjFile.DefaultProjFilePath;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                Response<CafProjFile> funcResp = CafProjFile.Build(dialog.FileName);

                if (funcResp.Success)
                {
                    CafProjFile _proj = funcResp.Object!;

                    // El primer argumento es la ubicacion del archivo .xls a cargar en el reporte
                    ReporteAsistencias report = new ReporteAsistencias(_proj);
                    
                    // Early Return para validar que no este abierto actualmente
                    string targetName = $"{report.ReportPeriod.Start:d} - {report.ReportPeriod.End:d}";
                    string[] nodesTexts = this.treeViewProyectosQuincenas.Nodes.Cast<TreeNode>().Select(n => n.Text.Trim()).ToArray();
                    if (nodesTexts.Contains(targetName))
                    {
                        Program.WriteStatus(false, $"Ya se encuentra abierto el reporte '{targetName}'");
                        return;
                    }

                    // Abrimos el MDI
                    mdiQuincenaView frm = new mdiQuincenaView(_proj.AssetsFile.Title, report, this, _proj, dialog.FileName);
                    frm.TopLevel = false;
                    frm.Tag = UtilityFunctions.HASHGenerator();
                    frm.Dock = DockStyle.Fill;
                    MDI_PANEL.Controls.Add(frm);
                    frm.BringToFront();
                    frm.Show();

                    // Añadimos el MDI al TreeView
                    this.treeViewProyectosQuincenas.Nodes.Add(new TreeNode()
                    {
                        Text = targetName,
                        Tag = Int32.Parse(frm.Tag.ToString()!)
                    });
                }

                string resultText = funcResp.Success ? $"Proyecto guardado en '{funcResp.Tag}'!" : funcResp.Message;
                Program.WriteStatus(funcResp.Success, resultText);
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Tarea en ticket ##100187##
            if (actualView is null)
            {
                Program.WriteStatus(false, $"Error inesperado. No se puede guardar cuando la vista actual es null");
                return;
            }

            string targetPath = actualView.ProjectFullname == "-1" ? _CommonSaveDialog() : actualView.ProjectFullname;

            if (targetPath == "-1")
                return;

            _CommonSaveMethod(targetPath);
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Tarea en ticket ##100187##
            if (actualView is null)
            {
                Program.WriteStatus(false, $"Error inesperado. No se puede guardar cuando la vista actual es null");
                return;
            }

            string targetPath = _CommonSaveDialog();

            if (targetPath == "-1")
                return;

            _CommonSaveMethod(targetPath);
        }
    }
}
