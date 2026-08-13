using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils;
using FlowCommonWorkcore.SqlUtils.SQLite;
using Microsoft.Data.Sqlite;

namespace Checador_FXE.Plantillas
{
    public class GlobalConfig
    {
        public static readonly string DB_NAME = "GlobalConfig.db";
        public static readonly string DB_PATH = $@"{Program.DbPath}\{DB_NAME}";
        public static readonly string TABLE_NAME = "global_config";

        [ColumnSqlName("config_id")]
        [ParamSqlKey("@ConfigId")]
        public int ID { get; set; }
        [ColumnSqlName("all_allowed_sites")]
        [ParamSqlKey("@AllowedSites")] 
        public string[] LocalidadesCompatibles { get; set; }
        [ColumnSqlName("config_name")]
        [ParamSqlKey("@ConfigName")]
        public string TituloConfiguracion { get; set; }

        public static void InitializeDb()
        {
            // Crea el archivo de la base de datos en caso de que no exista
            if (!File.Exists(DB_PATH))
            {
                Response _db_resp = SQLiteAssets.CreateDataBase(DB_PATH, TABLE_NAME, overwrite: false);

                if (!_db_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la base de datos '{DB_PATH}'! {_db_resp.Message}");

                Response _tb_resp = SQLiteAssets.CreateTable(DB_PATH, TABLE_NAME,
                    new ColumnTemplate(ColumnSqlName.GetValue<GlobalConfig>("ID"), DataTypes.INTEGER, isPrimaryKey: true, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<GlobalConfig>("LocalidadesCompatibles"), DataTypes.TEXT, isNotNull: true),
                    new ColumnTemplate(ColumnSqlName.GetValue<GlobalConfig>("TituloConfiguracion"), DataTypes.TEXT, isNotNull: true)
                );

                if (!_tb_resp.Success)
                    throw new NullReferenceException($"Ocurrio un error al crear la tabla '{TABLE_NAME}'! {_tb_resp.Message}");
            }

            // Asignamos los valores por defecto en caso de que no se encuentren asignados
            AssignDbDefaultValues();
        }

        static void AssignDbDefaultValues()
        {
            #region
            GlobalConfig conf = new GlobalConfig()
            {
                ID = 1,
                LocalidadesCompatibles = new string[] { "Hermosillo", "Nogales", "Sufragio" },
                TituloConfiguracion = "Default"
            };

            string[] columnNames = {    ColumnSqlName.GetValue<GlobalConfig>("ID"),
                                        ColumnSqlName.GetValue<GlobalConfig>("LocalidadesCompatibles"),
                                        ColumnSqlName.GetValue<GlobalConfig>("TituloConfiguracion") };
            string[] columnValues = {   ParamSqlKey.GetValue<GlobalConfig>("ID"),
                                        ParamSqlKey.GetValue<GlobalConfig>("LocalidadesCompatibles"),
                                        ParamSqlKey.GetValue<GlobalConfig>("TituloConfiguracion") };

            SqlCmdParamParsingConfig _scpConfig = new SqlCmdParamParsingConfig().DisableIgnoreSqlColumnsParamsNull()
                                                                                .AddParsingProcess(new ParsingProcess()
                                                                                {
                                                                                    Type = typeof(string[]),
                                                                                    Process = (object a) => String.Join(";", (a as string[])!)
                                                                                });
            // Validamos que no tenga los valores correspondientes
            Server.GeneralQuery query = new Server.GeneralQuery(new ConnectionsData(GlobalConfig.DB_PATH, GlobalConfig.TABLE_NAME))
                .SetParams(Common.BuildParamsArray<GlobalConfig>(conf, _scpConfig))
                .SetQuery($@"
UPDATE {GlobalConfig.TABLE_NAME}
SET 
    {columnNames[0]} = CASE 
                  WHEN {columnNames[0]} IS NULL OR {columnNames[0]} = '' THEN {columnValues[0]}
                  ELSE {columnNames[0]} 
               END,
    {columnNames[1]} = CASE 
                  WHEN {columnNames[1]} IS NULL OR {columnNames[1]} = '' THEN {columnValues[1]}
                  ELSE {columnNames[1]} 
               END,
    {columnNames[2]} = CASE 
                  WHEN {columnNames[2]} IS NULL OR {columnNames[2]} = '' THEN {columnValues[2]}
                  ELSE {columnNames[2]} 
               END
");
            var _resp = query.ExecuteNonQuery(ShowCommandPreview: false);

            conf.Save();
            #endregion
        }

        public static Response<GlobalConfig[]> GetAll(bool ShowObjectLog = false)
        {
            #region
            List<GlobalConfig> _list = new List<GlobalConfig>();
            Response<GlobalConfig[]> _resp = new Response<GlobalConfig[]>(false, "Iniciando obtencion de configuracion...", null);

            ConnectionsData _data = new ConnectionsData(DB_PATH, TABLE_NAME);
            Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
            SqliteDataReader _reader = _connection.MakeQuery("*");
            _resp.Log.Add("ConnectionsData y acciones de consulta realzadas...");

            List<string> _configLists = new List<string>();

            while (_reader.Read())
            {
                _configLists.Add(_reader.GetString(0));
                _resp.Log.Add($"ID de configuracion '{_reader.GetString(0)}' obtenido correctamente");
            }

            int fails = 0;

            // Consultamos todas las relaciones
            foreach (string i in _configLists)
            {
                var getResp = Get(i);

                if (!getResp.Success)
                {
                    _resp.Log.Add($"No se pudo obtener la configuracion correspondiente para el ID '{i}'");
                    fails++;
                    continue;
                }

                _list.Add(getResp.Object!);
                _resp.Log.Add($"Configuracion '{i}' obtenida correctamente...");
            }

            _resp.Success = true;
            _resp.Object = _list.ToArray();
            _resp.Message = $"Proceso de obtencion de Configuraicon finalizado {fails} errores!";

            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public static Response<GlobalConfig> Get(string id, bool ShowObjectLog = false)
        {
            #region
            Response<GlobalConfig> _response = new Response<GlobalConfig>(false, "Iniciando obtencion del objeto", null);

            try
            {
                ConnectionsData _data = new ConnectionsData(DB_PATH, TABLE_NAME);
                Server.SqlReadConnection _connection = new Server.SqlReadConnection(_data);
                SqliteDataReader _reader = _connection.MakeQuery("*");

                GlobalConfig _obj = new GlobalConfig();

                while (_reader.Read())
                {
                    _obj.ID = _reader.GetInt32(0);
                    _obj.LocalidadesCompatibles = _reader.GetString(1).Split(";");
                    _obj.TituloConfiguracion = _reader.GetString(2);
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

            if (ShowObjectLog)
                MessageBox.Show(_response.GetBuildedLog(), "Log del Objeto");

            return _response;
            #endregion
        }

        public Response Save(bool ShowObjectLog = false)
        {
            #region
            Response _resp = new Response(false, "Iniciando guardado del objeto...");

            Server.SqlWriteConnection _connection = new Server.SqlWriteConnection(new ConnectionsData(DB_PATH, TABLE_NAME));
            _resp.Log.Add($"Conexion de escritura establecida...");

            SqlCmdParamParsingConfig _scpConf = new SqlCmdParamParsingConfig().DisableIgnoreSqlColumnsParamsNull()
                                                                                .AddParsingProcess(new ParsingProcess()
                                                                                {
                                                                                    Type = typeof(string[]),
                                                                                    Process = (object a) =>
                                                                                    {
                                                                                        return String.Join(";", (a as string[])!);
                                                                                    }
                                                                                });

            SqlCmdParam[] parameters = Common.BuildParamsArray<GlobalConfig>(this, _scpConf);

            _resp.Log.Add($"Parametros construidos");

            string ADDITION_QUERY = Common.BuildInsertQuery<GlobalConfig>(this, GlobalConfig.TABLE_NAME);
            _resp.Log.Add("Cadena de adicion construida...");
            string UPDATE_QUERY = Common.BuildUpdateQuery<GlobalConfig>(this, GlobalConfig.TABLE_NAME, conditional: "config_id=@ConfigId");
            _resp.Log.Add("Cadena de actualizacion construida...");

            Response SERVER_RESPONSE = _connection.MakeQuery(
                ColumnSqlName.GetValue<GlobalConfig>("ID"),
                ID.ToString(),
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
    }
}
