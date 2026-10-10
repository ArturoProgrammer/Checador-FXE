using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Inputs;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Checador_FXE
{
    internal static class Program
    {
        internal static ToolStripStatusLabel? lblStatus;
        internal static ToolStripStatusLabel? lblOperation;
        private static readonly string _MutexName = $"ChecadorFXE-{Environment.Version}";
        internal static string DbPath = $@"{Application.StartupPath}\dbs";
        internal static CultureInfo CurrentCultureInfo { get; }  = new CultureInfo("es-MX");
        internal static int DefaultRowHeight { get; } = 30;
        internal static DataGridStyle StandardGridStyle { get; } = DataGridStylesGallery.BlueStyle.SetAutoSizeColumnsMode(DataGridViewAutoSizeColumnsMode.AllCells);
        internal static FormStyle StandardFormStyle { get; } = FormStylesGallery.BlueStyle;

        static string logsDir = Path.Combine(Application.StartupPath, "logs");
        static string logFilePath = $@"{logsDir}\execution_log_{DateTime.Now.ToString("ddMMyy-HHmm")}.txt";

        internal static SystemLog Log = new SystemLog(
            logFilePath,
            true,
            /*
            new SoftwareProperties()
            {
                AssemblyVersion = ProjectMetadataReader.GetAttributeValue("AssemblyVersion").Split(".").Select(t => int.Parse(t)).ToArray(),
                Author = ProjectMetadataReader.GetAttributeValue("Copyright"),
                Company = ProjectMetadataReader.GetAttributeValue("Company"),
                Compilation = "-1",
                Description = ProjectMetadataReader.GetAttributeValue("Description"),
                ID = "-1",
                Name = ProjectMetadataReader.GetAttributeValue("AssemblyTitle"),
            },*/
            new SoftwareProperties
            {
                AssemblyVersion = new[] { 1, 0, 0, 0 } ,
                Author = "IDC Arturo Venegas",
                Company = "Grupo Mexico Transportes",
                Compilation = Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
                Description = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "",
                ID = "-1",
                Name = "Checador FXE",
            },
            OnLiveSave: true);

        /// <summary>
        /// Nombre del recurso del formato de asistencia de la primer quincena del mes
        /// </summary>
        public static readonly (string Name, string Ext) FORMATO_ASIST_1_PROPS = ("FORMATO_ASISTENCIA_TAB_1_15_form", "pdf");
        /// <summary>
        /// Nombre del recurso del formato de asistencia de la segunda quincena del mes
        /// </summary>
        public static readonly (string Name, string Ext) FORMATO_ASIST_2_PROPS = ("FORMATO_ASISTENCIA_TAB_16_31_form", "pdf");

        /// <summary>
        /// Establece el icono de elemento seleccionado
        /// </summary>
        internal static Action<flExtendedDataGridView, EventArgs> DefaultRowSelectionChanged = (sender, e) =>
        {
            // Establecemos el icono de seleccionado
            if (sender.Rows.Count > 0 && sender.SelectedRows.Count > 0)
                if (sender.SelectedRows[0].Cells.Count > 0)
                    sender.SelectedRows[0].Cells[EmpleadosGridCells.ICON.GetIndex()].Value = IconGallery.NeutralObjectGreenSelected.Render(IconSize.S_64);
        };
        /// <summary>
        /// Reestablece el icono de elemento no seleccionado
        /// </summary>
        internal static Action<flExtendedDataGridView, DataGridViewCellCancelEventArgs> DefaultRowValidating = (sender, e) =>
        {
            // Establecemos el icono de no seleccionado
            if (sender.Rows.Count > 0 && sender.SelectedRows.Count > 0)
                if (sender.SelectedRows[0].Cells.Count > 0)
                    sender.Rows[e.RowIndex].Cells[EmpleadosGridCells.ICON.GetIndex()].Value = IconGallery.NeutralObjectGreenUnselected.Render(IconSize.S_64);
        };
        /// <summary>
        /// Valida el valor de turno ingresado en la celda
        /// </summary>
        internal static Action<Action<bool, string>, flExtendedDataGridView, List<DataGridViewRow>, DataGridViewCellValidatingEventArgs> DefaultCellValidating = (_WriteStatus, sender, actualView, e) =>
        {
            // Validar índices
            if (e.RowIndex < 0 || e.RowIndex >= sender.Rows.Count)
                return;

            var row = sender.Rows[e.RowIndex];

            // Ignorar fila nueva o columnas no editables
            if (row.IsNewRow || e.ColumnIndex <= RelacionHorariosGridCells.NOMBRE_COMP.GetIndex())
            {
                _WriteStatus(false, "No se puede editar esta celda!");
                return;
            }

            var cell = row.Cells[e.ColumnIndex];

            // Obtener valores sin llamar ToString() sobre null
            string oldValue = Convert.ToString(cell?.Value) ?? string.Empty;
            string newValue = e.FormattedValue?.ToString()?.Trim() ?? string.Empty;

            // Limpiar antes de validar
            if (cell != null)
                cell.ErrorText = string.Empty;

            // Toma lo que el usuario está intentando dejar en la celda
            int input = -1;

            if (String.IsNullOrEmpty(newValue))
                return;

            bool isNotNumber = !int.TryParse(newValue, out input);
            
            /*
            if (!int.TryParse(newValue, out input))
                isNotNumber = true;
            */

            // Valida que el ID exista en la lista de horarios
            if (!Utils.GetHorariosIDs().Contains(input) || isNotNumber)
            {
                List<string> turns = new List<string>();
                foreach (Turno i in Turno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS))
                    turns.Add($"* {i.ID} ({i.Nombre})");

                flMessageBox.Show($"Turno invalido. Escribe una de la lista disponible.\n\n{String.Join("\n", turns)}\n\nSi deseas agregar un turno nuevo, debes agregarlo desde Propiedades > Ajustes de Horario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;   // No permite salir de la celda
                return;
            }

            // Limpia error si es valido
            if (cell != null)
                cell.ErrorText = string.Empty;

            // Guardamos la nueva informacion
            if (actualView != null && e.RowIndex >= 0 && e.RowIndex < actualView.Count)
                actualView[e.RowIndex].Cells[e.ColumnIndex].Value = cell?.Value;

            _WriteStatus(true, $"Valor de celda actualizado de '{oldValue}' -> '{newValue}'");
        };


        /// <summary>
        /// Escribe el estatus y el texto de la operacion que se esta llevando acabo
        /// </summary>
        internal static void WriteStatus(bool Status, string Text)
        {
            if (lblStatus is null || lblOperation is null)
                return;

            lblStatus.ForeColor = Status ? Color.FromKnownColor(KnownColor.Green) : Color.FromKnownColor(KnownColor.IndianRed);
            lblStatus.Text = Status ? "Listo" : "Error";
            lblOperation.Text = $"[ {DateTime.Now:t} ] {Text}";
        }

        /// <summary>
        /// Escribe el estatus y el texto de la operacion que se esta llevando acabo y mostraremos un mensaje de error
        /// </summary>
        internal static void WriteStatus(bool Status, string Text, string ErrorMessage, string ErrorStack)
        {
            WriteStatus(Status, Text);
            ReportFailureExposed(Text, ErrorMessage, ErrorStack);

            flMessageBox.Show(ErrorMessage, "Error Inesperado - Seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static void ReportFailureExposed(string txt, string msg, string ers)
        {
            //System.Diagnostics.Debug.WriteLine($@"
            Trace.WriteLine($@"

*** ======================== [ ❌ ERROR INESPERADO] ======================== ***
    {txt}

    ERROR MESSAGE: {msg}

    ERROR STACK: {ers}
============================================================================

");
            Log.Add(EventType.ERROR, $"{msg}\n\n{ers}");
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
                //Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);

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

                FlowCommonWorkcore.UtilityFunctions.CreateDirectory(DbPath, ForceOverwrite: false);
                FlowCommonWorkcore.UtilityFunctions.CreateDirectory(logsDir, ForceOverwrite: false);

                Log.Add(EventType.OKAY, $"Manejadores de excepciones inicializados");
                Log.Add(EventType.OKAY, $"Directorios obligatorios creados");

                RelacionHorarios.InitializeDb();
                Empleado.InitializeDb();
                GlobalConfig.InitializeDb();
                Log.Add(EventType.OKAY, $"Bases de datos inicializadas");

                MainDesktop _mainDesktop = new MainDesktop();
                Application.Run(_mainDesktop);

                SingleInstance.Release(_mutex); // Liberamos el Mutex
            } else
            {
                flMessageBox.Show("Ya existe una instancia de la aplicacion abierta actualmente!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}