using DocumentFormat.OpenXml.InkML;
using Org.BouncyCastle.Tls;
using System.Runtime.CompilerServices;

namespace Checador_FXE
{
    internal static class Program
    {
        internal static ToolStripStatusLabel? lblStatus;
        internal static ToolStripStatusLabel? lblOperation;
        private static readonly string _MutexName = $"ChecadorFXE-{Environment.Version}";

        /// <summary>
        /// Nombre del recurso del formato de asistencia de la primer quincena del mes
        /// </summary>
        public static readonly (string Name, string Ext) FORMATO_ASIST_1_PROPS = ("FORMATO_ASISTENCIA_TAB_1_15_form", "pdf");
        /// <summary>
        /// Nombre del recurso del formato de asistencia de la segunda quincena del mes
        /// </summary>
        public static readonly (string Name, string Ext) FORMATO_ASIST_2_PROPS = ("FORMATO_ASISTENCIA_TAB_16_31_form", "pdf");


        static Action<bool, string> _writeStatusCommon = (s, t) =>
        {
            lblStatus.ForeColor = s ? Color.FromKnownColor(KnownColor.Green) : Color.FromKnownColor(KnownColor.IndianRed);
            lblStatus.Text = s ? "Listo" : "Error";
            lblOperation.Text = $"[ {DateTime.Now:t} ] {t}";
        };

        /// <summary>
        /// Escribe el estatus y el texto de la operacion que se esta llevando acabo
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Text"></param>
        internal static void WriteStatus(bool Status, string Text)
        {
            if (lblStatus == null || lblOperation == null)
                return;

            _writeStatusCommon(Status, Text);
        }

        /// <summary>
        /// Escribe el estatus y el texto de la operacion que se esta llevando acabo y mostraremos un mensaje de error
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Text"></param>
        internal static void WriteStatus(bool Status, string Text, string ErrorMessage, string ErrorStack)
        {
            if (lblStatus == null || lblOperation == null)
                return;

            _writeStatusCommon(Status, Text);
            System.Diagnostics.Debug.WriteLine($@"

*** ============ [ERROR INESPERADO] ============ ***
    {Text}

    ERROR MESSAGE: {ErrorMessage}

    ERROR STACK: {ErrorStack}
====================================================

");
            MessageBox.Show(ErrorMessage, "Error Inesperado - Seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Descargamos el recurso si no existe en el directorio de la aplicacion
        /// </summary>
        /// <param name="resourceName"></param>
        static void _DownloadResource((string name, string ext) resourceName)
        {
            string completeName = $"{resourceName.name}.{resourceName.ext}";
            string resourcePath = Path.Combine(Application.StartupPath, completeName);

            try
            {
                //if (File.Exists(resourcePath))
                //    return;

                //MessageBox.Show($"Iniciando la descarga del recurso: '{resourceName.name}'");

                // Validamos la existencia del nombre del recurso en el gestor del recursos
                var rm = Properties.Resources.ResourceManager;
                string[] resNames = rm.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true)
                                    .OfType<System.Collections.DictionaryEntry>()
                                    .Select(de => de.Key.ToString())
                                    .ToArray();
                //MessageBox.Show($"*{resourceName.name}* \n\n {String.Join("\n", resNames)}\n\n---[ {(resNames.Contains(resourceName.name) ? "SI" : "NO")} CONTIENE EL RECURSO ]---");

                if (resNames == null || !resNames.Contains(resourceName.name))
                    return;

                //MessageBox.Show("Descargando...");

                using (FileStream fs = new FileStream(resourcePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] resourceBytes = (byte[])Properties.Resources.ResourceManager.GetObject(resourceName.name)!;
                    fs.Write(resourceBytes, 0, resourceBytes.Length);
                }
                //MessageBox.Show($"Recurso '{resourceName.name}' descargado con exito en: '{resourcePath}'");
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ocurrio un error al descargar el recurso '{resourceName.name}' en: '{resourcePath}'. {ex.Message}\n{ex}");
            }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex _mutex;

            if (SingleInstance.Acquire(_MutexName, out _mutex))
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                //
                // EXCEPCIONES PRODUCIDAS EN HILLOS SECUNDARIOS NO DE LA INTERFAZ GRÁFICA
                //
                AppDomain.CurrentDomain.UnhandledException += (object s, UnhandledExceptionEventArgs e) => {
                    WriteStatus(false, "Ocurrió un error inesperado. Por favor, contacte al soporte técnico.", ((Exception)e.ExceptionObject).Message, ((Exception)e.ExceptionObject).ToString());
                };

                //
                // EXCEPCIONES PRODUCIDAS EN HILLOS DE INTERFAZ GRÁFICA
                //
                Application.ThreadException += (object s, ThreadExceptionEventArgs e) =>
                {
                    WriteStatus(false, "Ocurrió un error inesperado. Por favor, contacte al soporte técnico.", e.Exception.Message, e.Exception.ToString());
                };

                //
                // NOS ASEGURAMOS DE QUE LOS RECURSOS CORRESPONDIENTES EXISTAN EN EL DIRECTORIO
                //
                _DownloadResource(FORMATO_ASIST_1_PROPS);
                _DownloadResource(FORMATO_ASIST_2_PROPS);

                MainDesktop _mainDesktop = new MainDesktop();
                Application.Run(_mainDesktop);

                SingleInstance.Release(_mutex); // Liberamos el Mutex
            } else
            {
                MessageBox.Show("Ya existe una instancia de la aplicacion abierta actualmente!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}