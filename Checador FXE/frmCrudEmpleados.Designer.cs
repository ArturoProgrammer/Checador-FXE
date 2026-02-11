namespace Checador_FXE
{
    partial class frmCrudEmpleados
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCrudEmpleados));
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            flCustomToolStripSeparator2 = new FlowControls.flCustomToolStripSeparator();
            cboxLocalidadSeleccionada = new ToolStripComboBox();
            btnEstablecerSeleccion = new ToolStripButton();
            flCustomToolStripSeparator1 = new FlowControls.flCustomToolStripSeparator();
            lblLocalidadDefaultActualmente = new ToolStripLabel();
            toolStripLabel2 = new ToolStripLabel();
            dgvAjustesHorarios = new FlowControls.flExtendedDataGridView();
            colImageIcon = new DataGridViewTextBoxColumn();
            colNumEmpleado = new DataGridViewTextBoxColumn();
            colNombres = new DataGridViewTextBoxColumn();
            colApellidos = new DataGridViewTextBoxColumn();
            colPuesto = new DataGridViewTextBoxColumn();
            colRegion = new DataGridViewTextBoxColumn();
            colDivision = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            imageList1 = new ImageList(components);
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            exportarParaConfiguracionToolStripMenuItem = new ToolStripMenuItem();
            flCustomToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAjustesHorarios).BeginInit();
            SuspendLayout();
            // 
            // flCustomToolStrip1
            // 
            flCustomToolStrip1.BackColor = SystemColors.ActiveCaption;
            flCustomToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            flCustomToolStrip1.ImageScalingSize = new Size(18, 18);
            flCustomToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, flCustomToolStripSeparator2, cboxLocalidadSeleccionada, btnEstablecerSeleccion, flCustomToolStripSeparator1, lblLocalidadDefaultActualmente, toolStripLabel2, toolStripDropDownButton1 });
            flCustomToolStrip1.Location = new Point(0, 0);
            flCustomToolStrip1.Name = "flCustomToolStrip1";
            flCustomToolStrip1.Size = new Size(881, 30);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(25, 27);
            toolStripButton1.Text = "Guardar cambios";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(25, 27);
            toolStripButton2.Text = "Revertir cambios";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // flCustomToolStripSeparator2
            // 
            flCustomToolStripSeparator2.AutoSize = false;
            flCustomToolStripSeparator2.LineColor = Color.DimGray;
            flCustomToolStripSeparator2.LineMargin = 1;
            flCustomToolStripSeparator2.Margin = new Padding(3, 0, 0, 0);
            flCustomToolStripSeparator2.Name = "flCustomToolStripSeparator2";
            flCustomToolStripSeparator2.SeparatorHeight = 30;
            flCustomToolStripSeparator2.Size = new Size(6, 30);
            // 
            // cboxLocalidadSeleccionada
            // 
            cboxLocalidadSeleccionada.BackColor = SystemColors.GradientInactiveCaption;
            cboxLocalidadSeleccionada.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxLocalidadSeleccionada.Name = "cboxLocalidadSeleccionada";
            cboxLocalidadSeleccionada.Size = new Size(200, 30);
            cboxLocalidadSeleccionada.SelectedIndexChanged += cboxLocalidadSeleccionada_SelectedIndexChanged;
            // 
            // btnEstablecerSeleccion
            // 
            btnEstablecerSeleccion.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnEstablecerSeleccion.Image = (Image)resources.GetObject("btnEstablecerSeleccion.Image");
            btnEstablecerSeleccion.ImageTransparentColor = Color.Magenta;
            btnEstablecerSeleccion.Name = "btnEstablecerSeleccion";
            btnEstablecerSeleccion.Size = new Size(25, 27);
            btnEstablecerSeleccion.Text = "toolStripButton3";
            btnEstablecerSeleccion.Click += btnEstablecerSeleccion_Click;
            // 
            // flCustomToolStripSeparator1
            // 
            flCustomToolStripSeparator1.AutoSize = false;
            flCustomToolStripSeparator1.LineColor = Color.DimGray;
            flCustomToolStripSeparator1.LineMargin = 1;
            flCustomToolStripSeparator1.Margin = new Padding(3, 0, 0, 0);
            flCustomToolStripSeparator1.Name = "flCustomToolStripSeparator1";
            flCustomToolStripSeparator1.SeparatorHeight = 30;
            flCustomToolStripSeparator1.Size = new Size(6, 30);
            // 
            // lblLocalidadDefaultActualmente
            // 
            lblLocalidadDefaultActualmente.Alignment = ToolStripItemAlignment.Right;
            lblLocalidadDefaultActualmente.Font = new Font("Segoe UI", 8.830189F, FontStyle.Bold);
            lblLocalidadDefaultActualmente.ForeColor = SystemColors.HotTrack;
            lblLocalidadDefaultActualmente.Name = "lblLocalidadDefaultActualmente";
            lblLocalidadDefaultActualmente.Size = new Size(104, 27);
            lblLocalidadDefaultActualmente.Text = "%LOCALIDAD%";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(112, 27);
            toolStripLabel2.Text = "Localidad Default:";
            // 
            // dgvAjustesHorarios
            // 
            dgvAjustesHorarios.AllowUserToAddRows = false;
            dataGridViewCellStyle5.BackColor = SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle5.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.ActiveCaptionText;
            dgvAjustesHorarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvAjustesHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAjustesHorarios.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvAjustesHorarios.BorderStyle = BorderStyle.None;
            dgvAjustesHorarios.ButtonAddEnabled = true;
            dgvAjustesHorarios.ButtonEditEnabled = false;
            dgvAjustesHorarios.ButtonRemoveEnabled = false;
            dgvAjustesHorarios.ButtonViewEnabled = false;
            dgvAjustesHorarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.SteelBlue;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ButtonFace;
            dataGridViewCellStyle6.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvAjustesHorarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvAjustesHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAjustesHorarios.Columns.AddRange(new DataGridViewColumn[] { colImageIcon, colNumEmpleado, colNombres, colApellidos, colPuesto, colRegion, colDivision, colLocalidad });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle7.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle7.Padding = new Padding(3, 0, 3, 0);
            dataGridViewCellStyle7.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvAjustesHorarios.DefaultCellStyle = dataGridViewCellStyle7;
            dgvAjustesHorarios.Dock = DockStyle.Fill;
            dgvAjustesHorarios.EnableHeadersVisualStyles = false;
            dgvAjustesHorarios.GridColor = Color.FromArgb(210, 210, 210);
            dgvAjustesHorarios.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvAjustesHorarios.Location = new Point(0, 30);
            dgvAjustesHorarios.Margin = new Padding(0);
            dgvAjustesHorarios.MultiSelect = false;
            dgvAjustesHorarios.Name = "dgvAjustesHorarios";
            dgvAjustesHorarios.RowHeadersVisible = false;
            dgvAjustesHorarios.RowHeadersWidth = 45;
            dataGridViewCellStyle8.SelectionBackColor = Color.Orange;
            dgvAjustesHorarios.RowsDefaultCellStyle = dataGridViewCellStyle8;
            dgvAjustesHorarios.SelectedCellColor = Color.Moccasin;
            dgvAjustesHorarios.SelectedRowColor = Color.SteelBlue;
            dgvAjustesHorarios.SelectionForeColor = Color.Black;
            dgvAjustesHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAjustesHorarios.Size = new Size(881, 384);
            dgvAjustesHorarios.TabIndex = 4;
            dgvAjustesHorarios.OnAddClick += dgvAjustesHorarios_OnAddClick;
            dgvAjustesHorarios.RowValidating += dgvAjustesHorarios_RowValidating;
            dgvAjustesHorarios.SelectionChanged += dgvAjustesHorarios_SelectionChanged;
            // 
            // colImageIcon
            // 
            colImageIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colImageIcon.FillWeight = 64F;
            colImageIcon.HeaderText = "";
            colImageIcon.MinimumWidth = 6;
            colImageIcon.Name = "colImageIcon";
            colImageIcon.Resizable = DataGridViewTriState.False;
            colImageIcon.Width = 26;
            // 
            // colNumEmpleado
            // 
            colNumEmpleado.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colNumEmpleado.FillWeight = 40F;
            colNumEmpleado.HeaderText = "No. Emp.";
            colNumEmpleado.MinimumWidth = 6;
            colNumEmpleado.Name = "colNumEmpleado";
            colNumEmpleado.Width = 101;
            // 
            // colNombres
            // 
            colNombres.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colNombres.FillWeight = 80F;
            colNombres.HeaderText = "Nombres";
            colNombres.MinimumWidth = 6;
            colNombres.Name = "colNombres";
            colNombres.Width = 102;
            // 
            // colApellidos
            // 
            colApellidos.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colApellidos.FillWeight = 26.81313F;
            colApellidos.HeaderText = "Apellidos";
            colApellidos.MinimumWidth = 6;
            colApellidos.Name = "colApellidos";
            colApellidos.Width = 104;
            // 
            // colPuesto
            // 
            colPuesto.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colPuesto.FillWeight = 26.81313F;
            colPuesto.HeaderText = "Puesto";
            colPuesto.MinimumWidth = 6;
            colPuesto.Name = "colPuesto";
            colPuesto.Width = 85;
            // 
            // colRegion
            // 
            colRegion.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colRegion.FillWeight = 26.81313F;
            colRegion.HeaderText = "Region";
            colRegion.MinimumWidth = 6;
            colRegion.Name = "colRegion";
            colRegion.Width = 88;
            // 
            // colDivision
            // 
            colDivision.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colDivision.FillWeight = 26.81313F;
            colDivision.HeaderText = "Division";
            colDivision.MinimumWidth = 6;
            colDivision.Name = "colDivision";
            colDivision.Width = 94;
            // 
            // colLocalidad
            // 
            colLocalidad.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colLocalidad.FillWeight = 26.81313F;
            colLocalidad.HeaderText = "Localidad";
            colLocalidad.MinimumWidth = 6;
            colLocalidad.Name = "colLocalidad";
            colLocalidad.Width = 106;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "solicitud-32.png");
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { exportarParaConfiguracionToolStripMenuItem });
            toolStripDropDownButton1.Image = Properties.Resources.toolbox;
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(32, 27);
            toolStripDropDownButton1.Text = "Herramientas";
            // 
            // exportarParaConfiguracionToolStripMenuItem
            // 
            exportarParaConfiguracionToolStripMenuItem.Image = Properties.Resources.exportar_16;
            exportarParaConfiguracionToolStripMenuItem.Name = "exportarParaConfiguracionToolStripMenuItem";
            exportarParaConfiguracionToolStripMenuItem.Size = new Size(255, 24);
            exportarParaConfiguracionToolStripMenuItem.Text = "Exportar para configuracion...";
            exportarParaConfiguracionToolStripMenuItem.Click += exportarParaConfiguracionToolStripMenuItem_Click;
            // 
            // frmCrudEmpleados
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 414);
            Controls.Add(dgvAjustesHorarios);
            Controls.Add(flCustomToolStrip1);
            MinimumSize = new Size(675, 372);
            Name = "frmCrudEmpleados";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CRUD de Empleados";
            Load += frmCrudEmpleados_Load;
            flCustomToolStrip1.ResumeLayout(false);
            flCustomToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAjustesHorarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowControls.flCustomToolStrip flCustomToolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton btnEstablecerSeleccion;
        private FlowControls.flExtendedDataGridView dgvAjustesHorarios;
        private FlowControls.flCustomToolStripSeparator flCustomToolStripSeparator2;
        private ToolStripComboBox cboxLocalidadSeleccionada;
        private FlowControls.flCustomToolStripSeparator flCustomToolStripSeparator1;
        private ImageList imageList1;
        private ToolStripLabel lblLocalidadDefaultActualmente;
        private ToolStripLabel toolStripLabel2;
        private DataGridViewTextBoxColumn colImageIcon;
        private DataGridViewTextBoxColumn colNumEmpleado;
        private DataGridViewTextBoxColumn colNombres;
        private DataGridViewTextBoxColumn colApellidos;
        private DataGridViewTextBoxColumn colPuesto;
        private DataGridViewTextBoxColumn colRegion;
        private DataGridViewTextBoxColumn colDivision;
        private DataGridViewTextBoxColumn colLocalidad;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem exportarParaConfiguracionToolStripMenuItem;
    }
}