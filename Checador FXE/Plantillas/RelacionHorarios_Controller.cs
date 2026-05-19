using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils;
using FlowCommonWorkcore.SqlUtils.SQLite;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ZstdSharp.Unsafe;
using static Checador_FXE.Plantillas.RelacionHorarios;

namespace Checador_FXE.Plantillas
{
    internal static class Helpers
    {
        #region
        public static Func<string, TurnoEmpleadoCollection> parseJsonRelacion = (json) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return new TurnoEmpleadoCollection();

                var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<JsonTurno>>>(json);

                var collection = new TurnoEmpleadoCollection();

                if (dict == null)
                    return collection;

                foreach (var kv in dict)
                {
                    int noEmp = int.Parse(kv.Key);

                    foreach (var item in kv.Value)
                    {
                        collection.Add(new TurnoEmpleado(
                            noEmp,
                            item.Turno,
                            item.Nombre,
                            DateOnly.Parse(item.Dia)
                        ));
                    }
                }

                return collection;
            }
            catch
            {
                throw new Exception("Ocurrio un error inesperado al parsear la informacion del JsonRelaciones");
            }
        };
        public static Func<TurnoEmpleadoCollection, string> buildJsonRelacion = (relacion) =>
        {
            #region CODIGO
            try
            {
                Dictionary<int, List<TurnoEmpleado>> _relacionEmpleadosTurnos = new Dictionary<int, List<TurnoEmpleado>>();

                foreach (TurnoEmpleado t in relacion.Items)
                {
                    if (!_relacionEmpleadosTurnos.ContainsKey(t.NoEmp))
                        _relacionEmpleadosTurnos[t.NoEmp] = new List<TurnoEmpleado>();
                    _relacionEmpleadosTurnos[t.NoEmp].Add(t);
                }

                List<string> _employeeSections = new List<string>();

                foreach (int n_E in _relacionEmpleadosTurnos.Keys)
                {
                    List<string> _turnosParts = new List<string>();

                    foreach (TurnoEmpleado t in _relacionEmpleadosTurnos[n_E])
                        _turnosParts.Add($"{{\"Nombre\":\"{t.Nombre}\",\"Dia\":\"{t.Dia.ToString("yyyy-MM-dd")}\",\"Turno\":{t.Turno}}}");

                    _employeeSections.Add($"\"{n_E}\":[{string.Join(",\n", _turnosParts)}]");
                }

                return "{" + string.Join(",\n", _employeeSections) + "}";
            }
            catch
            {
                throw new Exception("Ocurrio un error inesperado al parsear la informacion del JsonRelaciones");
            }
            #endregion
        };
        public static Func<RelacionHorarioID, TurnoEmpleadoCollection> makeDefaultRelacion = (id) =>
        {
            Empleado[] _actualEmpelados = Empleado.GetAll(Properties.Settings.Default.LOCALIDAD_DEFAULT).Object ??
                throw new NullReferenceException($"Ocurrio un error durante la obtencion de los empleados para la localidad default.");

            TurnoEmpleadoCollection _collection = new TurnoEmpleadoCollection();
            foreach (Empleado e in _actualEmpelados)
            {
                int actualMonthNumber = DateTime.ParseExact(id.Month, "MMMM", CultureInfo.CurrentCulture).Month;
                for (int day = 1; day <= DateTime.DaysInMonth(id.Year, actualMonthNumber); day++)
                {
                    DateOnly d_Only = new DateOnly(id.Year, actualMonthNumber, day);
                    int _turnoSelected = d_Only.DayOfWeek is DayOfWeek.Sunday ? -1 : e.TurnoDefault;
                    _collection.Add(
                        new TurnoEmpleado(Int32.Parse(e.NoEmp), _turnoSelected, $"{e.Nombres} {e.Apellidos}", d_Only)
                    );
                }
            }

            return _collection;
        };
        #endregion
    }

    public struct RelacionHorarioID
    {
        public string Month { get; } // Mes en formato de texto (Ej: Enero, Febrero, etc.)
        public int Year { get; } // Año en formato numérico (Ej: 2024)

        public RelacionHorarioID(string month, int year)
        {
            this.Month = month;
            this.Year = year;
        }

        public override string ToString() => $"{Month}-{Year}";
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is RelacionHorarioID id && this.Month == id.Month && this.Year == id.Year;
        public override int GetHashCode() => HashCode.Combine(Month, Year);
        /// <summary>
        /// Obtiene el ID de relacion de horarios correspondiente al mes y año actual
        /// </summary>
        /// <returns></returns>
        public static RelacionHorarioID GetActualId() => new RelacionHorarioID(DateTime.Now.ToString("MMMM", new CultureInfo("es-MX")), DateTime.Now.Year);
    }

    public struct TurnoEmpleado
    {
        public int NoEmp { get; set; } // Número de empleado
        public int Turno { get; set; } // Número de turno asignado (Ej: 1, 2, 3, etc.)
        public string Nombre { get; set; }
        public DateOnly Dia { get; set; }

        public TurnoEmpleado(int noEmp, int turno, string nombre, DateOnly dia)
        {
            this.NoEmp = noEmp;
            this.Turno = turno;
            this.Nombre = nombre;
            this.Dia = dia;
        }
    }

    public class TurnoEmpleadoCollection
    {
        private List<TurnoEmpleado> _items = new();

        // Propiedades
        public int Count => _items.Count;
        public TurnoEmpleado[] Items => _items.ToArray();
        
        // Constructores
        public TurnoEmpleadoCollection() { }
        public TurnoEmpleadoCollection(TurnoEmpleado[] items) => _items.AddRange(items);

        // Indexer
        /// <summary>
        /// Obtiene el turno de un empleado especifico
        /// </summary>
        /// <param name="noEmp">Numero de empleado a buscar</param>
        /// <param name="diaNumber">Dia del turno a buscar</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public TurnoEmpleado this[int noEmp, int diaNumber]
        {
            get
            {
                TurnoEmpleado? target = _items.Cast<TurnoEmpleado>().FirstOrDefault(t => t.NoEmp == noEmp && t.Dia.Day == diaNumber);

                if (!target.HasValue)
                    throw new IndexOutOfRangeException($"No se encontro el numero de empleado proporcionado. '{noEmp}'");

                return target.Value;
            }
        }

        // Metodos
        public void Add(TurnoEmpleado item) => _items.Add(item);
        public void AddRange(TurnoEmpleado[] items) => _items.AddRange(items);
        public void Clear() => _items.Clear();
        public void Remove(TurnoEmpleado item) => _items.Remove(item);
        public void Remove(int index) => _items.RemoveAt(index);
        public bool Contains(TurnoEmpleado item) => _items.Contains(item);
        public bool ContainsKey(int noEmp) => _items.Any(t => t.NoEmp == noEmp);

        public string BuildJson() => Helpers.buildJsonRelacion(this);
        public static TurnoEmpleadoCollection ParseJson(string json) => Helpers.parseJsonRelacion(json);
    }


    /// <summary>
    /// Relacion de horarios-turnos para un mes especifico
    /// </summary>
    internal class RelacionHorarios
    {
        public static readonly string DB_NAME = "RelacionTurnos.db";
        public static readonly string DB_PATH = $@"{Program.DbPath}\{DB_NAME}";
        public static readonly string TABLE_NAME = "relacion_horarios";

        [ColumnSqlName("ID")]
        [ParamSqlKey("@id")]
        public RelacionHorarioID ID { get; set; } // Mes-Año
        [ColumnSqlName("jsonRelacion")]
        [ParamSqlKey("@jsonRelacion")]
        public TurnoEmpleadoCollection Relacion { get; set; } // Arreglo de objetos TurnoEmpleado que representan la relación de horarios para cada empleado
        [ColumnSqlName("hash")]
        [ParamSqlKey("@hash")]
        public HexaHash HASH { get; set; } = new HexaHash();

        public static void InitializeDb()
        {
            // Crea el archivo de la base de datos en caso de que no exista
            if (!File.Exists(DB_PATH))
            {
                Response _db_resp = SQLiteAssets.CreateDataBase(DB_PATH, TABLE_NAME, overwrite: false);

                if (!_db_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la base de datos '{DB_PATH}'! {_db_resp.Message}");

                Response _tb_resp = SQLiteAssets.CreateTable(DB_PATH, TABLE_NAME,
                    new ColumnTemplate("position", DataTypes.INTEGER, isPrimaryKey: true, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("ID"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("Relacion"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("HASH"), DataTypes.TEXT, isNotNull: true));

                if (!_tb_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la tabla '{TABLE_NAME}'! {_tb_resp.Message}");

            }
        }

        public class JsonTurno
        {
            public string Nombre { get; set; }
            public string Dia { get; set; }
            public int Turno { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID de la relacion a buscar</param>
        /// <param name="SaveIfDefault">En caso de que no exista el periodo indicado, generara una relacion por defecto y la guardara en la base de datos</param>
        /// <param name="ShowObjectLog"></param>
        /// <returns></returns>
        public static Response<RelacionHorarios> Get(RelacionHorarioID id, bool SaveIfDefault = true, bool ShowObjectLog = false)
        {
            #region
            Response<RelacionHorarios> _response = new Response<RelacionHorarios>(false, "Iniciando obtencion del objeto", null);

            /*
             * True en caso de que existan relaciones de horarios existentes para el mes consultado, 
             * de lo contrario se creara por defecto
            */
            bool haveRelaciones = false;

            try
            {
                ConnectionsData _data = new ConnectionsData(DB_PATH, TABLE_NAME);
                Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
                SqliteDataReader _reader = _connection.MakeQuery("*", "WHERE (ID=@ID)", ("@ID", id.ToString()));

                RelacionHorarios _obj = new RelacionHorarios();
                _obj.ID = id;

                while (_reader.Read())
                {
                    _obj.Relacion = TurnoEmpleadoCollection.ParseJson(_reader.GetString(2));
                    _obj.HASH = new HexaHash(_reader.GetString(3));
                    haveRelaciones = true;
                }

                if (!haveRelaciones)
                {
                    #region GENERACION DE HORARIO POR DEFECTO
                    _obj.Relacion = Helpers.makeDefaultRelacion(id);
                    _obj.HASH = new HexaHash();

                    if (SaveIfDefault)
                    {
                        Response saveResp = _obj.Save(ShowObjectLog: false);
                        _response.Log.Add(saveResp.GetBuildedLog());
                        if (!saveResp.Success)
                            _response.Log.Add($"No se pudo guardar la relacion de horarios por defecto para el mes '{id}'");
                        else
                            _response.Log.Add($"Relacion de horarios por defecto para el mes '{id}' guardada correctamente!");
                    }
                    #endregion
                }

                _response.Success = true;
                _response.Message = $"Objeto '{id}' obtenido correctamente";
                _response.Object = _obj;
                _response.Tag = haveRelaciones;
            }
            catch (Exception ex)
            {
                _response.Log.Add(ex.ToString());
                _response.Success = false;
                _response.Message = ex.Message;
                _response.Object = null;
            }

            if (ShowObjectLog)
                MessageBox.Show(_response.GetBuildedLog(), "Log del Objeto");

            return _response;
            #endregion
        }

        public static Response<RelacionHorarios[]> GetAll(bool ShowObjectLog = false)
        {
            #region
            Response<RelacionHorarios[]> _resp = new Response<RelacionHorarios[]>(false, "Iniciando obtencion de relaciones de horarios existentes", null);
            List<RelacionHorarios> _list = new List<RelacionHorarios>();
            List<RelacionHorarioID> _IdsList = new List<RelacionHorarioID>();

            // Obtenemos todos los IDs
            ConnectionsData _data = new ConnectionsData($@"{Program.DbPath}\RelacionTurnos.db", TABLE_NAME);
            Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
            SqliteDataReader _reader = _connection.MakeQuery("ID");
            _resp.Log.Add("ConnectionsData y acciones de consulta realzadas...");

            while (_reader.Read())
            {
                _IdsList.Add(new RelacionHorarioID(_reader.GetString(0).Split("-")[0], int.Parse(_reader.GetString(0).Split("-")[1])));
                _resp.Log.Add($"ID '{_reader.GetString(0)}' obtenido correctamente");
            }

            int fails = 0;

            // Consultamos todas las relaciones
            foreach (RelacionHorarioID i in _IdsList)
            {
                var getResp = Get(i);
                
                if (!getResp.Success)
                {
                    _resp.Log.Add($"No se pudo obtener la relacion de horarios con el ID '{i}'");
                    fails++;
                    continue;
                }

                _list.Add(getResp.Object!);
                _resp.Log.Add($"Relacion de Horario '{i}' obtenida correctamente...");
            }

            _resp.Success = true;
            _resp.Object = _list.ToArray();
            _resp.Message = $"Proceso de obtencion de ID finalizado {fails} errores!";

            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public void UpdateByGrid(DataGridViewRow[] rows)
        {
            #region
            List<TurnoEmpleado> turnos = new List<TurnoEmpleado>();
            foreach (DataGridViewRow r in rows)
            {
                TurnoEmpleado obj = new TurnoEmpleado()
                {
                    NoEmp = int.Parse(r.Cells[RelacionHorariosGridCells.NO_EMP.GetIndex()].Value.ToString()!),
                    Nombre = r.Cells[RelacionHorariosGridCells.NOMBRE_COMP.GetIndex()].Value.ToString()!,
                };

                // Recorremos los dias
                for (int d_i = RelacionHorariosGridCells.DAYS_START.GetIndex(); d_i < r.Cells.Count; d_i++)
                {
                    obj.Dia = new DateOnly(this.ID.Year, 
                                           DateTime.ParseExact(this.ID.Month, "MMMM", new CultureInfo("es-MX")).Month, 
                                           d_i - 2);
                    obj.Turno = String.IsNullOrEmpty(r.Cells[d_i].Value.ToString()!.Trim()) ? -1 : Int32.Parse(r.Cells[d_i].Value.ToString()!);
                    Debug.WriteLine($"Guardado: {obj.NoEmp}.{obj.Nombre} // {obj.Dia.ToString("dddd, dd - MMMM - yyyy")} ===> {obj.Turno}");
                    turnos.Add(obj);
                }
            }

            this.Relacion = new TurnoEmpleadoCollection(turnos.ToArray());
            #endregion
        }

        public Response Save(bool ShowObjectLog = false)
        {
            #region
            Response _resp = new Response(false, "Iniciando guardado");
            try
            {
                ConnectionsData _data = new ConnectionsData($@"{Program.DbPath}\RelacionTurnos.db", TABLE_NAME);
                _resp.Log.Add($"Datos de conexion creados...");
                Server.SqlWriteConnection _connection = new Server.SqlWriteConnection(_data);
                _resp.Log.Add($"Conexion establecida...");

                (string, object)[] _queryParams = new (string, object)[]
                {
                    (ParamSqlKey.GetValue<RelacionHorarios>("ID"), ID.ToString()),
                    (ParamSqlKey.GetValue<RelacionHorarios>("Relacion"), Relacion.BuildJson()),
                    (ParamSqlKey.GetValue<RelacionHorarios>("HASH"), HASH.Hash)
                };
                _resp.Log.Add($"Parametros SQL construidos...");

                string insertQuery = Common.BuildInsertQuery<RelacionHorarios>(this, TABLE_NAME, IgnoreSqlColumnsParamsNull: true);
                _resp.Log.Add($"Cadena de insercion construida...");
                string updateQuery = Common.BuildUpdateQuery<RelacionHorarios>(this, TABLE_NAME, $"{ColumnSqlName.GetValue<RelacionHorarios>("HASH")}={ParamSqlKey.GetValue<RelacionHorarios>("HASH")}", IgnoreSqlColumnsParamsNull: true);
                _resp.Log.Add($"Cadena de actualizacion construida...");

                Response _SERV_RESP = _connection.MakeQuery("hash", HASH.Hash, insertQuery, updateQuery, _queryParams);
                _resp.Log.Add($"Consulta SQL realizada...");

                if (!_SERV_RESP.Success)
                    _resp.Log.Add(_SERV_RESP.Message);

                _resp.Success = _SERV_RESP.Success;
            }
            catch (Exception ex)
            {
                _resp.Log.Add(ex.ToString());
                _resp.Success = false;
            }
            
            _resp.Message = _resp.Success ? $"Relacion de horario para '{ID}' guardado correctamente!" : $"Ocurrio un error al intentar guardar la relacion de horario '{ID}'!";

            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public static Response<RelacionHorarios> Parse(string relacionId, string relacionTurnosJson, string relacionHash, bool ShowObjectLog = false)
        {
            Response<RelacionHorarios> _resp = new Response<RelacionHorarios>(false, "Iniciando operacion de parsing", null);

            MessageBox.Show(relacionTurnosJson, "JSON a parsear");

            try 
            {
                RelacionHorarios _obj = new RelacionHorarios();
                _obj.ID = new RelacionHorarioID(relacionId.Split("-")[0], int.Parse(relacionId.Split("-")[1]));
                _resp.Log.Add("ID asignado...");
                _obj.Relacion = TurnoEmpleadoCollection.ParseJson(relacionTurnosJson);
                _resp.Log.Add("Relacion de Turnos asignada...");
                _obj.HASH = new HexaHash(relacionHash);
                _resp.Log.Add("HASH asignado...");

                _resp.Object = _obj;
                _resp.Success = true;
                _resp.Message = $"Parsing de relacion de horarios realizado correctamente!";
            }
            catch (Exception ex)
            {
                _resp.Log.Add(ex.ToString());
                _resp.Success = false;
                _resp.Message = $"Ocurrio un error al intentar parsear la relacion de horarios! {ex.Message}";
                _resp.Object = null;
            }


            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
        }
    }
}
