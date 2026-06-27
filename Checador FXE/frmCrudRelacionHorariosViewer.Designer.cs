namespace Checador_FXE
{
    partial class frmCrudRelacionHorariosViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCrudRelacionHorariosViewer));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStrpBtn_Guardar = new ToolStripButton();
            flCustomToolStripSeparator1 = new FlowControls.flCustomToolStripSeparator();
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
            flCustomToolStrip1.Items.AddRange(new ToolStripItem[] { toolStrpBtn_Guardar, flCustomToolStripSeparator1, toolStripLabel1, cboxParametroLimitacion, txtValorLimitacion, btnLimitarAmbito });
            flCustomToolStrip1.Location = new Point(0, 0);
            flCustomToolStrip1.Name = "flCustomToolStrip1";
            flCustomToolStrip1.Size = new Size(897, 25);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStrpBtn_Guardar
            // 
            toolStrpBtn_Guardar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtn_Guardar.Image = (Image)resources.GetObject("toolStrpBtn_Guardar.Image");
            toolStrpBtn_Guardar.ImageTransparentColor = Color.Magenta;
            toolStrpBtn_Guardar.Name = "toolStrpBtn_Guardar";
            toolStrpBtn_Guardar.Size = new Size(25, 22);
            toolStrpBtn_Guardar.Text = "Guardar cambios";
            toolStrpBtn_Guardar.Click += toolStripButton1_Click;
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
            dataGridViewCellStyle1.BackColor = SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle1.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ActiveCaptionText;
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvRelacionDeHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRelacionDeHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelacionDeHorarios.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvRelacionDeHorarios.BorderStyle = BorderStyle.None;
            dgvRelacionDeHorarios.ButtonAddEnabled = true;
            dgvRelacionDeHorarios.ButtonEditEnabled = false;
            dgvRelacionDeHorarios.ButtonRemoveEnabled = false;
            dgvRelacionDeHorarios.ButtonViewEnabled = false;
            dgvRelacionDeHorarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ButtonFace;
            dataGridViewCellStyle2.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvRelacionDeHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle3.Padding = new Padding(3, 0, 3, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvRelacionDeHorarios.DefaultCellStyle = dataGridViewCellStyle3;
            dgvRelacionDeHorarios.EnableHeadersVisualStyles = false;
            dgvRelacionDeHorarios.GridColor = Color.FromArgb(210, 210, 210);
            dgvRelacionDeHorarios.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvRelacionDeHorarios.Location = new Point(0, 25);
            dgvRelacionDeHorarios.Margin = new Padding(0);
            dgvRelacionDeHorarios.MultiSelect = false;
            dgvRelacionDeHorarios.Name = "dgvRelacionDeHorarios";
            dgvRelacionDeHorarios.RowHeadersVisible = false;
            dgvRelacionDeHorarios.RowHeadersWidth = 45;
            dataGridViewCellStyle4.SelectionBackColor = Color.Orange;
            dgvRelacionDeHorarios.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvRelacionDeHorarios.SelectedCellColor = Color.Moccasin;
            dgvRelacionDeHorarios.SelectedRowColor = Color.SteelBlue;
            dgvRelacionDeHorarios.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelacionDeHorarios.ShowContextMenu = true;
            dgvRelacionDeHorarios.Size = new Size(897, 362);
            dgvRelacionDeHorarios.TabIndex = 4;
            dgvRelacionDeHorarios.OnAddClick += dgvAjustesHorarios_OnAddClick;
            dgvRelacionDeHorarios.CellEnter += dgvAjustesEmpleados_CellEnter;
            dgvRelacionDeHorarios.CellValidating += dgvAjustesEmpleados_CellValidating;
            dgvRelacionDeHorarios.RowValidating += dgvAjustesHorarios_RowValidating;
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
            // frmCrudRelacionHorariosViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(897, 409);
            Controls.Add(statusStrip1);
            Controls.Add(dgvRelacionDeHorarios);
            Controls.Add(flCustomToolStrip1);
            MinimumSize = new Size(675, 372);
            Name = "frmCrudRelacionHorariosViewer";
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
        private ImageList imageList1;
        private FlowControls.flExtendedDataGridView dgvRelacionDeHorarios;
        private FlowControls.flCustomToolStripSeparator flCustomToolStripSeparator1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lblMessage;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox cboxParametroLimitacion;
        private ToolStripTextBox txtValorLimitacion;
        private ToolStripButton btnLimitarAmbito;
    }
}