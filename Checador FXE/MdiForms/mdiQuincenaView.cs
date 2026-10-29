//#define DEBUG_MODE

using Checador_FXE.Plantillas;
using FlowCommonWorkcore;
using FlowControls;
using FlowControls.Inputs;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace Checador_FXE.MdiForms
{
    internal partial class mdiQuincenaView : Form
    {
        internal ReporteAsistencias Report { get; }
        internal MainDesktop LegacyParent { get; }
        internal CafProjFile ActualCafProject { get; set; }
        internal RelacionHorarios RelacionHorarioSelected { get; set; }
        internal string ProjectFullname { get; set; } = "-1";
        internal string LugarRemitente { get; set; } = Properties.Settings.Default.LOCALIDAD_DEFAULT;

        bool projByCafOpened = false;

        void includeDebugTools()
        {
#if DEV_R1
            this.flQuickAccessPanel1.Buttons.Add(new QuickAccessButton()
            {
                Name = "btnDebTools",
                Text = "DEBUG TOOLS"
            });

            Program.Log.Add("DEBUG TOOLS inicializadas.");
#endif
        }

        /// <summary>
        /// Constructor para opcion: "Nuevo proyecto"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rpt"></param>
        /// <param name="mdiParent"></param>
        internal mdiQuincenaView(string title, ReporteAsistencias rpt, MainDesktop mdiParent, string localidadRemitente)
        {
            InitializeComponent();
            includeDebugTools();
            this.Text = title;
            this.Report = rpt;
            this.LegacyParent = mdiParent;
            this.RelacionHorarioSelected = RelacionHorarios.Get(rpt.RelacionID).Object ?? throw new Exception("Error durante el parseo de datos");
            this.LugarRemitente = localidadRemitente;

            LoadAllData();

            this.ActualCafProject = new CafProjFile(this); // Creamos el objeto de proyecto
        }

        /// <summary>
        /// Constructor para opcion: "Abrir proyecto"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rpt"></param>
        /// <param name="mdiParent"></param>
        /// <param name="projCaf"></param>
        /// <param name="projFullname"></param>
        internal mdiQuincenaView(string title, ReporteAsistencias rpt, MainDesktop mdiParent, CafProjFile projCaf, string projFullname)
        {
            InitializeComponent();
            includeDebugTools();
            projByCafOpened = true;

            this.Text = title;
            this.Report = rpt;
            this.LegacyParent = mdiParent;
            projCaf.MdiForm = this; this.ActualCafProject = projCaf;
            this.ProjectFullname = projFullname;
            this.LugarRemitente = projCaf.General.LugarRemitente;
            //this.RelacionHorarioSelected = projCaf.ConfiguracionCasting.Relacion ?? throw new Exception("Error durante el parseo de datos");
            this.RelacionHorarioSelected = RelacionHorarios.Parse(projCaf.ConfiguracionCasting.RelacionTurnosID,
                                                                    projCaf.ConfiguracionCasting.TurnosEmpleadoJson,
                                                                    projCaf.ConfiguracionCasting.RelacionHash.ToString(), ShowObjectLog: false).Object ?? throw new Exception("Error durante el parseo de datos");
            LoadAllData();
        }

        Action<DataGridView, string> loadTurnos = (dgv, turnosJson) =>
        {
            dgv.Rows.Clear();
            foreach (Turno i in Turno.GetAll(turnosJson))
                dgv.Rows.Add(new[]
                {
                    $"{i.ID}",
                    $"{i.Nombre}",
                    Utils.WriteNotEmptyTimes(i.PrimerHorario.Entrada, i.PrimerHorario.EntradaEstablecida),
                    Utils.WriteNotEmptyTimes(i.PrimerHorario.Salida, i.PrimerHorario.SalidaEstablecida),
                    Utils.WriteNotEmptyTimes(i.SegundoHorario.Entrada, i.SegundoHorario.EntradaEstablecida),
                    Utils.WriteNotEmptyTimes(i.SegundoHorario.Salida, i.SegundoHorario.SalidaEstablecida)
                });
        };

        void LoadAllData()
        {
            // Inicializamos los DGV con sus propiedades comunes
            foreach (flExtendedDataGridView dgv in new[] { this.dgvRelacionDeHorarios, this.dgvTurnosHorarios })
            {
                dgv.SetGridStyle(Program.StandardGridStyle);
                dgv.RowTemplate.Height = Program.DefaultRowHeight;
                dgv.AllowUserToResizeRows = false;
            }

            // Cargamos los valores en la visualizacion
            this.calendarAsistencias.FechaActual = DateOnly.Parse(Report.ReportPeriod.Start.ToString("d"));
            this.calendarEmpleadoCasteado.FechaActual = DateOnly.Parse(Report.ReportPeriod.Start.ToString("d"));

            // Cargamos los valores de las propiedades
            this.txtAreaRemitente.Value = projByCafOpened ? ActualCafProject.General.AreaRemitente : "";
            this.txtLugarRemitente.Value = projByCafOpened ? ActualCafProject.General.LugarRemitente : LugarRemitente;
            this.dateFechaRemitente.Value = projByCafOpened ? ActualCafProject.General.Fecha.ToDateTime(TimeOnly.MinValue) : DateTime.Now;
            this.txtNombreElaborador.Value = projByCafOpened ? ActualCafProject.General.NombreElaborador : "";
            this.txtAutorizador.Value = projByCafOpened ? ActualCafProject.General.Autorizador : "";

            int counter = 1;
            // Cargamos los chequeos obtenidos del analisis
            foreach (string employeeName in Report.Chequeos.Keys)
            {
                int noEmp = Report.Turnos.Items.FirstOrDefault(t => t.Nombre.Equals(employeeName, StringComparison.InvariantCultureIgnoreCase))!.NoEmp;
                ListViewItem lvitem = new ListViewItem("", 0)
                {
                    Tag = Report.Chequeos[employeeName],
                    ImageIndex = 0,
                    StateImageIndex = 1,
                };
                lvitem.SubItems.AddRange(new string[]
                {
                    $"{counter}",
                    $"{noEmp}",
                    employeeName,
                    $"{Report.Chequeos[employeeName].Length}"
                });

                this.lviewRegistros.Items.Add(lvitem);
                counter++;
            }

            // Cargamos las configuraciones de horario que usaremos
            // Un proyecto CAF debe conservar los horarios con que se generó, incluso si la
            // configuración global cambió después. Esto también conserva correctamente los turnos nocturnos.
            loadTurnos(this.dgvTurnosHorarios.Grid,
                projByCafOpened ? ActualCafProject.ConfiguracionCasting.HorariosTurnosCrudJson : Properties.Settings.Default.TURNOS_HORARIOS);
            // Cargamos el tiempo maximo de retraso permitido
            this.txtMaximoRetrasoMinutosPermitidos.Value = projByCafOpened
                ? ActualCafProject.ConfiguracionCasting.TiempoRetrasoPermitido
                : new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0);
            this.chckDomingosNoLaborables.Checked = projByCafOpened
                ? ActualCafProject.ConfiguracionCasting.DomingosNoLaborables
                : this.chckDomingosNoLaborables.Checked;

            /*
             * SE MUEVE AL METODO LOAD DEL FORMULARIO DEBIDO A ERRORES DE INVOKE, YA QUE SE ESTABAN
             * INTENTANDO ACCEDER A RECURSOS QUE AUN NO ERAN CREADOS
             * 
            //Ejecuciones requeridas
            this.flExtendedTabControl1.SelectedTab = this.pageParsingResults;
            this.flQuickAccessPanel1.PerformButtonClick(4);     // EJECUTAMOS AUTOMATICAMENTE EL CASTING
            */

            // Carga de la relacion mensual correspondiente al proyecto
            string _site = RelacionHorarioSelected.ID.Site;
            int _year = RelacionHorarioSelected.ID.Year;
            int _month = DateTime.Parse($"01-{RelacionHorarioSelected.ID.Month}-0001").Month;
            Response loadProccess = RelacionHorarioSelected.LoadCrudBaseView(this.dgvRelacionDeHorarios, _month, _year, LugarRemitente);

            this.flLabelHeader2.HeaderText += $" ({_site}/{RelacionHorarioSelected.ID.Month}/{_year})";

            if (!loadProccess.Success)
                Program.WriteStatus(false, $"Error inesperado. {loadProccess.Message}!");
        }

        private void mdiQuincenaView_Load(object sender, EventArgs e)
        {
            // Las pestañas todavía pueden estar ajustando su tamaño durante Load. Se difiere
            // la distribución para usar el espacio final del formulario, no valores absolutos.
            BeginInvoke((MethodInvoker)ConfigureInitialSplitContainers);

            this.dgvRelacionDeHorarios.SetGridStyle(DataGridStylesGallery.BlueStyle);
            this.dgvRelacionDeHorarios.MouseHoverEffectEnabled = false;
            this.dgvTurnosHorarios.SetGridStyle(DataGridStylesGallery.BlueStyle);
            this.dgvTurnosHorarios.MouseHoverEffectEnabled = false;

            // Cargamos las imagenes del imageList1
            this.imageList1.Images.Clear();
            IconSize imgLstSz = IconSize.S_32;
            this.imageList1.Images.AddRange(new Image[]
            {
                IconGallery.NeutralObjectGreenUnselected.Render(imgLstSz),    // => 0
                IconGallery.NeutralObjectGreenSelected.Render(imgLstSz),      // => 1
                IconGallery.NeutralObjectYellowUnselected.Render(imgLstSz),   // => 2
                IconGallery.NeutralObjectYellowSelected.Render(imgLstSz),     // => 3
                IconGallery.NeutralObjectRedSelected.Render(imgLstSz),        // => 4
                IconGallery.NeutralObjectRedUnselected.Render(imgLstSz),      // => 5
                IconGallery.NeutralObjectOrangeSelected.Render(imgLstSz),     // => 6
                IconGallery.NeutralObjectOrangeUnselected.Render(imgLstSz),   // => 7
            });

            #region Coloreamos los dias del reporte en ambos EventCalendar
            Color BG_C = Color.Moccasin;

            List<DateOnly> rptDays = new List<DateOnly>();
            DateOnly start = DateOnly.Parse(this.Report.ReportPeriod.Start.ToString("yyyy-MM-dd"));
            DateOnly end = DateOnly.Parse(this.Report.ReportPeriod.End.ToString("yyyy-MM-dd"));

            for (DateOnly i = start; i <= end; i = i.AddDays(1))
            {
                rptDays.Add(i);
            }

            this.calendarAsistencias.PaintDay(rptDays.ToArray(), BG_C);
            this.calendarEmpleadoCasteado.PaintDay(rptDays.ToArray(), BG_C);
            #endregion

            // Ejecuciones requeridas
            this.flExtendedTabControl1.SelectedTab = this.pageParsingResults;
            this.flQuickAccessPanel1.PerformButtonClick(4);     // EJECUTAMOS AUTOMATICAMENTE EL CASTING

            // Establecemos las filas innaccesibles
            this.dgvRelacionDeHorarios.SetLockedColumns(2);
        }

        void ConfigureInitialSplitContainers()
        {
            if (IsDisposed)
                return;

            // Vista de registros: calendario y detalle arriba, lista de empleados abajo.
            ConfigureHorizontalSplit(splitContainer1, preferredPanel2Size: 180, panel1Minimum: 250, panel2Minimum: 150);

            // Calendario de checadas y detalle diario: ambos deben crecer con la ventana.
            ConfigureVerticalSplit(splitContainer2, preferredPanel1Ratio: .60, panel1Minimum: 380, panel2Minimum: 280);

            // Resultados del casting: árbol de empleados y calendario del empleado seleccionado.
            ConfigureVerticalSplit(splitResultadosCasting_Background, preferredPanel1Ratio: .32, panel1Minimum: 240, panel2Minimum: 380);
        }

        static void ConfigureHorizontalSplit(SplitContainer container, int preferredPanel2Size, int panel1Minimum, int panel2Minimum)
        {
            int availableSize = Math.Max(0, container.ClientSize.Height - container.SplitterWidth);
            int effectivePanel1Minimum = Math.Min(panel1Minimum, Math.Max(0, availableSize - panel2Minimum));
            int effectivePanel2Minimum = Math.Min(panel2Minimum, Math.Max(0, availableSize - effectivePanel1Minimum));

            container.Panel1MinSize = effectivePanel1Minimum;
            container.Panel2MinSize = effectivePanel2Minimum;
            container.SplitterDistance = Math.Clamp(
                availableSize - preferredPanel2Size,
                effectivePanel1Minimum,
                availableSize - effectivePanel2Minimum);
        }

        static void ConfigureVerticalSplit(SplitContainer container, double preferredPanel1Ratio, int panel1Minimum, int panel2Minimum)
        {
            int availableSize = Math.Max(0, container.ClientSize.Width - container.SplitterWidth);
            int effectivePanel1Minimum = Math.Min(panel1Minimum, Math.Max(0, availableSize - panel2Minimum));
            int effectivePanel2Minimum = Math.Min(panel2Minimum, Math.Max(0, availableSize - effectivePanel1Minimum));
            int preferredDistance = (int)Math.Round(availableSize * preferredPanel1Ratio);

            container.Panel1MinSize = effectivePanel1Minimum;
            container.Panel2MinSize = effectivePanel2Minimum;
            container.SplitterDistance = Math.Clamp(
                preferredDistance,
                effectivePanel1Minimum,
                availableSize - effectivePanel2Minimum);
        }

        ListViewItem actualSelectedEmpleado = null;
        Checada[] actualChequeosDiaSeleccionado = null;

        private void lviewRegistros_DoubleClick(object sender, EventArgs e)
        {
            // Mostramos la visualizacion de asistencias correspondientes a ese empleado
            actualSelectedEmpleado = this.lviewRegistros.SelectedItems[0];
            Checada[]? selectedData = actualSelectedEmpleado.Tag as Checada[];

            /* 
             * Cargamos los valores primero en el calendario
             * */
            this.calendarAsistencias.ClearEvents(); // Limpiamos el control antes de cargar los nuevos resultados
            List<DateTime> diasAsistidos = new List<DateTime>();

            foreach (Checada i in selectedData)
            {
                if (!diasAsistidos.Contains(i.Fecha))
                    diasAsistidos.Add(i.Fecha);
            }

            Dictionary<DateOnly, int> regs = new Dictionary<DateOnly, int>();

            foreach (DateTime i in diasAsistidos)
            {
                DateOnly day = DateOnly.Parse(i.ToString("d"));
                if (!regs.ContainsKey(day))
                    regs.Add(day, 0);
                regs[day] = selectedData.Count(d => d.Fecha.DayOfYear == day.DayOfYear);
            }

            foreach (DateOnly i in regs.Keys)
                this.calendarAsistencias.AddEvent(DateOnly.Parse(i.ToString("d")), $"{regs[i]} Registro(s)", Color.Red);

            this.txtEmpleadoSeleccionado.Value = actualSelectedEmpleado.SubItems[3]!.Text;

            /* 
             * Limpiamos los valores del listView de registros del dia seleccionado
             * */
            this.lviewDayEvents.Items.Clear();
        }

        private void calendarAsistencias_OnDayDoubleClick(object sender, DayCalendarEventArgs e)
        {
            /* 
             * Cargamos los eventos del dia seleccionado
             * */
            try
            {
                if (actualSelectedEmpleado == null)
                {
                    this.lviewDayEvents.Items.Clear();  // Limpiamos los eventos
                    this.dateFechaSeleccionada.Value = null; // Indicamos el dia seleccionado
                    return;
                }

                this.lviewDayEvents.Items.Clear();  // Limpiamos los eventos

                Checada[]? selectedData = actualSelectedEmpleado.Tag as Checada[];
                actualChequeosDiaSeleccionado = selectedData!.Cast<Checada>().Where(t => t.Fecha.Date == DateTime.Parse($"{e.Date}")).ToArray();

                this.dateFechaSeleccionada.Value = DateTime.Parse(e.Date.ToString()); // Indicamos el dia seleccionado

                // En listamos los eventos de registro del dia
                foreach (Checada i in actualChequeosDiaSeleccionado)
                {
                    ListViewItem item = new ListViewItem()
                    {
                        ImageIndex = 2,
                        StateImageIndex = 3,
                    };
                    item.SubItems.Add(i.Fecha.ToString("t"));
                    item.SubItems.Add(i.Tipo.GetText());

                    this.lviewDayEvents.Items.Add(item);
                }

                Program.WriteStatus(true, $"Asistencias del empleado '{this.txtEmpleadoSeleccionado.Value}' para el dia '{e.Date}' cargados con exito!");
            }
            catch (Exception ex)
            {
                Program.WriteStatus(false, "Error inesperado", $"{ex.Message}", $"{ex}");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.G:
                    this.flQuickAccessPanel1.PerformButtonClick("btnGuardar");
                    break;
                case Keys.Control | Keys.P:
                    this.flQuickAccessPanel1.PerformButtonClick("btnImprimir");
                    break;
                case Keys.F5:
                    this.flQuickAccessPanel1.PerformButtonClick("btnGenerar");
                    break;
                case Keys.Control | Keys.E:
                    this.flQuickAccessPanel1.PerformButtonClick("btnEjecutar");
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Bandera para saber si el archivo esta guardado
        /// </summary>
        internal bool SavedFlag = false;

        private void flQuickAccessPanel1_OnButtonClicked(object sender, ButtonClickedEventArgs e)
        {
            /* 
             * Ejecutamos las acciones correspondientes
             * */
            switch (e.Button.Name)
            {
                case "btnGuardar":
                    SavedFlag = false;
                    this.LegacyParent.guardarToolStripMenuItem.PerformClick();
                    break;
                case "btnCerrar":
                    #region CODIGO
                    DialogResult d_r = flMessageBox.Show("¿Deseas salir sin guardar los cambios realizados?", "Confirmacion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (d_r == DialogResult.No)
                    {
                        SavedFlag = false;
                        this.LegacyParent.guardarToolStripMenuItem.PerformClick(); // Guardamos

                        if (SavedFlag)
                            this.Close();
                    }
                    else if (d_r == DialogResult.Yes)
                    {
                        this.Close(); // Solo salimos
                    }
                    else
                    {
                        return; // Cancelamos la operacion
                    }
                    #endregion
                    break;
                case "btnImprimir":
                    #region CODIGO
                    {
                        string[] _paths = GeneratePdf($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{Properties.Settings.Default.DEFAULT_FILENAME}.pdf");

                        if (_paths == null || _paths.Length == 0 || _paths[0] == "-1")
                        {
                            flMessageBox.Show("No hay archivo disponible para imprimir. Ejecuta 'Ejecutar' para generar datos antes de imprimir.", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }

                        foreach (string filePath in _paths)
                        {
                            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                            {
                                flMessageBox.Show("No hay archivo disponible para imprimir.", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            }

                            using (PrintDialog pd = new PrintDialog())
                            {
                                pd.AllowSomePages = false;
                                pd.AllowSelection = false;
                                pd.AllowCurrentPage = false;
                                pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

                                if (pd.ShowDialog() != DialogResult.OK)
                                    break;

                                try
                                {
                                    // Otros formatos: usar la app asociada vía Shell (PrintTo) apuntando a la impresora seleccionada
                                    var psi = new ProcessStartInfo
                                    {
                                        FileName = filePath,
                                        Arguments = $"\"{pd.PrinterSettings.PrinterName}\"",
                                        Verb = "printto",
                                        CreateNoWindow = true,
                                        WindowStyle = ProcessWindowStyle.Hidden,
                                        UseShellExecute = true
                                    };
                                    var p = Process.Start(psi);

                                    // Intentar esperar un breve periodo a que la aplicación lance la impresión
                                    if (p != null)
                                    {
                                        try { p.WaitForExit(10000); } catch { }
                                    }
                                    else
                                    {
                                        // Si Process.Start devolvió null (caso de shell), esperar un poco antes de eliminar archivos
                                        System.Threading.Thread.Sleep(2000);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    flMessageBox.Show($"Error al imprimir: {ex.Message}", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        // Eliminamos los archivos correspondientes de forma segura
                        foreach (string p in _paths)
                        {
                            try
                            {
                                if (File.Exists(p))
                                    File.Delete(p);
                            }
                            catch { /* Ignorar errores al eliminar archivos temporales */ }
                        }
                    }
                    #endregion
                    break;
                case "btnGenerar":
                    #region CODIGO PARA INVOCACION DE FUNCION DE GENERACION PDF
                    string[] paths = GeneratePdf($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{Properties.Settings.Default.DEFAULT_FILENAME}.pdf");

                    if (paths.Length > 0 && paths[0] == "-1")
                        break;

                    // Abrimos los archivos generados
                    if (Properties.Settings.Default.ABRIR_SIEMPRE_AL_GENERAR)
                        foreach (string i in paths)
                        {
                            ProcessStartInfo psi = new ProcessStartInfo()
                            {
                                FileName = i,
                                UseShellExecute = true
                            };

                            Process.Start(psi);
                        }

                    Program.WriteStatus(true, "Generacion de informe finalizado con exito!");
                    #endregion
                    break;
                case "btnEjecutar":
                    #region Ejecutamos el proceso de generacion de informe
                    // Primero debemos validar que la configuracion de turnos y horarios se encuentre vacia
                    if (Utils.IsDgvEmpty(this.dgvTurnosHorarios.Grid))
                        break;

                    Program.WriteStatus(true, "Iniciando procesamiendo del casting...");
                    PairEmpleado_FechaAsistencia = null;    // Establecemos este valor default para evitar conflictos
                    PairEmpleado_FechaAsistenciaOriginal = null;

                    #region HELPERS DE CODIGO
                    RelacionHorarios _relac = this.RelacionHorarioSelected.UpdateByGrid(this.dgvRelacionDeHorarios.Rows.Cast<DataGridViewRow>().Select(r => r).ToArray());

                    // Obtiene el turno correspondiente del dia y el usuario indicado (con validaciones)
                    /* 
                     * VERSION NUEVA DE LA FUNCION EN LA CUAL YA SE IMPLEMENTA LA CLASE DE TURNOS, 
                     * POR LO QUE SE SIMPLIFICA BASTANTE EL PROCESO DE OBTENCION DEL TURNO CORRESPONDIENTE
                     * Esta versión añade comprobaciones para evitar NullReference/IndexOutOfRange.
                     */
                    Func<TurnoEmpleadoCollection, DateOnly, string, int> _GetTurn = delegate (TurnoEmpleadoCollection c, DateOnly fecha, string empNombre)
                    {
                        var emp = c.Items.Cast<TurnoEmpleado>().FirstOrDefault(e => e.Nombre == empNombre);
                        if (emp == null)
                            return -1;

                        try
                        {
                            var info = c[emp.NoEmp, fecha.Day];
                            return info?.Turno ?? -1;
                        }
                        catch
                        {
                            return -1;
                        }
                    };

                    // Determina si el empleado debe trabajar ese día.
                    // Devuelve NINGUNO si no hay turno asignado o si el turno es -1 (descanso),
                    // devuelve FALTA por defecto cuando se sabe que debía trabajar y no hay registro.
                    Func<int, DateOnly, TipoAsistencia> _HaveToWorkToday = delegate (int NoEmp, DateOnly dia)
                    {
                        // Si NoEmp no es válido, asumimos que no hay asignación y por tanto es descanso
                        if (NoEmp <= 0)
                            return TipoAsistencia.NINGUNO;

                        // Usar la relación ya cargada cuando esté disponible evita reconstruirla por mes/año
                        //RelacionHorarios _relac = this.RelacionHorarioSelected ?? throw new Exception("No se obtuvo la relacion correspondiente");


                        TurnoEmpleado? _target = null;
                        try
                        {
                            _target = _relac.Relacion[NoEmp, dia.Day];
                        }
                        catch
                        {
                            // Si no se encuentra la asociación, considerarlo como descanso
                            return TipoAsistencia.NINGUNO;
                        }

                        if (_target is null)
                            return TipoAsistencia.NINGUNO;

                        if (_target.Turno <= 0)
                            return TipoAsistencia.NINGUNO;

                        return TipoAsistencia.FALTA;
                    };

                    // Construye el diccionario correspondiente de fechas del periodo correspondiente
                    Func<Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>> BuildPeriodTimeList = delegate ()
                    {
                        Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _registro = new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>();

                        foreach (string empNombre in Report.Chequeos.Keys)
                        {
                            Dictionary<DateOnly, TipoAsistencia> _diasAsistenciaPair = new Dictionary<DateOnly, TipoAsistencia>();

                            // Agregamos todos los dias del periodo a reportear
                            for (DateTime dia = Report.ReportPeriod.Start; dia <= Report.ReportPeriod.End; dia = dia.AddDays(1))
                            {
                                // Obtener número de empleado preferentemente desde registros de checadas; si no existe, será 0
                                int numEmp = Report.Chequeos[empNombre].Select(t => t.NumEmpleado).FirstOrDefault();

                                TipoAsistencia a = _HaveToWorkToday(numEmp, DateOnly.FromDateTime(dia)); // Buscamos si era un dia laborable segun su turno asignado

                                if (dia.DayOfWeek is DayOfWeek.Sunday)
                                    a = TipoAsistencia.NINGUNO;

                                _diasAsistenciaPair.Add(DateOnly.FromDateTime(dia), a);
                            }

                            /* 
                             * HACK: Aqui se debe de añadir la evaluacion de los dias trabajados
                             * */
                            _registro.Add(empNombre, _diasAsistenciaPair);
                        }

                        return _registro;
                    };
                    #endregion

                    bool useSavedCasting = projByCafOpened && ActualCafProject.ResultadosCasting.PeriodoCasteado.Count > 0;
                    Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _PeriodoCasteado = useSavedCasting
                        ? CloneCastingResults(ActualCafProject.ResultadosCasting.PeriodoCasteado)
                        : BuildPeriodTimeList();
                    //PeriodoCasteadoCollection _PeriodoCasteado = projByCafOpened ? ActualCafProject.ResultadosCasting.PeriodoCasteado : BuildPeriodTimeList();

                    if (!useSavedCasting)
                    {
                        #region ANALIZAMOS EL CHEQUEO CON LOS HORARIOS Y TURNOS CONFIGURADOS
                        Turno[] _turnos = Utils.ParseHorariosTurnosByDgv(this.dgvTurnosHorarios);   // Hay que modificar aqui

                        foreach (var i in Report.Chequeos)
                        {
                            string empleado = i.Key;

                            foreach (Checada j in i.Value.Where(c => c.Tipo == TipoChecada.ENTRADA))
                            {
                                int noEmp = j.NumEmpleado;
                                DateOnly fechaCalendario = DateOnly.FromDateTime(j.Fecha);
                                int turnoActual = this.RelacionHorarioSelected.Relacion[noEmp, fechaCalendario.Day]?.Turno ?? -1;
                                Turno? configuracionActual = _turnos.Where(t => t.ID == turnoActual).Select(t => (Turno?)t).FirstOrDefault();

                                // Una entrada ocurrida después de medianoche puede pertenecer al turno
                                // nocturno asignado al día anterior. La asistencia siempre se registra en
                                // la fecha de inicio del turno, nunca en la fecha de salida.
                                DateOnly fechaLaboral = fechaCalendario;
                                int turnoLaboral = turnoActual;
                                Turno? configuracionLaboral = configuracionActual;
                                if (fechaCalendario.Day > 1)
                                {
                                    int turnoAnterior = this.RelacionHorarioSelected.Relacion[noEmp, fechaCalendario.Day - 1]?.Turno ?? -1;
                                    Turno? configuracionAnterior = _turnos.Where(t => t.ID == turnoAnterior).Select(t => (Turno?)t).FirstOrDefault();
                                    if (configuracionAnterior.HasValue && CalendarioTurnos.CruzaMedianoche(CalendarioTurnos.ObtenerHorarioLaboral(configuracionAnterior.Value)) &&
                                        CalendarioTurnos.FechaLaboralDeChecada(j.Fecha, configuracionAnterior.Value) == fechaCalendario.AddDays(-1))
                                    {
                                        fechaLaboral = fechaCalendario.AddDays(-1);
                                        turnoLaboral = turnoAnterior;
                                        configuracionLaboral = configuracionAnterior;
                                    }
                                }

                                if (turnoLaboral <= 0 || !configuracionLaboral.HasValue)
                                {
                                    continue;
                                }

                                _PeriodoCasteado[empleado][fechaLaboral] = CalendarioTurnos.EsRetardo(
                                    j.Fecha,
                                    fechaLaboral,
                                    configuracionLaboral.Value,
                                    new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0))
                                    ? TipoAsistencia.RETARDO
                                    : TipoAsistencia.ASISTENCIA;
                            }
                        }
                        #endregion
                    }

                    Program.WriteStatus(true, "Procesando resultados en interfaz...");

                    /*
                     * FALTA MOSTRAR LOS RESULTADOS DE ASISTENCIAS FINALES
                     * */
                    this.txtBusqueda.Value = "";
                    this.treePagingResultadosCasting.Items.Clear();

                    ObservableCollection<InteropGenericObject> _list = new ObservableCollection<InteropGenericObject>();

                    PairEmpleado_FechaAsistencia = CloneCastingResults(_PeriodoCasteado);
                    PairEmpleado_FechaAsistenciaOriginal = useSavedCasting
                        ? CloneCastingResults(ActualCafProject.ResultadosCasting.PeriodoCasteadoOriginal)
                        : CloneCastingResults(_PeriodoCasteado);

                    foreach (string empleado in PairEmpleado_FechaAsistencia.Keys)
                        _list.Add(InteropGenericObject.Compatibilize(empleado,
                                                                    "",
                                                                    PairEmpleado_FechaAsistencia[empleado],
                                                                    new HexaHash().Hash,
                                                                    0, 1));

                    CASTING_RESULT = _list;
                    this.treePagingResultadosCasting.Items = _list;

                    this.lblTotalDeEmpleados.InfoLabelText = PairEmpleado_FechaAsistencia.Keys.Count().ToString();

                    Program.WriteStatus(true, $"Ejecucion de casting finalizada con exito!");
                    #endregion
                    break;
#if DEV_MODE
                case "btnDebTools":
                    #region HERRAMIENTAS DEBUG DISPONIBLES
                    flDialogResult<string> result = flComboBoxInput.Show("Herramientas de depuracion", "Selecciona una opcion", new[]
                    { "[1] - Agregar registro ficticio" }, formStyle: FormStylesGallery.SapClassicStyle);

                    if (result.DialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(result.Response))
                        break;

                    string choice = result.Response.Split('-')[0].Trim();
                    switch (choice)
                    {
                        case "[1]": // Agregar registro ficticio
                            {
                                flDialogResult<string> selectionBox_Empleado = flListItemExplorerBox.Show("Empleados del proyecto actual",
                                                                                                          Report.Chequeos.Keys.ToArray(),
                                                                                                          FormStylesGallery.SapClassicStyle);
                                if (selectionBox_Empleado.DialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(selectionBox_Empleado.Response))
                                    return;

                                flDialogResult<string> selectionBox_Day = flListItemExplorerBox.Show("Dias disponibles",
                                                                                                     Utils.GetReportDates(Report.ReportPeriod.Start, Report.ReportPeriod.End).Select(t => t.ToString("d")).ToArray(),
                                                                                                     FormStylesGallery.SapClassicStyle);
                                if (selectionBox_Day.DialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(selectionBox_Day.Response))
                                    return;
                                /* 
                                 * Agregamos entradas aleatorias en el dia indicado.
                                 * Genera entre 1 y 4 marcas (preferentemente 2-4). Si hay marcas
                                 * con horas que retroceden (p. ej. 22:00 seguido de 06:00) se
                                 * ajusta la segunda marca al día siguiente para simular turnos nocturnos.
                                 */
                                List<Checada> PRE_DEAD = Report.Chequeos[selectionBox_Empleado.Response].ToList();

                                DateTime baseDate;
                                if (!DateTime.TryParse(selectionBox_Day.Response, out baseDate))
                                    baseDate = DateTime.Now.Date;
                                baseDate = baseDate.Date;
                                int count = Random.Shared.Next(2, 5); // 2..4 marcas

                                // Generamos horas aleatorias (no ordenadas a propósito para simular cruces)
                                List<DateTime> marcas = new List<DateTime>();
                                for (int i = 0; i < count; i++)
                                {
                                    int hour = Random.Shared.Next(0, 24);
                                    int minute = Random.Shared.Next(0, 60);
                                    marcas.Add(baseDate.AddHours(hour).AddMinutes(minute));
                                }

                                // Ordenamos marcas por hora del dia para asignar tipos de forma cronologica
                                marcas = marcas.OrderBy(m => m.TimeOfDay).ToList();

                                // Ajuste: si detectamos un cruce (una marca con TimeOfDay menor que la precedente), la consideramos del dia siguiente
                                for (int i = 1; i < marcas.Count; i++)
                                {
                                    if (marcas[i].TimeOfDay < marcas[i - 1].TimeOfDay)
                                        marcas[i] = marcas[i].AddDays(1);
                                }

                                int numEmp = Report.Chequeos[selectionBox_Empleado.Response].FirstOrDefault()?.NumEmpleado ?? -1;

                                for (int i = 0; i < marcas.Count; i++)
                                {
                                    TipoChecada tipo;
                                    if (marcas.Count == 1)
                                        tipo = TipoChecada.ENTRADA;
                                    else if (marcas.Count == 2)
                                        tipo = (i == 0) ? TipoChecada.ENTRADA : TipoChecada.SALIDA;
                                    else if (marcas.Count == 3)
                                    {
                                        tipo = i switch
                                        {
                                            0 => TipoChecada.ENTRADA,
                                            1 => TipoChecada.IDA_COMIDA,
                                            2 => TipoChecada.SALIDA,
                                            _ => TipoChecada.UNKWN
                                        };
                                    }
                                    else // 4 o más
                                    {
                                        tipo = i switch
                                        {
                                            0 => TipoChecada.ENTRADA,
                                            1 => TipoChecada.IDA_COMIDA,
                                            2 => TipoChecada.REGRESO_COMIDA,
                                            3 => TipoChecada.SALIDA,
                                            _ => TipoChecada.UNKWN
                                        };
                                    }

                                    PRE_DEAD.Add(new Checada()
                                    {
                                        Fecha = marcas[i],
                                        Empleado = selectionBox_Empleado.Response,
                                        NumEmpleado = numEmp,
                                        Tipo = tipo
                                    });
                                }

                                // Reemplazamos el array con las nuevas marcas añadidas
                                var updatedArray = PRE_DEAD.ToArray();
                                this.Report.Chequeos[selectionBox_Empleado.Response] = updatedArray;

                                // Actualizamos el Tag del ListViewItem correspondiente si existe (el nombre del empleado está en SubItems[3])
                                var lvItem = this.lviewRegistros.Items.Cast<ListViewItem>()
                                                    .FirstOrDefault(l => l.SubItems.Count > 3 && l.SubItems[3].Text == selectionBox_Empleado.Response);
                                if (lvItem != null)
                                    lvItem.Tag = updatedArray;

                                // Añadimos los dias generados al calendario de visualización de registros
                                // Obtenemos las fechas distintas (DateOnly) de las marcas añadidas
                                try
                                {
                                    var dias = updatedArray.Select(c => DateOnly.FromDateTime(c.Fecha.Date)).Distinct();
                                    foreach (var d in dias)
                                    {
                                        // Añadimos un evento visual para indicar que hay registro
                                        this.calendarAsistencias.AddEvent(d, "Registró", Color.Red);
                                    }

                                    // Si el empleado modificado está actualmente seleccionado, refrescamos la vista del día
                                    if (actualSelectedEmpleado != null && actualSelectedEmpleado.SubItems.Count > 3 && actualSelectedEmpleado.SubItems[3].Text == selectionBox_Empleado.Response)
                                    {
                                        // Simular la carga que hace el doble click en el listview
                                        this.calendarAsistencias.ClearEvents();
                                        var selectedData = updatedArray;
                                        var diasAsistidos = selectedData.Select(c => c.Fecha).Distinct();
                                        foreach (DateTime dt in diasAsistidos)
                                            this.calendarAsistencias.AddEvent(DateOnly.FromDateTime(dt.Date), "Registró", Color.Red);

                                        this.txtEmpleadoSeleccionado.Value = actualSelectedEmpleado.SubItems[3].Text;
                                        this.lviewDayEvents.Items.Clear();
                                    }
                                }
                                catch { /* Ignorar errores visuales en debug tools */ }
                            }
                            break;
                    }
                    #endregion
                    break;
#endif
            }
        }

        string ACTUAL_EMPLEADO_SELECCIONADO = "-1";
        /// <summary>
        /// LISTA DE CASTING ORIGINAL
        /// </summary>
        ObservableCollection<InteropGenericObject> CASTING_RESULT = new ObservableCollection<InteropGenericObject>();
        /// <summary>
        /// Direccion que relacionada:
        /// Empleado -> (Fecha -> Tipo de asistencia en la fecha)
        /// </summary>
        Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>? PairEmpleado_FechaAsistencia = null;
        Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>? PairEmpleado_FechaAsistenciaOriginal = null;

        internal Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> CurrentCastingResults =>
            PairEmpleado_FechaAsistencia is null
                ? new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>()
                : CloneCastingResults(PairEmpleado_FechaAsistencia);

        internal Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> OriginalCastingResults =>
            PairEmpleado_FechaAsistenciaOriginal is null
                ? new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>()
                : CloneCastingResults(PairEmpleado_FechaAsistenciaOriginal);

        static Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> CloneCastingResults(
            Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> source) => source.ToDictionary(
                employee => employee.Key,
                employee => new Dictionary<DateOnly, TipoAsistencia>(employee.Value));

        /// <summary>
        /// Ruta en la que se generara (guardara el pdf)
        /// </summary>
        /// <param name="pdf_path">Sufijo de la ubicacion en la que se guardara el/los PDF</param>
        /// <returns>Ruta absoluta (directorio y archivo) del archivo generado. En caso de algun error, retornara "-1"</returns>
        string[] GeneratePdf(string pdf_path)
        {
            string[] path = { "-1" };

            try
            {
                // Validar que existan resultados antes de generar
                if (PairEmpleado_FechaAsistencia == null)
                {
                    Program.WriteStatus(false, "No hay resultados para generar informe", "Ejecuta 'Ejecutar' antes de generar o imprimir.", "");
                    return new string[] { "-1" };
                }

                Response<string[]> generateProc = Reporteador.Generate(pdf_path, Report, PairEmpleado_FechaAsistencia, new ReportProperties(
                        this.txtAreaRemitente.Value,
                        this.txtLugarRemitente.Value,
                        DateOnly.FromDateTime(this.dateFechaRemitente.Value.HasValue ? this.dateFechaRemitente.Value.Value : DateTime.Now),
                        this.txtNombreElaborador.Value,
                        this.txtAutorizador.Value
                    ));

                if (generateProc.Success is false)
                    throw (generateProc.Tag as Exception)!;

                path = generateProc.Object!;

                Program.WriteStatus(true, $"Informes PDF ({path.Length}), listos...");
            }
            catch (Exception ex)
            {
                //path = { "-1" };
                Program.WriteStatus(false, "Error inesperado", $"Ha ocurrido un error inesperado! {ex.Message}", ex.ToString());
            }

            return path;
        }

        private void btnSincronizarAjustes_Click(object sender, EventArgs e)
        {
            if (flMessageBox.Show("¿Seguro que deseas sincronizar los horarios? Perderas las modificaciones locales que haz hecho", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            // Cargamos las configuraciones de horario que usaremos
            loadTurnos(this.dgvTurnosHorarios.Grid, Properties.Settings.Default.TURNOS_HORARIOS);

            this.txtMaximoRetrasoMinutosPermitidos.Value = new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0);
        }

        /// <summary>
        /// Funcion encargada de obtener el color para el texto segun tipo de asistencia
        /// </summary>
        Func<TipoAsistencia, Color> setColorOfAsistencia = (TipoAsistencia t) => t switch
        {
            TipoAsistencia.FALTA => Color.FromKnownColor(KnownColor.IndianRed),
            TipoAsistencia.VACACIONES => Color.FromKnownColor(KnownColor.Blue),
            TipoAsistencia.ASISTENCIA => Color.FromKnownColor(KnownColor.DarkGreen),
            TipoAsistencia.RETARDO => Color.OrangeRed,
            _ => Color.FromKnownColor(KnownColor.ActiveCaptionText)
        };

        private void treePagingResultadosCasting_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            /* 
             * MOSTRAMOS LOS REGISTROS DEL EMPLEADO SELECCIONADO
             * */
            try
            {
                ACTUAL_EMPLEADO_SELECCIONADO = e.Node.Text;
                RefreshSelectedEmployeeCalendar();

                Program.WriteStatus(true, "Registros el empleado cargados!");
            }
            catch (Exception ex)
            {
                Program.WriteStatus(false, "Error inesperado", $"{ex.Message}", $"{ex}");
            }
        }

        private void txtBusqueda_OnTextChanged(object sender, EventArgs e)
        {
            string searchText = this.txtBusqueda.Value.Trim();
            if (String.IsNullOrEmpty(searchText))
                this.treePagingResultadosCasting.Items = CASTING_RESULT;

            List<InteropGenericObject> _list = CASTING_RESULT.Cast<InteropGenericObject>()
                                                            .Where(i => i.ObjectTitle.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                                            .ToList();
            ObservableCollection<InteropGenericObject> _readyList = new ObservableCollection<InteropGenericObject>();
            foreach (InteropGenericObject i in _list)
                _readyList.Add(i);

            this.treePagingResultadosCasting.Items = _readyList;
        }

        private void calendarEmpleadoCasteado_OnDayDoubleClick(object sender, DayCalendarEventArgs e)
        {
            /* 
             * Mostramos las propiedades de ese dia seleccionado
             */
            var frmPopUp = flComboBoxInput.Show($"Propiedad del dia '{e.Date:dd/MM/yyyy}'",
                                                "Estatus:",
                                                Enum.GetValues<TipoAsistencia>().Select(i => i.GetText()).ToArray(),
                                                Program.StandardFormStyle);

            if (frmPopUp.DialogResult == DialogResult.Cancel)
                return;

            SetCustomAsistencia(TipoAsistenciaExtensions.Parse(frmPopUp.Response), e.Date);
        }

        private void mdiQuincenaView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (projByCafOpened)
                ActualCafProject.CleanupTemporaryFiles();
            /* 
             * Eliminamos el nodo correspondiente en el arbol de quincenas abiertas
             * */
            foreach (TreeNode i in LegacyParent.treeViewProyectosQuincenas.Nodes)
            {
                if (i.Tag.ToString()!.Equals(this.Tag!.ToString()))
                {
                    LegacyParent.treeViewProyectosQuincenas.Nodes.Remove(i);
                    Program.WriteStatus(true, $"Vista de '{ProjectFullname}' cerrada!");
                    break;
                }
            }
        }

        private void dgvAjustesHorarios_SelectionChanged(object sender, EventArgs e) => Program.DefaultRowSelectionChanged(this.dgvRelacionDeHorarios, e);

        private void dgvAjustesHorarios_RowValidating(object sender, DataGridViewCellCancelEventArgs e) => Program.DefaultRowValidating(this.dgvRelacionDeHorarios, e);


        DataGridViewRow[] _CloneRows()
        {
            var clones = new List<DataGridViewRow>();

            foreach (DataGridViewRow src in this.dgvRelacionDeHorarios.Rows)
            {
                // Clonar la estructura de la fila
                var newRow = (DataGridViewRow)src.Clone();

                // Asegurar que existan las celdas clonadas
                for (int i = 0; i < src.Cells.Count; i++)
                {
                    // Copiar valor
                    newRow.Cells[i].Value = src.Cells[i].Value;

                    // Copiar estilo visual básico
                    newRow.Cells[i].Style = src.Cells[i].Style;
                    newRow.Cells[i].ToolTipText = src.Cells[i].ToolTipText;

                    // Copiar Tag si existe (por seguridad, no referencia a controles)
                    try
                    {
                        newRow.Cells[i].Tag = src.Cells[i].Tag;
                    }
                    catch { }
                }

                // Copiar propiedades de fila
                newRow.HeaderCell.Value = src.HeaderCell.Value;
                try { newRow.Tag = src.Tag; } catch { }

                clones.Add(newRow);
            }

            return clones.ToArray();
        }

        private void btnExpandir_Click(object sender, EventArgs e)
        {
            // Visualizamos la relacion en una ventana externa

            frmCrudRelacionHorariosViewer frm = new frmCrudRelacionHorariosViewer(
                this.dgvRelacionDeHorarios.Columns.Cast<DataGridViewColumn>().Select(c => (DataGridViewColumn)c.Clone()).ToArray(),
                _CloneRows()
            );

            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                Program.WriteStatus(true, "Operacion cancelada por el usuario!");
                return;
            }

            // Actualizamos la relacion de horarios
            if (!frm.Response.Success)
                return;

            this.dgvRelacionDeHorarios.Rows.Clear();
            foreach (DataGridViewRow r in frm.Response.Object!)
                this.dgvRelacionDeHorarios.Rows.Add(r);

            Program.WriteStatus(true, "Relación de horarios actualizada!");
        }

        private void btnDefinirRelacionMensual_Click(object sender, EventArgs e)
        {
            // Actualizamos la relacion de horario actualmente seleccionado
            RelacionHorarioSelected = RelacionHorarioSelected.UpdateByGrid(this.dgvRelacionDeHorarios.Rows.Cast<DataGridViewRow>().ToArray());
            Program.WriteStatus(true, $"Relacion de horario actualizada!");
        }

        private void lviewRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lviewRegistros.SelectedItems.Count == 0)
                return;

            // Regresamos a indice 0 todos los indices de imagenes
            foreach (ListViewItem i in this.lviewRegistros.Items)
                i.ImageIndex = 0;

            int selectedIndex = this.lviewRegistros.SelectedItems[0].Index;
            this.lviewRegistros.Items[selectedIndex].ImageIndex = 1;    // Cambiamos el indice de la seleccion
        }

        private void calendarAsistencias_Click(object sender, EventArgs e)
        {

        }

        private void dgvTurnosHorarios_OnAddClick(object sender, EventArgs e)
        {
            int ROW_I = this.dgvTurnosHorarios.Rows.Add(1);    // Se agrega una linea por defecto

            // Establecemos el ID del turno segun el turno anterior
            this.dgvTurnosHorarios.Rows[ROW_I].Cells[0].Value =
                this.dgvTurnosHorarios.Rows.Count > 1
                ? (int.Parse(this.dgvTurnosHorarios.Rows[ROW_I - 1].Cells[0].Value.ToString()) + 1).ToString()
                : "1";
        }

        private void calendarEmpleadoCasteado_Click(object sender, EventArgs e)
        {

        }

        private void restaurarAOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_contextMenuEvArgs is null || PairEmpleado_FechaAsistenciaOriginal is null ||
                !PairEmpleado_FechaAsistenciaOriginal.TryGetValue(ACTUAL_EMPLEADO_SELECCIONADO, out var originalResults) ||
                !originalResults.TryGetValue(_contextMenuEvArgs.Date, out TipoAsistencia original))
            {
                Program.WriteStatus(false, "No existe un resultado original para restaurar.");
                return;
            }

            SetCustomAsistencia(original, _contextMenuEvArgs.Date);
        }

        void SetCustomAsistencia(TipoAsistencia asistencia, DateOnly date)
        {
            if (ACTUAL_EMPLEADO_SELECCIONADO == "-1" || PairEmpleado_FechaAsistencia is null ||
                !PairEmpleado_FechaAsistencia.TryGetValue(ACTUAL_EMPLEADO_SELECCIONADO, out Dictionary<DateOnly, TipoAsistencia>? records) ||
                !records.ContainsKey(date))
            {
                Program.WriteStatus(false, "Debes seleccionar un empleado y un día válido para continuar.");
                return;
            }

            try
            {
                records[date] = asistencia;
                InteropGenericObject? employeeItem = CASTING_RESULT
                    .FirstOrDefault(item => item.ObjectTitle == ACTUAL_EMPLEADO_SELECCIONADO);
                if (employeeItem is not null)
                    employeeItem.GenericObject = records;

                this.treePagingResultadosCasting.Items = CASTING_RESULT;
                RefreshSelectedEmployeeCalendar();
                Program.WriteStatus(true, $"Asistencia de '{ACTUAL_EMPLEADO_SELECCIONADO}' actualizada para {date:dd/MM/yyyy}.");
            }
            catch (Exception ex)
            {
                Program.WriteStatus(false, "Error inesperado", ex.Message, ex.ToString());
            }
        }

        void RefreshSelectedEmployeeCalendar()
        {
            this.calendarEmpleadoCasteado.ClearEvents();
            if (PairEmpleado_FechaAsistencia is null ||
                !PairEmpleado_FechaAsistencia.TryGetValue(ACTUAL_EMPLEADO_SELECCIONADO, out Dictionary<DateOnly, TipoAsistencia>? records))
                return;

            foreach (DateOnly day in records.Keys)
                this.calendarEmpleadoCasteado.AddEvent(day,
                    records[day] is TipoAsistencia.NINGUNO ? "" : records[day].GetText(),
                    setColorOfAsistencia(records[day]));
        }

        DayContextMenuEventArgs? _contextMenuEvArgs;

        private void calendarEmpleadoCasteado_OnDayContextMenuOpening(object sender, DayContextMenuEventArgs e)
        {
            _contextMenuEvArgs = e;
            bool canRestore = PairEmpleado_FechaAsistencia is not null &&
                PairEmpleado_FechaAsistenciaOriginal is not null &&
                PairEmpleado_FechaAsistencia.TryGetValue(ACTUAL_EMPLEADO_SELECCIONADO, out var currentResults) &&
                PairEmpleado_FechaAsistenciaOriginal.TryGetValue(ACTUAL_EMPLEADO_SELECCIONADO, out var originalResults) &&
                currentResults.TryGetValue(e.Date, out TipoAsistencia current) &&
                originalResults.TryGetValue(e.Date, out TipoAsistencia original) &&
                current != original;
            restaurarAOriginalToolStripMenuItem.Enabled = canRestore;
        }

        private void asignarAsistenciaToolStripMenuItem_Click(object sender, EventArgs e) { if (_contextMenuEvArgs is not null) SetCustomAsistencia(TipoAsistencia.ASISTENCIA, _contextMenuEvArgs.Date); }
        private void asignarFaltaToolStripMenuItem_Click(object sender, EventArgs e) { if (_contextMenuEvArgs is not null) SetCustomAsistencia(TipoAsistencia.FALTA, _contextMenuEvArgs.Date); }
        private void asignarDescansoToolStripMenuItem_Click(object sender, EventArgs e) { if (_contextMenuEvArgs is not null) SetCustomAsistencia(TipoAsistencia.DESCANSO_LABORADO, _contextMenuEvArgs.Date); }
        private void asignarDescansoSemanalToolStripMenuItem_Click(object sender, EventArgs e) { if (_contextMenuEvArgs is not null) SetCustomAsistencia(TipoAsistencia.DESCANSO_SEM, _contextMenuEvArgs.Date); }
        private void asignarVacacionesToolStripMenuItem_Click(object sender, EventArgs e) { if (_contextMenuEvArgs is not null) SetCustomAsistencia(TipoAsistencia.VACACIONES, _contextMenuEvArgs.Date); }

    }
}
