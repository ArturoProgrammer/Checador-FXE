namespace Checador_FXE
{
    partial class frmConfiguraciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfiguraciones));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            flTabMenuControl1 = new FlowControls.flTabMenuControl();
            tabPage1 = new TabPage();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flLabelHeader1 = new FlowControls.flLabelHeader();
            txtMaximoRetrasoMinutosPermitidos = new FlowControls.flTimeLabelJoint();
            cboxDispositivoDefault = new FlowControls.flComboBoxLabelJoint();
            cboxColorPincel = new FlowControls.flComboBoxLabelJoint();
            txtNombreArchivoDefecto = new FlowControls.flTextBoxLabelJoint();
            flLabelHeader2 = new FlowControls.flLabelHeader();
            cboxLocalidadEstablecida = new FlowControls.flComboBoxLabelJoint();
            tabAjustesHorario = new TabPage();
            dgvAjustesHorarios = new FlowControls.flExtendedDataGridView();
            colTurnoNom = new DataGridViewTextBoxColumn();
            colHorarioUno_Entrada = new DataGridViewTextBoxColumn();
            colHorarioUno_Salida = new DataGridViewTextBoxColumn();
            colHorarioDos_Entrada = new DataGridViewTextBoxColumn();
            colHorarioDos_Salida = new DataGridViewTextBoxColumn();
            tabServidor = new TabPage();
            flowLayoutPanel2 = new FlowLayoutPanel();
            flLabelHeader3 = new FlowControls.flLabelHeader();
            txtHostnameTcpIp = new FlowControls.flTextBoxLabelJoint();
            txtUsuarioServidor = new FlowControls.flTextBoxLabelJoint();
            txtPassServidor = new FlowControls.flTextBoxLabelJoint();
            txtPuerto = new FlowControls.flTextBoxLabelJoint();
            btnTestConnection = new FlowControls.flCustomButton();
            imageList1 = new ImageList(components);
            btnCerrar = new FlowControls.flCustomButton();
            btnAceptar = new FlowControls.flCustomButton();
            flTabMenuControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tabAjustesHorario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAjustesHorarios).BeginInit();
            tabServidor.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // flTabMenuControl1
            // 
            flTabMenuControl1.Alignment = TabAlignment.Right;
            flTabMenuControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flTabMenuControl1.ControlBackColor = SystemColors.GradientInactiveCaption;
            flTabMenuControl1.Controls.Add(tabPage1);
            flTabMenuControl1.Controls.Add(tabAjustesHorario);
            flTabMenuControl1.Controls.Add(tabServidor);
            flTabMenuControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            flTabMenuControl1.ForeSelectionColor = Color.White;
            flTabMenuControl1.ForeUnselectedColor = Color.DimGray;
            flTabMenuControl1.HoverColor = Color.FromArgb(50, 200, 200, 200);
            flTabMenuControl1.ImageList = imageList1;
            flTabMenuControl1.ItemSize = new Size(50, 175);
            flTabMenuControl1.Location = new Point(0, 0);
            flTabMenuControl1.Multiline = true;
            flTabMenuControl1.Name = "flTabMenuControl1";
            flTabMenuControl1.SelectedIndex = 0;
            flTabMenuControl1.SelectionColor = Color.DeepSkyBlue;
            flTabMenuControl1.Size = new Size(799, 360);
            flTabMenuControl1.SizeMode = TabSizeMode.Fixed;
            flTabMenuControl1.TabIndex = 0;
            flTabMenuControl1.UnselectionColor = Color.LightGray;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.GradientInactiveCaption;
            tabPage1.BorderStyle = BorderStyle.FixedSingle;
            tabPage1.Controls.Add(flowLayoutPanel1);
            tabPage1.ImageIndex = 0;
            tabPage1.Location = new Point(4, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(616, 352);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "General";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(flLabelHeader1);
            flowLayoutPanel1.Controls.Add(txtMaximoRetrasoMinutosPermitidos);
            flowLayoutPanel1.Controls.Add(cboxDispositivoDefault);
            flowLayoutPanel1.Controls.Add(cboxColorPincel);
            flowLayoutPanel1.Controls.Add(txtNombreArchivoDefecto);
            flowLayoutPanel1.Controls.Add(flLabelHeader2);
            flowLayoutPanel1.Controls.Add(cboxLocalidadEstablecida);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(608, 344);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // flLabelHeader1
            // 
            flLabelHeader1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flLabelHeader1.BackColor = SystemColors.GradientInactiveCaption;
            flLabelHeader1.Font = new Font("Segoe UI", 14F);
            flLabelHeader1.HeaderText = "Reporteador";
            flLabelHeader1.HeaderTextAlign = ContentAlignment.BottomLeft;
            flLabelHeader1.HeaderTextColor = SystemColors.ControlText;
            flLabelHeader1.LabelImage = (Image)resources.GetObject("flLabelHeader1.LabelImage");
            flLabelHeader1.LineColor = Color.Black;
            flLabelHeader1.LineThickness = 1;
            flLabelHeader1.Location = new Point(3, 3);
            flLabelHeader1.MinimumSize = new Size(0, 45);
            flLabelHeader1.Name = "flLabelHeader1";
            flLabelHeader1.Padding = new Padding(20, 3, 20, 5);
            flLabelHeader1.Size = new Size(602, 45);
            flLabelHeader1.TabIndex = 3;
            // 
            // txtMaximoRetrasoMinutosPermitidos
            // 
            txtMaximoRetrasoMinutosPermitidos.EntryFont = new Font("Consolas", 10F);
            txtMaximoRetrasoMinutosPermitidos.Label = "Tiempo maximo de retraso permitido:";
            txtMaximoRetrasoMinutosPermitidos.Location = new Point(3, 54);
            txtMaximoRetrasoMinutosPermitidos.MinimumSize = new Size(100, 34);
            txtMaximoRetrasoMinutosPermitidos.Name = "txtMaximoRetrasoMinutosPermitidos";
            txtMaximoRetrasoMinutosPermitidos.RootLineColor = Color.Gray;
            txtMaximoRetrasoMinutosPermitidos.ShowSeconds = false;
            txtMaximoRetrasoMinutosPermitidos.Size = new Size(365, 34);
            txtMaximoRetrasoMinutosPermitidos.TabIndex = 0;
            txtMaximoRetrasoMinutosPermitidos.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtMaximoRetrasoMinutosPermitidos.TextBoxWidth = 75;
            txtMaximoRetrasoMinutosPermitidos.Value = null;
            txtMaximoRetrasoMinutosPermitidos.Validating += txtMaximoRetrasoMinutosPermitidos_Validating;
            // 
            // cboxDispositivoDefault
            // 
            cboxDispositivoDefault.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxDispositivoDefault.ComboBoxWidth = 275;
            cboxDispositivoDefault.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxDispositivoDefault.EnableItemSearch = true;
            cboxDispositivoDefault.EnableSelectionConfirmation = false;
            cboxDispositivoDefault.EntryFont = new Font("Consolas", 10F);
            cboxDispositivoDefault.Items.Add("(Seleccione un elemento...)");
            cboxDispositivoDefault.Label = "Dispositivo por defecto:";
            cboxDispositivoDefault.Location = new Point(3, 94);
            cboxDispositivoDefault.MinimumSize = new Size(150, 30);
            cboxDispositivoDefault.Name = "cboxDispositivoDefault";
            cboxDispositivoDefault.RootLineColor = Color.Gray;
            cboxDispositivoDefault.Size = new Size(565, 33);
            cboxDispositivoDefault.TabIndex = 1;
            cboxDispositivoDefault.Value = "(Seleccione un elemento...)";
            cboxDispositivoDefault.Validating += cboxDispositivoDefault_Validating;
            // 
            // cboxColorPincel
            // 
            cboxColorPincel.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxColorPincel.ComboBoxWidth = 200;
            cboxColorPincel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxColorPincel.EnableItemSearch = true;
            cboxColorPincel.EnableSelectionConfirmation = false;
            cboxColorPincel.EntryFont = new Font("Consolas", 10F);
            cboxColorPincel.Items.Add("(Seleccione un elemento...)");
            cboxColorPincel.Items.Add("Azul");
            cboxColorPincel.Items.Add("Negro");
            cboxColorPincel.Items.Add("Rojo");
            cboxColorPincel.Items.Add("Gris");
            cboxColorPincel.Label = "Color de Pincel:";
            cboxColorPincel.Location = new Point(3, 133);
            cboxColorPincel.MinimumSize = new Size(150, 30);
            cboxColorPincel.Name = "cboxColorPincel";
            cboxColorPincel.RootLineColor = Color.Gray;
            cboxColorPincel.Size = new Size(490, 33);
            cboxColorPincel.TabIndex = 2;
            cboxColorPincel.Value = "(Seleccione un elemento...)";
            cboxColorPincel.Validating += cboxColorPincel_Validating;
            // 
            // txtNombreArchivoDefecto
            // 
            txtNombreArchivoDefecto.EntryFont = new Font("Consolas", 9F);
            txtNombreArchivoDefecto.InputContentType = FlowControls.InputMode.GENERAL;
            txtNombreArchivoDefecto.Label = "Nombre del archivo:";
            txtNombreArchivoDefecto.Location = new Point(3, 172);
            txtNombreArchivoDefecto.MinimumSize = new Size(100, 30);
            txtNombreArchivoDefecto.Name = "txtNombreArchivoDefecto";
            txtNombreArchivoDefecto.Placeholder = "";
            txtNombreArchivoDefecto.RootLineColor = Color.Gray;
            txtNombreArchivoDefecto.Size = new Size(565, 33);
            txtNombreArchivoDefecto.TabIndex = 6;
            txtNombreArchivoDefecto.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtNombreArchivoDefecto.TextBoxWidth = 275;
            txtNombreArchivoDefecto.Value = "";
            txtNombreArchivoDefecto.Validating += txtNombreArchivoDefecto_Validating;
            // 
            // flLabelHeader2
            // 
            flLabelHeader2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flLabelHeader2.BackColor = SystemColors.GradientInactiveCaption;
            flLabelHeader2.Font = new Font("Segoe UI", 14F);
            flLabelHeader2.HeaderText = "Datos de la Localidad";
            flLabelHeader2.HeaderTextAlign = ContentAlignment.BottomLeft;
            flLabelHeader2.HeaderTextColor = SystemColors.ControlText;
            flLabelHeader2.LabelImage = (Image)resources.GetObject("flLabelHeader2.LabelImage");
            flLabelHeader2.LineColor = Color.Black;
            flLabelHeader2.LineThickness = 1;
            flLabelHeader2.Location = new Point(3, 218);
            flLabelHeader2.Margin = new Padding(3, 10, 3, 3);
            flLabelHeader2.MinimumSize = new Size(0, 45);
            flLabelHeader2.Name = "flLabelHeader2";
            flLabelHeader2.Padding = new Padding(20, 3, 20, 5);
            flLabelHeader2.Size = new Size(602, 45);
            flLabelHeader2.TabIndex = 4;
            // 
            // cboxLocalidadEstablecida
            // 
            cboxLocalidadEstablecida.ComboBoxBackColor = SystemColors.GradientActiveCaption;
            cboxLocalidadEstablecida.ComboBoxWidth = 275;
            cboxLocalidadEstablecida.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxLocalidadEstablecida.EnableItemSearch = true;
            cboxLocalidadEstablecida.EnableSelectionConfirmation = false;
            cboxLocalidadEstablecida.EntryFont = new Font("Consolas", 10F);
            cboxLocalidadEstablecida.Items.Add("(Seleccione un elemento...)");
            cboxLocalidadEstablecida.Label = "Localidad establecida:";
            cboxLocalidadEstablecida.Location = new Point(3, 269);
            cboxLocalidadEstablecida.MinimumSize = new Size(150, 30);
            cboxLocalidadEstablecida.Name = "cboxLocalidadEstablecida";
            cboxLocalidadEstablecida.RootLineColor = Color.Gray;
            cboxLocalidadEstablecida.Size = new Size(565, 33);
            cboxLocalidadEstablecida.TabIndex = 5;
            cboxLocalidadEstablecida.Value = "(Seleccione un elemento...)";
            cboxLocalidadEstablecida.Validating += cboxLocalidadEstablecida_Validating;
            // 
            // tabAjustesHorario
            // 
            tabAjustesHorario.BackColor = SystemColors.GradientInactiveCaption;
            tabAjustesHorario.BorderStyle = BorderStyle.FixedSingle;
            tabAjustesHorario.Controls.Add(dgvAjustesHorarios);
            tabAjustesHorario.ImageIndex = 1;
            tabAjustesHorario.Location = new Point(4, 4);
            tabAjustesHorario.Name = "tabAjustesHorario";
            tabAjustesHorario.Size = new Size(616, 352);
            tabAjustesHorario.TabIndex = 1;
            tabAjustesHorario.Text = "Ajustes de Horario";
            // 
            // dgvAjustesHorarios
            // 
            dgvAjustesHorarios.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle1.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ActiveCaptionText;
            dgvAjustesHorarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAjustesHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAjustesHorarios.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvAjustesHorarios.BorderStyle = BorderStyle.None;
            dgvAjustesHorarios.ButtonAddEnabled = true;
            dgvAjustesHorarios.ButtonEditEnabled = false;
            dgvAjustesHorarios.ButtonRemoveEnabled = false;
            dgvAjustesHorarios.ButtonViewEnabled = false;
            dgvAjustesHorarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ButtonFace;
            dataGridViewCellStyle2.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAjustesHorarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAjustesHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAjustesHorarios.Columns.AddRange(new DataGridViewColumn[] { colTurnoNom, colHorarioUno_Entrada, colHorarioUno_Salida, colHorarioDos_Entrada, colHorarioDos_Salida });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle3.Padding = new Padding(3, 0, 3, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.Orange;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAjustesHorarios.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAjustesHorarios.Dock = DockStyle.Fill;
            dgvAjustesHorarios.EnableHeadersVisualStyles = false;
            dgvAjustesHorarios.GridColor = Color.FromArgb(210, 210, 210);
            dgvAjustesHorarios.LabelCounterForeColor = SystemColors.ButtonFace;
            dgvAjustesHorarios.Location = new Point(0, 0);
            dgvAjustesHorarios.Margin = new Padding(0);
            dgvAjustesHorarios.MultiSelect = false;
            dgvAjustesHorarios.Name = "dgvAjustesHorarios";
            dgvAjustesHorarios.RowHeadersVisible = false;
            dgvAjustesHorarios.RowHeadersWidth = 45;
            dataGridViewCellStyle4.SelectionBackColor = Color.Orange;
            dgvAjustesHorarios.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvAjustesHorarios.SelectedCellColor = Color.Moccasin;
            dgvAjustesHorarios.SelectedRowColor = Color.SteelBlue;
            dgvAjustesHorarios.SelectionForeColor = Color.Black;
            dgvAjustesHorarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAjustesHorarios.Size = new Size(614, 350);
            dgvAjustesHorarios.TabIndex = 3;
            // 
            // colTurnoNom
            // 
            colTurnoNom.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colTurnoNom.HeaderText = "Turno No.";
            colTurnoNom.MinimumWidth = 6;
            colTurnoNom.Name = "colTurnoNom";
            colTurnoNom.Width = 106;
            // 
            // colHorarioUno_Entrada
            // 
            colHorarioUno_Entrada.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colHorarioUno_Entrada.HeaderText = "1er Hor. Ent.";
            colHorarioUno_Entrada.MinimumWidth = 6;
            colHorarioUno_Entrada.Name = "colHorarioUno_Entrada";
            colHorarioUno_Entrada.Width = 122;
            // 
            // colHorarioUno_Salida
            // 
            colHorarioUno_Salida.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colHorarioUno_Salida.HeaderText = "1er Hor. Sal.";
            colHorarioUno_Salida.MinimumWidth = 6;
            colHorarioUno_Salida.Name = "colHorarioUno_Salida";
            colHorarioUno_Salida.Width = 121;
            // 
            // colHorarioDos_Entrada
            // 
            colHorarioDos_Entrada.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colHorarioDos_Entrada.HeaderText = "2do Hor. Ent.";
            colHorarioDos_Entrada.MinimumWidth = 6;
            colHorarioDos_Entrada.Name = "colHorarioDos_Entrada";
            colHorarioDos_Entrada.Width = 127;
            // 
            // colHorarioDos_Salida
            // 
            colHorarioDos_Salida.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colHorarioDos_Salida.HeaderText = "2do Hor. Sal.";
            colHorarioDos_Salida.MinimumWidth = 6;
            colHorarioDos_Salida.Name = "colHorarioDos_Salida";
            colHorarioDos_Salida.Width = 126;
            // 
            // tabServidor
            // 
            tabServidor.BackColor = SystemColors.GradientInactiveCaption;
            tabServidor.BorderStyle = BorderStyle.FixedSingle;
            tabServidor.Controls.Add(flowLayoutPanel2);
            tabServidor.Controls.Add(btnTestConnection);
            tabServidor.ImageIndex = 2;
            tabServidor.Location = new Point(4, 4);
            tabServidor.Name = "tabServidor";
            tabServidor.Padding = new Padding(3);
            tabServidor.Size = new Size(616, 352);
            tabServidor.TabIndex = 2;
            tabServidor.Text = "Servidor";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(flLabelHeader3);
            flowLayoutPanel2.Controls.Add(txtHostnameTcpIp);
            flowLayoutPanel2.Controls.Add(txtUsuarioServidor);
            flowLayoutPanel2.Controls.Add(txtPassServidor);
            flowLayoutPanel2.Controls.Add(txtPuerto);
            flowLayoutPanel2.Location = new Point(7, 6);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(601, 296);
            flowLayoutPanel2.TabIndex = 8;
            // 
            // flLabelHeader3
            // 
            flLabelHeader3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flLabelHeader3.BackColor = SystemColors.GradientInactiveCaption;
            flLabelHeader3.Font = new Font("Segoe UI", 14F);
            flLabelHeader3.HeaderText = "Datos de Conexion";
            flLabelHeader3.HeaderTextAlign = ContentAlignment.BottomLeft;
            flLabelHeader3.HeaderTextColor = SystemColors.ControlText;
            flLabelHeader3.LabelImage = (Image)resources.GetObject("flLabelHeader3.LabelImage");
            flLabelHeader3.LineColor = Color.Black;
            flLabelHeader3.LineThickness = 1;
            flLabelHeader3.Location = new Point(3, 3);
            flLabelHeader3.MinimumSize = new Size(0, 45);
            flLabelHeader3.Name = "flLabelHeader3";
            flLabelHeader3.Padding = new Padding(20, 3, 20, 5);
            flLabelHeader3.Size = new Size(598, 45);
            flLabelHeader3.TabIndex = 4;
            // 
            // txtHostnameTcpIp
            // 
            txtHostnameTcpIp.EntryFont = new Font("Consolas", 9F);
            txtHostnameTcpIp.InputContentType = FlowControls.InputMode.GENERAL;
            txtHostnameTcpIp.Label = "Direccion hostname o TCP/IP:";
            txtHostnameTcpIp.Location = new Point(3, 54);
            txtHostnameTcpIp.MinimumSize = new Size(100, 30);
            txtHostnameTcpIp.Name = "txtHostnameTcpIp";
            txtHostnameTcpIp.Placeholder = "";
            txtHostnameTcpIp.RootLineColor = Color.Gray;
            txtHostnameTcpIp.Size = new Size(456, 33);
            txtHostnameTcpIp.TabIndex = 0;
            txtHostnameTcpIp.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtHostnameTcpIp.TextBoxWidth = 175;
            txtHostnameTcpIp.Value = "";
            // 
            // txtUsuarioServidor
            // 
            txtUsuarioServidor.EntryFont = new Font("Consolas", 9F);
            txtUsuarioServidor.InputContentType = FlowControls.InputMode.GENERAL;
            txtUsuarioServidor.Label = "Usuario:";
            txtUsuarioServidor.Location = new Point(3, 93);
            txtUsuarioServidor.MinimumSize = new Size(100, 30);
            txtUsuarioServidor.Name = "txtUsuarioServidor";
            txtUsuarioServidor.Placeholder = "";
            txtUsuarioServidor.RootLineColor = Color.Gray;
            txtUsuarioServidor.Size = new Size(456, 33);
            txtUsuarioServidor.TabIndex = 1;
            txtUsuarioServidor.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtUsuarioServidor.TextBoxWidth = 175;
            txtUsuarioServidor.Value = "";
            // 
            // txtPassServidor
            // 
            txtPassServidor.EntryFont = new Font("Consolas", 9F);
            txtPassServidor.InputContentType = FlowControls.InputMode.PASSWORD;
            txtPassServidor.Label = "Contraseña:";
            txtPassServidor.Location = new Point(3, 132);
            txtPassServidor.MinimumSize = new Size(100, 30);
            txtPassServidor.Name = "txtPassServidor";
            txtPassServidor.Placeholder = "";
            txtPassServidor.RootLineColor = Color.Gray;
            txtPassServidor.Size = new Size(456, 33);
            txtPassServidor.TabIndex = 2;
            txtPassServidor.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtPassServidor.TextBoxWidth = 175;
            txtPassServidor.Value = "";
            // 
            // txtPuerto
            // 
            txtPuerto.EntryFont = new Font("Consolas", 9F);
            txtPuerto.InputContentType = FlowControls.InputMode.NUMBERS;
            txtPuerto.Label = "Puerto:";
            txtPuerto.Location = new Point(3, 171);
            txtPuerto.MinimumSize = new Size(100, 30);
            txtPuerto.Name = "txtPuerto";
            txtPuerto.Placeholder = "";
            txtPuerto.RootLineColor = Color.Gray;
            txtPuerto.Size = new Size(356, 33);
            txtPuerto.TabIndex = 7;
            txtPuerto.TextBoxBackColor = SystemColors.GradientActiveCaption;
            txtPuerto.TextBoxWidth = 75;
            txtPuerto.Value = "";
            // 
            // btnTestConnection
            // 
            btnTestConnection.BackColor = SystemColors.ActiveCaption;
            btnTestConnection.FlatStyle = FlatStyle.Flat;
            btnTestConnection.Font = new Font("Segoe UI", 10F);
            btnTestConnection.Image = (Image)resources.GetObject("btnTestConnection.Image");
            btnTestConnection.Location = new Point(412, 308);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(196, 36);
            btnTestConnection.TabIndex = 6;
            btnTestConnection.Text = " Testear Conexion";
            btnTestConnection.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTestConnection.UseVisualStyleBackColor = false;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "home-24.png");
            imageList1.Images.SetKeyName(1, "calendar-24.png");
            imageList1.Images.SetKeyName(2, "server-24.png");
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = SystemColors.ActiveCaption;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 10F);
            btnCerrar.Image = Properties.Resources.cancel_16;
            btnCerrar.Location = new Point(676, 368);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(123, 40);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = " Cerrar";
            btnCerrar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = SystemColors.ActiveCaption;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F);
            btnAceptar.Image = Properties.Resources.check;
            btnAceptar.Location = new Point(547, 368);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(123, 40);
            btnAceptar.TabIndex = 5;
            btnAceptar.Text = " Aceptar";
            btnAceptar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // frmConfiguraciones
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            CancelButton = btnCerrar;
            ClientSize = new Size(811, 420);
            Controls.Add(btnAceptar);
            Controls.Add(btnCerrar);
            Controls.Add(flTabMenuControl1);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "frmConfiguraciones";
            Text = "Configuraciones";
            Load += frmConfiguraciones_Load;
            flTabMenuControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            tabAjustesHorario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAjustesHorarios).EndInit();
            tabServidor.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowControls.flTabMenuControl flTabMenuControl1;
        private TabPage tabPage1;
        private TabPage tabAjustesHorario;
        private ImageList imageList1;
        private FlowControls.flExtendedDataGridView dgvAjustesHorarios;
        private FlowControls.flCustomButton btnCerrar;
        private FlowControls.flCustomButton btnAceptar;
        private FlowControls.flTimeLabelJoint txtMaximoRetrasoMinutosPermitidos;
        private DataGridViewTextBoxColumn colTurnoNom;
        private DataGridViewTextBoxColumn colHorarioUno_Entrada;
        private DataGridViewTextBoxColumn colHorarioUno_Salida;
        private DataGridViewTextBoxColumn colHorarioDos_Entrada;
        private DataGridViewTextBoxColumn colHorarioDos_Salida;
        private FlowControls.flComboBoxLabelJoint cboxDispositivoDefault;
        private FlowControls.flComboBoxLabelJoint cboxColorPincel;
        private TabPage tabServidor;
        private FlowControls.flTextBoxLabelJoint txtPassServidor;
        private FlowControls.flTextBoxLabelJoint txtUsuarioServidor;
        private FlowControls.flTextBoxLabelJoint txtHostnameTcpIp;
        private FlowControls.flCustomButton btnTestConnection;
        private FlowControls.flTextBoxLabelJoint txtPuerto;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowControls.flLabelHeader flLabelHeader1;
        private FlowControls.flLabelHeader flLabelHeader2;
        private FlowControls.flComboBoxLabelJoint cboxLocalidadEstablecida;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowControls.flLabelHeader flLabelHeader3;
        private FlowControls.flTextBoxLabelJoint txtNombreArchivoDefecto;
    }
}