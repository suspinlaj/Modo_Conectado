using Microsoft.Data.Sqlite;
using ModoConectado.Interfaz;
using ModoConectado.Modelos;

namespace ModoConectado.Servicio
{
    internal class EmpleadoSqliteDAO : IEmpleadoDAO
    {
        private readonly string _dbRuta;

        public EmpleadoSqliteDAO(string dbRuta)
        {
            _dbRuta = dbRuta;
            InicializarBaseDatos();
        }

        // CREAR TABLAS en la base de datos
        private void InicializarBaseDatos()
        {
            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Empleado (
                    EmpNo INTEGER PRIMARY KEY AUTOINCREMENT,
                    Apellido TEXT,
                    Oficio TEXT,
                    Salario REAL,
                    Comision REAL,
                    FechaAlta TEXT,
                    DeptNo INTEGER
                );

                CREATE TABLE IF NOT EXISTS Departamento (
                    DeptNo INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT,
                    Localizacion TEXT
                );";

            command.ExecuteNonQuery();
            InicializarDatos();
        }

        // PONER DATOS en las tablas
        private void InicializarDatos()
        {
            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            connection.Open();

            var command = connection.CreateCommand();

            // para comprobar si ya hay datos y no du`plicar
            command.CommandText = "SELECT COUNT(*) FROM Departamento";
            long count = (long)command.ExecuteScalar();

            if (count == 0)
            {
                // Insertar datos departamentos
                command.CommandText = @"
                    INSERT INTO Departamento (Nombre, Localizacion) VALUES
                    ('Recursos Humanos', 'Madrid'),
                    ('Finanzas', 'Barcelona'),
                    ('IT', 'Valencia'),
                    ('Marketing', 'Sevilla'),
                    ('Ventas', 'Bilbao');";
                command.ExecuteNonQuery();

                // insertar datos empleados
                command.CommandText = @"
                    INSERT INTO Empleado (APELLIDO, OFICIO, SALARIO, COMISION, FECHA_ALT, DEPT_NO) VALUES
                    ('García', 'Gerente', 50000, 5000, '2022-01-01', 1),
                    ('Martínez', 'Asistente', 30000, 3000, '2022-02-01', 1),
                    ('López', 'Contador', 45000, 4500, '2022-03-01', 2),
                    ('Pérez', 'Analista', 40000, 4000, '2022-04-01', 2),
                    ('Sánchez', 'Desarrollador', 60000, 6000, '2022-05-01', 3),
                    ('Fernández', 'Soporte', 35000, 3500, '2022-06-01', 3),
                    ('Gómez', 'Especialista', 55000, 5500, '2022-07-01', 4),
                    ('Díaz', 'Coordinador', 37000, 3700, '2022-08-01', 4),
                    ('Rodríguez', 'Vendedor', 48000, 4800, '2022-09-01', 5),
                    ('Hernández', 'Representante', 42000, 4200, '2022-10-01', 5);";
                command.ExecuteNonQuery();
            }
        }

        // OBTENER empleados
        public async Task<List<Empleado>> GetEmpleados()
        {
            var empleados = new List<Empleado>();

            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Empleado";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                empleados.Add(new Empleado
                {
                    EmpNo = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Oficio = reader.GetString(2),
                    Salario = reader.GetDecimal(3),
                    Comision = reader.GetDecimal(4),
                    FechaAlta = reader.GetDateTime(5),
                    DeptNo = reader.GetInt32(6)
                });
            }
            return empleados;
        }

        // OBTENER los empleados por el criterio de BUSQUDA
        public async Task<List<Empleado>> GetEmpleadosByBusqueda(string busqueda, string valor)
        {
            var empleados = new List<Empleado>();

            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            // para poder buscar por una parte
            command.CommandText = $"SELECT * FROM Empleado WHERE {busqueda} LIKE @Busqueda";
            command.Parameters.AddWithValue("@Busqueda", $"%{valor}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                empleados.Add(new Empleado
                {
                    EmpNo = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Oficio = reader.GetString(2),
                    Salario = reader.GetDecimal(3),
                    Comision = reader.GetDecimal(4),
                    FechaAlta = reader.GetDateTime(5),
                    DeptNo = reader.GetInt32(6)
                });
            }
            return empleados;
        }

        // OBTENER los empleados POR DEPARTAMENTO
        public async Task<List<Empleado>> GetEmpleadosByDepartamento(int deptNo)
        {
            var empleados = new List<Empleado>();

            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Empleado WHERE DeptNo = @DeptNo";
            command.Parameters.AddWithValue("@DeptNo", deptNo);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                empleados.Add(new Empleado
                {
                    EmpNo = reader.GetInt32(0),
                    Apellido = reader.GetString(1),
                    Oficio = reader.GetString(2),
                    Salario = reader.GetDecimal(3),
                    Comision = reader.GetDecimal(4),
                    FechaAlta = reader.GetDateTime(5),
                    DeptNo = reader.GetInt32(6)
                });
            }
            return empleados;
        }

        // GUARDAR empleado
        public async Task<int> SaveEmpleado(Empleado empleado)
        {
            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            if (empleado.EmpNo != 0)
            {
                command.CommandText = @"
                    UPDATE Empleado
                    SET Apellido = @Apellido, Oficio = @Oficio, Salario = @Salario, Comision = @Comision, FechaAlta = @FechaAlt, DeptNo = @DeptNo
                    WHERE EmpNo = @EmpNo";
                command.Parameters.AddWithValue("@EmpNo", empleado.EmpNo);
            }
            else
            {
                command.CommandText = @"
                    INSERT INTO Empleado (Apellido, Oficio, Salario, Comision, FechaAlta, DeptNo)
                    VALUES (@Apellido, @Oficio, @Salario, @Comision, @FechaAlt, @DeptNo)";
            }
            command.Parameters.AddWithValue("@Apellido", empleado.Apellido);
            command.Parameters.AddWithValue("@Oficio", empleado.Oficio);
            command.Parameters.AddWithValue("@Salario", empleado.Salario);
            command.Parameters.AddWithValue("@Comision", empleado.Comision);
            command.Parameters.AddWithValue("@FechaAlt", empleado.FechaAlta);
            command.Parameters.AddWithValue("@DeptNo", empleado.DeptNo);
            return await command.ExecuteNonQueryAsync();
        }

        // ACTUALIZAR empleado
        public async Task<int> UpdateEmpleado(Empleado empleado)
        {
            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Empleado SET Apellido = @Apellido, Oficio = @Oficio, 
                                    Salario = @Salario, Comision = @Comision, FechaAlta = @FechaAlt  
                                    WHERE EmpNo = @EmpNo";
            command.Parameters.AddWithValue("@Apellido", empleado.Apellido);
            command.Parameters.AddWithValue("@Oficio", empleado.Oficio);
            command.Parameters.AddWithValue("@Salario", empleado.Salario);
            command.Parameters.AddWithValue("@Comision", empleado.Comision);
            command.Parameters.AddWithValue("@FechaAlt", empleado.FechaAlta);
            command.Parameters.AddWithValue("@DeptNo", empleado.DeptNo);
            command.Parameters.AddWithValue("@EmpNo", empleado.EmpNo);
            return await command.ExecuteNonQueryAsync();
        }

        // BORRAR empleado
        public async Task<int> DeleteEmpleado(Empleado empleado)
        {
            using var connection = new SqliteConnection($"Data Source={_dbRuta}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Empleado WHERE EmpNo = @EmpNo";
            command.Parameters.AddWithValue("@EmpNo", empleado.EmpNo);

            return await command.ExecuteNonQueryAsync();
        }
    }
}
