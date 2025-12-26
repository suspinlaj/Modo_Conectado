using ModoConectado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModoConectado.Interfaz
{
    internal interface IDepartamentoDAO
    {
        Task<List<Departamento>> GetDepartamentos();
    }
}
