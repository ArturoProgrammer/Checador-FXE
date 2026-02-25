using FlowControls;

namespace Checador_FXE
{
    partial class frmExportadorDeConfiguracion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportadorDeConfiguracion));
            btnAceptar = new flCustomButton();
            btnCerrar = new flCustomButton();
            txtRutaIngreso = new flTextBoxLabelJoint();
            btnExaminarIngreso = new flCustomButton();
            btnExaminarDestino = new flCustomButton();
            txtRutaDestino = new flTextBoxLabelJoint();
            rtxtResumenOperacion = new flRichTextBoxLabelJoint();
            flLabelHeader1 = new flLabelHeader();
            SuspendLayout();
            // 
            // btnAceptar
            // 
            btnAceptar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAceptar.BackColor = SystemColors.ActiveCaption;
            btnAceptar.Enabled = false;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F);
            btnAceptar.Image = Properties.Resources.check;
            btnAceptar.Location = new Point(362, 278);
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
            btnCerrar.Location = new Point(473, 278);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(98, 29);
            btnCerrar.TabIndex = 10;
            btnCerrar.Text = " Cerrar";
            btnCerrar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // txtRutaIngreso
            // 
            txtRutaIngreso.Enabled = false;
            txtRutaIngreso.EntryFont = new Font("Consolas", 9F);
            txtRutaIngreso.InputContentType = InputMode.GENERAL;
            txtRutaIngreso.Label = "Origen:";
            txtRutaIngreso.Location = new Point(28, 63);
            txtRutaIngreso.MinimumSize = new Size(79, 28);
            txtRutaIngreso.Name = "txtRutaIngreso";
            txtRutaIngreso.Placeholder = "";
            txtRutaIngreso.RootLineColor = Color.Gray;
            txtRutaIngreso.Size = new Size(474, 30);
            txtRutaIngreso.TabIndex = 11;
            txtRutaIngreso.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtRutaIngreso.TextBoxWidth = 350;
            txtRutaIngreso.Value = "";
            // 
            // btnExaminarIngreso
            // 
            btnExaminarIngreso.BackColor = SystemColors.ActiveCaption;
            btnExaminarIngreso.FlatStyle = FlatStyle.Flat;
            btnExaminarIngreso.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExaminarIngreso.Image = Properties.Resources.buscar_16;
            btnExaminarIngreso.Location = new Point(508, 64);
            btnExaminarIngreso.Name = "btnExaminarIngreso";
            btnExaminarIngreso.Size = new Size(52, 25);
            btnExaminarIngreso.TabIndex = 12;
            btnExaminarIngreso.UseVisualStyleBackColor = false;
            btnExaminarIngreso.Click += btnExaminar_Click;
            // 
            // btnExaminarDestino
            // 
            btnExaminarDestino.BackColor = SystemColors.ActiveCaption;
            btnExaminarDestino.FlatStyle = FlatStyle.Flat;
            btnExaminarDestino.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExaminarDestino.Image = Properties.Resources.buscar_16;
            btnExaminarDestino.Location = new Point(508, 101);
            btnExaminarDestino.Name = "btnExaminarDestino";
            btnExaminarDestino.Size = new Size(52, 25);
            btnExaminarDestino.TabIndex = 14;
            btnExaminarDestino.UseVisualStyleBackColor = false;
            btnExaminarDestino.Click += btnExaminarDestino_Click;
            // 
            // txtRutaDestino
            // 
            txtRutaDestino.Enabled = false;
            txtRutaDestino.EntryFont = new Font("Consolas", 9F);
            txtRutaDestino.InputContentType = InputMode.GENERAL;
            txtRutaDestino.Label = "Destino:";
            txtRutaDestino.Location = new Point(28, 100);
            txtRutaDestino.MinimumSize = new Size(79, 28);
            txtRutaDestino.Name = "txtRutaDestino";
            txtRutaDestino.Placeholder = "";
            txtRutaDestino.RootLineColor = Color.Gray;
            txtRutaDestino.Size = new Size(474, 30);
            txtRutaDestino.TabIndex = 13;
            txtRutaDestino.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtRutaDestino.TextBoxWidth = 350;
            txtRutaDestino.Value = "";
            // 
            // rtxtResumenOperacion
            // 
            rtxtResumenOperacion.Enabled = false;
            rtxtResumenOperacion.EntryFont = new Font("Consolas", 9F);
            rtxtResumenOperacion.Label = "Resumen:";
            rtxtResumenOperacion.Location = new Point(28, 139);
            rtxtResumenOperacion.MinimumSize = new Size(100, 30);
            rtxtResumenOperacion.Name = "rtxtResumenOperacion";
            rtxtResumenOperacion.Placeholder = "Escribe aqui...";
            rtxtResumenOperacion.RichTextBoxBackColor = SystemColors.GradientActiveCaption;
            rtxtResumenOperacion.RichTextBoxWidth = 350;
            rtxtResumenOperacion.RootLineColor = Color.Gray;
            rtxtResumenOperacion.Size = new Size(474, 116);
            rtxtResumenOperacion.TabIndex = 15;
            rtxtResumenOperacion.Value = "";
            // 
            // flLabelHeader1
            // 
            flLabelHeader1.BackColor = SystemColors.GradientInactiveCaption;
            flLabelHeader1.Dock = DockStyle.Top;
            flLabelHeader1.Font = new Font("Segoe UI", 14F);
            flLabelHeader1.HeaderText = "Configuracion de Exportacion";
            flLabelHeader1.HeaderTextAlign = ContentAlignment.BottomLeft;
            flLabelHeader1.HeaderTextColor = SystemColors.ControlText;
            flLabelHeader1.LabelImage = (Image)resources.GetObject("flLabelHeader1.LabelImage");
            flLabelHeader1.LineColor = Color.Black;
            flLabelHeader1.LineThickness = 1;
            flLabelHeader1.Location = new Point(0, 0);
            flLabelHeader1.Margin = new Padding(0);
            flLabelHeader1.MinimumSize = new Size(0, 45);
            flLabelHeader1.Name = "flLabelHeader1";
            flLabelHeader1.Padding = new Padding(20, 3, 20, 5);
            flLabelHeader1.Size = new Size(583, 47);
            flLabelHeader1.TabIndex = 16;
            // 
            // frmExportadorDeConfiguracion
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(583, 319);
            Controls.Add(flLabelHeader1);
            Controls.Add(rtxtResumenOperacion);
            Controls.Add(btnExaminarDestino);
            Controls.Add(txtRutaDestino);
            Controls.Add(btnExaminarIngreso);
            Controls.Add(txtRutaIngreso);
            Controls.Add(btnAceptar);
            Controls.Add(btnCerrar);
            Name = "frmExportadorDeConfiguracion";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Exportar Configuracion";
            Load += frmExportadorDeConfiguracion_Load;
            ResumeLayout(false);
        }

        #endregion

        private FlowControls.flCustomButton btnAceptar;
        private FlowControls.flCustomButton btnCerrar;
        private FlowControls.flTextBoxLabelJoint txtRutaIngreso;
        private FlowControls.flCustomButton btnExaminarIngreso;
        private FlowControls.flCustomButton btnExaminarDestino;
        private FlowControls.flTextBoxLabelJoint txtRutaDestino;
        private FlowControls.flRichTextBoxLabelJoint rtxtResumenOperacion;
        private FlowControls.flLabelHeader flLabelHeader1;
    }
}