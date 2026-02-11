namespace Checador_FXE
{
    partial class popUpComboBoxEntry
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
            btnAceptar = new FlowControls.flCustomButton();
            btnCerrar = new FlowControls.flCustomButton();
            cboxEntry = new FlowControls.flComboBoxLabelJoint();
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
            btnAceptar.Location = new Point(219, 54);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(98, 29);
            btnAceptar.TabIndex = 7;
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
            btnCerrar.Location = new Point(332, 54);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(98, 29);
            btnCerrar.TabIndex = 6;
            btnCerrar.Text = " Cerrar";
            btnCerrar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // cboxEntry
            // 
            cboxEntry.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboxEntry.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxEntry.ComboBoxWidth = 300;
            cboxEntry.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxEntry.EnableItemSearch = true;
            cboxEntry.EnableSelectionConfirmation = false;
            cboxEntry.EntryFont = new Font("Consolas", 10F);
            cboxEntry.Font = new Font("Segoe UI", 11F);
            cboxEntry.Items.Add("(Seleccione un elemento...)");
            cboxEntry.Label = "Estatus:";
            cboxEntry.Location = new Point(12, 11);
            cboxEntry.MinimumSize = new Size(150, 34);
            cboxEntry.Name = "cboxEntry";
            cboxEntry.RootLineColor = Color.Gray;
            cboxEntry.Size = new Size(413, 34);
            cboxEntry.TabIndex = 8;
            cboxEntry.Value = "(Seleccione un elemento...)";
            cboxEntry.OnSelectedIndexChanged += cboxEntry_OnSelectedIndexChanged;
            // 
            // popUpComboBoxEntry
            // 
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            CancelButton = btnCerrar;
            ClientSize = new Size(440, 90);
            Controls.Add(cboxEntry);
            Controls.Add(btnAceptar);
            Controls.Add(btnCerrar);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "popUpComboBoxEntry";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Propiedad del dia '%FECHA%'";
            Load += popUpComboBoxEntry_Load;
            ResumeLayout(false);
        }

        #endregion

        private FlowControls.flCustomButton btnAceptar;
        private FlowControls.flCustomButton btnCerrar;
        private FlowControls.flComboBoxLabelJoint cboxEntry;
    }
}