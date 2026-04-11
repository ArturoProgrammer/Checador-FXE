namespace Checador_FXE
{
    partial class MainDesktop
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDesktop));
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            abrirToolStripMenuItem = new ToolStripMenuItem();
            nuevoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            guardarToolStripMenuItem = new ToolStripMenuItem();
            guardarComoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            propiedadesToolStripMenuItem = new ToolStripMenuItem();
            verToolStripMenuItem = new ToolStripMenuItem();
            editorDePersonalToolStripMenuItem = new ToolStripMenuItem();
            registroDePersonalToolStripMenuItem = new ToolStripMenuItem();
            relacionDeHorariosToolStripMenuItem = new ToolStripMenuItem();
            STATUS_BAR = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lblStatusText = new ToolStripStatusLabel();
            lblOperationText = new ToolStripStatusLabel();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            toolStrpBtnNuevo = new ToolStripButton();
            toolStrpBtnAbrir = new ToolStripButton();
            splitContainer_ProyectosVisualizacion = new SplitContainer();
            treeViewProyectosQuincenas = new FlowControls.flTreeView();
            imageList1 = new ImageList(components);
            MDI_PANEL = new Panel();
            lblBienvenido = new Label();
            menuStrip1.SuspendLayout();
            STATUS_BAR.SuspendLayout();
            flCustomToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer_ProyectosVisualizacion).BeginInit();
            splitContainer_ProyectosVisualizacion.Panel1.SuspendLayout();
            splitContainer_ProyectosVisualizacion.Panel2.SuspendLayout();
            splitContainer_ProyectosVisualizacion.SuspendLayout();
            MDI_PANEL.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ActiveCaption;
            menuStrip1.Font = new Font("Segoe UI", 10F);
            menuStrip1.ImageScalingSize = new Size(18, 18);
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, verToolStripMenuItem, editorDePersonalToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1107, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abrirToolStripMenuItem, nuevoToolStripMenuItem, toolStripSeparator1, guardarToolStripMenuItem, guardarComoToolStripMenuItem, toolStripSeparator2, propiedadesToolStripMenuItem });
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(71, 24);
            archivoToolStripMenuItem.Text = "Archivo";
            // 
            // abrirToolStripMenuItem
            // 
            abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            abrirToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            abrirToolStripMenuItem.Size = new Size(198, 24);
            abrirToolStripMenuItem.Text = "Abrir";
            abrirToolStripMenuItem.Click += abrirToolStripMenuItem_Click;
            // 
            // nuevoToolStripMenuItem
            // 
            nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            nuevoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            nuevoToolStripMenuItem.Size = new Size(198, 24);
            nuevoToolStripMenuItem.Text = "Nuevo";
            nuevoToolStripMenuItem.Click += nuevoToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(195, 6);
            // 
            // guardarToolStripMenuItem
            // 
            guardarToolStripMenuItem.Enabled = false;
            guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            guardarToolStripMenuItem.Size = new Size(198, 24);
            guardarToolStripMenuItem.Text = "Guardar";
            guardarToolStripMenuItem.Click += guardarToolStripMenuItem_Click;
            // 
            // guardarComoToolStripMenuItem
            // 
            guardarComoToolStripMenuItem.Enabled = false;
            guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            guardarComoToolStripMenuItem.Size = new Size(198, 24);
            guardarComoToolStripMenuItem.Text = "Guardar como...";
            guardarComoToolStripMenuItem.Click += guardarComoToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(195, 6);
            // 
            // propiedadesToolStripMenuItem
            // 
            propiedadesToolStripMenuItem.Image = Properties.Resources.settings2;
            propiedadesToolStripMenuItem.Name = "propiedadesToolStripMenuItem";
            propiedadesToolStripMenuItem.Size = new Size(198, 24);
            propiedadesToolStripMenuItem.Text = "Propiedades";
            propiedadesToolStripMenuItem.Click += propiedadesToolStripMenuItem_Click;
            // 
            // verToolStripMenuItem
            // 
            verToolStripMenuItem.Enabled = false;
            verToolStripMenuItem.Name = "verToolStripMenuItem";
            verToolStripMenuItem.Size = new Size(42, 24);
            verToolStripMenuItem.Text = "Ver";
            // 
            // editorDePersonalToolStripMenuItem
            // 
            editorDePersonalToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { registroDePersonalToolStripMenuItem, relacionDeHorariosToolStripMenuItem });
            editorDePersonalToolStripMenuItem.Name = "editorDePersonalToolStripMenuItem";
            editorDePersonalToolStripMenuItem.Size = new Size(141, 24);
            editorDePersonalToolStripMenuItem.Text = "Editor de Personal";
            // 
            // registroDePersonalToolStripMenuItem
            // 
            registroDePersonalToolStripMenuItem.Name = "registroDePersonalToolStripMenuItem";
            registroDePersonalToolStripMenuItem.Size = new Size(223, 24);
            registroDePersonalToolStripMenuItem.Text = "Registro de Personal";
            registroDePersonalToolStripMenuItem.Click += registroDePersonalToolStripMenuItem_Click;
            // 
            // relacionDeHorariosToolStripMenuItem
            // 
            relacionDeHorariosToolStripMenuItem.Name = "relacionDeHorariosToolStripMenuItem";
            relacionDeHorariosToolStripMenuItem.Size = new Size(223, 24);
            relacionDeHorariosToolStripMenuItem.Text = "Relacion de Horarios";
            relacionDeHorariosToolStripMenuItem.Click += relacionDeHorariosToolStripMenuItem_Click;
            // 
            // STATUS_BAR
            // 
            STATUS_BAR.ImageScalingSize = new Size(18, 18);
            STATUS_BAR.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, lblStatusText, lblOperationText });
            STATUS_BAR.Location = new Point(0, 640);
            STATUS_BAR.Name = "STATUS_BAR";
            STATUS_BAR.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            STATUS_BAR.Size = new Size(1107, 22);
            STATUS_BAR.TabIndex = 2;
            STATUS_BAR.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(12, 17);
            toolStripStatusLabel1.Text = " ";
            // 
            // lblStatusText
            // 
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new Size(35, 17);
            lblStatusText.Text = "Listo";
            // 
            // lblOperationText
            // 
            lblOperationText.Name = "lblOperationText";
            lblOperationText.Size = new Size(139, 17);
            lblOperationText.Text = "Preparado para iniciar";
            // 
            // flCustomToolStrip1
            // 
            flCustomToolStrip1.BackColor = SystemColors.ActiveCaption;
            flCustomToolStrip1.Font = new Font("Segoe UI", 10F);
            flCustomToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            flCustomToolStrip1.ImageScalingSize = new Size(18, 18);
            flCustomToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStrpBtnNuevo, toolStrpBtnAbrir });
            flCustomToolStrip1.Location = new Point(0, 28);
            flCustomToolStrip1.Name = "flCustomToolStrip1";
            flCustomToolStrip1.Size = new Size(1107, 25);
            flCustomToolStrip1.TabIndex = 3;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(13, 22);
            toolStripLabel1.Text = " ";
            // 
            // toolStrpBtnNuevo
            // 
            toolStrpBtnNuevo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtnNuevo.Image = Properties.Resources.nuevo_documento;
            toolStrpBtnNuevo.ImageTransparentColor = Color.Magenta;
            toolStrpBtnNuevo.Name = "toolStrpBtnNuevo";
            toolStrpBtnNuevo.Size = new Size(25, 22);
            toolStrpBtnNuevo.Text = "Nuevo";
            toolStrpBtnNuevo.Click += toolStrpBtnNuevo_Click;
            // 
            // toolStrpBtnAbrir
            // 
            toolStrpBtnAbrir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtnAbrir.Enabled = false;
            toolStrpBtnAbrir.Image = Properties.Resources.open;
            toolStrpBtnAbrir.ImageTransparentColor = Color.Magenta;
            toolStrpBtnAbrir.Name = "toolStrpBtnAbrir";
            toolStrpBtnAbrir.Size = new Size(25, 22);
            toolStrpBtnAbrir.Text = "Abrir";
            toolStrpBtnAbrir.Click += toolStrpBtnAbrir_Click;
            // 
            // splitContainer_ProyectosVisualizacion
            // 
            splitContainer_ProyectosVisualizacion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer_ProyectosVisualizacion.FixedPanel = FixedPanel.Panel1;
            splitContainer_ProyectosVisualizacion.Location = new Point(12, 62);
            splitContainer_ProyectosVisualizacion.Name = "splitContainer_ProyectosVisualizacion";
            // 
            // splitContainer_ProyectosVisualizacion.Panel1
            // 
            splitContainer_ProyectosVisualizacion.Panel1.Controls.Add(treeViewProyectosQuincenas);
            // 
            // splitContainer_ProyectosVisualizacion.Panel2
            // 
            splitContainer_ProyectosVisualizacion.Panel2.Controls.Add(MDI_PANEL);
            splitContainer_ProyectosVisualizacion.Size = new Size(1083, 568);
            splitContainer_ProyectosVisualizacion.SplitterDistance = 221;
            splitContainer_ProyectosVisualizacion.SplitterWidth = 6;
            splitContainer_ProyectosVisualizacion.TabIndex = 4;
            // 
            // treeViewProyectosQuincenas
            // 
            treeViewProyectosQuincenas.BackColor = SystemColors.GradientInactiveCaption;
            treeViewProyectosQuincenas.BorderStyle = BorderStyle.FixedSingle;
            treeViewProyectosQuincenas.Dock = DockStyle.Fill;
            treeViewProyectosQuincenas.ImageIndex = 0;
            treeViewProyectosQuincenas.ImageList = imageList1;
            treeViewProyectosQuincenas.Indent = 20;
            treeViewProyectosQuincenas.ItemHeight = 34;
            treeViewProyectosQuincenas.Location = new Point(0, 0);
            treeViewProyectosQuincenas.Name = "treeViewProyectosQuincenas";
            treeViewProyectosQuincenas.ScrollbarBackColor = SystemColors.Control;
            treeViewProyectosQuincenas.SelectedImageKey = "solicitud-32.png";
            treeViewProyectosQuincenas.ShowRootLines = false;
            treeViewProyectosQuincenas.Size = new Size(221, 568);
            treeViewProyectosQuincenas.TabIndex = 0;
            treeViewProyectosQuincenas.DoubleClick += treeViewProyectosQuincenas_DoubleClick;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "solicitud-32.png");
            // 
            // MDI_PANEL
            // 
            MDI_PANEL.BackColor = SystemColors.GradientInactiveCaption;
            MDI_PANEL.BorderStyle = BorderStyle.FixedSingle;
            MDI_PANEL.Controls.Add(lblBienvenido);
            MDI_PANEL.Dock = DockStyle.Fill;
            MDI_PANEL.Location = new Point(0, 0);
            MDI_PANEL.Name = "MDI_PANEL";
            MDI_PANEL.Size = new Size(856, 568);
            MDI_PANEL.TabIndex = 0;
            MDI_PANEL.ControlAdded += MDI_PANEL_ControlAdded;
            MDI_PANEL.ControlRemoved += MDI_PANEL_ControlRemoved;
            // 
            // lblBienvenido
            // 
            lblBienvenido.Dock = DockStyle.Fill;
            lblBienvenido.Font = new Font("Segoe UI", 20F);
            lblBienvenido.Location = new Point(0, 0);
            lblBienvenido.Name = "lblBienvenido";
            lblBienvenido.Size = new Size(854, 566);
            lblBienvenido.TabIndex = 0;
            lblBienvenido.Text = "Bienvenido";
            lblBienvenido.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainDesktop
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(1107, 662);
            Controls.Add(splitContainer_ProyectosVisualizacion);
            Controls.Add(flCustomToolStrip1);
            Controls.Add(STATUS_BAR);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 10F);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "MainDesktop";
            Text = "Checador FXE";
            Load += MainDesktop_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            STATUS_BAR.ResumeLayout(false);
            STATUS_BAR.PerformLayout();
            flCustomToolStrip1.ResumeLayout(false);
            flCustomToolStrip1.PerformLayout();
            splitContainer_ProyectosVisualizacion.Panel1.ResumeLayout(false);
            splitContainer_ProyectosVisualizacion.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer_ProyectosVisualizacion).EndInit();
            splitContainer_ProyectosVisualizacion.ResumeLayout(false);
            MDI_PANEL.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem abrirToolStripMenuItem;
        private ToolStripMenuItem nuevoToolStripMenuItem;
        private ToolStripMenuItem verToolStripMenuItem;
        private StatusStrip STATUS_BAR;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lblStatusText;
        private FlowControls.flCustomToolStrip flCustomToolStrip1;
        private ToolStripButton toolStrpBtnNuevo;
        private ToolStripButton toolStrpBtnAbrir;
        private SplitContainer splitContainer_ProyectosVisualizacion;
        private Panel MDI_PANEL;
        private Label lblBienvenido;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem propiedadesToolStripMenuItem;
        private ToolStripStatusLabel lblOperationText;
        public FlowControls.flTreeView treeViewProyectosQuincenas;
        private ImageList imageList1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripMenuItem editorDePersonalToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        public ToolStripMenuItem guardarToolStripMenuItem;
        public ToolStripMenuItem guardarComoToolStripMenuItem;
        private ToolStripMenuItem registroDePersonalToolStripMenuItem;
        private ToolStripMenuItem relacionDeHorariosToolStripMenuItem;
    }
}
