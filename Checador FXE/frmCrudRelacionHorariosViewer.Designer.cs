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
            flCustomToolStrip1.Size = new Size(823, 25);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStrpBtn_Guardar
            // 
            toolStrpBtn_Guardar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStrpBtn_Guardar.Image = (Image)resources.GetObject("toolStrpBtn_Guardar.Image");
            toolStrpBtn_Guardar.ImageTransparentColor = Color.Magenta;
            toolStrpBtn_Guardar.Name = "toolStrpBtn_Guardar";
            toolStrpBtn_Guardar.Size = new Size(23, 22);
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
            toolStripLabel1.Size = new Size(62, 22);
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
            btnLimitarAmbito.Size = new Size(23, 22);
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
            dgvRelacionDeHorarios.AllowUserToDeleteRows = true;
            dgvRelacionDeHorarios.AllowUserToOrderColumns = false;
            dgvRelacionDeHorarios.AllowUserToResizeColumns = true;
            dgvRelacionDeHorarios.AllowUserToResizeRows = true;
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.GradientInactiveCaption;
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dgvRelacionDeHorarios.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRelacionDeHorarios.AutoGenerateColumns = true;
            dgvRelacionDeHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelacionDeHorarios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvRelacionDeHorarios.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvRelacionDeHorarios.ButtonAddEnabled = true;
            dgvRelacionDeHorarios.ButtonEditEnabled = false;
            dgvRelacionDeHorarios.ButtonRemoveEnabled = false;
            dgvRelacionDeHorarios.ButtonViewEnabled = false;
            dgvRelacionDeHorarios.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvRelacionDeHorarios.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            dgvRelacionDeHorarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ButtonFace;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Orange;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvRelacionDeHorarios.ColumnHeadersHeight = 4;
            dgvRelacionDeHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRelacionDeHorarios.CurrentCell = null;
            dgvRelacionDeHorarios.DataMember = "";
            dgvRelacionDeHorarios.DataSource = null;
            dgvRelacionDeHorarios.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRelacionDeHorarios.DefaultCellStyle.BackColor = Color.White;
            dgvRelacionDeHorarios.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvRelacionDeHorarios.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvRelacionDeHorarios.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            dgvRelacionDeHorarios.DefaultCellStyle.SelectionBackColor = Color.Orange;
            dgvRelacionDeHorarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvRelacionDeHorarios.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvRelacionDeHorarios.EnableHeadersVisualStyles = false;
            dgvRelacionDeHorarios.EnterKeyPressDefaultAction = FlowControls.EnterKeyAction.Default;
            dgvRelacionDeHorarios.ExtraReservedBottomHeight = 0;
            dgvRelacionDeHorarios.GridColor = Color.FromArgb(210, 210, 210);
            dgvRelacionDeHorarios.HoverRowBackColor = Color.AntiqueWhite;
            dgvRelacionDeHorarios.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvRelacionDeHorarios.Location = new Point(0, 22);
            dgvRelacionDeHorarios.Margin = new Padding(0);
            dgvRelacionDeHorarios.MouseHoverEffectEnabled = false;
            dgvRelacionDeHorarios.MultiSelect = false;
            dgvRelacionDeHorarios.Name = "dgvRelacionDeHorarios";
            dgvRelacionDeHorarios.ReadOnly = false;
            dgvRelacionDeHorarios.RowHeadersVisible = false;
            dgvRelacionDeHorarios.RowHeadersWidth = 45;
            dgvRelacionDeHorarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgvRelacionDeHorarios.RowsDefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dgvRelacionDeHorarios.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.SelectedCellColor = Color.Moccasin;
            dgvRelacionDeHorarios.SelectedRowColor = Color.SteelBlue;
            dgvRelacionDeHorarios.SelectionForeColor = Color.Black;
            dgvRelacionDeHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRelacionDeHorarios.ShowContextMenu = true;
            dgvRelacionDeHorarios.Size = new Size(823, 380);
            dgvRelacionDeHorarios.TabIndex = 4;
            dgvRelacionDeHorarios.OnAddClick += dgvAjustesHorarios_OnAddClick;
            dgvRelacionDeHorarios.RowValidating += dgvAjustesHorarios_RowValidating;
            dgvRelacionDeHorarios.CellValidating += dgvAjustesEmpleados_CellValidating;
            dgvRelacionDeHorarios.CellEnter += dgvAjustesEmpleados_CellEnter;
            dgvRelacionDeHorarios.SelectionChanged += dgvAjustesHorarios_SelectionChanged;
            dgvRelacionDeHorarios.KeyDown += dgvRelacionDeHorarios_KeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ActiveCaption;
            statusStrip1.ImageScalingSize = new Size(18, 18);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus, toolStripStatusLabel1, lblMessage });
            statusStrip1.Location = new Point(0, 402);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(823, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(59, 17);
            lblStatus.Text = "%Status%";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(10, 17);
            toolStripStatusLabel1.Text = " ";
            // 
            // lblMessage
            // 
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(73, 17);
            lblMessage.Text = "%Message%";
            // 
            // frmCrudRelacionHorariosViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(823, 424);
            Controls.Add(statusStrip1);
            Controls.Add(dgvRelacionDeHorarios);
            Controls.Add(flCustomToolStrip1);
            MinimumSize = new Size(675, 333);
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