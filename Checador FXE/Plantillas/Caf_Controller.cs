using Checador_FXE.MdiForms;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using FlowCommonWorkcore;
using FlowControls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Checador_FXE.Plantillas
{
    //
    // TODO: Archivo de proyecto. Ticket: ##100185##
    //
    /// <summary>
    /// Archivo de proyecto para la aplicacion
    /// </summary>
    internal class CafProjFile
    {
        public static readonly string FileExtension = "caf";
        public static readonly string FileExtensionName = "Chequeo de Asistencia Ferromex";
        public static string PathTempFile { get; private set; }
        public static string DefaultProjFilePath { get; private set;  }

        void InitializeClass()
        {
            // Asignamos la ruta de la carpeta temporal
            CafProjFile.PathTempFile = $@"{Path.GetTempPath()}\{Application.ProductName}";
            // Asignamos la ruta de guardado por defecto para los proyectos
            CafProjFile.DefaultProjFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Creamos la carpeta en caso de que no exista y la ocultamos
            if (!Directory.Exists(CafProjFile.PathTempFile))
            {
                Directory.CreateDirectory(CafProjFile.PathTempFile);
                DirectoryInfo di = new DirectoryInfo(CafProjFile.PathTempFile);
                di.Attributes |= FileAttributes.Hidden;
            }
        }

        internal class GeneralTab
        {
            public static readonly string FILEPATH = @"data\general.json";

            public string AreaRemitente { get; set; }
            public string LugarRemitente { get; set; }
            public DateOnly Fecha { get; set; }
            public string NombreElaborador { get; set; }
            public string Autorizador { get; set; }

            public string MakeJson() => JsonSerializer.Serialize<GeneralTab>(this, options: new JsonSerializerOptions() { WriteIndented = true });
            public static GeneralTab? Build(string jsonText) => JsonSerializer.Deserialize<GeneralTab>(jsonText, options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        internal class ConfiguracionCastingTab
        {
            public static readonly string FILEPATH = @"data\configuracion_casting.json";

            public string TurnosCrudJson { get; set; }
            public TimeSpan TiempoRetrasoPermitido { get; set; }
            public bool DomingosNoLaborables { get; set; }

            public string MakeJson() => JsonSerializer.Serialize<ConfiguracionCastingTab>(this, options: new JsonSerializerOptions() { WriteIndented = true });
            public static ConfiguracionCastingTab? Build(string jsonText) => JsonSerializer.Deserialize<ConfiguracionCastingTab>(jsonText, options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        internal class ResultadosCastingTab
        {
            public static readonly string FILEPATH = @"data\resultados_casting.json";

            /* 
             * TODO: ver la manera de que se guarden los resultados del casting tomando en cuenta
             *       que posiblemente ya se realizaron modificaciones previas.
             *       Posiblemente la idea sea una estructura de tipo de datos de diccionario, en
             *       donde la clave sera el nombre o numero de empleado y el valor los eventos en
             *       el calendario de ese empleado
             *       
             * */

            public Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> PeriodoCasteado { get; set; }

            public flTreeViewPaging DataSourceControl { get; }

            public ResultadosCastingTab(flTreeViewPaging pagingView)
            {
                this.DataSourceControl = pagingView;
            }

            public string MakeJson()
            {
                //string jsonText = JsonSerializer.Serialize<ResultadosCastingTab>(this, options: new JsonSerializerOptions() { WriteIndented = true });

                Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _array = new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>();
                MessageBox.Show(DataSourceControl.Items.Count.ToString());
                foreach (InteropGenericObject obj in DataSourceControl.Items)
                {
                    /*
                    Dictionary<string, string> item = new Dictionary<string, string>() {
                        { "" }
                    };
                    */
                    MessageBox.Show(obj.ObjectTitle);

                    //MessageBox.Show(JsonSerializer.Serialize<InteropGenericObject>(obj, options: new JsonSerializerOptions() { WriteIndented = true }));
                }

                //return jsonText;
                return "";
            }
            public static ResultadosCastingTab? Build(string jsonText) => JsonSerializer.Deserialize<ResultadosCastingTab>(jsonText, options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        internal class Assets
        {
            public static readonly string FILEPATH = $@"assets.json";

            public string Title { get; set; }
            public Dispositivo Device { get; set; }

            public string MakeJson() => JsonSerializer.Serialize<Assets>(this, options: new JsonSerializerOptions() { WriteIndented = true });
            public static Assets? Build(string jsonText) => JsonSerializer.Deserialize<Assets>(jsonText, options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        
        public mdiQuincenaView MdiForm { get; set; }
        public GeneralTab General { get; private set; }
        public ConfiguracionCastingTab ConfiguracionCasting { get; private set; }
        public ResultadosCastingTab ResultadosCasting { get; private set; }
        public (string Filename, byte[] Content) SourceFile { get; private set; }
        public Assets AssetsFile { get; private set; }


        /// <summary>
        /// Constructor basado en un nuevo proyecto
        /// </summary>
        /// <param name="frm"></param>
        public CafProjFile(mdiQuincenaView frm)
        {
            InitializeClass();

            this.MdiForm = frm;
            this.General = new GeneralTab()
            {
                AreaRemitente = MdiForm.txtAreaRemitente.Value,
                LugarRemitente = MdiForm.txtLugarRemitente.Value,
                Fecha = DateOnly.Parse(MdiForm.dateFechaRemitente.Value!.Value.ToString("d")),
                NombreElaborador = MdiForm.txtNombreElaborador.Value,
                Autorizador = MdiForm.txtAutorizador.Value
            };
            this.ConfiguracionCasting = new ConfiguracionCastingTab()
            {
                TurnosCrudJson = Utils.ParseJsonHorariosByDgv(MdiForm.dgvTurnosHorarios),
                TiempoRetrasoPermitido = MdiForm.txtMaximoRetrasoMinutosPermitidos.Value!.Value,
                DomingosNoLaborables = MdiForm.chckDomingosNoLaborables.Checked
            };
            this.ResultadosCasting = new ResultadosCastingTab(MdiForm.treePagingResultadosCasting);
            this.SourceFile = (new FileInfo(MdiForm.Report.SourcePath).Name, File.ReadAllBytes(MdiForm.Report.SourcePath));
            this.AssetsFile = new Assets()
            {
                Title = MdiForm.Text,
                Device = MdiForm.Report.DeviceModel,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>ASIGNA TODOS LOS PARAMETROS DE MANERA DEFAULT</remarks>
        public CafProjFile()
        {
            InitializeClass();
        }

        /// <summary>
        /// Guarda el proyecto en su archivo correspondiente
        /// </summary>
        /// <param name="filename">Direccion de guardado del archivo de proyecto (direccion + nombre de archivo y extension)</param>
        /// <returns></returns>
        public Response Save(string filename)
        {
            #region CODIGO
            Response _resp = new Response(false, "Iniciando funcion de guardado...");

            string _tempFileName = $"{MdiForm.txtAreaRemitente.Value}_{MdiForm.Report.ReportPeriod.Start.ToString("dd-MM-yyyy")}_{MdiForm.Report.ReportPeriod.End.ToString("dd-MM-yyyy")}";
            _resp.Log.Add($"Nombre de archivo temporal establecido en '{_tempFileName}'...");
            string _tempPathDir = $@"{CafProjFile.PathTempFile}\{_tempFileName}";
            _resp.Log.Add($"Ruta temporal establecida en '{_tempPathDir}'...");

            try
            {
                // Creamos la carpeta de guardado en el directorio temporal
                if (!Directory.Exists(_tempPathDir))
                    Directory.CreateDirectory(_tempPathDir);

                Directory.CreateDirectory($@"{_tempPathDir}\data");
                _resp.Log.Add($@"Directorio '{_tempPathDir}\data' creado con exito...");

                // Generamos y guardamos los archivos correspondientes dentro de la carpeta de guardado
                File.WriteAllText($@"{_tempPathDir}\{GeneralTab.FILEPATH}", General.MakeJson());
                _resp.Log.Add($@"Archivo '{_tempPathDir}\{GeneralTab.FILEPATH}' generado con exito...");
                File.WriteAllText($@"{_tempPathDir}\{ConfiguracionCastingTab.FILEPATH}", ConfiguracionCasting.MakeJson());
                _resp.Log.Add($@"Archivo '{_tempPathDir}\{ConfiguracionCastingTab.FILEPATH}' generado con exito...");
                File.WriteAllText($@"{_tempPathDir}\{ResultadosCastingTab.FILEPATH}", ResultadosCasting.MakeJson());
                _resp.Log.Add($@"Archivo '{_tempPathDir}\{ResultadosCastingTab.FILEPATH}' generado con exito...");
                File.WriteAllBytes($@"{_tempPathDir}\{SourceFile.Filename}", SourceFile.Content);
                _resp.Log.Add($@"Archivo '{MdiForm.Report.SourcePath}' copiado con exito en '{_tempPathDir}'...");
                File.WriteAllText($@"{_tempPathDir}\{Assets.FILEPATH}", AssetsFile.MakeJson());
                _resp.Log.Add($@"Archivo '{Assets.FILEPATH}' generado con exito...");

                // Creamos el archivo comprimido en extension .caf
                if (File.Exists(filename))
                    File.Delete(filename);

                ZipFile.CreateFromDirectory(
                    sourceDirectoryName: _tempPathDir,
                    destinationArchiveFileName: filename,
                    compressionLevel: CompressionLevel.Optimal,
                    includeBaseDirectory: false
                );
                _resp.Log.Add($"Archivo de proyecto generado en '{filename}'...");

                _resp.Tag = filename;
                _resp.Success = true;
                _resp.Message = "Archivo de proyecto generado con exito!";
            }
            catch (Exception ex)
            {
                _resp.Success = false;
                _resp.Message = $"Ocurrio un error al intentar generar el archivo de proyecto! {ex.Message}";
                _resp.Log.Add(ex.ToString());
            }
            finally
            {
                // Una vez terminado, eliminamos la carpeta temporal del directorio
                if (Directory.Exists(_tempPathDir))
                    Directory.Delete(_tempPathDir, true);
                _resp.Log.Add($"Residuos de '{_tempPathDir}' eliminados con exito...");
            }

            return _resp;
            #endregion
        }

        /// <summary>
        /// Descomprime el archivo de proyecto y crea el objeto de proyecto correspondiente
        /// </summary>
        /// <param name="filename">Ruta del archivo .caf</param>
        /// <returns>Response con el objeto CafProjFile reconstruido</returns>
        public static Response<CafProjFile> Build(string filename)
        {
            #region CODIGO
            Response<CafProjFile> _resp = new Response<CafProjFile>(false, "Iniciando construccion del objeto...", null);
            CafProjFile _obj = new CafProjFile();

            string _targetPathDir = $@"{CafProjFile.PathTempFile}\{new FileInfo(filename).Name.Replace(CafProjFile.FileExtension, "")}";

            // Verificamos la existencia del directorio temporal
            if (!Directory.Exists(_targetPathDir))
                Directory.CreateDirectory(_targetPathDir);

            try
            {
                // Extraemos el archivo .caf en el directorio temporal
                ZipFile.ExtractToDirectory(filename, _targetPathDir);
                
                // Leemos los archivos extraidos y generamos las propiedades correspondientes
                _obj.General = GeneralTab.Build(File.ReadAllText($@"{_targetPathDir}\{GeneralTab.FILEPATH}")) ?? throw new Exception($"Ocurrio un error al leer el archivo '{GeneralTab.FILEPATH}'");
                _resp.Log.Add($@"Archivo '{_targetPathDir}\{GeneralTab.FILEPATH}' leido con exito...");
                _obj.ConfiguracionCasting = ConfiguracionCastingTab.Build(File.ReadAllText($@"{_targetPathDir}\{ConfiguracionCastingTab.FILEPATH}")) ?? throw new Exception($"Ocurrio un error al leer el archivo '{ConfiguracionCastingTab.FILEPATH}'");
                _resp.Log.Add($@"Archivo '{_targetPathDir}\{ConfiguracionCastingTab.FILEPATH}' leido con exito...");
                _obj.ResultadosCasting = ResultadosCastingTab.Build(File.ReadAllText($@"{_targetPathDir}\{ResultadosCastingTab.FILEPATH}")) ?? throw new Exception($"Ocurrio un error al leer el archivo '{ResultadosCastingTab.FILEPATH}'");
                _resp.Log.Add($@"Archivo '{_targetPathDir}\{ResultadosCastingTab.FILEPATH}' leido con exito...");
                DirectoryInfo di = new DirectoryInfo(_targetPathDir);
                FileInfo? fi = di.GetFiles().Cast<FileInfo>()
                                            .Where(f => f.Extension.Equals(".xlsx"))
                                            .FirstOrDefault();
                if (fi is null)
                    throw new Exception($"No se encontro el archivo de reporte '.xlsx' en el directorio '{_targetPathDir}'");

                _obj.SourceFile = (fi.Name, File.ReadAllBytes(fi.FullName));
                _resp.Log.Add($"Archivo '{fi.Name}' cargado exitosamente...");
                _obj.AssetsFile = Assets.Build(File.ReadAllText($@"{Assets.FILEPATH}")) ?? throw new Exception($"Ocurrio un error al intentar leer el archivo '{Assets.FILEPATH}'");
                _resp.Log.Add($@"Archivo '{_targetPathDir}\{Assets.FILEPATH}' leido con exito...");

                // Indicamos la respuesta de la funcion
                _resp.Success = true;
                _resp.Message = "Objeto de proyecto construido correctamente!";
                _resp.Object  = _obj;
            }
            catch (Exception ex)
            {
                _resp.Success = false;
                _resp.Message = $"Ocurrio un error inesperado a la hora de construir el archivo de proyecto! {ex.Message}";
                _resp.Log.Add($"{ex}");
                MessageBox.Show($"Ocurrio un error inesperado construyendo el proyecto!", "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Una vez terminado, eliminamos la carpeta temporal del directorio
                if (Directory.Exists(_targetPathDir))
                    Directory.Delete(_targetPathDir, true);
                _resp.Log.Add($"Residuos de '{_targetPathDir}' eliminados con exito...");
            }
            
            return _resp;
            #endregion
        }
    }
}
