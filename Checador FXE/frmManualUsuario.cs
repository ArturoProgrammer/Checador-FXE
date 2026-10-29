using Microsoft.Web.WebView2.WinForms;

namespace Checador_FXE
{
    /// <summary>
    /// Muestra el manual HTML incluido con la aplicación en un explorador WebView2 local.
    /// </summary>
    internal sealed class frmManualUsuario : Form
    {
        private readonly WebView2 webView = new()
        {
            Dock = DockStyle.Fill
        };

        public frmManualUsuario()
        {
            Text = "Ayuda - Manual de usuario";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 600);
            Size = new Size(1100, 800);
            Controls.Add(webView);
            Load += frmManualUsuario_Load;
        }

        private async void frmManualUsuario_Load(object? sender, EventArgs e)
        {
            string manualPath = Path.Combine(Application.StartupPath, "MANUAL_USUARIO.html");

            if (!File.Exists(manualPath))
            {
                MessageBox.Show(
                    $"No se encontró el manual de usuario en:\n{manualPath}",
                    "Ayuda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            try
            {
                await webView.EnsureCoreWebView2Async();
                webView.Source = new Uri(manualPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible iniciar el visor de ayuda. Verifique que Microsoft Edge WebView2 Runtime esté instalado.\n\n" + ex.Message,
                    "Ayuda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
