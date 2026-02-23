using Checador_FXE.Plantillas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        enum Fields 
        {
            
        }

        void ValidateClauses()
        {

        }
    }
}
