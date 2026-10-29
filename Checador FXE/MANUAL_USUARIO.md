# Manual de usuario — Checador FXE

## Ficha del software

| Dato | Valor |
| --- | --- |
| Producto | Checador FXE |
| Versión manual | 1.0.4 |
| Versión de ensamblado | `1.0.4.AAMM` (se genera al compilar; por ejemplo, `1.0.4.2610` para una compilación de octubre de 2026) |
| Versión informativa | `1.0.4.AAMM-beta-dev` |
| Descripción | Generador y gestor de asistencias de personal sindicalizado FXE con hardware ZKTeco. |
| Plataforma | Aplicación de escritorio Windows x64, .NET 8 para Windows 7 o posterior |
| Autor / copyright | Arturo Venegas IDC |
| Compañía | Grupo México Transportes |
| Idioma | Español (México) |
| Repositorio | <https://github.com/ArturoProgrammer/Checador-FXE> |

> La parte `AAMM` de la versión de ensamblado se calcula automáticamente con el año y mes de compilación. Consulte **Ayuda > Acerca de** en la aplicación para ver la versión exacta de la instalación.

## 1. Propósito

Checador FXE es una aplicación de escritorio para elaborar reportes de asistencia de personal sindicalizado. Procesa un archivo de checadas exportado desde un dispositivo compatible, lo cruza con el catálogo de personal y con la relación mensual de turnos, permite revisar o corregir el resultado y genera el reporte final en PDF.

La aplicación también conserva el trabajo como un **proyecto CAF** (`.caf`) y permite actualizar plantillas de Excel con datos de personal, horarios o relaciones de turnos.

## 2. Antes de comenzar

Para procesar un periodo se necesita lo siguiente:

1. Un reporte de asistencias en formato **`.xlsx`** exportado del checador.
2. Seleccionar el modelo correcto de origen. Las opciones disponibles son **ZKTeco K40 KIT** y **ZKTeco K40**.
3. Tener registrados empleados para la localidad que se utilizará.
4. Contar con una relación de horarios del mes y año correspondientes.

> **Importante:** aunque los cuadros de selección permiten ver archivos `.xls`, la aplicación no los procesa directamente. Abra el archivo en Excel y guárdelo como `.xlsx` antes de usarlo.

El modelo del dispositivo es importante porque define cómo se interpreta el archivo de checadas. Si se elige un modelo equivocado, el reporte puede no cargarse o interpretarse de forma incorrecta.

## 3. Pantalla principal

Al abrir la aplicación se muestra la pantalla principal de **Checador FXE**.

- **Archivo:** crear, abrir y guardar proyectos.
- **Herramientas:** administrar el personal y las relaciones mensuales de horarios.
- **Propiedades:** configurar opciones generales, turnos y conexión.
- **Panel izquierdo:** lista de proyectos o quincenas abiertos. Haga doble clic en uno para traerlo al frente.
- **Barra de estado:** indica si la última operación terminó correctamente. El texto de la operación puede consultarse para abrir el registro temporal cuando se produjo un error.

La barra de acceso rápido ofrece las mismas acciones principales: **Nuevo**, **Abrir**, **Personal**, **Relaciones** y **Feriados**.

> **Nota:** la opción **Feriados e Inhábiles** aparece en la interfaz, pero en la versión actual aún no está implementada. No la utilice como parte del flujo de trabajo.

## 4. Flujo recomendado de trabajo

1. Revise la configuración general y los turnos.
2. Registre o actualice el personal de la localidad.
3. Verifique la relación mensual de horarios.
4. Cree un proyecto a partir del Excel de checadas.
5. Revise registros, turnos y resultado del cálculo.
6. Ajuste manualmente asistencias si hace falta y ejecute de nuevo el cálculo.
7. Guarde el proyecto CAF.
8. Genere o imprima el PDF final.

## 5. Configuración

Abra **Archivo > Propiedades**.

### General

Configure estos valores antes de crear proyectos nuevos:

- **Tolerancia de retraso:** minutos permitidos antes de clasificar una llegada como retardo.
- **Dispositivo predeterminado:** modelo del checador usado normalmente.
- **Color de pincel:** opción visual de la aplicación.
- **Nombre de archivo predeterminado:** nombre base para el PDF generado.
- **Localidad predeterminada:** localidad que la aplicación selecciona inicialmente.

### Ajustes de horario

Aquí se mantienen los turnos disponibles. Cada turno incluye un número, nombre, primer horario de entrada/salida y, si aplica, segundo horario de entrada/salida.

Use el botón de agregar de la cuadrícula para crear un turno. Al guardar:

- Los turnos quedan disponibles para asignarlos al personal y a la relación mensual.
- Si se elimina un turno que ya tiene referencias, la aplicación advierte sobre las afectaciones antes de aplicar los cambios.
- El turno predeterminado se ajusta si dejó de existir.

### Base de datos

Esta pestaña contiene los parámetros de conexión al servidor. Use **Testear conexión** para validar que la comunicación con MySQL funciona antes de guardar cambios.

Si se modifican los datos del servidor, la aplicación solicita confirmación para aplicar los cambios y reiniciarse. Esta configuración debe ser administrada por personal autorizado.

## 6. Registro de personal

Abra **Herramientas > Registro de Personal**.

1. Elija la **localidad** en la parte superior.
2. Revise la tabla de empleados de esa localidad.
3. Para agregar un empleado, use el botón **Agregar** de la cuadrícula.
4. Complete: número de empleado, nombres, apellidos, puesto, región, división, localidad y turno predeterminado.
5. Pulse **Guardar cambios** para persistir las modificaciones.

El número de empleado no puede duplicarse. La localidad debe existir en el catálogo de localidades y el turno predeterminado debe ser un turno válido. Puede seleccionar una localidad y usar **Establecer como default** para convertirla en la localidad predeterminada de la aplicación.

En **Herramientas > Exportar para configuración...** se puede llenar una plantilla de Excel con el listado de empleados de la localidad seleccionada.

> **Precaución:** el botón **Revertir cambios** está visible, pero no tiene implementación funcional en la versión actual. Si necesita descartar cambios no guardados, cierre la ventana y vuelva a abrirla.

## 7. Relación mensual de horarios

Abra **Herramientas > Relación de Horarios**. Esta pantalla asigna un turno a cada empleado para cada día del mes.

1. Seleccione el mes y capture el año.
2. Seleccione **Ir a mes indicado**.
3. Si necesita otra localidad, haga clic en el texto de localidad y elija una opción.
4. Edite los números de turno en las columnas de días.
5. Use **Guardar cambios** para almacenar la relación.

Opciones disponibles:

- **Limitar a:** filtra la vista por todo, número de empleado o nombre.
- **Nuevo turno:** abre directamente los ajustes de horario.
- **Establecer turnos por defecto:** aplica el turno predeterminado del empleado seleccionado a los días laborables de su fila; conserva los domingos.
- **Exportar ajustes de turnos:** prepara una plantilla de Excel con los horarios de los turnos.

Introduzca solamente números de turnos existentes. Los encabezados de los días no se pueden editar.

> **Precaución:** las acciones **Revertir cambios**, agregar una fila desde la cuadrícula y eliminar la relación están incompletas en esta versión. Para cambios ordinarios, edite la relación existente y guarde.

## 8. Crear un proyecto de asistencia

Seleccione **Archivo > Nuevo**.

1. Escriba un título descriptivo para el proyecto.
2. Pulse **Examinar** y seleccione el archivo de checadas `.xlsx`.
3. Seleccione el modelo del dispositivo que produjo el reporte.
4. Seleccione la localidad remitente.
5. Pulse **Aceptar**.

La aplicación valida que la localidad tenga personal registrado. Si no lo tiene, primero registre el personal y la relación de horarios correspondiente.

Al abrirse el proyecto, el sistema analiza automáticamente el archivo y ejecuta el cálculo inicial de asistencia. La relación mensual que corresponde al periodo se carga para que el cálculo pueda determinar días laborales, faltas, descansos y retardos.

## 9. Trabajar dentro de un proyecto

El proyecto incluye una barra de acciones:

| Acción | Atajo | Uso |
| --- | --- | --- |
| Guardar | `Ctrl+G` | Guarda el proyecto CAF. |
| Cerrar | — | Cierra el proyecto; permite guardar antes de salir. |
| Imprimir | `Ctrl+P` | Genera temporalmente el PDF y lo envía a la impresora elegida. |
| Generar | `F5` | Genera el PDF del reporte final. |
| Ejecutar | `Ctrl+E` | Recalcula el resultado de asistencia con la configuración actual. |

### General

Complete los datos que se mostrarán en el reporte, como área remitente, lugar remitente, fecha, nombre del elaborador y autorizador. También se muestran los parámetros de formato y la tolerancia usada para el cálculo.

### Resultados del parsing

Esta pestaña permite revisar los registros leídos desde el Excel.

1. Haga doble clic sobre un empleado de la lista inferior.
2. El calendario mostrará cuántos registros tiene por día.
3. Haga doble clic sobre un día para ver la hora y el tipo de cada registro (entrada, salida, ida a comida o regreso de comida).

Use esta revisión para detectar checadas faltantes o registros inesperados antes de emitir el informe.

### Configuración del casting

En esta pestaña se ajustan las reglas con las que se convierte el registro de checadas en una asistencia por día.

- **Turnos y horarios:** muestra los horarios usados por el proyecto. Puede agregar o corregir turnos locales para este proyecto.
- **Sincronizar parámetros:** recupera los horarios globales. Esta acción descarta las modificaciones locales de horarios; confirme solo si desea reemplazarlas.
- **Relación mensual:** muestra el turno asignado a cada empleado por día para el mes del reporte.
- **Definir relación mensual:** toma las modificaciones hechas en la cuadrícula para el cálculo actual.
- **Expandir visualización:** abre una vista ampliada de la relación.

Después de modificar turnos, tolerancia o relación mensual, pulse **Ejecutar** para recalcular los resultados.

### Resultados del casting

Aquí se muestran los resultados calculados por empleado y día. Seleccione o haga doble clic sobre un empleado para revisar su calendario.

En un día del calendario puede hacer doble clic para elegir un tipo de asistencia. También puede usar el menú contextual para asignar:

- Asistencia
- Falta
- Descanso laborado
- Descanso semanal
- Vacaciones
- Restaurar a original, cuando existe una corrección manual previa

Las correcciones se aplican al resultado del proyecto. Vuelva a generar el PDF después de ajustarlas.

## 10. Guardar y abrir proyectos CAF

Use **Archivo > Guardar** para crear o actualizar un archivo de proyecto `.caf`. Si el proyecto todavía no tiene ubicación, se abrirá el selector de destino. Use **Guardar como...** para crear una copia con otro nombre o ubicación.

El proyecto CAF conserva la información del proyecto, la configuración de turnos y relación utilizada, así como los resultados calculados. Para continuar después, use **Archivo > Abrir** y seleccione el archivo `.caf`.

El sistema evita abrir simultáneamente dos proyectos con el mismo periodo.

## 11. Generar e imprimir el informe

Antes de generar, confirme que el resultado esté recalculado con **Ejecutar** y que los datos generales sean correctos.

- **Generar (`F5`):** crea el PDF usando el nombre configurado. Si está habilitada la apertura automática, el archivo se abrirá al finalizar.
- **Imprimir (`Ctrl+P`):** solicita una impresora y envía el PDF generado. No conserva necesariamente el PDF temporal usado para la impresión.

El reporte se separa en los formatos correspondientes a los días 1–15 y 16–31 cuando el periodo lo requiere.

## 12. Exportar datos a una plantilla de Excel

El exportador se abre desde Personal o desde la Relación de Horarios. Seleccione:

1. La plantilla de entrada `.xlsx`.
2. La ubicación y nombre del archivo de destino.
3. El modo de escritura.
4. **Aceptar**.

Los modos disponibles son:

- **Sobrescribir todo:** limpia el contenido objetivo y escribe la información actual. Recomendado para la primera carga.
- **Actualizar y añadir:** actualiza registros presentes y agrega los nuevos.
- **Añadir solamente inexistentes:** conserva lo existente y agrega solo los datos que faltan. Recomendado para actualizaciones posteriores.

La aplicación crea una copia de respaldo antes de modificar el archivo. Mantenga la plantilla cerrada en Excel durante la exportación para evitar errores de acceso.

## 13. Solución de problemas

| Situación | Qué revisar |
| --- | --- |
| No se puede crear el proyecto | Confirme que el archivo sea `.xlsx`, el modelo del dispositivo sea correcto y existan empleados para la localidad. |
| El reporte no se interpreta correctamente | Revise que eligió el modelo de dispositivo que generó el Excel. |
| No se puede asignar un turno | Asegúrese de que el número exista en Propiedades > Ajustes de Horario. |
| No hay datos para calcular | Revise la relación mensual, los empleados y que el Excel contenga registros del periodo. |
| No se puede exportar a Excel | Cierre la plantilla en Excel y confirme rutas de origen y destino. |
| Falla la conexión | Revise los datos de Base de datos y use Testear conexión; solicite apoyo al administrador si persiste. |
| Se produjo un error | Consulte la barra de estado; al hacer clic en el detalle de la última operación se puede abrir el registro temporal cuando está disponible. |

## 14. Limitaciones conocidas de la versión actual

- La gestión de **Feriados e Inhábiles** no está implementada.
- Algunas acciones de reversión, alta/eliminación desde la relación mensual y eliminación de relaciones no están finalizadas.
- El procesamiento de archivos `.xls` no está disponible; conviértalos a `.xlsx`.

Estas limitaciones se indican para evitar que el manual presente como disponible una función que la aplicación aún no ejecuta.

