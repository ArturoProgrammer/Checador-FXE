namespace Checador_FXE
{
    partial class frmCrudRelacionHorarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCrudRelacionHorarios));
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStrpBtn_Guardar = new ToolStripButton();
            toolStrpBtn_RevertirCambios = new ToolStripButton();
            flCustomToolStripSeparator1 = new FlowControls.flCustomToolStripSeparator();
            cboxMonth = new ToolStripComboBox();
            txtYear = new ToolStripTextBox();
            btnIrAMes = new ToolStripButton();
            flCustomToolStripSeparator2 = new FlowControls.flCustomToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            cboxParametroLimitacion = new ToolStripComboBox();
            txtValorLimitacion = new ToolStripTextBox();
            btnLimitarAmbito = new ToolStripButton();
            imageList1 = new ImageList(components);
            dgvRelacionDeHorarios = new FlowControls.flExtendedDataGridView();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lblMessage = new ToolStripStatusLabel();
            toolStrpBtn_NuevoTurno = new ToolStripButton();
            flCustomToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRelacionDeHorarios).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // flCustomToolStrip1
            // 
            flCustomToolStrip1.BackColor = SystemColors.ActiveCaption;
            flCustomToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            flCustomToolStrip1.ImageScalingSize = new Size(18, 18);
            flCustomToolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStrpBtn_Guardar, toolStrpBtn_RevertirCambios, toolStrpBtn_NuevoTurno, flCustomToolStripSeparator1, cboxMonth, txtYear, btnIrAMes, flCustomToolStripSeparator2, toolStripLabel1, cboxParametroLimitacion, txtValorLimitacion, btnLimitarAmbito });
            flCustomToolStrip1.Location = new Point(0, 0);
            flCustomToolStrip1.Name = "flCustomToolStrip1";
            flCustomToolStrip1.Size = new Size(897, 25);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStrpBtn_Guardar
            // 
            this.toolStrpBtn_Guardar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStrpBtn_Guardar.Image = (Image)resources.GetObject("toolStrpBtn_Guardar.Image");
            this.toolStrpBtn_Guardar.ImageTransparentColor = Color.Magenta;
            this.toolStrpBtn_Guardar.Name = "toolStrpBtn_Guardar";
            this.toolStrpBtn_Guardar.Size = new Size(25, 22);
            this.toolStrpBtn_Guardar.Text = "Guardar cambios";
            this.toolStrpBtn_Guardar.Click += this.toolStripButton1_Click;
            // 
            // toolStrpBtn_RevertirCambios
            // 
            toolStrpBtn_RevertirCambios.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtn_RevertirCambios.Enabled = false;
            toolStrpBtn_RevertirCambios.Image = (Image)resources.GetObject("toolStrpBtn_RevertirCambios.Image");
            toolStrpBtn_RevertirCambios.ImageTransparentColor = Color.Magenta;
            toolStrpBtn_RevertirCambios.Name = "toolStrpBtn_RevertirCambios";
            toolStrpBtn_RevertirCambios.Size = new Size(25, 22);
            toolStrpBtn_RevertirCambios.Text = "Revertir cambios";
            toolStrpBtn_RevertirCambios.Click += toolStripButton2_Click;
            // 
            // flCustomToolStripSeparator1
            // 
            flCustomToolStripSeparator1.AutoSize = false;
            flCustomToolStripSeparator1.LineColor = Color.DimGray;
            flCustomToolStripSeparator1.LineMargin = 1;
            flCustomToolStripSeparator1.LineThickness = 2;
            flCustomToolStripSeparator1.Name = "flCustomToolStripSeparator1";
            flCustomToolStripSeparator1.SeparatorHeight = 24;
            flCustomToolStripSeparator1.Size = new Size(6, 24);
            // 
            // cboxMonth
            // 
            cboxMonth.BackColor = SystemColors.GradientActiveCaption;
            cboxMonth.Items.AddRange(new object[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" });
            cboxMonth.Name = "cboxMonth";
            cboxMonth.Size = new Size(121, 25);
            // 
            // txtYear
            // 
            txtYear.BackColor = SystemColors.GradientActiveCaption;
            txtYear.BorderStyle = BorderStyle.FixedSingle;
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(40, 25);
            txtYear.TextBoxTextAlign = HorizontalAlignment.Center;
            txtYear.KeyDown += txtYear_KeyDown;
            // 
            // btnIrAMes
            // 
            btnIrAMes.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnIrAMes.Image = Properties.Resources.play3_16;
            btnIrAMes.ImageTransparentColor = Color.Magenta;
            btnIrAMes.Name = "btnIrAMes";
            btnIrAMes.Size = new Size(25, 22);
            btnIrAMes.Text = "Ir a mes indicado";
            btnIrAMes.Click += btnIrAMes_Click;
            // 
            // flCustomToolStripSeparator2
            // 
            flCustomToolStripSeparator2.AutoSize = false;
            flCustomToolStripSeparator2.LineColor = Color.DimGray;
            flCustomToolStripSeparator2.LineMargin = 1;
            flCustomToolStripSeparator2.LineThickness = 2;
            flCustomToolStripSeparator2.Name = "flCustomToolStripSeparator2";
            flCustomToolStripSeparator2.SeparatorHeight = 24;
            flCustomToolStripSeparator2.Size = new Size(6, 24);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(67, 22);
            toolStripLabel1.Text = "Limitar a...";
            // 
            // cboxParametroLimitacion
            // 
            cboxParametroLimitacion.BackColor = SystemColors.GradientActiveCaption;
            cboxParametroLimitacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxParametroLimitacion.Name = "cboxParametroLimitacion";
            cboxParametroLimitacion.Size = new Size(121, 25);
            cboxParametroLimitacion.SelectedIndexChanged += cboxParametroLimitacion_SelectedIndexChanged;
            // 
            // txtValorLimitacion
            // 
            txtValorLimitacion.BackColor = SystemColors.GradientActiveCaption;
            txtValorLimitacion.BorderStyle = BorderStyle.FixedSingle;
            txtValorLimitacion.Name = "txtValorLimitacion";
            txtValorLimitacion.Size = new Size(160, 25);
            txtValorLimitacion.KeyDown += txtValorLimitacion_KeyDown;
            // 
            // btnLimitarAmbito
            // 
            btnLimitarAmbito.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnLimitarAmbito.Image = Properties.Resources.filter_24;
            btnLimitarAmbito.ImageTransparentColor = Color.Magenta;
            btnLimitarAmbito.Name = "btnLimitarAmbito";
            btnLimitarAmbito.Size = new Size(25, 22);
            btnLimitarAmbito.Text = "Limitar ambito";
            btnLimitarAmbito.Click += toolStripButton3_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "solicitud-32.png");
            // 
            // dgvRelacionDeHorarios
            // 
            dgvRelacionDeHorarios.AllowUserToAddRows = false;
            dataGridViewCellStyle9.BackColor = SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle9.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.ActiveCaptionText;
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            dgvRelacionDeHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRelacionDeHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelacionDeHorarios.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvRelacionDeHorarios.BorderStyle = BorderStyle.None;
            dgvRelacionDeHorarios.ButtonAddEnabled = true;
            dgvRelacionDeHorarios.ButtonEditEnabled = false;
            dgvRelacionDeHorarios.ButtonRemoveEnabled = false;
            dgvRelacionDeHorarios.ButtonViewEnabled = false;
            dgvRelacionDeHorarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.SteelBlue;
            dataGridViewCellStyle10.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle10.ForeColor = SystemColors.ButtonFace;
            dataGridViewCellStyle10.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dgvRelacionDeHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = Color.White;
            dataGridViewCellStyle11.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle11.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle11.Padding = new Padding(3, 0, 3, 0);
            dataGridViewCellStyle11.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle11.SelectionForeColor = Color.Black;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            dgvRelacionDeHorarios.DefaultCellStyle = dataGridViewCellStyle11;
            dgvRelacionDeHorarios.EnableHeadersVisualStyles = false;
            dgvRelacionDeHorarios.GridColor = Color.FromArgb(210, 210, 210);
            dgvRelacionDeHorarios.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvRelacionDeHorarios.Location = new Point(0, 25);
            dgvRelacionDeHorarios.Margin = new Padding(0);
            dgvRelacionDeHorarios.MultiSelect = false;
            dgvRelacionDeHorarios.Name = "dgvRelacionDeHorarios";
            dgvRelacionDeHorarios.RowHeadersVisible = false;
            dgvRelacionDeHorarios.RowHeadersWidth = 45;
            dataGridViewCellStyle12.SelectionBackColor = Color.Orange;
            dgvRelacionDeHorarios.RowsDefaultCellStyle = dataGridViewCellStyle12;
            dgvRelacionDeHorarios.SelectedCellColor = Color.Moccasin;
            dgvRelacionDeHorarios.SelectedRowColor = Color.SteelBlue;
            dgvRelacionDeHorarios.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelacionDeHorarios.ShowContextMenu = true;
            dgvRelacionDeHorarios.Size = new Size(897, 362);
            dgvRelacionDeHorarios.TabIndex = 4;
            dgvRelacionDeHorarios.CellEnter += dgvAjustesEmpleados_CellEnter;
            dgvRelacionDeHorarios.Grid.CellValidating += dgvAjustesEmpleados_CellValidating;
            dgvRelacionDeHorarios.Grid.RowValidating += dgvAjustesHorarios_RowValidating;
            dgvRelacionDeHorarios.SelectionChanged += dgvAjustesHorarios_SelectionChanged;
            dgvRelacionDeHorarios.KeyDown += dgvRelacionDeHorarios_KeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ActiveCaption;
            statusStrip1.ImageScalingSize = new Size(18, 18);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus, toolStripStatusLabel1, lblMessage });
            statusStrip1.Location = new Point(0, 387);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(897, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(65, 17);
            lblStatus.Text = "%Status%";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(12, 17);
            toolStripStatusLabel1.Text = " ";
            // 
            // lblMessage
            // 
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(83, 17);
            lblMessage.Text = "%Message%";
            // 
            // toolStrpBtn_NuevoTurno
            // 
            toolStrpBtn_NuevoTurno.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtn_NuevoTurno.Image = (Image)resources.GetObject("toolStrpBtn_NuevoTurno.Image");
            toolStrpBtn_NuevoTurno.ImageTransparentColor = Color.Magenta;
            toolStrpBtn_NuevoTurno.Name = "toolStrpBtn_NuevoTurno";
            toolStrpBtn_NuevoTurno.Size = new Size(25, 22);
            toolStrpBtn_NuevoTurno.Text = "toolStripButton3";
            toolStrpBtn_NuevoTurno.Click += toolStrpBtn_NuevoTurno_Click;
            // 
            // frmCrudRelacionHorarios
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(897, 409);
            Controls.Add(statusStrip1);
            Controls.Add(dgvRelacionDeHorarios);
            Controls.Add(flCustomToolStrip1);
            MinimumSize = new Size(675, 372);
            Name = "frmCrudRelacionHorarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Relacion de Horarios";
            Load += frmCrudRelacionHorarios_Load;
            flCustomToolStrip1.ResumeLayout(false);
            flCustomToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRelacionDeHorarios).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowControls.flCustomToolStrip flCustomToolStrip1;
        private ToolStripButton toolStrpBtn_Guardar;
        private ToolStripButton toolStrpBtn_RevertirCambios;
        private ImageList imageList1;
        private FlowControls.flExtendedDataGridView dgvRelacionDeHorarios;
        private FlowControls.flCustomToolStripSeparator flCustomToolStripSeparator1;
        private ToolStripComboBox cboxMonth;
        private ToolStripTextBox txtYear;
        private ToolStripButton btnIrAMes;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lblMessage;
        private FlowControls.flCustomToolStripSeparator flCustomToolStripSeparator2;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox cboxParametroLimitacion;
        private ToolStripTextBox txtValorLimitacion;
        private ToolStripButton btnLimitarAmbito;
        private ToolStripButton toolStrpBtn_NuevoTurno;
    }
}