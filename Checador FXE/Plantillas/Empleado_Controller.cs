using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils;
using FlowCommonWorkcore.SqlUtils.SQLite;
using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

namespace Checador_FXE.Plantillas
{
    public class Empleado
    {
        public static readonly string DB_NAME = "EmpleadosSindicalizados.db";
        public static readonly string DB_PATH = $@"{Program.DbPath}\{DB_NAME}";
        public static readonly string TABLE_NAME = "empleados_sind";

        [ParamSqlKey("@NoEmp")]
        [ColumnSqlName("no_emp")]
        public string NoEmp { get; set; }
        [ParamSqlKey("@Nombres")]
        [ColumnSqlName("nombres")]
        public string Nombres { get; set; }
        [ParamSqlKey("@Apellidos")]
        [ColumnSqlName("apellidos")]
        public string Apellidos { get; set; }
        [ParamSqlKey("@Puesto")]
        [ColumnSqlName("puesto")]
        public string Puesto { get; set; }
        [ParamSqlKey("@Area")]
        [ColumnSqlName("area")]
        public string Area { get; set; } = "UdA";   // TODO: Ticket de tarea ##100196##
        [ParamSqlKey("@Region")]
        [ColumnSqlName("region")]
        public string Region { get; set; }
        [ParamSqlKey("@Division")]
        [ColumnSqlName("division")]
        public string Division { get; set; }
        [ParamSqlKey("@Localidad")]
        [ColumnSqlName("localidad")]
        public string Localidad { get; set; }
        [ParamSqlKey("@TurnoDefault")]
        [ColumnSqlName("turnoDefault")]
        public int TurnoDefault { get; set; } = 1;

        public static void InitializeDb()
        {
            // Crea el archivo de la base de datos en caso de que no exista
            if (!File.Exists(DB_PATH))
            {
                Response _db_resp = SQLiteAssets.CreateDataBase(DB_PATH, TABLE_NAME, overwrite: false);

                if (!_db_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la base de datos '{DB_PATH}'! {_db_resp.Message}");

                Response _tb_resp = SQLiteAssets.CreateTable(DB_PATH, TABLE_NAME,
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("NoEmp"), DataTypes.INTEGER, isPrimaryKey: true, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Nombres"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Apellidos"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Puesto"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Area"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Region"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Division"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("Localidad"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<Empleado>("TurnoDefault"), DataTypes.INTEGER, isNotNull: true)
                );

                if (!_tb_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la tabla '{TABLE_NAME}'! {_tb_resp.Message}");
            }
        }
        
        public static Response<Empleado[]> GetAll(string localidad, bool ShowObjectLog = false)
        {
            #region
            List<Empleado> _list = new List<Empleado>();
            Response<Empleado[]> _resp = new Response<Empleado[]>(false, "Iniciando obtencion de empleados...", null);

            ConnectionsData _data = new ConnectionsData(DB_PATH, TABLE_NAME);
            Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
            SqliteDataReader _reader = _connection.MakeQuery("*", "WHERE (localidad=@Localidad)", ("@Localidad", localidad));
            _resp.Log.Add("ConnectionsData y acciones de consulta realzadas...");

            List<string> _noEmpLists = new List<string>();

            while (_reader.Read()) 
            {
                _noEmpLists.Add(_reader.GetString(0));
                _resp.Log.Add($"Numero de empleado '{_reader.GetString(0)}' obtenido correctamente");
            }


            int fails = 0;

            // Consultamos todas las relaciones
            foreach (string i in _noEmpLists)
            {
                var getResp = Get(i);

                if (!getResp.Success)
                {
                    _resp.Log.Add($"No se pudo obtener el empleado correspondiente para el NoEmp '{i}'");
                    fails++;
                    continue;
                }

                _list.Add(getResp.Object!);
                _resp.Log.Add($"Empleado '{i}' obtenida correctamente...");
            }

            _resp.Success = true;
            _resp.Object = _list.ToArray();
            _resp.Message = $"Proceso de obtencion de Empleados finalizado {fails} errores!";

            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public static Response<Empleado> Get(string noEmp, bool ShowObjectLog = false)
        {
            #region
            Response<Empleado> _response = new Response<Empleado>(false, "Iniciando obtencion del objeto", null);

            try
            {
                ConnectionsData _data = new ConnectionsData(DB_PATH, TABLE_NAME);
                Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
                SqliteDataReader _reader = _connection.MakeQuery("*", "WHERE (no_emp=@NoEmp)", ("@NoEmp", noEmp));

                Empleado _obj = new Empleado();

                while (_reader.Read())
                {
                    _obj.NoEmp = _reader.GetString(0);
                    _obj.Nombres = _reader.GetString(1);
                    _obj.Apellidos = _reader.GetString(2);
                    _obj.Puesto = _reader.GetString(3);
                    _obj.Area = _reader.GetString(4);
                    _obj.Region = _reader.GetString(5);
                    _obj.Division = _reader.GetString(6);
                    _obj.Localidad = _reader.GetString(7);
                    _obj.TurnoDefault = _reader.GetInt32(8);
                }

                _response.Success = true;
                _response.Message = $"Objeto '{noEmp}' obtenido correctamente";
                _response.Object = _obj;
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

        public Response Save(bool ShowObjectLog = false)
        {
            #region
            /*
            Response _resp = new Response(false, "Iniciando guardado del objeto...");

            Server.SqlWriteConnection _connection = new Server.SqlWriteConnection(new ConnectionsData(
               Properties.Settings.Default.SERVER_HOSTNAME,
               Properties.Settings.Default.SERVER_USER,
               Properties.Settings.Default.SERVER_PASS,
               Int32.Parse(Properties.Settings.Default.SERVER_PORT),
               Empleado.TABLE_NAME,
               Empleado.DATABASE_NAME
            ));

            var parameters = new (string, object)[]
            {
                ("@NoEmp", NoEmp),
                ("@Nombres", Nombres),
                ("@Apellidos", Apellidos),
                ("@Puesto", Puesto),
                ("@Region", Region),
                ("@Division", Division),
                ("@Localidad", Localidad),
                ("@Area", Area)
            };

            _resp.Log.Add("Parametros construidos...");

            string ADDITION_QUERY = Common.BuildInsertQuery<Empleado>(this, Empleado.DATABASE_NAME, Empleado.TABLE_NAME);
            _resp.Log.Add("Cadena de adicion construida...");
            string UPDATE_QUERY = Common.BuildUpdateQuery<Empleado>(this, Empleado.DATABASE_NAME, Empleado.TABLE_NAME, conditional: "no_emp=@NoEmp");
            _resp.Log.Add("Cadena de actualizacion construida...");

            Response SERVER_RESPONSE = _connection.MakeQuery(
                "no_emp",
                NoEmp,
                ADDITION_QUERY,
                UPDATE_QUERY,
                parameters
            );
            _resp.Log.Add($"Consulta al servidor realizada...");

            _resp.Success = SERVER_RESPONSE.Success;
            if (SERVER_RESPONSE.Success)
                _resp.Message = $"Informacion del objeto actualizada/insertada con exito!";
            else
                _resp.Message = $"Error al actualizar/insertar la informacion del objeto!\n{SERVER_RESPONSE.GetBuildedLog()}";


            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            */

            Response _resp = new Response(false, "Iniciando guardado del objeto...");
            
            Server.SqlWriteConnection _connection = new Server.SqlWriteConnection(new ConnectionsData(DB_PATH, TABLE_NAME));
            _resp.Log.Add($"Conexion de escritura establecida...");

            SqlCmdParam[] parameters = Common.BuildParamsArray<Empleado>(this);
            _resp.Log.Add($"Parametros construidos");

            string ADDITION_QUERY = Common.BuildInsertQuery<Empleado>(this, Empleado.DB_NAME, Empleado.TABLE_NAME);
            _resp.Log.Add("Cadena de adicion construida...");
            string UPDATE_QUERY = Common.BuildUpdateQuery<Empleado>(this, Empleado.DB_NAME, Empleado.TABLE_NAME, conditional: "no_emp=@NoEmp");
            _resp.Log.Add("Cadena de actualizacion construida...");

            Response SERVER_RESPONSE = _connection.MakeQuery(
                ColumnSqlName.GetValue<Empleado>("NoEmp"),
                NoEmp,
                ADDITION_QUERY,
                UPDATE_QUERY,
                parameters
            );
            _resp.Log.Add($"Consulta al servidor realizada...");

            _resp.Success = SERVER_RESPONSE.Success;
            if (SERVER_RESPONSE.Success)
                _resp.Message = $"Informacion del objeto actualizada/insertada con exito!";
            else
                _resp.Message = $"Error al actualizar/insertar la informacion del objeto!\n{SERVER_RESPONSE.GetBuildedLog()}";


            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) 
                return false;

            return ((Empleado)obj).NoEmp == this.NoEmp;
        }
    }
}
