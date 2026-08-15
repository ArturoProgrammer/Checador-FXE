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
            btnExaminar = new FlowControls.flCustomButton();
            cboxLocalidadRemitente = new FlowControls.flComboBoxLabelJoint();
            txtTitulo = new FlowControls.flTextBoxLabelJoint();
            label1 = new Label();
            flGroupBox1 = new FlowControls.flGroupBox();
            flGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtRutaArchivo
            // 
            txtRutaArchivo.Enabled = false;
            txtRutaArchivo.EntryFont = new Font("Consolas", 10F);
            txtRutaArchivo.InputContentType = FlowControls.InputMode.GENERAL;
            txtRutaArchivo.InputStyle = FlowControls.TextStyle.Normal;
            txtRutaArchivo.Label = "Archivo:";
            txtRutaArchivo.Location = new Point(3, 68);
            txtRutaArchivo.MinimumSize = new Size(79, 28);
            txtRutaArchivo.Name = "txtRutaArchivo";
            txtRutaArchivo.Placeholder = "";
            txtRutaArchivo.RootLineColor = Color.Gray;
            txtRutaArchivo.Size = new Size(435, 31);
            txtRutaArchivo.TabIndex = 4;
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
            cboxModeloDispositivo.Location = new Point(3, 105);
            cboxModeloDispositivo.MinimumSize = new Size(118, 28);
            cboxModeloDispositivo.Name = "cboxModeloDispositivo";
            cboxModeloDispositivo.RootLineColor = Color.Gray;
            cboxModeloDispositivo.Size = new Size(435, 31);
            cboxModeloDispositivo.TabIndex = 5;
            cboxModeloDispositivo.Value = "(Seleccione un elemento...)";
            cboxModeloDispositivo.OnSelectedIndexChanged += cboxModeloDispositivo_OnSelectedIndexChanged;
            // 
            // btnAceptar
            // 
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.BackColor = SystemColors.ActiveCaption;
            btnAceptar.BorderRadius = 0;
            btnAceptar.Enabled = false;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F);
            btnAceptar.Image = Properties.Resources.check;
            btnAceptar.Location = new Point(314, 266);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 34);
            btnAceptar.TabIndex = 8;
            btnAceptar.Text = " Aceptar";
            btnAceptar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.BackColor = SystemColors.ActiveCaption;
            btnCerrar.BorderRadius = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F);
            btnCerrar.Image = Properties.Resources.cancel_16;
            btnCerrar.Location = new Point(430, 266);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 34);
            btnCerrar.TabIndex = 9;
            btnCerrar.Text = " Cerrar";
            btnCerrar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnExaminar
            // 
            btnExaminar.BackColor = SystemColors.ActiveCaption;
            btnExaminar.BorderRadius = 0;
            btnExaminar.FlatStyle = FlatStyle.Flat;
            btnExaminar.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExaminar.Image = Properties.Resources.buscar_16;
            btnExaminar.Location = new Point(444, 71);
            btnExaminar.Name = "btnExaminar";
            btnExaminar.Size = new Size(52, 25);
            btnExaminar.TabIndex = 3;
            btnExaminar.UseVisualStyleBackColor = false;
            btnExaminar.Click += btnExaminar_Click;
            // 
            // cboxLocalidadRemitente
            // 
            cboxLocalidadRemitente.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxLocalidadRemitente.ComboBoxWidth = 300;
            cboxLocalidadRemitente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxLocalidadRemitente.EnableItemSearch = true;
            cboxLocalidadRemitente.EnableSelectionConfirmation = false;
            cboxLocalidadRemitente.EntryFont = new Font("Consolas", 10F);
            cboxLocalidadRemitente.Items.Add("(Seleccione un elemento...)");
            cboxLocalidadRemitente.Label = "Localidad:";
            cboxLocalidadRemitente.Location = new Point(4, 142);
            cboxLocalidadRemitente.MinimumSize = new Size(118, 28);
            cboxLocalidadRemitente.Name = "cboxLocalidadRemitente";
            cboxLocalidadRemitente.RootLineColor = Color.Gray;
            cboxLocalidadRemitente.Size = new Size(435, 31);
            cboxLocalidadRemitente.TabIndex = 6;
            cboxLocalidadRemitente.Value = "(Seleccione un elemento...)";
            // 
            // txtTitulo
            // 
            txtTitulo.EntryFont = new Font("Consolas", 10F);
            txtTitulo.InputContentType = FlowControls.InputMode.GENERAL;
            txtTitulo.InputStyle = FlowControls.TextStyle.Normal;
            txtTitulo.Label = "Titulo:";
            txtTitulo.Location = new Point(4, 32);
            txtTitulo.MinimumSize = new Size(79, 28);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Placeholder = "";
            txtTitulo.RootLineColor = Color.Gray;
            txtTitulo.Size = new Size(484, 30);
            txtTitulo.TabIndex = 2;
            txtTitulo.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtTitulo.TextBoxWidth = 350;
            txtTitulo.Value = "";
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Italic | FontStyle.Underline);
            label1.ForeColor = Color.IndianRed;
            label1.Location = new Point(4, 190);
            label1.Name = "label1";
            label1.Size = new Size(516, 49);
            label1.TabIndex = 7;
            label1.Text = "** ADVERTENCIA: Selecciona el modelo de dispositivo correcto con el que se genero el reporte de chequeos para procesar el documento correctamente con el algoritmo correspondiente.";
            // 
            // flGroupBox1
            // 
            flGroupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flGroupBox1.BackColor = SystemColors.GradientInactiveCaption;
            flGroupBox1.BorderColor = SystemColors.ActiveCaption;
            flGroupBox1.Controls.Add(txtTitulo);
            flGroupBox1.Controls.Add(btnExaminar);
            flGroupBox1.Controls.Add(cboxModeloDispositivo);
            flGroupBox1.Controls.Add(cboxLocalidadRemitente);
            flGroupBox1.Controls.Add(txtRutaArchivo);
            flGroupBox1.Controls.Add(label1);
            flGroupBox1.HeaderColor = SystemColors.ActiveCaption;
            flGroupBox1.HeaderStyle = FlowControls.HeaderStyle.Folder;
            flGroupBox1.Location = new Point(12, 12);
            flGroupBox1.Name = "flGroupBox1";
            flGroupBox1.Padding = new Padding(1, 29, 1, 1);
            flGroupBox1.Size = new Size(528, 248);
            flGroupBox1.TabIndex = 8;
            flGroupBox1.TitleFont = new Font("Segoe UI", 10F);
            flGroupBox1.TitleForeColor = Color.White;
            flGroupBox1.TitleText = "Propiedades";
            // 
            // frmNuevoProyecto
            // 
            AcceptButton = btnAceptar;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.GradientInactiveCaption;
            CancelButton = btnCerrar;
            ClientSize = new Size(552, 312);
            Controls.Add(flGroupBox1);
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
            flGroupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowControls.flTextBoxLabelJoint txtRutaArchivo;
        private FlowControls.flComboBoxLabelJoint cboxModeloDispositivo;
        private FlowControls.flCustomButton btnAceptar;
        private FlowControls.flCustomButton btnCerrar;
        private FlowControls.flCustomButton btnExaminar;
        private Label label1;
        private FlowControls.flTextBoxLabelJoint txtTitulo;
        private FlowControls.flComboBoxLabelJoint cboxLocalidadRemitente;
        private FlowControls.flGroupBox flGroupBox1;
    }
}