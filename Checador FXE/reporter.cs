using Checador_FXE.Plantillas;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2016.Excel;
using FlowCommonWorkcore;
using FlowControls.Inputs;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.ServiceModel.Channels;

namespace Checador_FXE
{
    internal class ReportProperties
    {
        internal string Area { get; }
        internal string Lugar { get; }
        internal DateOnly Fecha { get; }
        internal string NombreElaboro { get; }
        internal string NombreAutorizo { get; }

        internal ReportProperties(string area, string lugar, DateOnly fecha, string nombreElaboro, string nombreAutorizo)
        {
            Area = area;
            Lugar = lugar;
            Fecha = fecha;
            NombreElaboro = nombreElaboro;
            NombreAutorizo = nombreAutorizo;
        }
    }

    internal static class Reporteador
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="absPath">Ruta absoluta en la que guardara el archivo generado</param>
        /// <param name="rpt"></param>
        /// <returns>Retorna las ubicaciones de las listas de asistencias generadas</returns>
        internal static Response<string[]> Generate(string absPath, ReporteAsistencias rpt, Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _RelacionAsistencias, ReportProperties props)
        {
            Response<string[]> _resp = new Response<string[]>(false, "Inciando generacion de informes", null);
            Program.WriteStatus(true, $"Generando informe/s para el reporte de asistencias '{rpt.RelacionID}'", ReportOnGui: false);

            try
            {
                int quincenaNumber = 0;
                string[] invalidVal = { "-1" };

                string[] nombresCompletosFormatos = new string[2] {
                $"{Program.FORMATO_ASIST_1_PROPS.Name}.{Program.FORMATO_ASIST_1_PROPS.Ext}",
                $"{Program.FORMATO_ASIST_2_PROPS.Name}.{Program.FORMATO_ASIST_2_PROPS.Ext}" };
                Program.WriteStatus(true, "Prospectos de nombres de archivos de los formatos construido", ReportOnGui: false);

                string[] pathsFormatos = {
                Path.Combine(Application.StartupPath, nombresCompletosFormatos[0]),
                Path.Combine(Application.StartupPath, nombresCompletosFormatos[1]) };
                Program.WriteStatus(true, "Rutas prospecto para los formatos construida", ReportOnGui: false);

                Dictionary<string, Checada[]>[] quincenas = { new Dictionary<string, Checada[]>(), new Dictionary<string, Checada[]>() };
                #region CLASIFICAMOS LAS ASISTENCIAS POR QUINCENAS
                Dictionary<int, Turno> turnosConfigurados = Turno.GetAll(Properties.Settings.Default.TURNOS_HORARIOS)
                    .ToDictionary(turno => turno.ID);

                DateOnly ObtenerFechaLaboral(Checada checada)
                {
                    DateOnly fechaFisica = DateOnly.FromDateTime(checada.Fecha);
                    DateOnly fechaAnterior = fechaFisica.AddDays(-1);

                    TurnoEmpleado? turnoDelDiaAnterior = rpt.Turnos.Items.FirstOrDefault(turno =>
                        turno.NoEmp == checada.NumEmpleado && turno.Dia == fechaAnterior);

                    if (turnoDelDiaAnterior is not null &&
                        turnosConfigurados.TryGetValue(turnoDelDiaAnterior.Turno, out Turno turno) &&
                        CalendarioTurnos.CruzaMedianoche(CalendarioTurnos.ObtenerHorarioLaboral(turno)) &&
                        CalendarioTurnos.FechaLaboralDeChecada(checada.Fecha, turno) == fechaAnterior)
                    {
                        return fechaAnterior;
                    }

                    return fechaFisica;
                }

                Func<ReporteAsistencias, (int, int), Dictionary<string, Checada[]>> castAsistencias = delegate (ReporteAsistencias rpt, (int Start, int End) Dates)
                {
                    Dictionary<string, Checada[]> a = new Dictionary<string, Checada[]>();
                    try
                    {
                        a = rpt.Chequeos.Cast<KeyValuePair<string, Checada[]>>()
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value
                            .Where(c =>
                            {
                                DateOnly fechaLaboral = ObtenerFechaLaboral(c);
                                return fechaLaboral.Month == rpt.ReportPeriod.Start.Month &&
                                       fechaLaboral.Day >= Dates.Start && fechaLaboral.Day <= Dates.End;
                            })
                            .OrderBy(c => c.Fecha)
                            .ToArray());
                    }
                    catch (Exception ex)
                    {
                        flMessageBox.Show($"{ex.Message}\n{ex}");
                    }
                    return a;
                };

                quincenas[0] = castAsistencias(rpt, (1, 15));
                quincenas[1] = castAsistencias(rpt, (16, 31));
                #endregion
                Program.WriteStatus(true, "Clasificacion de asistencias por quincenas realizada con exito", ReportOnGui: false);

                List<string> generatedPaths = new List<string>();

                foreach (var i in quincenas)
                {
                    quincenaNumber++;
                    if (i.Keys.Count == 0) continue;

                    string pdfTemplatePath = Path.Combine(Application.StartupPath, (quincenaNumber == 1 ? nombresCompletosFormatos[0] : nombresCompletosFormatos[1]));
                    string targetPath = $"{absPath.Replace(".pdf", $"_{quincenaNumber}.pdf")}";

                    // HACK: Para evitar errores, añadimos este "guard clause" para saltarnos en caso de que el formato no exista
                    if (!File.Exists(pdfTemplatePath))
                    {
                        Program.WriteStatus(false, $"No se encontro el archivo de plantilla '{pdfTemplatePath}'!");
                        continue;
                    }

                    /* 
                     * 
                     * En el diccionario se establecen las relaciones de pares:
                     * 
                     * [ nombre de empleado : numero de empleado ]
                     * 
                     * para poder ser usado en el final del proceso del archivo para agregar
                     * los numeros de empleados correspondientes de cada uno en su linea
                     * indicada.
                     * 
                     * */
                    Dictionary<string, int> pairNombreEmpNo = new Dictionary<string, int>();
                    #region CLASIFICAMOS LA ASOCIACION DE CADA EMPLEADO CON SU NUMERO DE EMPLEADO CORRESPONDIENTE
                    foreach (Dictionary<string, Checada[]> q in quincenas)
                    {
                        foreach (string nombre in q.Keys)
                        {
                            if (!pairNombreEmpNo.ContainsKey(nombre))
                            {
                                // Buscamos el numero de empleado en alguna checada que se haya realizado
                                if (q[nombre].Length == 0)
                                    continue;

                                pairNombreEmpNo.Add(nombre, q[nombre][0].NumEmpleado);  // Añadimos
                                continue;
                            }
                        }
                    }
                    #endregion
                    Program.WriteStatus(true, "Clasificacion de la asociacion de cada empleado con su numero de empleado realizada con exito", ReportOnGui: false);

                    PdfFont cachedFont = PdfFontFactory.CreateFont(Path.Combine(Application.StartupPath, "Helvetica.ttf"));
                    System.Drawing.Color configuredColor = Utils.GetBaseColorByName(Properties.Settings.Default.COLOR_PINCEL);
                    DeviceRgb cachedColor = new DeviceRgb(configuredColor.R, configuredColor.G, configuredColor.B);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Program.WriteStatus(true, $"Inicializando documento iText 7 para '{pdfTemplatePath}'", ReportOnGui: false);

                        using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfTemplatePath), new PdfWriter(ms)))
                        {
                            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, false)
                                ?? throw new InvalidDataException($"La plantilla '{Path.GetFileName(pdfTemplatePath)}' no contiene campos AcroForm.");
                            IDictionary<string, PdfFormField> formFields = form.GetFormFields();
                            PdfCanvas canvas = new PdfCanvas(pdfDocument.GetFirstPage());

                            void SetField(string name, string value)
                            {
                                if (!formFields.TryGetValue(name, out PdfFormField? field))
                                    throw new InvalidDataException($"La plantilla no contiene el campo '{name}'.");
                                field.SetValue(value ?? string.Empty);
                            }

                            void WriteText(string text, float x, float y) => canvas.SetTextMatrix(x, y).ShowText(text);

                            canvas.BeginText().SetFontAndSize(cachedFont, 7).SetFillColor(cachedColor);
                            int A_L = 1;

                            SetField("area_remitente", props.Area);
                            SetField("lugar_fecha_remitente", $"{props.Lugar} al {props.Fecha}");
                            SetField("elaboro_remitente", props.NombreElaboro);
                            SetField("nombre_autorizacion", props.NombreAutorizo);

                                foreach (string empName in i.Keys)
                                {
                                    if (A_L > 10)
                                        throw new IndexOutOfRangeException("No se puede crear un archivo de más de 10 renglones (empleados)!");

                                    // Llenamos los campos de formulario
                                    SetField($"num_nomina_{A_L}", pairNombreEmpNo[empName].ToString());
                                    SetField($"nombre_trabajador_{A_L}", empName);

                                    (int X, int Y) INITIAL_COORDS = (319, 422);
                                    (int X, int Y) JUMP_ON_COORDS = (11, 7);

                                    (int X, int Y) IHC_S = (496, 416); // INITIAL_HOURS_COORDS_START
                                    (int X, int Y) IHC_E = (510, 405); // INITIAL_HOURS_COORDS_END
                                    int IHC_JUMPS = 29;

                                    int dayIndex = 1;
                                    // Obtenemos de forma segura las asistencias por fecha para el empleado
                                    if (!_RelacionAsistencias.TryGetValue(empName, out var asistenciaPorFechas))
                                    {
                                        asistenciaPorFechas = new Dictionary<DateOnly, TipoAsistencia>();
                                    }

                                    foreach (DateOnly j in asistenciaPorFechas.Keys)
                                    {
                                        // TODO: Validar que la fecha pertenezca a la quincena actual

                                        if ((quincenaNumber == 1 && j.Day > 15) || (quincenaNumber == 2 && j.Day < 16))
                                            continue;

                                        /* 
                                         * HACK: Saltamos los dias que no pertenecen al mes del reporte
                                         * */
                                        if (!j.Month.Equals(rpt.ReportPeriod.Start.Month))
                                            continue;

                                        TipoAsistencia T_P = asistenciaPorFechas[j];
                                        T_P = T_P is TipoAsistencia.RETARDO ? TipoAsistencia.ASISTENCIA : T_P;

                                        WriteText(
                                            T_P.GetShort(),
                                            INITIAL_COORDS.X + ((JUMP_ON_COORDS.X * Utils.TranslateDayOnCell(j, quincenaNumber)) - JUMP_ON_COORDS.X + (dayIndex >= 6 ? 2 : 0)),
                                            INITIAL_COORDS.Y - ((A_L > 1 ? (JUMP_ON_COORDS.Y * 4) * (A_L > 2 ? A_L - 1 : 1) : 0) + (A_L - 1)));

                                        if (j.Day == 15)
                                            dayIndex = 0;   // Restauramos por defecto al terminar de analizar la primer quincena

                                        dayIndex++;
                                    }

                                    // Escribimos el horario del empleado
                                    Func<TurnoEmpleado[], int, TimeSpan> _GetWorkTimeSchedule = delegate (TurnoEmpleado[] turnos, int tipo)
                                    {
                                        Horario t = new Horario();

                                        foreach (var i in turnos)
                                        {
                                            if (i.Turno == 0 || i.Turno == -1)
                                                continue;

                                            t = Turno.GetInOutTimes(i.Turno);
                                            break;
                                        }

                                        // tipo: 0 -> Entrada, 1 -> Salida
                                        return tipo == 0 ? t.Entrada : t.Salida;
                                    };

                                    //TimeSpan start = _GetWorkTimeSchedule(rpt.Turnos[empName], 0);
                                    //TimeSpan end = _GetWorkTimeSchedule(rpt.Turnos[empName], 1);

                                    TimeSpan start = _GetWorkTimeSchedule(rpt.Turnos.Items.Cast<TurnoEmpleado>().Where(t => t.Nombre == empName).ToArray(), 0);
                                    TimeSpan end = _GetWorkTimeSchedule(rpt.Turnos.Items.Cast<TurnoEmpleado>().Where(t => t.Nombre == empName).ToArray(), 1);

                                    WriteText($"{start.Hours:00}:{start.Minutes:00}", IHC_S.X, (A_L > 1 ? IHC_S.Y - (IHC_JUMPS * (A_L - 1)) : IHC_S.Y));
                                    WriteText($"{end.Hours:00}:{end.Minutes:00}", IHC_E.X, (A_L > 1 ? IHC_E.Y - (IHC_JUMPS * (A_L - 1)) : IHC_E.Y));

                                    A_L++;
                                }

                            // Escribimos el total de registros del documento
                            WriteText($"{A_L - 1}", 319, 123);
                            canvas.EndText();
                            Program.WriteStatus(true, $"--> Archivo de Reporte PDF '{pdfTemplatePath}' llenado con éxito mediante iText 7!", ReportOnGui: false);
                        }

                        /*
                         * 
                         * SE DEBE ELIMINAR EL ARCHIVO GENERADO PREVIAMENTE (SI EXISTE) YA QUE AL ESTABLECER EL ATRIBUTO 
                         * DE SOLO LECTURA, ES IMPOSIBLE SOBREESCRIBIR EL ARCHIVO YA EXISTENTE.
                         * 
                         * */
                        if (File.Exists(targetPath))
                        {
                            // Quitamos el atributo ReadOnly si lo tiene
                            var attrs = File.GetAttributes(targetPath);
                            if ((attrs & FileAttributes.ReadOnly) != 0)
                                File.SetAttributes(targetPath, attrs & ~FileAttributes.ReadOnly);

                            // Eliminamos el archivo
                            File.Delete(targetPath);
                        }

                        // Guardamos el contenido del MemoryStream en el archivo
                        File.WriteAllBytes(targetPath, ms.ToArray());
                        File.SetAttributes(targetPath, File.GetAttributes(targetPath) | FileAttributes.ReadOnly);   // Establecemos la propiedad de solo lectura

                        Program.WriteStatus(true, $"----> '{targetPath}' guardado con exito!", ReportOnGui: false);
                    }

                    generatedPaths.Add(targetPath);
                }

                _resp.EndWithSuccess("Generacion de archivos con exito!", generatedPaths.ToArray());
            } catch (Exception ex)
            {
                _resp.EndWithError($"Ocurrio un error inesperado! {ex.Message}", ex);
            }

            return _resp;
        }
    }
}
