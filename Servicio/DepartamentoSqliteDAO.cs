using Microsoft.Data.Sqlite;
using ModoConectado.Interfaz;
using ModoConectado.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModoConectado.Servicio
{
    internal class DepartamentoSqliteDAO : IDepartamentoDAO
    {
        private readonly string _dbRuta;

        public DepartamentoSqliteDAO(string dbRuta)
        {
            _dbRuta = dbRuta;
        }

        public async Task<List<Departamento>> GetDepartamentos()
        {
            var departamentosLista = new List<Departamento>();

            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Departamento";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departamentosLista.Add(new Departamento
                {
                    DeptNo = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Localizacion = reader.GetString(2)
                });
            }
            return departamentosLista;
        }
    }
}
