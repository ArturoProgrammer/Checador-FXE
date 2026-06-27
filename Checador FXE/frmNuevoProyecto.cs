using Checador_FXE.Plantillas;

namespace Checador_FXE
{
    internal partial class frmNuevoProyecto : Form
    {
        public (string Titulo, string Path, Dispositivo Device, string LocalidadRemitente) Response;

        public frmNuevoProyecto()
        {
            InitializeComponent();
        }

        private void frmNuevoProyecto_Load(object sender, EventArgs e)
        {
            // Cargamos los modelos de dispositivos soportados
            this.cboxModeloDispositivo.Items.AddRange(DispositivoExtensions.GetSupportedModels());
            this.cboxModeloDispositivo.Value = Properties.Settings.Default.DISPOSITIVO_DEFAULT; // Seleccionamos el dispositivo por default

            // Cargamos las localidades compatibles
            this.cboxLocalidadRemitente.Items.AddRange(GlobalConfig.Get("1").Object!.LocalidadesCompatibles);
            this.cboxLocalidadRemitente.Value = Properties.Settings.Default.LOCALIDAD_DEFAULT; // Seleccionamos la localidad por default

            this.txtTitulo.Focus();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            /* 
             * Buscamos el archivo del reporte a procesar
             * */
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Crear nuevo proyecto - Selecciona el archivo de asistencias...";
                ofd.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx, *.xls)|*.xlsx;*.xls";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Extension == ".xls")
                    {
                        MessageBox.Show("El archivo proporcionado es formato '*.xls' por lo que se debe convertir a '*.xlsx'. Abre el archivo .xls en Excel y guardalo en formato .xlsx para posteriormente abrirlo en este programa.", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.txtRutaArchivo.Value = ofd.FileName;
                    this.btnAceptar.Select();
                    this.btnAceptar.Focus();
                }
            }
            ValidateClauses();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        void ValidateClauses()
        {
            bool flag = false;
            if (!String.IsNullOrEmpty(this.txtTitulo.Value.Trim()) && 
                !String.IsNullOrEmpty(this.txtRutaArchivo.Value.Trim()) && 
                !this.cboxModeloDispositivo.IsNonSelectedTextSelected &&
                !this.cboxLocalidadRemitente.IsNonSelectedTextSelected)
            {
                flag = true;
            }

            this.btnAceptar.Enabled = flag;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmNuevoProyecto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                return;

            Response = (this.txtTitulo.Value.Trim(), this.txtRutaArchivo.Value, DispositivoExtensions.Parse(this.cboxModeloDispositivo.Value), this.cboxLocalidadRemitente.Value);
        }

        private void cboxModeloDispositivo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateClauses();
        }
    }
}
