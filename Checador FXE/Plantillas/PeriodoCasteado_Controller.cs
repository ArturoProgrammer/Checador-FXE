using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checador_FXE.Plantillas
{
    /// <summary>
    /// Periodo casteado de un empleado en especifico
    /// </summary>
    internal class PeriodoCasteado
    {
        public required string NombreEmpleado { get; set; }
        public required int NoEmp { get; set; }
        public required Dictionary<DateOnly, TipoAsistencia> RelacionFechaAsistencia { get; set; }

        public void AddAsistencia(DateOnly dia, TipoAsistencia tipo)
        {
            if (RelacionFechaAsistencia.ContainsKey(dia))
                RelacionFechaAsistencia[dia] = tipo;
            else
                RelacionFechaAsistencia.Add(dia, tipo);
        }
    }

    internal class PeriodoCasteadoCollection
    {
        private List<PeriodoCasteado> _items;
        public PeriodoCasteado[] Items => _items.ToArray();

        // CONSTRUCTORES
        public PeriodoCasteadoCollection() =>_items = new List<PeriodoCasteado>();

        // METODOS
        public void Add(PeriodoCasteado item)
        {
            if (ContainsNoEmp(item.NoEmp))
                throw new ArgumentException($"Ya existe un empleado con el número de empleado {item.NoEmp} en la colección.");

            if (ContainsNombre(item.NombreEmpleado))
                throw new ArgumentException($"Ya existe un empleado con el nombre {item.NombreEmpleado} en la colección.");

            _items.Add(item);
        }
        public void AddRange(IEnumerable<PeriodoCasteado> items) => _items.AddRange(items);
        public void Clear() => _items.Clear();
        public bool ContainsNombre (string nombre) => _items.Any(x => x.NombreEmpleado == nombre);
        public bool ContainsNoEmp (int noEmp) => _items.Any(x => x.NoEmp == noEmp);


        // INDEXERS
        public PeriodoCasteado this[int noEmp]
        {
            get => _items.FirstOrDefault(x => x.NoEmp == noEmp) ?? throw new IndexOutOfRangeException(nameof(noEmp));
            set
            {
                int index = _items.FindIndex(x => x.NoEmp == noEmp);
                if (index == -1)
                    throw new IndexOutOfRangeException(nameof(noEmp));
                _items[index] = value;
            }
        }
        public PeriodoCasteado this[string nombreEmpleado]
        {
            get => _items.FirstOrDefault(x => x.NombreEmpleado == nombreEmpleado) ?? throw new IndexOutOfRangeException(nameof(nombreEmpleado));
            set
            {
                int index = _items.FindIndex(x => x.NombreEmpleado == nombreEmpleado);
                if (index == -1)
                    throw new IndexOutOfRangeException(nameof(nombreEmpleado));
                _items[index] = value;
            }
        }
    }
}
