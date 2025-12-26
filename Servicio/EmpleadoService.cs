using ModoConectado.Interfaz;
using ModoConectado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModoConectado.Servicio
{
    internal class EmpleadoService
    {
        private readonly IEmpleadoDAO _empleadoDAO;

        public EmpleadoService(IEmpleadoDAO empleadoDAO)
        {
            _empleadoDAO = empleadoDAO;
        }

        public Task<List<Empleado>> GetTodosEmpleados()
        {
            return _empleadoDAO.GetEmpleados();
        }

        public Task<List<Empleado>> GetEmpleadoPorDepartamento(int deptNo)
        {
            return _empleadoDAO.GetEmpleadosByDepartamento(deptNo);
        }

        public Task<List<Empleado>> GetEmpleadoPorCriterio(string criterio, string valor)
        {
            return _empleadoDAO.GetEmpleadosByBusqueda(criterio, valor);
        }

        public Task<int> GuardarEmpleado(Empleado empleado)
        {
            return _empleadoDAO.SaveEmpleado(empleado);
        }

        public Task<int> ActualizarEmpleado(Empleado empleado)
        {
            return _empleadoDAO.UpdateEmpleado(empleado);
        }

        public Task<int> EliminarEmpleado(Empleado empleado)
        {
            return _empleadoDAO.DeleteEmpleado(empleado);
        }
    }
}
