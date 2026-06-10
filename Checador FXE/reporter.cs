using Checador_FXE.Plantillas;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2016.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
        internal static string[] Generate(string absPath, ReporteAsistencias rpt, Dictionary<string, Dictionary<DateOnly, TipoAsistencia>> _RelacionAsistencias, ReportProperties props)
        {
            int quincenaNumber = 0;
            string[] invalidVal = { "-1" };

            string[] nombresCompletosFormatos = new string[2] {
                $"{Program.FORMATO_ASIST_1_PROPS.Name}.{Program.FORMATO_ASIST_1_PROPS.Ext}",
                $"{Program.FORMATO_ASIST_2_PROPS.Name}.{Program.FORMATO_ASIST_2_PROPS.Ext}" };

            string[] pathsFormatos = {
                Path.Combine(Application.StartupPath, nombresCompletosFormatos[0]),
                Path.Combine(Application.StartupPath, nombresCompletosFormatos[1]) };

            Dictionary<string, Checada[]>[] quincenas = { new Dictionary<string, Checada[]>(), new Dictionary<string, Checada[]>() };
            #region CLASIFICAMOS LAS ASISTENCIAS POR QUINCENAS
            Func<ReporteAsistencias, (int, int), Dictionary<string, Checada[]>> castAsistencias = delegate (ReporteAsistencias rpt, (int Start, int End) Dates)
            {
                Dictionary<string, Checada[]> a = new Dictionary<string, Checada[]>();
                try
                {
                    a = rpt.Chequeos.Cast<KeyValuePair<string, Checada[]>>()
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value
                        .Where(c => c.Fecha.Day >= Dates.Start && c.Fecha.Day <= Dates.End)
                        .OrderBy(c => c.Fecha)
                        .ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}\n{ex}");
                }
                return a;
            };

            quincenas[0] = castAsistencias(rpt, (1, 15));
            quincenas[1] = castAsistencias(rpt, (16, 31));
            #endregion

            List<string> generatedPaths = new List<string>();

            foreach (var i in quincenas)
            {
                quincenaNumber++;
                if (i.Keys.Count == 0) continue;

                string pdfTemplatePath = Path.Combine(Application.StartupPath, (quincenaNumber == 1 ? nombresCompletosFormatos[0] : nombresCompletosFormatos[1]));
                string targetPath = $"{absPath.Replace(".pdf", $"_{quincenaNumber}.pdf")}";

                // HACK: Para evitar errores, añadimos este "guard clause" para saltarnos en caso de que el formato no exista
                if (!File.Exists(pdfTemplatePath))
                    continue;

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

                // Cacheamos la fuente y el color para evitar recrearlos en cada iteración
                BaseFont cachedBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var cachedColor = Utils.GetBaseColorByName(Properties.Settings.Default.COLOR_PINCEL);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfReader pdfReader = new PdfReader(pdfTemplatePath))
                    {
                        using (PdfStamper pdfStamper = new PdfStamper(pdfReader, ms))
                        {
                            PdfContentByte cb = pdfStamper.GetOverContent(1);
                            cb.BeginText();

                            AcroFields frmFields = pdfStamper.AcroFields;
                            int A_L = 1;

                            frmFields.SetField("area_remitente", props.Area);
                            frmFields.SetField("lugar_fecha_remitente", $"{props.Lugar} al {props.Fecha}");
                            frmFields.SetField("elaboro_remitente", props.NombreElaboro);
                            frmFields.SetField("nombre_autorizacion", props.NombreAutorizo);

                            foreach (string empName in i.Keys)
                            {
                                if (A_L > 10)
                                    throw new IndexOutOfRangeException("No se puede crear un archivo de más de 10 renglones (empleados)!");

                                // Llenamos los campos de formulario
                                frmFields.SetField($"num_nomina_{A_L}", pairNombreEmpNo[empName].ToString());
                                frmFields.SetField($"nombre_trabajador_{A_L}", empName);

                                // Llenamos las asistencias
                                cb.SetFontAndSize(cachedBaseFont, 7);
                                cb.SetColorFill(cachedColor);

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

                                    cb.ShowTextAligned(
                                        PdfContentByte.ALIGN_LEFT,
                                        T_P.GetShort(),
                                        INITIAL_COORDS.X + ((JUMP_ON_COORDS.X * Utils.TranslateDayOnCell(j, quincenaNumber)) - JUMP_ON_COORDS.X + (dayIndex >= 6 ? 2 : 0)),
                                        INITIAL_COORDS.Y - ((A_L > 1 ? (JUMP_ON_COORDS.Y * 4) * (A_L > 2 ? A_L - 1 : 1) : 0) + (A_L - 1)), 0
                                    );

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
                                
                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"{start.Hours:00}:{start.Minutes:00}", IHC_S.X, (A_L > 1 ? IHC_S.Y - (IHC_JUMPS * (A_L - 1)) : IHC_S.Y), 0); // Horario de entrada
                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"{end.Hours:00}:{end.Minutes:00}", IHC_E.X, (A_L > 1 ? IHC_E.Y - (IHC_JUMPS * (A_L - 1)) : IHC_E.Y), 0);     // Horario de salida

                                A_L++;
                            }

                            // Escribimos el total de registros del documento
                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"{A_L - 1}", 319, 123, 0);

                            cb.EndText();
                        }
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
                }

                generatedPaths.Add(targetPath);
            }

            return generatedPaths.ToArray();
        }
    }
}
