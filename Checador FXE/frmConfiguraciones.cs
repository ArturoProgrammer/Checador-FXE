using FlowCommonWorkcore.SqlUtils.MySQL;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using FlowControls;
using FlowControls.Utils;
using FlowControls.Security;

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
            /* 
             * Formato a resolver:
             * 
             * 
				 {
					"1" : {
						"primer_horario": {
							"entrada": 800,
							"salida": 1500
						},
						"segundo_horario" : {
							"entrada": 0,
							"salida": 0
						},
						"tiempo_extra": {
							"entrada": 0,
							"salida": 0
						}
					},
					"2" : {
						"primer_horario": {
							"entrada": 800,
							"salida": 1300
						},
						"segundo_horario" : {
							"entrada": 1500,
							"salida": 1700
						},
						"tiempo_extra": {
							"entrada": 0,
							"salida": 0
						}
					},
					"3" : {
						"primer_horario": {
							"entrada": 1500,
							"salida": 1700
						},
						"segundo_horario" : {
							"entrada": 0,
							"salida": 0
						},
						"tiempo_extra": {
							"entrada": 0,
							"salida": 0
						}
					}
				}
             * */

            foreach (Turno i in Turno.GetAll(text))
            {
                this.dgvAjustesHorarios.Rows.Add(new[]
                {
                    $"{i.ID}",
                    $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.PrimerHorario.Salida)}",
                    $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Entrada)}", $"{Utils.WriteNotEmptyTimes(i.SegundoHorario.Salida)}"
                });
            }
            #endregion
        }

        private void frmConfiguraciones_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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
            DISPOSITIVO_DEFAULT, COLOR_PINCEL, NOMBRE_ARCHIVO, LOCALIDAD_DEFAULT,

            // Pestaña de ajustes
            AJUSTES_HORARIO,

            DIRECCION_SERVIDOR, USUARIO_SERVIDOR, PASS_SERVIDOR, PUERTO_SERVIDOR
            #endregion
        }

        void MultiValidator()
        {
            #region

            #endregion
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Ejecutamos los multivalidadores


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

        Dictionary<string, string> _ServerProps = new Dictionary<string, string>()
        {
            { "hostname", Properties.Settings.Default.SERVER_HOSTNAME },
            { "user", Properties.Settings.Default.SERVER_USER },
            { "pass", Properties.Settings.Default.SERVER_PASS },
            { "port", Properties.Settings.Default.SERVER_PORT.ToString() }
        };

        bool _ServerPropsHasChanged()
        {
            if (this.txtHostnameTcpIp.Value != _ServerProps["hostname"])
                return true;
            if (this.txtUsuarioServidor.Value != _ServerProps["user"])
                return true;
            if (this.txtPassServidor.Value != _ServerProps["pass"])
                return true;
            if (this.txtPuerto.Value.ToString() != _ServerProps["port"])
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
                    new (string, object)[] { ("@Name", "Default") }
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

        private void txtNombreArchivoDefecto_Validating(object sender, CancelEventArgs e)
        {

        }

        private void cboxLocalidadEstablecida_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboxLocalidadEstablecida.IsNonSelectedTextSelected)
                MessageBox.Show("Debes de seleccionar un elemento!");
        }
    }
}
