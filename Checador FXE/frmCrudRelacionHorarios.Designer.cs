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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            flCustomToolStrip1 = new FlowControls.flCustomToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            imageList1 = new ImageList(components);
            dgvRelacionDeHorarios = new FlowControls.flExtendedDataGridView();
            flCustomToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRelacionDeHorarios).BeginInit();
            SuspendLayout();
            // 
            // flCustomToolStrip1
            // 
            flCustomToolStrip1.BackColor = SystemColors.ActiveCaption;
            flCustomToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            flCustomToolStrip1.ImageScalingSize = new Size(18, 18);
            flCustomToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2 });
            flCustomToolStrip1.Location = new Point(0, 0);
            flCustomToolStrip1.Name = "flCustomToolStrip1";
            flCustomToolStrip1.Size = new Size(852, 25);
            flCustomToolStrip1.TabIndex = 1;
            flCustomToolStrip1.Text = "flCustomToolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(25, 22);
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
            toolStripButton2.Size = new Size(25, 22);
            toolStripButton2.Text = "Revertir cambios";
            toolStripButton2.Click += toolStripButton2_Click;
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
            dgvRelacionDeHorarios.Dock = DockStyle.Fill;
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
            dgvRelacionDeHorarios.Size = new Size(852, 419);
            dgvRelacionDeHorarios.TabIndex = 4;
            dgvRelacionDeHorarios.OnAddClick += dgvAjustesHorarios_OnAddClick;
            dgvRelacionDeHorarios.CellEndEdit += dgvAjustesEmpleados_CellEndEdit;
            dgvRelacionDeHorarios.CellEnter += dgvAjustesEmpleados_CellEnter;
            dgvRelacionDeHorarios.CellValidating += dgvAjustesEmpleados_CellValidating;
            dgvRelacionDeHorarios.RowValidating += dgvAjustesHorarios_RowValidating;
            dgvRelacionDeHorarios.SelectionChanged += dgvAjustesHorarios_SelectionChanged;
            // 
            // frmCrudRelacionHorarios
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(852, 444);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowControls.flCustomToolStrip flCustomToolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ImageList imageList1;
        private FlowControls.flExtendedDataGridView dgvRelacionDeHorarios;
    }
}