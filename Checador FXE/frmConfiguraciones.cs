using FlowCommonWorkcore.SqlUtils.MySQL;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using FlowControls.Utils;
using FlowControls.Security;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Checador_FXE
{
    public partial class frmConfiguraciones : Form
    {
        public frmConfiguraciones()
        {
            InitializeComponent();
        }

        private void frmConfiguraciones_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            /*
             * Establecemos por seguridad una contraseña para las opciones de configuracion de
             * conexion al servidor
             */
            this.flTabMenuControl1.SetPrivateTab("tabServidor", new PasswordAuthPolicy("F3rr0m3x1c0"));

            //
            // GENERAL
            //
            this.txtMaximoRetrasoMinutosPermitidos.Value = new TimeSpan(0, Properties.Settings.Default.MINUTOS_TOLERANCIA, 0);
            this.cboxDispositivoDefault.Items.AddRange(DispositivoExtensions.GetSupportedModels());
            this.cboxDispositivoDefault.Value = Properties.Settings.Default.DISPOSITIVO_DEFAULT; // Seleccionamos el dispositivo por default
            this.cboxColorPincel.Value = Properties.Settings.Default.COLOR_PINCEL;
            this.txtNombreArchivoDefecto.Value = Properties.Settings.Default.DEFAULT_FILENAME;

            //this.cboxLocalidadEstablecida.Items.AddRange(Utils.GetLocalidadesDisponibles());
            this.cboxLocalidadEstablecida.Items.AddRange(new[] { "Hermosillo", "Nogales", "Sufragio" });
            this.cboxLocalidadEstablecida.Value = Properties.Settings.Default.LOCALIDAD_DEFAULT;

            //
            // AJUSTES DE HORARIO
            //
            _LoadHorariosFromJson(Properties.Settings.Default.TURNOS_HORARIOS);

            //
            // SERVIDOR
            //
            this.txtHostnameTcpIp.Value = Properties.Settings.Default.SERVER_HOSTNAME;
            this.txtUsuarioServidor.Value = Properties.Settings.Default.SERVER_USER;
            this.txtPassServidor.Value = Properties.Settings.Default.SERVER_PASS;
            this.txtPuerto.Value = Properties.Settings.Default.SERVER_PORT;

            this.Cursor = Cursors.Default;
        }

        void _LoadHorariosFromJson(string text)
        {
            #region CARGA DE VALORES JSON
            foreach (Turno i in Turno.GetAll(text))
            {
                this.dgvAjustesHorarios.Rows.Add(new[]
                {
                    $"{i.ID}",
                    $"{i.Nombre}",
                    $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Salida)}",
                    $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Salida)}"
                });
            }
            #endregion
        }

        private void frmConfiguraciones_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.btnCerrar.PerformClick();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        enum Fields
        {
            #region
            [ControlValidateAttrib("txtMaximoRetrasoMinutosPermitidos", ControlField.FLTEXTBOXLABELJOINT)]
            TIEMPO_MAXIMO_RETRASO,

            [ControlValidateAttrib("cboxDispositivoDefault", ControlField.FLCOMBOBOXLABELJOINT)]
            DISPOSITIVO_DEFAULT,
            [ControlValidateAttrib("cboxColorPincel", ControlField.FLCOMBOBOXLABELJOINT)]
            COLOR_PINCEL, 
            [ControlValidateAttrib("txtNombreArchivoDefecto", ControlField.FLTEXTBOXLABELJOINT)]
            NOMBRE_ARCHIVO,
            [ControlValidateAttrib("cboxLocalidadEstablecida", ControlField.FLCOMBOBOXLABELJOINT)]
            LOCALIDAD_DEFAULT,

            // Pestaña de ajustes
            [ControlValidateAttrib("dgvAjustesHorarios", ControlField.GENERIC)]
            AJUSTES_HORARIO,

            [ControlValidateAttrib("txtHostnameTcpIp", ControlField.FLTEXTBOXLABELJOINT)]
            DIRECCION_SERVIDOR, 
            [ControlValidateAttrib("txtUsuarioServidor", ControlField.FLTEXTBOXLABELJOINT)] 
            USUARIO_SERVIDOR, 
            [ControlValidateAttrib("txtPassServidor", ControlField.FLTEXTBOXLABELJOINT)]
            PASS_SERVIDOR,
            [ControlValidateAttrib("txtPuerto", ControlField.FLTEXTBOXLABELJOINT)]
            PUERTO_SERVIDOR,

            // Datos de conexion local
            [ControlValidateAttrib("txtRutaLocalDb", ControlField.FLTEXTBOXLABELJOINT)]
            RUTA_DB_LOCAL,
            #endregion
        }

        bool MultiValidator(Fields f)
        {
            #region
            Multivalidator mv = new Multivalidator(this);
            bool flag;
            switch (f)
            {
                case Fields.AJUSTES_HORARIO:
                    flag = mv.Validate<Fields>(f, invalidValues: null, () => {
                        // Realizamos la validacion del DGV de los turnos existentes
                        if (this.dgvAjustesHorarios.Rows.Count == 0)
                            return false;

                        // Las condiciones son: ID, titulo y que al menos haya un horario establecido
                        List<bool> fails = new List<bool>();
                        foreach (DataGridViewRow row in this.dgvAjustesHorarios.Rows)
                        {
                            // Validamos primer condicion
                            if (row.Cells[TurnosGridCells.NUMBER.GetIndex()].Value == null || string.IsNullOrWhiteSpace(row.Cells[TurnosGridCells.NUMBER.GetIndex()].Value.ToString()?.Trim()))
                                fails.Add(false);
                            // Validamos segunda condicion
                            if (row.Cells[TurnosGridCells.NOMBRE.GetIndex()].Value == null || string.IsNullOrWhiteSpace(row.Cells[TurnosGridCells.NOMBRE.GetIndex()].Value.ToString()?.Trim()))
                                    fails.Add(false);
                            
                            // Validamos tercer condicion, minimo, debe estar establecido el primer horario
                            if (String.IsNullOrEmpty(row.Cells[TurnosGridCells.FIRST_IN.GetIndex()].Value.ToString()?.Trim()) || 
                                String.IsNullOrEmpty(row.Cells[TurnosGridCells.FIRST_OUT.GetIndex()].Value.ToString()?.Trim()))
                                fails.Add(false);

                            // En caso de haber un segundo horario, debe de estar completo (con entrada y salida valida)
                            if (!String.IsNullOrEmpty(row.Cells[TurnosGridCells.SECOND_IN.GetIndex()].Value.ToString()?.Trim()) || 
                                !String.IsNullOrEmpty(row.Cells[TurnosGridCells.SECOND_OUT.GetIndex()].Value.ToString()?.Trim()))
                            {
                                if (String.IsNullOrEmpty(row.Cells[TurnosGridCells.SECOND_IN.GetIndex()].Value.ToString()?.Trim()) || 
                                    String.IsNullOrEmpty(row.Cells[TurnosGridCells.SECOND_OUT.GetIndex()].Value.ToString()?.Trim()))
                                    fails.Add(false);
                            }
                        }

                        return fails.Count == 0;
                    }, ValidationParams.CUSTOM_ACTION, ValidationParams.NOT_EMPTY_ENTRY).Success;
                    break;
                case Fields.RUTA_DB_LOCAL:
                    //
                    // Omitimos la validacion por el momento
                    //
                    flag = true;
                    break;
                default:
                    flag = mv.Validate<Fields>(f, ValidationParams.NOT_EMPTY_ENTRY).Success;
                    break;
            }

            return flag;
            #endregion
        }

        bool PassAllValidations()
        {
            List<bool> _validations = new List<bool>();

            foreach (Fields field in Enum.GetValues(typeof(Fields)))
            {
                _validations.Add(MultiValidator(field));
            }

            return _validations.All(v => v);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Ejecutamos los multivalidadores
            if (!PassAllValidations())
                return;
            /* 
             * Guardamos los cambios efectuados
             * */
            //
            // GENERAL
            //
            Properties.Settings.Default.DISPOSITIVO_DEFAULT = this.cboxDispositivoDefault.Value;
            Properties.Settings.Default.MINUTOS_TOLERANCIA = this.txtMaximoRetrasoMinutosPermitidos.Value!.Value.Minutes;
            Properties.Settings.Default.COLOR_PINCEL = this.cboxColorPincel.Value;
            Properties.Settings.Default.DEFAULT_FILENAME = this.txtNombreArchivoDefecto.Value;

            Properties.Settings.Default.LOCALIDAD_DEFAULT = this.cboxLocalidadEstablecida.Value;

            //
            // AJUSTES DE HORARIO
            //
            Properties.Settings.Default.TURNOS_HORARIOS = Utils.ParseJsonHorariosByDgv(this.dgvAjustesHorarios);

            //
            // SERVIDOR
            //
            if (_ServerPropsHasChanged())
            {
                DialogResult dr = MessageBox.Show("Ha realizado cambios en la configuración del servidor.\n¿Desea aplicar los cambios y reiniciar la aplicación para que surtan efecto?", "Cambios en configuración del servidor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Properties.Settings.Default.SERVER_HOSTNAME = this.txtHostnameTcpIp.Value;
                    Properties.Settings.Default.SERVER_USER = this.txtUsuarioServidor.Value;
                    Properties.Settings.Default.SERVER_PASS = this.txtPassServidor.Value;
                    Properties.Settings.Default.SERVER_PORT = this.txtPuerto.Value;
                    Application.Restart();
                }
            }
            this.Close();
        }

        struct ServerProps
        {
            public static string hostname { get; } = Properties.Settings.Default.SERVER_HOSTNAME;
            public static string user { get; } = Properties.Settings.Default.SERVER_USER;
            public static string password { get; } = Properties.Settings.Default.SERVER_PASS;
            public static string port { get; } = Properties.Settings.Default.SERVER_PORT;
        }

        bool _ServerPropsHasChanged()
        {
            if (this.txtHostnameTcpIp.Value != ServerProps.hostname)
                return true;
            if (this.txtUsuarioServidor.Value != ServerProps.user)
                return true;
            if (this.txtPassServidor.Value != ServerProps.password)
                return true;
            if (this.txtPuerto.Value.ToString() != ServerProps.port)
                return true;
            return false;
        }

        /// <summary>
        /// Probamos la conexion indicada con el servidor
        /// </summary>
        /// <param name="hostname">Direccion IP o Hostname del servidor</param>
        /// <param name="user">Usuario de la base de datos</param>
        /// <param name="pass">Contraseña de la base de datos</param>
        /// <param name="port">Puerto de conexion a la base de datos</param>
        /// <returns></returns>
        async Task<bool> TestConexion(string hostname, string user, string pass, int port)
        {
            #region
            MySqlDataReader? _query = null;
            bool flag = false;

            try
            {
                ConnectionsData _connection = new ConnectionsData(
                    hostname, user, pass, port,
                    "global_config",
                    "checador_fxe_db"
                );

                _query = new Server.GeneralQuery(_connection).ExecuteQuery(
                    $"SELECT config_id FROM checador_fxe_db.global_config WHERE (config_name=@Name);",
                    ShowCommandPreview: false,
                    ("@Name", "Default")
                );

                flag = _query.HasRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se ha producido una excepcion inesperada al establecer la conexion! {ex.Message}", "Excepcion inesperada");
            }
            finally
            {
                _query?.Close();
            }

            return flag;
            #endregion
        }


        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (TestConexion(this.txtHostnameTcpIp.Value, this.txtUsuarioServidor.Value, this.txtPassServidor.Value, Int32.Parse(this.txtPuerto.Value)).Result)
            {
                MessageBox.Show("Conexión exitosa al servidor MySQL.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se pudo conectar al servidor MySQL. Verifique la configuración e intente nuevamente.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void txtHostnameTcpIp_OnTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtHostnameTcpIp.Value.Trim()))
                MessageBox.Show("No puedes dejar este campo vacio!");
        }

        private void txtUsuarioServidor_OnTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtUsuarioServidor.Value.Trim()))
                MessageBox.Show("No puedes dejar este campo vacio!");
        }

        private void txtPassServidor_OnTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPassServidor.Value.Trim()))
                MessageBox.Show("No puedes dejar este campo vacio!");
        }

        private void txtPuerto_OnTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPuerto.Value.Trim()))
                MessageBox.Show("No puedes dejar este campo vacio!");
        }

        private void txtMaximoRetrasoMinutosPermitidos_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtMaximoRetrasoMinutosPermitidos.Value.HasValue && !String.IsNullOrWhiteSpace(this.txtMaximoRetrasoMinutosPermitidos.Text))
                return;
        }

        private void cboxDispositivoDefault_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboxDispositivoDefault.IsNonSelectedTextSelected)
                MessageBox.Show("Debes de seleccionar un elemento!");
        }

        private void cboxColorPincel_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboxColorPincel.IsNonSelectedTextSelected)
                MessageBox.Show("Debes de seleccionar un elemento!");
        }

        private void cboxLocalidadEstablecida_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboxLocalidadEstablecida.IsNonSelectedTextSelected)
                MessageBox.Show("Debes de seleccionar un elemento!");
        }

        private void dgvAjustesHorarios_OnAddClick(object sender, EventArgs e)
        {
            this.dgvAjustesHorarios.Rows.Add(1);    // Se agrega una linea por defecto

            // Establecemos el ID del turno segun el turno anterior
            this.dgvAjustesHorarios.Rows[this.dgvAjustesHorarios.Rows.Count - 1].Cells[0].Value =
                this.dgvAjustesHorarios.Rows.Count > 1
                ? (int.Parse(this.dgvAjustesHorarios.Rows[this.dgvAjustesHorarios.Rows.Count - 2].Cells[0].Value.ToString()) + 1).ToString()
                : "1";
        }
    }
}
