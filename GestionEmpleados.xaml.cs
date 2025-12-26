using Microsoft.Data.Sqlite;
using ModoConectado.Interfaz;
using ModoConectado.Modelos;
using ModoConectado.Servicio;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ModoConectado;

public partial class GestionEmpleados : ContentPage
{
    private const string NombreArchivoBD = "empresa_bd.db";
    private string rutaBD;

    // Servicios
    private EmpleadoService empleadoService;
    private DepartamentoService departamentoService;

    // Colecciones para poner en las ListViews
    public ObservableCollection<Empleado> Empleados { get; set; }
    public ObservableCollection<Departamento> Departamentos { get; set; }
    public ObservableCollection<string> Localizaciones { get; set; }


    public GestionEmpleados()
    {
        InitializeComponent();

        CamposBusqueda();

        rutaBD = Path.Combine(FileSystem.AppDataDirectory, NombreArchivoBD);

        // Inicializar DAO y Servicios
        var empleadoDao = new EmpleadoSqliteDAO(rutaBD);
        var departamentoDao = new DepartamentoSqliteDAO(rutaBD);
        empleadoService = new EmpleadoService(empleadoDao);
        departamentoService = new DepartamentoService(departamentoDao);

        Empleados = new ObservableCollection<Empleado>();
        Departamentos = new ObservableCollection<Departamento>();
        Localizaciones = new ObservableCollection<string>();

        // Poner datos a las listview
        listaDepartamentos.ItemsSource = Departamentos;
        listaEmpleados.ItemsSource = Empleados;
        listaLocalizacion.ItemsSource = Localizaciones;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Cargar Departamentos
        Departamentos.Clear();
        var deps = await departamentoService.GetTodosDepartamentos();
        foreach (var d in deps)
            Departamentos.Add(d);

        Empleados.Clear();
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

    // mostrar empleados y localizacion del departamento seleccionado
    private async void OnDepartamentoSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var dept = (Departamento)e.SelectedItem;

        Localizaciones.Clear();
        // mostrar la localización en el ListView
        Localizaciones.Add(dept.Localizacion);

        // Cargar empleados del departamento
        Empleados.Clear();
        var empleadosDept = await empleadoService.GetEmpleadoPorDepartamento(dept.DeptNo);
        foreach (var emp in empleadosDept)
            Empleados.Add(emp);
    }

    // poner los datos del empleado seleccionado en los entrys
    private async void OnEmpleadoSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

       
    }
}
