using ModoConectado.Modelos;
using System.Collections.ObjectModel;

namespace ModoConectado;

public partial class GestionEmpleados : ContentPage
{
    // Colecciones para los ListView
    private ObservableCollection<Empleado> empleados = new ObservableCollection<Empleado>();
    private ObservableCollection<Departamento> departamentos = new ObservableCollection<Departamento>();

    public GestionEmpleados()
    {
        InitializeComponent();

        CamposBusqueda();
        CargarDatosEjemplo();

        // Asignar ItemsSource de tus ListView
        listaEmpleados.ItemsSource = empleados;
        listaDepartamentos.ItemsSource = departamentos;
        listaLocalizacion.ItemsSource = departamentos;
    }

    private void CamposBusqueda()
    {
        ObservableCollection<string> camposBusqueda = new ObservableCollection<string>
        {
            "Apellido",
            "Oficio",
            "Salario",
            "Fecha Alta",
            "Comisión"
        };

        listaCampos.ItemsSource = camposBusqueda;
    }

    private void CargarDatosEjemplo()
    {
        // Departamentos
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });
        departamentos.Add(new Departamento { DeptNo = 1, Nombre = "Ventas", Localizacion = "Madrid" });
        departamentos.Add(new Departamento { DeptNo = 2, Nombre = "IT", Localizacion = "Barcelona" });
        departamentos.Add(new Departamento { DeptNo = 3, Nombre = "RRHH", Localizacion = "Valencia" });

        // Empleados
        empleados.Add(new Empleado { EmpNo = 100, Apellido = "Pérez", Oficio = "Vendedor", Salario = 2000, Comision = 150, FechaAlta = DateTime.Parse("2020-01-10"), DeptNo = 1 });
        empleados.Add(new Empleado { EmpNo = 101, Apellido = "Gómez", Oficio = "Programador", Salario = 3000, Comision = 0, FechaAlta = DateTime.Parse("2019-05-20"), DeptNo = 2 });
        empleados.Add(new Empleado { EmpNo = 102, Apellido = "López", Oficio = "RRHH", Salario = 2500, Comision = 0, FechaAlta = DateTime.Parse("2021-07-15"), DeptNo = 3 });
        empleados.Add(new Empleado { EmpNo = 100, Apellido = "Pérez", Oficio = "Vendedor", Salario = 2000, Comision = 150, FechaAlta = DateTime.Parse("2020-01-10"), DeptNo = 1 });
        empleados.Add(new Empleado { EmpNo = 101, Apellido = "Gómez", Oficio = "Programador", Salario = 3000, Comision = 0, FechaAlta = DateTime.Parse("2019-05-20"), DeptNo = 2 });
        empleados.Add(new Empleado { EmpNo = 102, Apellido = "López", Oficio = "RRHH", Salario = 2500, Comision = 0, FechaAlta = DateTime.Parse("2021-07-15"), DeptNo = 3 });

        empleados.Add(new Empleado { EmpNo = 103, Apellido = "Martínez", Oficio = "Programador", Salario = 3200, Comision = 0, FechaAlta = DateTime.Parse("2022-03-01"), DeptNo = 2 });
    }
}
