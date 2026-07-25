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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            flCustomToolStripSeparator2 = new FlowControls.flCustomToolStripSeparator();
            cboxLocalidadSeleccionada = new ToolStripComboBox();
            btnEstablecerSeleccion = new ToolStripButton();
            flCustomToolStripSeparator1 = new FlowControls.flCustomToolStripSeparator();
            lblLocalidadDefaultActualmente = new ToolStripLabel();
            toolStripLabel2 = new ToolStripLabel();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            exportarParaConfiguracionToolStripMenuItem = new ToolStripMenuItem();
            dgvAjustesEmpleados = new FlowControls.flExtendedDataGridView();
            colImageIcon = new DataGridViewTextBoxColumn();
            colNumEmpleado = new DataGridViewTextBoxColumn();
            colNombres = new DataGridViewTextBoxColumn();
            colApellidos = new DataGridViewTextBoxColumn();
            colPuesto = new DataGridViewTextBoxColumn();
            colRegion = new DataGridViewTextBoxColumn();
            colDivision = new DataGridViewTextBoxColumn();
            colLocalidad = new DataGridViewTextBoxColumn();
            colTurnoDefault = new DataGridViewTextBoxColumn();
            imageList1 = new ImageList(components);
            flCustomToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAjustesEmpleados).BeginInit();
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
            flCustomToolStrip1.Size = new Size(852, 30);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 27);
            toolStripButton1.Text = "Guardar cambios";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Enabled = false;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(23, 27);
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
            cboxLocalidadSeleccionada.Click += cboxLocalidadSeleccionada_Click;
            // 
            // btnEstablecerSeleccion
            // 
            btnEstablecerSeleccion.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnEstablecerSeleccion.Image = (Image)resources.GetObject("btnEstablecerSeleccion.Image");
            btnEstablecerSeleccion.ImageTransparentColor = Color.Magenta;
            btnEstablecerSeleccion.Name = "btnEstablecerSeleccion";
            btnEstablecerSeleccion.Size = new Size(23, 27);
            btnEstablecerSeleccion.Text = "Establecer como default";
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
            lblLocalidadDefaultActualmente.Size = new Size(93, 27);
            lblLocalidadDefaultActualmente.Text = "%LOCALIDAD%";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(102, 27);
            toolStripLabel2.Text = "Localidad Default:";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { exportarParaConfiguracionToolStripMenuItem });
            toolStripDropDownButton1.Image = Properties.Resources.toolbox;
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(31, 27);
            toolStripDropDownButton1.Text = "Herramientas";
            // 
            // exportarParaConfiguracionToolStripMenuItem
            // 
            exportarParaConfiguracionToolStripMenuItem.Image = Properties.Resources.exportar_16;
            exportarParaConfiguracionToolStripMenuItem.Name = "exportarParaConfiguracionToolStripMenuItem";
            exportarParaConfiguracionToolStripMenuItem.Size = new Size(229, 22);
            exportarParaConfiguracionToolStripMenuItem.Text = "Exportar para configuracion...";
            exportarParaConfiguracionToolStripMenuItem.Click += exportarParaConfiguracionToolStripMenuItem_Click;
            // 
            // dgvAjustesEmpleados
            // 
            dgvAjustesEmpleados.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle1.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ActiveCaptionText;
            dgvAjustesEmpleados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAjustesEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAjustesEmpleados.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvAjustesEmpleados.BorderStyle = BorderStyle.None;
            dgvAjustesEmpleados.ButtonAddEnabled = true;
            dgvAjustesEmpleados.ButtonEditEnabled = false;
            dgvAjustesEmpleados.ButtonRemoveEnabled = true;
            dgvAjustesEmpleados.ButtonViewEnabled = false;
            dgvAjustesEmpleados.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ButtonFace;
            dataGridViewCellStyle2.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAjustesEmpleados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAjustesEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAjustesEmpleados.Columns.AddRange(new DataGridViewColumn[] { colImageIcon, colNumEmpleado, colNombres, colApellidos, colPuesto, colRegion, colDivision, colLocalidad, colTurnoDefault });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle3.Padding = new Padding(3, 0, 3, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAjustesEmpleados.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAjustesEmpleados.Dock = DockStyle.Fill;
            dgvAjustesEmpleados.EnableHeadersVisualStyles = false;
            dgvAjustesEmpleados.ExtraReservedBottomHeight = 0;
            dgvAjustesEmpleados.GridColor = Color.FromArgb(210, 210, 210);
            dgvAjustesEmpleados.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvAjustesEmpleados.Location = new Point(0, 30);
            dgvAjustesEmpleados.Margin = new Padding(0);
            dgvAjustesEmpleados.MultiSelect = false;
            dgvAjustesEmpleados.Name = "dgvAjustesEmpleados";
            dgvAjustesEmpleados.RowHeadersVisible = false;
            dgvAjustesEmpleados.RowHeadersWidth = 45;
            dataGridViewCellStyle4.SelectionBackColor = Color.Orange;
            dgvAjustesEmpleados.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvAjustesEmpleados.ScrollBars = ScrollBars.Vertical;
            dgvAjustesEmpleados.SelectedCellColor = Color.Moccasin;
            dgvAjustesEmpleados.SelectedRowColor = Color.SteelBlue;
            dgvAjustesEmpleados.SelectionForeColor = Color.Black;
            dgvAjustesEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAjustesEmpleados.ShowContextMenu = true;
            dgvAjustesEmpleados.Size = new Size(852, 362);
            dgvAjustesEmpleados.TabIndex = 4;
            dgvAjustesEmpleados.OnAddClick += dgvAjustesHorarios_OnAddClick;
            dgvAjustesEmpleados.OnRemoveClick += dgvAjustesEmpleados_OnRemoveClick;
            dgvAjustesEmpleados.CellEndEdit += dgvAjustesEmpleados_CellEndEdit;
            dgvAjustesEmpleados.CellEnter += dgvAjustesEmpleados_CellEnter;
            dgvAjustesEmpleados.CellValidating += dgvAjustesEmpleados_CellValidating;
            dgvAjustesEmpleados.RowValidating += dgvAjustesHorarios_RowValidating;
            dgvAjustesEmpleados.SelectionChanged += dgvAjustesHorarios_SelectionChanged;
            // 
            // colImageIcon
            // 
            colImageIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colImageIcon.FillWeight = 64F;
            colImageIcon.HeaderText = "";
            colImageIcon.MinimumWidth = 6;
            colImageIcon.Name = "colImageIcon";
            colImageIcon.Resizable = DataGridViewTriState.False;
            colImageIcon.Width = 24;
            // 
            // colNumEmpleado
            // 
            colNumEmpleado.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colNumEmpleado.FillWeight = 40F;
            colNumEmpleado.HeaderText = "No. Emp.";
            colNumEmpleado.MinimumWidth = 6;
            colNumEmpleado.Name = "colNumEmpleado";
            colNumEmpleado.Width = 94;
            // 
            // colNombres
            // 
            colNombres.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colNombres.FillWeight = 80F;
            colNombres.HeaderText = "Nombres";
            colNombres.MinimumWidth = 6;
            colNombres.Name = "colNombres";
            colNombres.Width = 95;
            // 
            // colApellidos
            // 
            colApellidos.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colApellidos.FillWeight = 26.81313F;
            colApellidos.HeaderText = "Apellidos";
            colApellidos.MinimumWidth = 6;
            colApellidos.Name = "colApellidos";
            colApellidos.Width = 94;
            // 
            // colPuesto
            // 
            colPuesto.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colPuesto.FillWeight = 26.81313F;
            colPuesto.HeaderText = "Puesto";
            colPuesto.MinimumWidth = 6;
            colPuesto.Name = "colPuesto";
            colPuesto.Width = 81;
            // 
            // colRegion
            // 
            colRegion.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colRegion.FillWeight = 26.81313F;
            colRegion.HeaderText = "Region";
            colRegion.MinimumWidth = 6;
            colRegion.Name = "colRegion";
            colRegion.Width = 81;
            // 
            // colDivision
            // 
            colDivision.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colDivision.FillWeight = 26.81313F;
            colDivision.HeaderText = "Division";
            colDivision.MinimumWidth = 6;
            colDivision.Name = "colDivision";
            colDivision.Width = 87;
            // 
            // colLocalidad
            // 
            colLocalidad.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colLocalidad.FillWeight = 26.81313F;
            colLocalidad.HeaderText = "Localidad";
            colLocalidad.MinimumWidth = 6;
            colLocalidad.Name = "colLocalidad";
            colLocalidad.Width = 96;
            // 
            // colTurnoDefault
            // 
            colTurnoDefault.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colTurnoDefault.HeaderText = "Turno Def.";
            colTurnoDefault.MinimumWidth = 6;
            colTurnoDefault.Name = "colTurnoDefault";
            colTurnoDefault.Width = 103;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "solicitud-32.png");
            // 
            // frmCrudEmpleados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(852, 392);
            Controls.Add(dgvAjustesEmpleados);
            Controls.Add(flCustomToolStrip1);
            MinimumSize = new Size(675, 333);
            Name = "frmCrudEmpleados";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CRUD de Empleados";
            Load += frmCrudEmpleados_Load;
            flCustomToolStrip1.ResumeLayout(false);
            flCustomToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAjustesEmpleados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowControls.flCustomToolStrip flCustomToolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton btnEstablecerSeleccion;
        private FlowControls.flExtendedDataGridView dgvAjustesEmpleados;
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
        private DataGridViewTextBoxColumn colTurnoDefault;
    }
}