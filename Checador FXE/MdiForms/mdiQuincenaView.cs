using Checador_FXE.Plantillas;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Excel;
using FlowCommonWorkcore;
using FlowControls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Checador_FXE.MdiForms
{
    internal partial class mdiQuincenaView : Form
    {
        internal ReporteAsistencias Report { get; }
        internal MainDesktop LegacyParent { get; }
        internal CafProjFile ActualCafProject { get; set; }
        internal string ProjectFullname { get; set; } = "-1";

        /// <summary>
        /// Constructor para opcion: "Nuevo proyecto"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rpt"></param>
        /// <param name="mdiParent"></param>
        internal mdiQuincenaView(string title, ReporteAsistencias rpt, MainDesktop mdiParent)
        {
            InitializeComponent();
            this.Text = title;
            this.Report = rpt;
            this.LegacyParent = mdiParent;
         
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
            this.Text = title;
            this.Report = rpt;
            this.LegacyParent = mdiParent;
            projCaf.MdiForm = this; this.ActualCafProject = projCaf;
            this.ProjectFullname = projFullname;

            LoadAllData();
        }

        Action<DataGridView> loadTurnosBySettings = (dgv) =>
        {
            dgv.Rows.Clear();

            foreach (HorarioTurno i in HorarioTurno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS))
            {
                dgv.Rows.Add(new[]
                {
                    $"{i.ID}",
                    $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Salida)}",
                    $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Salida)}"
                });
            }
        };

        void LoadAllData()
        {
            // Cargamos los valores en la visualizacion
            this.calendarAsistencias.FechaActual = DateOnly.Parse(Report.ReportPeriod.Start.ToString("d"));
            this.calendarEmpleadoCasteado.FechaActual = DateOnly.Parse(Report.ReportPeriod.Start.ToString("d"));

            // Cargamos los valores de las propiedades
            this.txtAreaRemitente.Value = "";
            this.txtLugarRemitente.Value = Properties.Settings.Default.LUGAR_REMITENTE_DEFAULT;
            this.dateFechaRemitente.Value = DateTime.Now;

            // Cargamos los chequeos obtenidos del analisis
            foreach (string key in Report.Chequeos.Keys)
            {
                string employeeName = key;
                ListViewItem lvitem = new ListViewItem(employeeName, 0)
                {
                    Tag = Report.Chequeos[key]
                };

                this.lviewRegistros.Items.Add(lvitem);
            }

            // Cargamos las configuraciones de horario que usaremos
            loadTurnosBySettings(this.dgvTurnosHorarios);   // TODO: ESTO ENTRA EN CONFLICTO CON LA FUNCION DE ABRIR, ASI QUE HAY QUE REFORMULARLO
            // Cargamos el tiempo maximo de retraso permitido
            this.txtMaximoRetrasoMinutosPermitidos.Value = new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0);

            // Ejecuciones requeridas
            this.flExtendedTabControl1.SelectedTab = this.pageParsingResults;
            this.flQuickAccessPanel1.PerformButtonClick(4);     // EJECUTAMOS AUTOMATICAMENTE EL CASTING
        }

        private void mdiQuincenaView_Load(object sender, EventArgs e)
        {
            this.splitContainer2.SplitterDistance = 570;
            this.splitContainer1.SplitterDistance = 280;
            this.splitResultadosCasting_Background.SplitterDistance = 275;
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

            foreach (DateTime i in diasAsistidos)
                this.calendarAsistencias.AddEvent(DateOnly.Parse(i.ToString("d")), "Registró", Color.Red);

            this.txtEmpleadoSeleccionado.Value = actualSelectedEmpleado.Text;

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
                        Text = i.Fecha.ToString("t"),
                        ImageIndex = 1,
                        StateImageIndex = 1,
                    };
                    item.SubItems.Add(i.Tipo.GetText());

                    this.lviewDayEvents.Items.Add(item);
                }

                Program.WriteStatus(true, $"Asistencias el empleado cargados con exito!");
            }
            catch (Exception ex)
            {
                Program.WriteStatus(false, "Error inesperado", $"{ex.Message}", $"{ex}");
            }
        }

        private void flQuickAccessPanel1_OnButtonClicked(object sender, ButtonClickedEventArgs e)
        {
            /* 
             * Ejecutamos las acciones correspondientes
             * */
            switch (e.Button.Name)
            {
                case "btnGuardar":
                    // TODO: METODO DE GUARDADO DEL PROYECTO
                    Program.WriteStatus(false, "Error inesperado", $"Funcion no implementada aun!", $"Funcion no implementada aun!");
                    break;
                case "btnCerrar":
                    if (MessageBox.Show("¿Seguro que deseas salir sin guardar los cambios previamente?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;

                    // Guardamos
                    // TODO: METODO DE GUARDADO DEL PROYECTO

                    this.Close(); // Cerramos
                    break;
                case "btnImprimir":
                    Program.WriteStatus(false, "Error inesperado", $"Funcion no implementada aun!", $"Funcion no implementada aun!");
                    break;
                case "btnGenerar":
                    #region CODIGO
                    string[] paths = GeneratePdf($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{Properties.Settings.Default.DEFAULT_FILENAME}.pdf");

                    if (paths.Length > 0 && paths[0] == "-1")
                        break;

                    // Abrimos los archivos generados
                    if (Properties.Settings.Default.ABRIR_SIEMPRE_AL_GENERAR)
                    {
                        foreach (string i in paths)
                        {
                            ProcessStartInfo psi = new ProcessStartInfo()
                            {
                                FileName = i,
                                UseShellExecute = true
                            };

                            Process.Start(psi);
                        }
                    }

                    Program.WriteStatus(true, "Generacion de informe finalizado con exito!");
                    #endregion
                    break;
                case "btnEjecutar":
                    //
                    // Ejecutamos el proceso de generacion de informe
                    //
                    #region
                    // Primero debemos validar que la configuracion de turnos y horarios se encuentre vacia
                    if (Utils.IsDgvEmpty(this.dgvTurnosHorarios))
                        break;

                    Program.WriteStatus(true, "Iniciando procesmiendo de casting...");
                    PairEmpleado_FechaAsistencia = null;    // Establecemos este valor default para evitar conflictos

                    // Obtiene el tiempo de entrada maximo
                    Func<TimeSpan, TimeSpan> _GetMaximumTime = (TimeSpan entradaNormal) => entradaNormal.Add(new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0));

                    // Obtiene el turno correspondiente del dia y el usuario indicado
                    Func<Dictionary<string, List<(DateOnly, int)>>, DateOnly, string, int> _GetTurn = delegate (Dictionary<string, List<(DateOnly, int)>> c, DateOnly fecha, string empNombre)
                    {
                        foreach (var o in c[empNombre])
                        {
                            if (o.Item1.Equals(fecha))
                                return o.Item2;
                        }

                        return -1;
                    };

                    // Evalua si llego a tiempo o presento un retardo. True en caso de ser un retardo y false en caso contrario
                    Func<TimeSpan, TimeSpan, bool> _IsARetardEntry = delegate (TimeSpan entReal, TimeSpan entIdeal)
                    {
                        int real = int.Parse($"{entReal.Hours}{entReal.Minutes}");
                        int ideal = int.Parse($"{entIdeal.Hours}{entIdeal.Minutes}");

                        return (real > ideal);
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
                                TipoAsistencia a = TipoAsistencia.FALTA;

                                if (dia.DayOfWeek is DayOfWeek.Sunday)
                                    a = TipoAsistencia.NINGUNO;

                                _diasAsistenciaPair.Add(DateOnly.FromDateTime(dia), a);
                            }

                            //MessageBox.Show(JsonConvert.SerializeObject(_diasAsistenciaPair, Newtonsoft.Json.Formatting.Indented));

                            /* 
                             * HACK: Aqui se debe de añadir la evaluacion de los dias trabajados
                             * */
                            _registro.Add(empNombre, _diasAsistenciaPair);
                        }

                        return _registro;
                    };

                    Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _PeriodoCasteado = BuildPeriodTimeList();

                    #region ANALIZAMOS EL CHEQUEO CON LOS HORARIOS Y TURNOS CONFIGURADOS
                    HorarioTurno[] _turnos = Utils.ParseHorariosTurnosByDgv(this.dgvTurnosHorarios);

                    foreach (var i in Report.Chequeos)
                    {
                        string empleado = i.Key;

                        foreach (Checada j in i.Value)
                        {
                            DateOnly today = DateOnly.Parse(j.Fecha.ToString("d"));
                            int turnOfToday = _GetTurn(Report.Turnos, today, empleado);

                            if (turnOfToday == -1)
                                throw new IndexOutOfRangeException("No se ha encontrado el turno correspondiente al dia indicado");

                            TimeSpan limiteEntrada = _GetMaximumTime(_turnos.Cast<HorarioTurno>()
                                                                            .Where(h => h.ID == turnOfToday)
                                                                            .Select(t => t.PrimerHorario.Entrada)
                                                                            .FirstOrDefault());
                            //
                            // CON ESTA LINEA NOSOTROS LO QUE HACEMOS ES SABER SI TUVO UN RETARDO O UNA ASISTENCIA
                            //
                            _PeriodoCasteado[empleado][today] = _IsARetardEntry(TimeSpan.Parse($"{j.Fecha.Hour:00}:{j.Fecha.Minute:00}"), limiteEntrada) ? TipoAsistencia.RETARDO : TipoAsistencia.FALTA;
                        }
                    }
                    #endregion

                    Program.WriteStatus(true, "Procesando resultados en interfaz...");

                    /*
                     * FALTA MOSTRAR LOS RESULTADOS DE ASISTENCIAS FINALES
                     * */
                    this.txtBusqueda.Value = "";
                    this.treePagingResultadosCasting.Items.Clear();

                    List<InteropGenericObject> _list = new List<InteropGenericObject>();

                    foreach (string empleado in _PeriodoCasteado.Keys)
                    {
                        _list.Add(InteropGenericObject.Compatibilize(empleado, "", _PeriodoCasteado[empleado], new HexaHash().ToString(), 1, 1));
                    }

                    PairEmpleado_FechaAsistencia = _PeriodoCasteado;
                    CASTING_RESULT = _list;
                    this.treePagingResultadosCasting.Items = _list;

                    this.lblTotalDeEmpleados.InfoLabelText = _PeriodoCasteado.Keys.Count().ToString();

                    Program.WriteStatus(true, $"Ejecucion de casting finalizada con exito!");
                    #endregion
                    break;
            }
        }

        string ACTUAL_EMPLEADO_SELECCIONADO = "-1";
        /// <summary>
        /// LISTA DE CASTING ORIGINAL
        /// </summary>
        List<InteropGenericObject> CASTING_RESULT = new List<InteropGenericObject>();
        /// <summary>
        /// Direccion que relacionada:
        /// Empleado -> (Fecha -> Tipo de asistencia en la fecha)
        /// </summary>
        Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>? PairEmpleado_FechaAsistencia = null;

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
                path = Reporteador.Generate(pdf_path, Report, PairEmpleado_FechaAsistencia, new ReportProperties(
                        this.txtAreaRemitente.Value,
                        this.txtLugarRemitente.Value,
                        DateOnly.FromDateTime(this.dateFechaRemitente.Value.HasValue ? this.dateFechaRemitente.Value.Value : DateTime.Now),
                        this.txtNombreElaborador.Value,
                        this.txtAutorizador.Value
                    ));
                Program.WriteStatus(true, $"Informes PDF ({path.Length}), listos...");

                // TODO: AÑADIMOS UNA FUNCION PARA EN CASO DE QUE SEA MAS DE UN ARCHIVO PDF GENERADO, UNIRLOS EN UNO SOLO?
            }
            catch (Exception ex)
            {
                //path = { "-1" };
                Program.WriteStatus(false, "Error inesperado", $"Ha ocurrido un error inesperado! {ex.Message}", ex.ToString());
            }

            return path;
        }

        /// <summary>
        /// Ruta del documento PDF a imprimir
        /// </summary>
        /// <param name="pdf_path"></param>
        void PrintReport(string pdf_path)
        {

        }

        private void btnSincronizarAjustes_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas sincronizar los horarios? Perderas las modificaciones locales que haz hecho", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            // Cargamos las configuraciones de horario que usaremos
            loadTurnosBySettings(this.dgvTurnosHorarios);

            this.txtMaximoRetrasoMinutosPermitidos.Value = new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0);
        }

        private void treePagingResultadosCasting_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            /* 
             * MOSTRAMOS LOS REGISTROS DEL EMPLEADO SELECCIONADO
             * */
            this.calendarEmpleadoCasteado.ClearEvents();
            Dictionary<DateOnly, TipoAsistencia> registros = e.Object.GenericObject as Dictionary<DateOnly, TipoAsistencia>;

            try
            {
                foreach (DateOnly dia in registros.Keys)
                {
                    this.calendarEmpleadoCasteado.AddEvent(dia, (registros[dia] is TipoAsistencia.NINGUNO ? "" : registros[dia].GetText()), Color.FromKnownColor(KnownColor.ActiveCaptionText));
                }
                ACTUAL_EMPLEADO_SELECCIONADO = e.Node.Text;

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
            this.treePagingResultadosCasting.Items = _list;
        }

        private void calendarEmpleadoCasteado_OnDayDoubleClick(object sender, DayCalendarEventArgs e)
        {
            /* 
             * Mostramos las propiedades de ese dia seleccionado
             * */
            if (e.Events is null || e.Events.Length == 0)
            {
                Program.WriteStatus(false, $"Debes seleccionar un empleado para continuar!");
                return;
            }

            List<string> optionLists = new List<string>();
            foreach (var i in Enum.GetValues<TipoAsistencia>())
            {
                optionLists.Add(i.GetText());
            }

            popUpComboBoxEntry frmPopUp = new popUpComboBoxEntry(e.Date, optionLists.ToArray(), e.Events.Cast<CalendarEvent>().FirstOrDefault().Texto);

            if (frmPopUp.ShowDialog() == DialogResult.Cancel)
                return;

            if (ACTUAL_EMPLEADO_SELECCIONADO == "-1")
                return;

            // Indicamos el nuevo estatus seleccionado
            try
            {
                // Actualizamos los eventos registrados
                for (int i = 0; i < CASTING_RESULT.Count; i++)
                {
                    if (CASTING_RESULT[i].ObjectTitle == ACTUAL_EMPLEADO_SELECCIONADO)
                    {
                        Dictionary<DateOnly, TipoAsistencia> registros = CASTING_RESULT[i].GenericObject as Dictionary<DateOnly, TipoAsistencia>;
                        registros[e.Date] = TipoAsistenciaExtensions.Parse(frmPopUp.Response);

                        CASTING_RESULT[i].GenericObject = registros;
                        break;
                    }
                }

                // Recargamos los registros del empleado seleccionado
                this.treePagingResultadosCasting.Items = CASTING_RESULT;

                // Recargamos los eventos
                this.calendarEmpleadoCasteado.ClearEvents();
                InteropGenericObject? iop = CASTING_RESULT.Cast<InteropGenericObject>()
                                                                .Where(i => i.ObjectTitle == ACTUAL_EMPLEADO_SELECCIONADO)
                                                                .FirstOrDefault();
                if (iop == null)
                    return;

                Dictionary<DateOnly, TipoAsistencia> _registros = iop.GenericObject as Dictionary<DateOnly, TipoAsistencia>;

                foreach (DateOnly dia in _registros.Keys)
                {
                    this.calendarEmpleadoCasteado.AddEvent(dia, (_registros[dia] is TipoAsistencia.NINGUNO ? "" : _registros[dia].GetText()), Color.FromKnownColor(KnownColor.ActiveCaptionText));
                }
            }
            catch (Exception ex)
            {
                Program.WriteStatus(false, "Error inesperado", $"{ex.Message}", $"{ex}");
            }
        }

        private void mdiQuincenaView_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* 
             * Eliminamos el nodo correspondiente en el arbol de quincenas abiertas
             * */
            foreach (TreeNode i in LegacyParent.treeViewProyectosQuincenas.Nodes)
            {
                if (i.Tag.ToString().Equals(this.Tag.ToString()))
                {
                    LegacyParent.treeViewProyectosQuincenas.Nodes.Remove(i);
                    break;
                }
            }
        }

        private void treePagingResultadosCasting_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void splitResultadosCasting_Background_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
