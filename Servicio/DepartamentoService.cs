using ModoConectado.Interfaz;
using ModoConectado.Modelos;

namespace ModoConectado.Servicio
{
    internal class DepartamentoService
    {
        private readonly IDepartamentoDAO _departamentoDAO;

        public DepartamentoService(IDepartamentoDAO departamentoDAO)
        {
            _departamentoDAO = departamentoDAO;
        }

        public Task<List<Departamento>> GetTodosDepartamentos()
        {
            return _departamentoDAO.GetDepartamentos();
        }
    }
}
