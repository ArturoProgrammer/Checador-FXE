using Checador_FXE.Plantillas;
using FlowControls;
using FlowControls.Inputs;

namespace Checador_FXE
{
    internal partial class frmNuevoProyecto : Form
    {
        public (string Titulo, string Path, Dispositivo Device, string LocalidadRemitente) Response;

        public frmNuevoProyecto()
        {
            InitializeComponent();
        }

        bool FastCreation = false;
        string fastCreationPath = "-1";

        /// <summary>
        /// Creacion rapida de proyecto
        /// </summary>
        /// <param name="path"></param>
        public frmNuevoProyecto(string rptPath)
        {
            InitializeComponent();
            FastCreation = true;
            fastCreationPath = rptPath;
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

#if DEV_R1
            if (FastCreation)
            {
                this.txtTitulo.Value = "Reporte de Prueba Rapida (DEV MODE)";
                this.txtRutaArchivo.Value = fastCreationPath;
                // Localidad = por defecto
                // Aparato checador = por defecto

                ValidateClauses();
                this.btnAceptar.PerformClick();
            }
#endif
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
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Extension.ToLower() == ".xls")
                    {
                        flMessageBox.Show("El archivo proporcionado es formato '*.xls' por lo que se debe convertir a '*.xlsx'. Abre el archivo .xls en Excel y guardalo en formato .xlsx para posteriormente abrirlo en este programa.", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // Validamos que exista personal y relacion de horarios asignados para esta localidad
            string site = this.cboxLocalidadRemitente.Value.Trim();
            int? personalCount = Empleado.GetAll(site).Object?.Length;
            bool havePersonal = personalCount != null && personalCount > 0;

            //int? horariosCount = RelacionHorarios.Get(RelacionHorarioID.GetActualId(site)).Object?;
            //bool haveHorarios = horariosCount != null && personalCount > 0;

            if (!havePersonal)
            {
                flMessageBox.Show("No se puede crear el objeto debido a que no existen datos de referencia para la localidad seleccionada.\n\nAsegura la existencia de personal registrado en esa localidad y su relacion de horario correspondiente.",
                                  "Nuevo proyecto",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning,
                                  FormStylesGallery.BlueStyle);
                return;
            }

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
