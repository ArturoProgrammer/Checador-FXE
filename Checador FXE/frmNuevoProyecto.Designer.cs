namespace Checador_FXE
{
    partial class frmNuevoProyecto
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
            txtRutaArchivo = new FlowControls.flTextBoxLabelJoint();
            cboxModeloDispositivo = new FlowControls.flComboBoxLabelJoint();
            btnAceptar = new FlowControls.flCustomButton();
            btnCerrar = new FlowControls.flCustomButton();
            groupBox1 = new GroupBox();
            btnExaminar = new FlowControls.flCustomButton();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtRutaArchivo
            // 
            txtRutaArchivo.Enabled = false;
            txtRutaArchivo.EntryFont = new Font("Consolas", 9F);
            txtRutaArchivo.InputContentType = FlowControls.InputMode.GENERAL;
            txtRutaArchivo.Label = "Archivo:";
            txtRutaArchivo.Location = new Point(5, 25);
            txtRutaArchivo.MinimumSize = new Size(79, 28);
            txtRutaArchivo.Name = "txtRutaArchivo";
            txtRutaArchivo.Placeholder = "";
            txtRutaArchivo.RootLineColor = Color.Gray;
            txtRutaArchivo.Size = new Size(435, 30);
            txtRutaArchivo.TabIndex = 0;
            txtRutaArchivo.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtRutaArchivo.TextBoxWidth = 300;
            txtRutaArchivo.Value = "";
            // 
            // cboxModeloDispositivo
            // 
            cboxModeloDispositivo.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxModeloDispositivo.ComboBoxWidth = 300;
            cboxModeloDispositivo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxModeloDispositivo.EnableItemSearch = true;
            cboxModeloDispositivo.EnableSelectionConfirmation = false;
            cboxModeloDispositivo.EntryFont = new Font("Consolas", 10F);
            cboxModeloDispositivo.Items.Add("(Seleccione un elemento...)");
            cboxModeloDispositivo.Label = "Dispositivo:";
            cboxModeloDispositivo.Location = new Point(5, 62);
            cboxModeloDispositivo.MinimumSize = new Size(118, 28);
            cboxModeloDispositivo.Name = "cboxModeloDispositivo";
            cboxModeloDispositivo.RootLineColor = Color.Gray;
            cboxModeloDispositivo.Size = new Size(435, 30);
            cboxModeloDispositivo.TabIndex = 1;
            cboxModeloDispositivo.Value = "(Seleccione un elemento...)";
            cboxModeloDispositivo.OnSelectedIndexChanged += cboxModeloDispositivo_OnSelectedIndexChanged;
            // 
            // btnAceptar
            // 
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.BackColor = SystemColors.ActiveCaption;
            btnAceptar.Enabled = false;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F);
            btnAceptar.Image = Properties.Resources.check;
            btnAceptar.Location = new Point(329, 170);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(98, 29);
            btnAceptar.TabIndex = 9;
            btnAceptar.Text = " Aceptar";
            btnAceptar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.BackColor = SystemColors.ActiveCaption;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F);
            btnCerrar.Image = Properties.Resources.cancel_16;
            btnCerrar.Location = new Point(442, 170);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(98, 29);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = " Cerrar";
            btnCerrar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtRutaArchivo);
            groupBox1.Controls.Add(cboxModeloDispositivo);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(528, 152);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Propiedades";
            // 
            // btnExaminar
            // 
            btnExaminar.BackColor = SystemColors.ActiveCaption;
            btnExaminar.FlatStyle = FlatStyle.Flat;
            btnExaminar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExaminar.Image = Properties.Resources.buscar_16;
            btnExaminar.Location = new Point(458, 39);
            btnExaminar.Name = "btnExaminar";
            btnExaminar.Size = new Size(52, 25);
            btnExaminar.TabIndex = 2;
            btnExaminar.UseVisualStyleBackColor = false;
            btnExaminar.Click += btnExaminar_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 8F, FontStyle.Italic | FontStyle.Underline);
            label1.ForeColor = Color.IndianRed;
            label1.Location = new Point(6, 109);
            label1.Name = "label1";
            label1.Size = new Size(516, 31);
            label1.TabIndex = 2;
            label1.Text = "** ADVERTENCIA: Selecciona el modelo de dispositivo correcto con el que se genero el reporte de chequeos para procesar el documento correctamente con el algoritmo correspondiente.";
            // 
            // frmNuevoProyecto
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(552, 211);
            Controls.Add(btnExaminar);
            Controls.Add(groupBox1);
            Controls.Add(btnAceptar);
            Controls.Add(btnCerrar);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmNuevoProyecto";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo Proyecto";
            FormClosing += frmNuevoProyecto_FormClosing;
            Load += frmNuevoProyecto_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowControls.flTextBoxLabelJoint txtRutaArchivo;
        private FlowControls.flComboBoxLabelJoint cboxModeloDispositivo;
        private FlowControls.flCustomButton btnAceptar;
        private FlowControls.flCustomButton btnCerrar;
        private GroupBox groupBox1;
        private FlowControls.flCustomButton btnExaminar;
        private Label label1;
    }
}