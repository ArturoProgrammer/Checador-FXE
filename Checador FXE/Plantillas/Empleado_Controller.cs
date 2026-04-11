using DocumentFormat.OpenXml.Drawing.Diagrams;
using FlowCommonWorkcore;
using FlowCommonWorkcore.SqlUtils;
using FlowCommonWorkcore.SqlUtils.MySQL;
using MySql.Data.MySqlClient;

namespace Checador_FXE.Plantillas
{
    public class Empleado
    {
        public static readonly string TABLE_NAME = "empleados_sind";
        public static readonly string DATABASE_NAME = "checador_fxe_db";

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

        public Empleado() { }

        public static Response<Empleado[]> GetAll(string localidad, bool ShowObjectLog = false)
        {
            #region
            List<Empleado> objList = new List<Empleado>();
            Response<Empleado[]> _resp = new Response<Empleado[]>(false, "Iniciando obtencion de empleados...", null);

            MySqlDataReader _query = new Server.GeneralQuery(new ConnectionsData(
                Properties.Settings.Default.SERVER_HOSTNAME,
                Properties.Settings.Default.SERVER_USER,
                Properties.Settings.Default.SERVER_PASS,
                int.Parse(Properties.Settings.Default.SERVER_PORT),
                Empleado.TABLE_NAME,
                Empleado.DATABASE_NAME
            )).ExecuteQuery(
                $"SELECT * FROM {Empleado.DATABASE_NAME}.{Empleado.TABLE_NAME} WHERE (Localidad=@Localidad);",
                new (string, object)[] { ("@Localidad", localidad) }
            );
            _resp.Log.Add("Conexion con el servidor realizada...");

            try
            {
                _resp.Log.Add($"Iniciando lectura de los objetos encontrados...");
                int count = 1;
                while (_query.Read())
                {
                    _resp.Log.Add($"Leyendo objeto '{count}'");

                    objList.Add(new Empleado()
                    {
                        NoEmp = _query.GetInt32(0).ToString(),
                        Nombres = _query.GetString(1),
                        Apellidos = _query.GetString(2),
                        Puesto = _query.GetString(3),
                        Region = _query.GetString(4),
                        Division = _query.GetString(5),
                        Localidad = _query.GetString(6)
                    });
                    _resp.Log.Add($"Empleado '{_query.GetInt32(0)}' obtenido...");

                    count++;
                }

                _resp.Object = objList.ToArray();
                _resp.Success = true;
                _resp.Message = $"Todos los empleados para la localidad de '{localidad}' obtenido con exito!";
            }
            catch (Exception ex)
            {
                _resp.Success = false;
                _resp.Message = $"Excepcion inesperada al consultar los empleados!\n{ex.Message}";
                _resp.Log.Add($"{ex}");
                MessageBox.Show($"Ocurrió un error al cargar los empleados de la localidad seleccionada.\n\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _query.Close();
            }

            if (ShowObjectLog)
                MessageBox.Show(_resp.GetBuildedLog(), "Log del Objeto");

            return _resp;
            #endregion
        }

        public Response Save(bool ShowObjectLog = false)
        {
            #region
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
