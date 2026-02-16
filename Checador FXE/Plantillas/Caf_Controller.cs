using Checador_FXE.MdiForms;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using FlowCommonWorkcore;
using FlowControls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
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

            public Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> PeriodoCasteado { get; private set; }

            public ResultadosCastingTab() { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="pagingView">Origen de datos de donde obtendremos los resultados</param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public string MakeJson(flTreeViewPaging pagingView)
            {
                Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _array = new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>();
                List<string> _nodes = new List<string>();

                foreach (InteropGenericObject obj in pagingView.Items)
                {
                    _array.Add(obj.ObjectTitle, new Dictionary<DateOnly, TipoAsistencia>());    // Añadimos al diccionario los pares

                    Dictionary<DateOnly, TipoAsistencia> _objTag = (obj.GenericObject as Dictionary<DateOnly, TipoAsistencia>) ?? throw new Exception("Error inesperado al parseo del objeto!");
                    _array[obj.ObjectTitle] = _objTag;

                    List<string> _Lines = new List<string>();
                    foreach (DateOnly dia in _array[obj.ObjectTitle].Keys)
                    {
                        _Lines.Add($@"""{dia.ToString("yyyy-MM-dd")}"" : ""{_array[obj.ObjectTitle][dia].GetText()}""");
                    }

                    _nodes.Add($@"""{obj.ObjectTitle}"" : {{
    {String.Join(",\n", _Lines)}
}}");
                }
                return $@"{{ 
    {String.Join(",\n", _nodes)} 
}}";
            }
            public static ResultadosCastingTab? Build(string jsonText)
            {
                if (string.IsNullOrWhiteSpace(jsonText))
                    throw new ArgumentException("El JSON de entrada está vacío.", nameof(jsonText));

                // 1) Deserializar el JSON a: Empleado -> (Fecha(string) -> TextoAsistencia(string))
                Dictionary<string, Dictionary<string, string>>? plano;
                try
                {
                    plano = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
                        jsonText,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                catch (JsonException jex)
                {
                    throw new ArgumentException("El JSON proporcionado no tiene el formato esperado.", jex);
                }

                if (plano is null)
                    throw new ArgumentException("El JSON se deserializó a null (estructura no compatible).", nameof(jsonText));

                // 2) Construir la estructura destino: Empleado -> (Fecha(DateOnly) -> TipoAsistencia)
                var resultado = new Dictionary<string, Dictionary<DateOnly, TipoAsistencia>>();

                foreach (var empleadoKvp in plano)
                {
                    var empleado = empleadoKvp.Key ?? string.Empty;
                    var fechasTexto = empleadoKvp.Value ?? new Dictionary<string, string>();

                    var mapaFechas = new Dictionary<DateOnly, TipoAsistencia>();

                    foreach (var fechaAsistencia in fechasTexto)
                    {
                        var fechaStr = fechaAsistencia.Key;
                        var asistenciaTexto = fechaAsistencia.Value ?? string.Empty;

                        // Parse de fecha con formato fijo "yyyy-MM-dd"
                        if (!DateOnly.TryParseExact(
                                fechaStr,
                                "yyyy-MM-dd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var fecha))
                        {
                            // Si la fecha no es válida, se omite este par
                            continue;
                        }

                        // Parse del texto al enum (usa tu extensión; si no encuentra, devuelve FALTA)
                        var tipo = TipoAsistenciaExtensions.Parse(asistenciaTexto);

                        // Insertar (si hay repetidas, se sobreescribe la última)
                        mapaFechas[fecha] = tipo;
                    }

                    // Sólo agregar si hay al menos una fecha válida
                    if (mapaFechas.Count > 0)
                        resultado[empleado] = mapaFechas;
                }

                // === En este punto 'resultado' contiene el diccionario reconstruido ===
                // Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> resultado

                // Tú te encargas de mapearlo a 'ResultadosCastingTab' si así lo requieres.
                // Por ejemplo, podrías almacenarlo en una propiedad interna de ResultadosCastingTab,
                // o convertirlo a otro modelo. Aquí retornamos null, como acordamos.
                ResultadosCastingTab _obj = new ResultadosCastingTab()
                {
                    PeriodoCasteado = resultado,
                };

                return _obj;
            }
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
        /// Directorio de archivos temporales
        /// </summary>
        public string TempDir { get; private set; }


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
            this.ResultadosCasting = new ResultadosCastingTab();
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
                File.WriteAllText($@"{_tempPathDir}\{ResultadosCastingTab.FILEPATH}", ResultadosCasting.MakeJson(MdiForm.treePagingResultadosCasting));
                _resp.Log.Add($@"Archivo '{_tempPathDir}\{ResultadosCastingTab.FILEPATH}' generado con exito...");
                File.WriteAllBytes($@"{_tempPathDir}\{SourceFile.Filename}", SourceFile.Content);
                _resp.Log.Add($@"Archivo '{MdiForm.Report.SourcePath}' copiado con exito en '{_tempPathDir}'...");
                File.WriteAllText($@"{_tempPathDir}\{Assets.FILEPATH}", AssetsFile.MakeJson());
                _resp.Log.Add($@"Archivo '{Assets.FILEPATH}' generado con exito...");
                TempDir = _tempPathDir;
                _resp.Log.Add($"Propedad 'TempDir' : '{_tempPathDir}' asignado...");

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
        public static Response<CafProjFile> Build(string filename, bool ShowObjectLog = false)
        {
            #region CODIGO
            Response<CafProjFile> _resp = new Response<CafProjFile>(false, "Iniciando construccion del objeto...", null);
            CafProjFile _obj = new CafProjFile();

            string _targetPathDir = $@"{CafProjFile.PathTempFile}\{new FileInfo(filename).Name.Replace(CafProjFile.FileExtension, "")}";

            // Verificamos la existencia del directorio temporal
            if (Directory.Exists(_targetPathDir))
                Directory.Delete(_targetPathDir, recursive: true);

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
                _obj.AssetsFile = Assets.Build(File.ReadAllText($@"{_targetPathDir}\{Assets.FILEPATH}")) ?? throw new Exception($"Ocurrio un error al intentar leer el archivo '{Assets.FILEPATH}'");
                _resp.Log.Add($@"Archivo '{_targetPathDir}\{Assets.FILEPATH}' leido con exito...");
                _obj.TempDir = _targetPathDir;
                _resp.Log.Add($"Propedad 'TempDir' : '{_targetPathDir}' asignado...");

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
                /*
                if (Directory.Exists(_targetPathDir))
                    Directory.Delete(_targetPathDir, true);
                _resp.Log.Add($"Residuos de '{_targetPathDir}' eliminados con exito...");
                */
            }

            if (ShowObjectLog) MessageBox.Show(_resp.GetBuildedLog());

            return _resp;
            #endregion
        }
    }
}
