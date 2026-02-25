using Checador_FXE.Plantillas;
using FlowControls.Utils;
using SpreadsheetLight;

namespace Checador_FXE
{
    public partial class frmExportadorDeConfiguracion : Form
    {
        public frmExportadorDeConfiguracion(Empleado[] data, string localidadOrigen)
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecciona la plantilla de configuracion...";
                ofd.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx, *.xls)|*.xlsx;*.xls";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Extension == ".xls")
                    {
                        MessageBox.Show("El archivo proporcionado es formato '*.xls' por lo que se debe convertir a '*.xlsx'. Abre el archivo .xls en Excel y guardalo en formato .xlsx para posteriormente abrirlo en este programa.", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.txtRutaIngreso.Value = ofd.FileName;
                }
            }
            ValidateClauses();
        }

        private void btnExaminarDestino_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx)|*.xlsx";
                dialog.InitialDirectory = CafProjFile.DefaultProjFilePath;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                this.txtRutaIngreso.Value = dialog.FileName;
            }
            ValidateClauses();
        }

        enum Fields
        {
            [ControlValidateAttrib("txtRutaIngreso", ControlField.FLTEXTBOXLABELJOINT)]
            Origen,
            [ControlValidateAttrib("txtRutaDestino", ControlField.FLTEXTBOXLABELJOINT)]
            Destino
        }

        void ValidateClauses()
        {
            Multivalidator mv = new Multivalidator(this);

            bool origen = mv.Validate<Fields>(Fields.Origen, new[] { ValidationParams.NOT_EMPTY_ENTRY }).Success;
            bool destino = mv.Validate<Fields>(Fields.Destino, new[] { ValidationParams.NOT_EMPTY_ENTRY }).Success;

            this.btnAceptar.Enabled = origen && destino;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Iniciamos el proceso de llenado del archivo de configuracion
            try
            {
                using (SLDocument sl = new SLDocument(this.txtRutaIngreso.Value))
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al procesar el archivo de plantilla. {ex.Message}\n{ex}", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmExportadorDeConfiguracion_Load(object sender, EventArgs e)
        {

        }
    }
}
