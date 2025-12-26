using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModoConectado.Modelos
{
    public class Empleado
    {
        public int EmpNo { get; set; }
        public string Apellido { get; set; }
        public string Oficio { get; set; }
        public decimal Salario { get; set; }
        public decimal Comision { get; set; }
        public DateTime FechaAlta { get; set; }
        public int DeptNo { get; set; }
    }
}
