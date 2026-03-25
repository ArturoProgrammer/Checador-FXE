using Checador_FXE.Plantillas;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils.MySQL;
using FlowControls;
using ICSharpCode.TextEditor.Actions;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZstdSharp.Unsafe;

namespace Checador_FXE
{
    internal class CrudEventLog
    {
        //
        // TODO: Reportador de eventos sobre acciones CRUD a la BD; Ticket ##100188##
        //
        readonly string DataBaseName = "checador_fxe_db";
        readonly string TableName = "events_log";

        public string Title { get; } = "";
        public string Description { get; } = "";
        public int NoEmpleado { get; } = 0;

        public CrudEventLog(int _noemp, string _title, string _description) 
        { 
            this.Title = _title;
            this.Description = _description;
            this.NoEmpleado = _noemp;
        }

        /// <summary>
        /// Envia un mensaje al servidor informando sobre una accion CRUD realizada.
        /// </summary>
        /// <param name="title">Titulo del evento</param>
        /// <param name="message">Mensaje del evento</param>
        /// <returns>Respuesta de la funcion.</returns>
        [Description("Envia un mensaje al servidor informando sobre una accion CRUD realizada.")]
        internal Response SendMessage()
        {
            #region CODIGO
            Response _resp = new Response(false, "Iniciando consulta de Log de accion CRUD...");

            string hostname = Environment.MachineName;
            _resp.Log.Add($"Hostname del equipo obtenido...");
            HexaHash hash = new HexaHash(12);
            _resp.Log.Add("HASH para el evento generado...");
            
            Response SERV_RESP = new Server.GeneralQuery(new ConnectionsData(
                Properties.Settings.Default.SERVER_HOSTNAME,
                Properties.Settings.Default.SERVER_USER,
                Properties.Settings.Default.SERVER_PASS,
                Int32.Parse(Properties.Settings.Default.SERVER_PORT),
                TableName,
                DataBaseName
            )).ExecuteNonQuery(
                $@"INSERT INTO {DataBaseName}.{TableName} (Titulo, Descripcion, Hostname, HASH, NoEmp) VALUES (@Titulo, @Descripcion, @Hostname, @HASH, @NoEmp)",
                new (string, object)[] { 
                    ("@Titulo", this.Title.Trim()),
                    ("@Descripcion", this.Description.Trim()),
                    ("@Hostname", hostname),
                    ("@HASH", hash.ToString()),
                    ("@Fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    ("@NoEmp", this.NoEmpleado),
                }
            );
            _resp.Log.Add("Consulta NonQuery realiza...");
            _resp.Success = SERV_RESP.Success;

            if (!SERV_RESP.Success)
            {
                _resp.Log.Add(SERV_RESP.Message);
                _resp.Message = "No se pudo realizar la consulta!";
                MessageBox.Show($"Ha ocurrido un error inesperado en la comunicacion con el servidor! {SERV_RESP.Message}", "Excepcion inesperada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return _resp;
            }

            _resp.Message = "Consulta al servidor realizada con exito!";

            return _resp;
            #endregion
        }

        #region PLANTILLAS
        /// <summary>
        /// Plantillas del objeto
        /// </summary>
        public static class Templates
        {
            public static CrudEventLog CreateNewEmployee(string nombre, int no_emp) =>
                new CrudEventLog(no_emp, $"Adicion de empleado", $"Se añadio un nuevo registro de empleado; '{no_emp}' - '{nombre}' agregado con exito!");

            public static CrudEventLog UpdateEmployee(int no_emp, string param_name) =>
                new CrudEventLog(no_emp, $"Modificacion de empleado", $"Se modifico el parametro '{param_name}'");

            public static CrudEventLog DeleteEmployee(string nombre, int no_emp) =>
                new CrudEventLog(no_emp, $"Eliminacion de empleado", $"Se ha eliminado al empleado '{no_emp}' - '{nombre}' de la base de datos!!") ;

            public static CrudEventLog ChangeEmployeeLocalidad(string nombre, int no_emp, string localidad) =>
                new CrudEventLog(no_emp, $"Cambio de localidad", $"Se cambio la localidad del empleado a '{localidad}'");
        }
        #endregion
    }

    /// <summary>
    /// Listado de dispositivos permitidos como origen de datos
    /// </summary>
    internal enum Dispositivo
    {
        [Description("name:ZKTECO K40 KIT;")]
        ZKTECO_K40_KIT,
        [Description("name:ZKTECO K40;")]
        ZKTECO_K40,
    }


    internal static class TipoChecadaExtensions
    {
        internal static string GetText(this TipoChecada tipo) => tipo switch
        {
            TipoChecada.ENTRADA => "Entrada",
            TipoChecada.SALIDA => "Salida",
            TipoChecada.IDA_COMIDA => "Ida a Comida",
            TipoChecada.REGRESO_COMIDA => "Regreso de Comida",
            TipoChecada.UNKWN => "Desconocido",
            _ => "Desconocido",
        };
    }

    internal static class DispositivoExtensions
    {
        internal static string GetText(this Dispositivo dispositivo)
        {
            FieldInfo fi = dispositivo.GetType().GetField(dispositivo.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                string desc = attributes[0].Description;
                // Extraemos el nombre del dispositivo del atributo
                string namePart = desc.Split(";").FirstOrDefault(part => part.StartsWith("name:"));
                if (namePart != null)
                {
                    return namePart.Replace("name:", "").Trim();
                }
            }

            return "Desconocido";
        }

        internal static Dispositivo Parse(string text)
        {
            foreach (Dispositivo device in Enum.GetValues(typeof(Dispositivo)))
            {
                if (device.GetText().Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    return device;
                }
            }

            throw new ArgumentException("Texto de dispositivo desconocido.");
        }

        internal static string[] GetSupportedModels()
        {
            List<string> models = new List<string>();
            foreach (Dispositivo device in Enum.GetValues(typeof(Dispositivo)))
            {
                models.Add(device.GetText());
            }

            return models.ToArray();
        }
    }

    internal class Utils
    {
        internal static BaseColor GetBaseColorByName(string bc_name) => bc_name.Trim().ToUpper() switch
        {
            "ROJO" => BaseColor.RED,
            "AZUL" => BaseColor.BLUE,
            "NEGRO" => BaseColor.BLACK,
            "GRIS" => BaseColor.GRAY,
            _ => BaseColor.GRAY,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">Fecha de la asistencia</param>
        /// <param name="quinNumber">Numero de quincena</param>
        /// <returns></returns>
        internal static int TranslateDayOnCell(DateOnly date, int quinNumber)
        {
            switch (quinNumber)
            {
                case 1:
                    if (date.Day < 1 || date.Day > 15)
                        throw new ArgumentOutOfRangeException("El día no pertenece a la primera quincena.");
                    return date.Day; // Día tal cual
                case 2:
                    if (date.Day < 16)
                        throw new ArgumentOutOfRangeException("El día no pertenece a la segunda quincena.");
                    return date.Day - 15; // Día ajustado
                default:
                    throw new ArgumentException("Número de quincena inválido (solo 1 o 2).");
            }
        }


        internal static string ParseJsonHorariosByDgv(flExtendedDataGridView dgv)
        {
            List<string> _jsonParts = new List<string>();
            Func<TimeSpan, int> _buildMilitaryTimeString = (TimeSpan time) => int.Parse($"{time.Hours:00}{time.Minutes:00}");

            foreach (HorarioTurno i in ParseHorariosTurnosByDgv(dgv))
            {
                string words = $@"""{i.ID}"" : {{
    ""primer_horario"" : 
    {{
        ""entrada"" : {_buildMilitaryTimeString(i.PrimerHorario.Entrada)},
        ""salida"" : {_buildMilitaryTimeString(i.PrimerHorario.Salida)}
    }},
    ""segundo_horario"" : 
    {{
        ""entrada"" : {_buildMilitaryTimeString(i.SegundoHorario.Entrada)},
        ""salida"" : {_buildMilitaryTimeString(i.SegundoHorario.Salida)}
    }},
    ""tiempo_extra"" : 
    {{
        ""entrada"" : {_buildMilitaryTimeString(i.TiempoExtra.Entrada)},
        ""salida"" : {_buildMilitaryTimeString(i.TiempoExtra.Salida)}
    }}
}}";

                _jsonParts.Add(words);
            }

            return $"{{ {string.Join(",", _jsonParts)} }}";
        }


        internal static HorarioTurno[] ParseHorariosTurnosByDgv(flExtendedDataGridView dgv)
        {
            if (IsDgvEmpty(dgv))
                throw new ArgumentException("No se puede cargar una tabla sin contenido para el parseo de datos.");

            List<HorarioTurno> _horariosTurnos = new List<HorarioTurno>();

            foreach (DataGridViewRow row in dgv.Rows)
            {

                Func<object, TimeSpan> _TryParseTime = delegate (object cellValue)
                {
                    string tVal = (string)cellValue;
                    if (String.IsNullOrEmpty(tVal.Trim()))
                        return TimeSpan.Zero;

                    return TimeSpan.Parse(tVal);
                };

                int turnoNum = int.Parse(row.Cells[0].Value.ToString());
                (TimeSpan entrada, TimeSpan salida) primerHorario = (_TryParseTime(row.Cells[1].Value), _TryParseTime(row.Cells[2].Value));
                (TimeSpan entrada, TimeSpan salida) segundoHorario = (_TryParseTime(row.Cells[3].Value), _TryParseTime(row.Cells[4].Value));

                _horariosTurnos.Add(new HorarioTurno()
                {
                    ID = turnoNum,
                    PrimerHorario = primerHorario,
                    SegundoHorario = segundoHorario
                });
            }

            return _horariosTurnos.ToArray();
        }

        /// <summary>
        /// Valida si un DataGridView esta vacio
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns>True en caso de estar vacio, False en caso contrario</returns>
        internal static bool IsDgvEmpty(DataGridView dgv)
        {
            if (dgv == null)
                return true;

            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0)
                return true;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                // Ignorar la fila nueva si está habilitada
                if (row.IsNewRow)
                    continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        // Si encontramos al menos una celda con contenido, no está vacío
                        return false;
                    }
                }
            }

            // Si no se encontró ninguna celda con contenido
            return true;
        }


        internal static Func<TimeSpan, string> WriteNotEmptyTimes = (TimeSpan time) => !time.Equals(TimeSpan.Zero) ? $"{time.Hours:00}:{time.Minutes:00}" : "";

        internal static int GetColumnInt(string colLetter)
        {
            int numeroColumna = 0;
            colLetter = colLetter.ToUpper();

            for (int i = 0; i < colLetter.Length; i++)
            {
                numeroColumna *= 26;
                numeroColumna += (colLetter[i] - 'A' + 1);
            }

            return numeroColumna;
        }

        internal static string[] SeparateEntryChain(string texto)
        {
            List<string> partes = new List<string>();

            for (int i = 0; i < texto.Length; i += 5)
            {
                int longitud = Math.Min(5, texto.Length - i);
                partes.Add(texto.Substring(i, longitud));
            }

            return partes.ToArray();
        }

        internal static string[] GetMonthsInPeriod(DateTime start, DateTime end)
        {
            List<string> meses = new List<string>();
            DateTime actual = new DateTime(start.Year, start.Month, 1);

            while (actual <= end)
            {
                string nombreMes = actual.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                if (!meses.Contains(nombreMes))
                {
                    meses.Add(nombreMes);
                }

                actual = actual.AddMonths(1);
            }

            return meses.ToArray();
        }

        /// <summary>
        /// Obtenemos el listado de las localidades disponibles desde el servidor
        /// </summary>
        /// <returns></returns>
        internal static string[] GetLocalidadesDisponibles()
        {
            List<string> localidades = new List<string>();

            MySqlDataReader _query = new Server.GeneralQuery(new ConnectionsData(
                Properties.Settings.Default.SERVER_HOSTNAME,
                Properties.Settings.Default.SERVER_USER,
                Properties.Settings.Default.SERVER_PASS,
                Empleado.TABLE_NAME,
                Empleado.DATABASE_NAME
            )).ExecuteQuery(
                $"SELECT all_allowed_sites FROM checador_fxe_db.global_config WHERE (config_name=@Name);",
                new (string, object)[] { ("@Name", "Default") }
            );

            try
            {
                while (_query.Read())
                {
                    string rawSites = _query.GetString(0);
                    localidades.AddRange(rawSites.Split(";").Select(site => site.Trim()).Where(site => !string.IsNullOrEmpty(site)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar el listado de las localidades disponibles.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                localidades.AddRange(new[] { "Hermosillo", "Sufragio", "Nogales" });
            }
            finally
            {
                _query.Close();
            }

            return localidades.ToArray();
        }
    }

    internal enum TipoChecada
    {
        ENTRADA,
        SALIDA,
        IDA_COMIDA,
        REGRESO_COMIDA,
        UNKWN,
    }

    internal class ReporteAsistencias
    {
        /// <summary>
        /// Chequeos registrados en el reporte (nombre de empleado : chequeos)
        /// </summary>
        internal Dictionary<string, Checada[]> Chequeos { get; set; }
        /// <summary>
        /// Relacion de turnos y los empleados
        /// </summary>
        internal Dictionary<string, List<(DateOnly Day, int TurnNumber)>> Turnos { get; set; }
        /// <summary>
        /// Ruta del archivo .xlsx que se usa para la generacion del reporte
        /// </summary>
        internal string SourcePath { get; }
        /// <summary>
        /// Modelo del dispositivo que genero el archivo de reporte
        /// </summary>
        internal Dispositivo DeviceModel { get; }
        /// <summary>
        /// Fecha de inicio y fin del periodo reporteado
        /// </summary>
        internal (DateTime Start, DateTime End) ReportPeriod { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Ruta del archivo .xls o .xlsx</param>
        /// <param name="sourceDevice">Modelo del dispositivo de origen de archivo de chequeos</param>
        internal ReporteAsistencias(string path, Dispositivo sourceDevice)
        {
            var parser = sourceDevice switch
            {
                Dispositivo.ZKTECO_K40_KIT => READER_ZK_TECO_K40(path),
                Dispositivo.ZKTECO_K40 => READER_ZK_TECO_K40(path),
                _ => throw new IndexOutOfRangeException($"No se encuentra en la lista de dispositivos admitidos al elemento '{sourceDevice.GetText()}'!")
            };

            if (!parser.Status)
                return;

            Chequeos = parser.Chequeos;
            Turnos = parser.RelacionTurnos;
            ReportPeriod = parser.PeriodTime;
            DeviceModel = sourceDevice;
            SourcePath = path;
        }

        /// <summary>
        /// Constructor para abrir un proyecto mediante su archivo *.caf
        /// </summary>
        /// <param name="proj">Objeto de proyecto a abrir</param>
        internal ReporteAsistencias(CafProjFile proj)
        {
            string path = $@"{proj.TempDir}\{proj.SourceFile.Filename}";

            var parser = proj.AssetsFile.Device switch
            {
                Dispositivo.ZKTECO_K40_KIT => READER_ZK_TECO_K40(path),
                Dispositivo.ZKTECO_K40 => READER_ZK_TECO_K40(path),
                _ => throw new IndexOutOfRangeException($"No se encuentra en la lista de dispositivos admitidos al elemento '{proj.AssetsFile.Device.GetText()}'!")
            };

            if (!parser.Status)
                return;

            Chequeos = parser.Chequeos;
            Turnos = parser.RelacionTurnos;
            ReportPeriod = parser.PeriodTime;
            DeviceModel = proj.AssetsFile.Device;
            SourcePath = path;
        }

        

        /// <summary>
        /// Lector e interprete del formato
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private (bool Status, 
                Dictionary<string, List<(DateOnly Day, int TurnNumber)>> RelacionTurnos, 
                Dictionary<string, Checada[]> Chequeos, 
                (DateTime Start, DateTime End) PeriodTime) 
        READER_ZK_TECO_K40(string path)
        {
            /* 
             * ESTA FUNCION LEE LOS REPORTES DE EMPLEADOS GENERADOS POR LOS MODELOS
             * ZK_TECO K40 Y ZKTECO K40 KIT
             * 
             * POSIBLEMENTE TAMBIEN SEA COMPATIBLE CON MAS MODELOS, Y ES POR ESTA MISMA
             * RAZON QUE ES MAS CONVENIENTE EL FACTORIZAR DICHO CODIGO
             * */
            // GLOBALES
            int TRGT_LIMIT = 0;                         // Columna limite hasta donde analizaremos nuestro repote
            string[] MONTHS_OF_PERIOD = new string[0];  // Meses del periodo
            int ACTUAL_RPT_YEAR = -1;

            // VALORES DE HOJA DE ASISTENCIAS
            const string SHEET_ASISTENCIAS_NAME = "Reporte de Asistencia";
            const string SHEET_TURNOS_NAME = "Reporte de Turnos";
            const char NO_EMP_COLUMN = 'C';
            const char NOM_EMP_COLUMN = 'K';
            const int FIRST_ROW = 6;                // Primer fila con registro de asistencia
            const int PERIOD_DAYS_ROW = 4;          // Fila que indica los dias que abarca el reporte
            const char PERIOD_REPORT_COLUMN = 'C';  // Columna donde establece la fecha inicial y final a la que pertenece el reporte

            // VALORES DE HOJA DE TURNOS
            const char NOM_EMP_TURNSHEET_COLUMN = 'B';  // Columna en la que se encuentran los nombres
            const int FIRST_NOM_EMP_TURNSHEET_ROW = 5;  // Primer fila donde estan los nombres
            const char START_TURNOS_COL = 'D';          // Columna en la que inician los dias del reporte que analizaremos
            const int PERIOD_DAYS_TURNSHEET_ROW = 3;    // Fila en la que se escribe el numero del dia


            // DICCIONARIO DE RESULTADOS PREVIOS
            Dictionary<string, List<Checada>> PRE_DEAD_RESULT = new Dictionary<string, List<Checada>>();

            // Convierte los valores al resultado de la funcion
            Func<Dictionary<string, Checada[]>> GetDeadResult = delegate ()
            {
                Dictionary<string, Checada[]> DEAD_RESPONSE = new Dictionary<string, Checada[]>();

                foreach (var item in PRE_DEAD_RESULT)
                {
                    DEAD_RESPONSE.Add(item.Key, item.Value.ToArray());
                }

                return DEAD_RESPONSE;
            };

            (DateTime Start, DateTime End) period = (DateTime.MinValue, DateTime.MinValue);

            // DICCIONARIO DE RELACION ("USUARIO" : "TURNO")
            Dictionary<string, List<(DateOnly, int)>> DEAD_USUARIO_TURNO = new Dictionary<string, List<(DateOnly, int)>>();

            try
            {
                #region FUNCIONES ANONIMAS GLOBALES
                // Creamos la matriz de coordenadas que indican las columnas de inicio y fin de los meses del periodo reporteado
                Func<int, int, int, SLDocument, (int, int)[]> MakeMonthCoordsArray = delegate (int periodDaysRow, int startCol, int endCol, SLDocument sl)
                {
                    (int Start, int End)[] _MONTH_COORDS = new (int Start, int End)[MONTHS_OF_PERIOD.Length];
                    int X_MONTH_COORD = 1;  // Inicio del mes
                    int Y_MONTH_COORD;  // Fin del mes

                    int _jumps = 0;
                    for (int i = startCol; i <= endCol; i++)
                    {
                        /* 
                         * UBICAMOS LAS COORDENADAS DE CADA MES
                         * 
                         * ES DECIR, DE QUE COLUMNA A QUE COLUMNA REPRESENTA UN MES
                         * */
                        string NXT_DAY = sl.GetCellValueAsString(periodDaysRow, i + 1).Trim();
                        //MessageBox.Show($"Analizando Col: {i}->{NXT_DAY} (Limite: {TRGT_LIMIT}) X: {X_MONTH_COORD}");

                        if (String.IsNullOrEmpty(NXT_DAY))
                        {
                            Y_MONTH_COORD = i;  // Indicamos la finalizacion del mes
                            _MONTH_COORDS[_jumps] = (X_MONTH_COORD, Y_MONTH_COORD);
                            break;  // Rompemos el ciclo en caso de que ya no haya mas dias
                        }

                        int[] finalDays = { 28, 29, 30, 31 };
                        int actualDay = sl.GetCellValueAsInt32(periodDaysRow, i);
                        int nextDay = Int32.Parse(NXT_DAY);

                        if (finalDays.Contains(actualDay) && actualDay > nextDay)
                        {
                            // Indicamos que cambiamos de mes
                            Y_MONTH_COORD = i;  // Indicamos la finalizacion del mes
                            _MONTH_COORDS[_jumps] = (X_MONTH_COORD, Y_MONTH_COORD);

                            /* 
                             * Indicamos que vamos a analizar el siguiente mes, 
                             * por lo que hacemos el salto de mes
                             * */
                            _jumps++;
                            X_MONTH_COORD = i + 1; // Indicamos el inicio del nuevo mes
                        }
                    }

                    return _MONTH_COORDS.ToArray();
                };

                // Obtenemos el mes segun sus coordenadas actuales
                Func<int, (int, int)[], string> GetMonthByCoords = delegate (int a_index, (int Start, int End)[] coordsArray)
                {
                    int index = 0;
                    foreach (var coords in coordsArray)
                    {
                        if (a_index >= coords.Start && a_index <= coords.End)
                            break;

                        index++;
                    }
                    return MONTHS_OF_PERIOD[index];    // Retornamos el mes correspondiente
                };
                #endregion
                #region ANALISIS DE HOJA DE ASISTENCIAS
                using (SLDocument sl = new SLDocument(path, SHEET_ASISTENCIAS_NAME))
                {
                    while (true)
                    {
                        /* 
                         * DETECTAMOS CUAL ES LA COLUMNA LIMITE HASTA LA QUE DESEAMOS CAPTURAR
                         * LOS DATOS. ES DEDIR, DESDE QUE FECHA INICIA Y TERMINA EL REPOR DE LAS
                         * FECHAS.
                         * */
                        if (String.IsNullOrEmpty(sl.GetCellValueAsString(PERIOD_DAYS_ROW, TRGT_LIMIT + 1).Trim()))
                            break;

                        TRGT_LIMIT++;
                    }


                    // Primero extraemos la cadena del periodo "YYYY-MM-DD ~ YYYY-MM-DD"
                    string[] chainPeriod = (sl.GetCellValueAsString(3, Utils.GetColumnInt(PERIOD_REPORT_COLUMN.ToString()))).Trim().Replace(" ", "").Split("~");
                    period = (
                        DateTime.Parse(chainPeriod[0]), // Fecha inicial
                        DateTime.Parse(chainPeriod[1])  // Fecha final
                    );
                    MONTHS_OF_PERIOD = Utils.GetMonthsInPeriod(
                        period.Start,   // fecha incial
                        period.End      // fecha final
                    );   // Meses del periodo

                    ACTUAL_RPT_YEAR = period.Start.Year;

                    /* CODIGO ANTERIOR EN DESUSO
                    (int Start, int End)[] _MONTH_COORDS = new (int Start, int End)[meses.Length];

                    int X_MONTH_COORD = 1;  // Inicio del mes
                    int Y_MONTH_COORD;  // Fin del mes

                    int _jumps = 0;
                    for (int i = 1; i <= TRGT_LIMIT; i++)
                    {
                        // 
                        // UBICAMOS LAS COORDENADAS DE CADA MES
                        //
                        // ES DECIR, DE QUE COLUMNA A QUE COLUMNA REPRESENTA UN MES
                        //

                        string NXT_DAY = sl.GetCellValueAsString(PERIOD_DAYS_ROW, i + 1).Trim();
                        //MessageBox.Show($"Analizando Col: {i}->{NXT_DAY} (Limite: {TRGT_LIMIT}) X: {X_MONTH_COORD}");

                        if (String.IsNullOrEmpty(NXT_DAY))
                        {
                            Y_MONTH_COORD = i;  // Indicamos la finalizacion del mes
                            _MONTH_COORDS[_jumps] = (X_MONTH_COORD, Y_MONTH_COORD);
                            break;  // Rompemos el ciclo en caso de que ya no haya mas dias
                        }

                        int[] finalDays = { 28, 29, 30, 31 };
                        int actualDay = sl.GetCellValueAsInt32(PERIOD_DAYS_ROW, i);
                        int nextDay = Int32.Parse(NXT_DAY);

                        if (finalDays.Contains(actualDay) && actualDay > nextDay)
                        {
                            // Indicamos que cambiamos de mes
                            Y_MONTH_COORD = i;  // Indicamos la finalizacion del mes
                            _MONTH_COORDS[_jumps] = (X_MONTH_COORD, Y_MONTH_COORD);

                            //
                            // Indicamos que vamos a analizar el siguiente mes, 
                            // por lo que hacemos el salto de mes
                            //
                            _jumps++;
                            X_MONTH_COORD = i + 1; // Indicamos el inicio del nuevo mes
                        }
                    }
                    */

                    (int Start, int End)[] _MONTH_COORDS = MakeMonthCoordsArray(PERIOD_DAYS_ROW, 1, TRGT_LIMIT, sl);
                    

                    #region FUNCIONES ANONIMAS PRIVADAS REQUERIDAS
                    // Funcion para crear un DateTime a partir de dia, mes y año
                    Func<int, string, int, DateTime> MakeDateTimeParse = (d, m, y) => DateTime.Parse($"{d}-{m}-{y}");

                    // Funcion que parsea una cantidad de tiempos y datos en un arreglo de chequeos pertinentes
                    Func<string, int, DateTime, string[], Checada[]> ParseEntryTimes = delegate (string nombreEmpleado, int numeroEmpleado, DateTime fechaAsistencia, string[] inOutTimes)
                    {
                        List<Checada> _results = new List<Checada>();

                        /* CASOS DE PARSEO DE HORAS
                         * 
                         * PARA EL CASO DE 1 HORAS:
                         *      HORA 1 -> ENTRADA
                         * 
                         * PARA EL CASO DE 2 HORAS:
                         *      HORA 1 -> ENTRADA
                         *      HORA 2 -> SALIDA
                         *  
                         * PARA EL CASO DE 3 HORAS:
                         *      HORA 1 -> ENTRADA
                         *      HORA 2 -> IDA_COMER
                         *      HORA 3 -> SALIDA
                         *      
                         * PARA EL CASO DE 4 HORAS:
                         *      HORA 1 -> ENTRADA
                         *      HORA 2 -> IDA_COMER
                         *      HORA 3 -> REGRESO_COMER
                         *      HORA 4 -> SALIDA
                         *      HORAS EXTRAS -> UNKWN
                         * */

                        switch (inOutTimes.Length)
                        {
                            case 1:
                                #region
                                _results.Add(new Checada()
                                {
                                    Empleado = nombreEmpleado,
                                    NumEmpleado = numeroEmpleado,
                                    Fecha = MakeDateTimeParse(
                                        fechaAsistencia.Day,
                                        fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                        fechaAsistencia.Year
                                    ).Add(TimeSpan.Parse(inOutTimes[0])),
                                    Tipo = TipoChecada.ENTRADA
                                });
                                #endregion
                                break;
                            case 2:
                                #region
                                _results.AddRange(new[]
                                {
                                    new Checada() {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.ENTRADA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.SALIDA
                                    }
                                });
                                #endregion
                                break;
                            case 3:
                                #region
                                _results.AddRange(new[]
                                {
                                    new Checada() {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.ENTRADA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.IDA_COMIDA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.SALIDA
                                    }
                                });
                                #endregion
                                break;
                            case 4:
                                #region
                                _results.AddRange(new[]
                                {
                                    new Checada() {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.ENTRADA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.IDA_COMIDA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.REGRESO_COMIDA
                                    },
                                    new Checada()
                                    {
                                        Empleado = nombreEmpleado,
                                        NumEmpleado = numeroEmpleado,
                                        Fecha = MakeDateTimeParse(
                                            fechaAsistencia.Day,
                                            fechaAsistencia.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")),
                                            fechaAsistencia.Year
                                        ).Add(TimeSpan.Parse(inOutTimes[0])),
                                        Tipo = TipoChecada.SALIDA
                                    }
                                });
                                #endregion
                                break;
                        }

                        return _results.ToArray();
                    };

                    // Obtiene todas las filas en las que hay registros de asistencias
                    Func<int[]> GetRowEntrys = delegate ()
                    {
                        List<int> rows = new List<int>();
                        int actualRow = FIRST_ROW;

                        while (true)
                        {
                            if (String.IsNullOrEmpty(sl.GetCellValueAsString(actualRow - 1, Utils.GetColumnInt("A")).Trim()))
                                break;

                            rows.Add(actualRow);
                            actualRow += 2;
                        }

                        return rows.ToArray();
                    };

                    // Validamos si el nombre del empleado ya se encuentra registrado en el diccionario
                    Action<string> ValidateEmployeeOnDict = delegate (string nombreEmpleado)
                    {
                        if (!PRE_DEAD_RESULT.ContainsKey(nombreEmpleado))
                            PRE_DEAD_RESULT.Add(nombreEmpleado, new List<Checada>());
                    };
                    #endregion

                    // Analizamos los registros de empleados
                    foreach (int actualRow in GetRowEntrys())
                    {
                        int actualHeaderRow = actualRow - 1;
                        int actualCol = Utils.GetColumnInt("A");    // Columna actual, desde la que se iniciara el recorrido de datos

                        string actualNombreEmpleado = sl.GetCellValueAsString(actualHeaderRow, Utils.GetColumnInt(NOM_EMP_COLUMN.ToString()));

                        // Analizamos las columnas de cada registro
                        for (int i = 1; i <= TRGT_LIMIT; i++)
                        {
                            //MessageBox.Show($"Columna actual: {i} (Limite: {TRGT_LIMIT})");
                            actualCol = i;
                            if (String.IsNullOrEmpty(sl.GetCellValueAsString(actualRow, actualCol).Trim()))
                                continue;

                            ValidateEmployeeOnDict(actualNombreEmpleado);

                            // METODO DE OBTENCION DEL AÑO ACTUAL

                            // Analizamos el dia laborado
                            Checada[] a = ParseEntryTimes(
                                actualNombreEmpleado,
                                //
                                // HACK: NO SE DEBE REEMPLAZAR "GetCellValueAsString" POR "GetCellValueAsInt32" DEBIDO A ERRORES EN EL PARSEO DE DATOS.
                                // SI SE USA EL SEGUNDO METODO, NO PARSEARA EL NUMERO Y SIEMPRE RETORNARA "0".
                                //
                                Int32.Parse(sl.GetCellValueAsString(actualHeaderRow, Utils.GetColumnInt(NO_EMP_COLUMN.ToString()))),
                                MakeDateTimeParse(
                                    sl.GetCellValueAsInt32(PERIOD_DAYS_ROW, actualCol),     // Dia del mes
                                    GetMonthByCoords(i, _MONTH_COORDS),                     // Mes del año analizado
                                    ACTUAL_RPT_YEAR                                         // Año actual analizado
                                ),
                                Utils.SeparateEntryChain(sl.GetCellValueAsString(actualRow, actualCol)));

                            PRE_DEAD_RESULT[actualNombreEmpleado].AddRange(a);
                        }

                    }
                }
                #endregion
                #region ANALISIS DE HOJA DE TURNOS
                using (SLDocument sl = new SLDocument(path, SHEET_TURNOS_NAME))
                {
                    int actualEmployeeRow = FIRST_NOM_EMP_TURNSHEET_ROW;    // Primer empleado a analizar
                    int LAST_EMPLOYEE_ROW = actualEmployeeRow + (PRE_DEAD_RESULT.Keys.Count - 1);   // Ultima fila a analizar
                    int dayLimit = (Utils.GetColumnInt($"{START_TURNOS_COL}") + TRGT_LIMIT - 1);

                    (int Start, int End)[] _MONTH_COORDS = MakeMonthCoordsArray(PERIOD_DAYS_TURNSHEET_ROW, Utils.GetColumnInt($"{START_TURNOS_COL}"), dayLimit, sl);

                    while (actualEmployeeRow <= LAST_EMPLOYEE_ROW)
                    {
                        string employeeName = sl.GetCellValueAsString(actualEmployeeRow, Utils.GetColumnInt(NOM_EMP_TURNSHEET_COLUMN.ToString()));
                        for (int day = Utils.GetColumnInt(START_TURNOS_COL.ToString()); day <= dayLimit; day++)
                        {
                            int turnOfDay = sl.GetCellValueAsInt32(actualEmployeeRow, day);
                            string numberOfDay = sl.GetCellValueAsString(PERIOD_DAYS_TURNSHEET_ROW, day);

                            var _dataTuple = (DateOnly.Parse($"{numberOfDay}-{GetMonthByCoords(day, _MONTH_COORDS)}-{ACTUAL_RPT_YEAR}"), turnOfDay);

                            if (DEAD_USUARIO_TURNO.ContainsKey(employeeName))
                            {
                                DEAD_USUARIO_TURNO[employeeName].Add(_dataTuple);
                            }
                            else
                            {
                                DEAD_USUARIO_TURNO.Add(employeeName, new List<(DateOnly, int)>() { _dataTuple });
                            }
                        }

                        actualEmployeeRow++;
                    }
                }
                #endregion

                return (true, DEAD_USUARIO_TURNO, GetDeadResult(), period);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex}", "Error Inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false, new Dictionary<string, List<(DateOnly, int)>>(), new Dictionary<string, Checada[]>(), period);
            }
        }
    }


    internal class Checada
    {
        public DateTime Fecha { get; set; }
        public string Empleado { get; set; } = "";
        public int NumEmpleado { get; set; }
        public TipoChecada Tipo { get; set; }

        public override string ToString() => $"{NumEmpleado} - {Empleado} - {Fecha.ToString("dd/MM/yyyy HH:mm:ss")} : {Tipo.ToString()}";
    }


    internal class HorarioTurno
    {
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public (TimeSpan Entrada, TimeSpan Salida) PrimerHorario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Se refiere al horario despues de comer</remarks>
        public (TimeSpan Entrada, TimeSpan Salida) SegundoHorario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>No siempre aplica</remarks>
        public (TimeSpan Entrada, TimeSpan Salida) TiempoExtra { get; set; }

        /// <summary>
        /// Texto JSON con la configuracion de horarios correspondiente
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HorarioTurno[] GetAll(string text)
        {
            #region 
            List<HorarioTurno> _turnosDeTrabajo = new List<HorarioTurno>();

            foreach (var i in JObject.Parse(text))
            {
                HorarioTurno _turn = new HorarioTurno();

                _turn.ID = int.Parse(i.Key.ToString());

                Func<string, TimeSpan> _DivideMakeTimeSpan = delegate (string t)
                {
                    List<string> grupos = new List<string>();

                    for (int i = t.Length; i > 0; i -= 2)
                    {
                        int start = Math.Max(i - 2, 0);
                        int length = i - start;
                        grupos.Insert(0, t.Substring(start, length));
                    }

                    return TimeSpan.Parse($"{grupos[0]:00}:{grupos[1]:00}");
                };

                Func<string, (TimeSpan Entrada, TimeSpan Salida)> BuildTimeSpan = delegate (string SCHEDULE)
                {
                    var horario = i.Value[SCHEDULE];
                    string entrada = horario["entrada"].ToString();
                    string salida = horario["salida"].ToString();

                    if (int.Parse(entrada) <= 0 && int.Parse(salida) <= 0)
                        return (TimeSpan.Zero, TimeSpan.Zero);

                    return (
                        _DivideMakeTimeSpan(entrada), _DivideMakeTimeSpan(salida)
                    );
                };

                _turn.PrimerHorario = BuildTimeSpan("primer_horario");
                _turn.SegundoHorario = BuildTimeSpan("segundo_horario");
                _turn.TiempoExtra = BuildTimeSpan("tiempo_extra");

                _turnosDeTrabajo.Add(_turn);
            }

            return _turnosDeTrabajo.ToArray();
            #endregion
        }

        /// <summary>
        /// Obtiene el valor del tiempo de entrada y salida del empleado de un turno en especifico
        /// </summary>
        /// <param name="turnNumber"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static (TimeSpan Entrada, TimeSpan Salida) GetInOutTimes(int turnNumber)
        {
            HorarioTurno[] _horariosTurnos = GetAll(Properties.Settings.Default.TURNOS_HORARIOS);

            if (turnNumber > _horariosTurnos.Length - 1)
                throw new IndexOutOfRangeException("El numero de turno proporcionado no existe en la lista de turnos.");

            HorarioTurno _targetTurn = _horariosTurnos[turnNumber];

            if (_targetTurn.SegundoHorario != (TimeSpan.Zero, TimeSpan.Zero))
            {
                return (_targetTurn.PrimerHorario.Entrada, _targetTurn.SegundoHorario.Salida);
            }

            return (_targetTurn.PrimerHorario.Entrada, _targetTurn.PrimerHorario.Salida);
        }
    }

    /// <summary>
    /// Tipos de asistencias permitidos por el sistema
    /// </summary>
    internal enum TipoAsistencia
    {
        [Description("text: Ninguno; short: ;")]
        NINGUNO,
        [Description("text: Asistencia; short: X;")]
        ASISTENCIA,
        [Description("text: Retardo; short: R;")]
        RETARDO,
        [Description("text: Falta; short: O;")]
        FALTA,
        [Description("text: Permiso; short: P;")]
        PERMISO,
        [Description("text: Festivo Laborado; short: FL;")]
        FESTIVO_LABORADO,
        [Description("text: Descanso Laborado; short: DL;")]
        DESCANSO_LABORADO,
        [Description("text: Vacaciones Disfrutadas; short: V;")]
        VACACIONES,
        [Description("text: Descanso Semanal; short: D;")]
        DESCANSO_SEM,
        [Description("text: Capacitacion; short: C;")]
        CAPACITACION,
        [Description("text: Incapacidad por Accidente de Trabajo; short: IA;")]
        INCAPACIDAD_DE_TRABAJO,
        [Description("text: Incapacidad por Enfermedad General; short: IE;")]
        INCAPACIDAD_ENF_GNRL,
        [Description("text: Suspension; short: S;")]
        SUSPENSION,
    }

    internal static class TipoAsistenciaExtensions
    {
        static Func<TipoAsistencia, (string _text, string _short)> _getText = delegate (TipoAsistencia tipo)
        {
            FieldInfo? field = tipo.GetType().GetField(tipo.ToString());
            (string, string) result = ("-1", "-1");

            if (field != null)
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    string _description = attribute.Description;

                    result = (
                            _description.Split(';')[0].Replace("text:", "").Trim(), 
                            _description.Split(';')[1].Replace("short:", "").Trim()
                        );
                }
            }
            
            // Si no tiene atributo Description, se devuelve el nombre del enum
            return result;
        };

        public static string GetText(this TipoAsistencia tipo) => _getText(tipo)._text;
        public static string GetShort(this TipoAsistencia tipo) => _getText(tipo)._short;


        public static TipoAsistencia Parse(string text)
        {
            foreach (TipoAsistencia ta in Enum.GetValues<TipoAsistencia>())
            {
                if (ta.GetText().Equals(text, StringComparison.OrdinalIgnoreCase))
                    return ta;
            }

            return TipoAsistencia.FALTA;
        }
    }
}

public enum GridCells
{
    ICON,
    NO_EMP,
    NOMBRE,
    APELLIDOS,
    PUESTO,
    REGION,
    DIVISION,
    LOCALIDAD
}

public static class GridCellsExtension
{

    public static int GetIndex(this GridCells gc) => gc switch
    {
        GridCells.ICON => 0,
        GridCells.NO_EMP => 1,
        GridCells.NOMBRE => 2,
        GridCells.APELLIDOS => 3,
        GridCells.PUESTO => 4,
        GridCells.REGION => 5,
        GridCells.DIVISION => 6,
        GridCells.LOCALIDAD => 7,
        _ => throw new ArgumentOutOfRangeException(nameof(gc), gc, null)
    };
}

public static class SingleInstance
{
    /// <summary>
    /// Intenta adquirir un mutex nombrado y devuelve true si esta es la primera instancia.
    /// </summary>
    public static bool Acquire(string appKey, out Mutex mutex, int timeoutMs = 0)
    {
        if (string.IsNullOrWhiteSpace(appKey))
            throw new ArgumentException("appKey no puede ser vacío.");

        // Nombre único por usuario (evita conflictos entre sesiones RDS)
        string userSid = WindowsIdentity.GetCurrent()?.User?.Value ?? "UnknownUser";
        string name = $@"Local\{appKey}_{userSid}";

        mutex = new Mutex(initiallyOwned: false, name, out bool createdNew);

        try
        {
            // Intento con timeout (0 = no esperar)
            if (!mutex.WaitOne(timeoutMs))
                return false;

            return true; // Bloque adquirido: somos instancia única
        }
        catch (AbandonedMutexException)
        {
            // La instancia anterior murió sin liberar; podemos continuar
            return true;
        }
    }

    public static void Release(Mutex mutex)
    {
        try { mutex?.ReleaseMutex(); } catch { /* Ignorar si ya se liberó */ }
        mutex?.Dispose();
    }
}

