using ModoConectado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModoConectado.Interfaz
{
    internal interface IEmpleadoDAO
    {
        Task<List<Empleado>> GetEmpleados();
        Task<List<Empleado>> GetEmpleadosByDepartamento(int deptNo);
        Task<List<Empleado>> GetEmpleadosByBusqueda(string busqueda, string value);
        Task<int> SaveEmpleado(Empleado empleado);
        Task<int> UpdateEmpleado(Empleado empleado);
        Task<int> DeleteEmpleado(Empleado empleado);
    }
}
