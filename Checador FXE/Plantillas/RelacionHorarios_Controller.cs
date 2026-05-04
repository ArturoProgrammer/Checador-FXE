using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils;
using FlowCommonWorkcore.SqlUtils.SQLite;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.CodeDom;
using System.Diagnostics.CodeAnalysis;
using ZstdSharp.Unsafe;

namespace Checador_FXE.Plantillas
{
    public struct TurnoEmpleado
    {
        public int NoEmp { get; set; } // Número de empleado
        public int Turno { get; set; } // Número de turno asignado (Ej: 1, 2, 3, etc.)
        public string Nombre { get; set; }
        public DateOnly Dia { get; set; }
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
    }

    public class TurnoEmpleadoCollection
    {
        private readonly List<TurnoEmpleado> _items = new();
        public int Count => _items.Count;
        public TurnoEmpleado[] Items => _items.ToArray();
        
        public TurnoEmpleado this[int noEmp]
        {
            get
            {
                TurnoEmpleado? target = null;
                foreach (var t in _items)
                    if (t.NoEmp == noEmp)
                    {
                        target = t;
                        break;
                    }

                if (!target.HasValue)
                    throw new IndexOutOfRangeException($"No se encontro el numero de empleado proporcionado. '{noEmp}'");

                return target.Value;
            }
            set
            {
                TurnoEmpleado? target = null;
                foreach (var t in _items)
                    if (t.NoEmp == noEmp)
                    {
                        target = t;
                        break;
                    }

                if (!target.HasValue)
                    throw new IndexOutOfRangeException($"No se encontro el numero de empleado proporcionado. '{noEmp}'");

                _items[_items.IndexOf(target.Value)] = value;
            }
        }

        public int this[int noEmp, DateOnly dia]
        {
            get
            {
                this[noEmp];
            }
        }


        public void Add(TurnoEmpleado item) => _items.Add(item);
        public void Remove(TurnoEmpleado item)
        {
            // Remueve de la coleccion segun el numero de empleado
        }
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
                Response _resp = SQLiteAssets.CreateTable(DB_PATH, TABLE_NAME,
                    new ColumnTemplate("position", DataTypes.INTEGER, isPrimaryKey: true, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("ID"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("Relacion"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<RelacionHorarios>("HASH"), DataTypes.TEXT, isNotNull: true));
            }
        }

        static Func<string, TurnoEmpleado[]> parseJsonRelacion = (json) =>
        {
            try
            {
                return JsonConvert.DeserializeObject<TurnoEmpleado[]>(json);
            }
            catch
            {
                throw new Exception("Ocurrio un error inesperado al parsear la informacion del JsonRelaciones");
            }
        };

        Func<TurnoEmpleado[], string> buildJsonRelacion = (relacion) =>
        {
            try
            {
                List<string> parts = new List<string>();
                foreach (TurnoEmpleado i in relacion)
                {
                    parts.Add($@"{i.NoEmp} : {{
    ""Turno"" : ""{i.Turno}"",
    ""Nombre"" : ""{i.Nombre}"",
    ""Dia"" : ""{i.Dia}""
}}");
                }

                return JsonConvert.SerializeObject(parts, Formatting.Indented);
            }
            catch
            {
                throw new Exception("Ocurrio un error inesperado al parsear la informacion del JsonRelaciones");
            }
        };

        public static Response<RelacionHorarios> Get(RelacionHorarioID id, bool ShowObjectLog = false)
        {
            #region
            Response<RelacionHorarios> _response = new Response<RelacionHorarios>(false, "Iniciando obtencion del objeto", null);

            try
            {
                ConnectionsData _data = new ConnectionsData($@"{Program.DbPath}\RelacionTurnos.db", TABLE_NAME);
                Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
                SqliteDataReader _reader = _connection.MakeQuery("*");

                RelacionHorarios _obj = new RelacionHorarios();

                while (_reader.Read())
                {
                    _obj.ID = new RelacionHorarioID(_reader.GetString(1).Split("-")[0], int.Parse(_reader.GetString(0).Split("-")[1]));
                    _obj.Relacion = parseJsonRelacion(_reader.GetString(2));
                    _obj.HASH = new HexaHash(_reader.GetString(3));
                }

                _response.Success = true;
                _response.Message = $"Objeto '{id}' obtenido correctamente";
                _response.Object = _obj;
            }
            catch (Exception ex)
            {
                _response.Log.Add(ex.ToString());
                _response.Success = false;
                _response.Message = ex.Message;
                _response.Object = null;
            }

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

            return _resp;
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
                    (ParamSqlKey.GetValue<RelacionHorarios>("Relacion"), buildJsonRelacion(Relacion)),
                    (ParamSqlKey.GetValue<RelacionHorarios>("HASH"), HASH.Hash)
                };
                _resp.Log.Add($"Parametros SQL construidos...");

                string insertQuery = Common.BuildInsertQuery<RelacionHorarios>(this, DB_PATH, TABLE_NAME, IgnoreSqlColumnsParamsNull: true);
                _resp.Log.Add($"Cadena de insercion construida...");
                string updateQuery = Common.BuildUpdateQuery<RelacionHorarios>(this, DB_PATH, TABLE_NAME, $"{ColumnSqlName.GetValue<RelacionHorarios>("HASH")}={ParamSqlKey.GetValue<RelacionHorarios>("HASH")}", IgnoreSqlColumnsParamsNull: true);
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
            
            _resp.Message = _resp.Success ? $"Relacion de horario para '{ID}' guardad correctamente!" : $"Ocurrio un error al intentar guardar la relacion de horario '{ID}'!";

            return _resp;
            #endregion
        }
    }
}
