using Microsoft.Data.Sqlite;
using ModoConectado.Interfaz;
using ModoConectado.Modelos;
using ModoConectado.Servicio;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModoConectado;

public partial class GestionEmpleados : ContentPage
{
    private Departamento departamentoSeleccionado;
    private Empleado empleadoSeleccionado;

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

        LimpiarPantalla();

        // Cargar Departamentos
        Departamentos.Clear();
        var deps = await departamentoService.GetTodosDepartamentos();
        foreach (var d in deps)
            Departamentos.Add(d);

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

        departamentoSeleccionado = (Departamento)e.SelectedItem;

        Localizaciones.Clear();
        // mostrar la localización en el ListView
        Localizaciones.Add(departamentoSeleccionado.Localizacion);

        // Cargar empleados del departamento
        Empleados.Clear();
        var empleadosDept = await empleadoService.GetEmpleadoPorDepartamento(departamentoSeleccionado.DeptNo);
        foreach (var emp in empleadosDept)
            Empleados.Add(emp);
    }

    // poner los datos del empleado seleccionado en los entrys
    private async void OnEmpleadoSeleccionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;
        empleadoSeleccionado = (Empleado)e.SelectedItem;

        entryApellidos.Text = empleadoSeleccionado.Apellido;
        entryOficios.Text = empleadoSeleccionado.Oficio;
        entrySalarios.Text = empleadoSeleccionado.Salario.ToString();
        entryComisión.Text = empleadoSeleccionado.Comision.ToString();
        dateFechaAlta.Date = empleadoSeleccionado.FechaAlta;
    }
    
    // comprobar si los entrys están vacios o no
    public bool ComprobarEntrys()
    {
        List<string> listaDatos = new List<string>();

        listaDatos.Add(entryApellidos.Text);
        listaDatos.Add(entryOficios.Text);
        listaDatos.Add(entrySalarios.Text);
        listaDatos.Add(entryComisión.Text);
        listaDatos.Add(dateFechaAlta.Date.ToString("dd/MM/yyyy"));

        // Verificar si alguno está vacío
        foreach (var entry in listaDatos)
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                return true;
            }
        }
        return false;
    }

    private void OnClickedBuscar(object sender, EventArgs e)
    { 

    }

    private async void OnClickedGuardar(object sender, EventArgs e)
    {
        try
        {
            if (departamentoSeleccionado == null)
            {
                await DisplayAlert("Error", "Se debe seleccionar un departamento", "Aceptar");
                return;
            }else if(ComprobarEntrys())
            {
                await DisplayAlert("Error", "Se deben rellenar todos los datos", "Aceptar");
                return;

            }
            else
            {

                //crear empleado con datos de los entrys
                Empleado nuevoEmpleado = new Empleado()
                {
                    Apellido = entryApellidos.Text,
                    Oficio = entryOficios.Text,
                    Salario = decimal.Parse(entrySalarios.Text),
                    Comision = decimal.Parse(entryComisión.Text),
                    FechaAlta = dateFechaAlta.Date,
                    DeptNo = departamentoSeleccionado.DeptNo
                };

                await empleadoService.GuardarEmpleado(nuevoEmpleado);

                await DisplayAlert("OK", "Empleado guardado correctamente", "Aceptar");

                await CargarEmpleadosDeDepartamento();

            }
        }
        catch (Exception ex) {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void OnClickedActualizar(object sender, EventArgs e)
    {
        try
        {
            if (departamentoSeleccionado == null)
            {
                await DisplayAlert("Error", "Se debe seleccionar un departamento", "Aceptar");
                return;
            }

            if (empleadoSeleccionado == null)
            {
                await DisplayAlert("Error", "Se debe seleccionar un empleado para modificar", "Aceptar");
                return;
            }

            if (!ComprobarEntrys()) {

                //crear empleado con datos de los entrys
                Empleado empleadoActualizado = new Empleado()
                {
                    EmpNo = empleadoSeleccionado.EmpNo,
                    Apellido = entryApellidos.Text,
                    Oficio = entryOficios.Text,
                    Salario = decimal.Parse(entrySalarios.Text),
                    Comision = decimal.Parse(entryComisión.Text),
                    FechaAlta = dateFechaAlta.Date,
                    DeptNo = departamentoSeleccionado.DeptNo
                };

                // editar empleado
                await empleadoService.ActualizarEmpleado(empleadoActualizado);

                await DisplayAlert("OK", "Empleado modificado correctamente", "Aceptar");

                //actualizar listview
                await CargarEmpleadosDeDepartamento();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void OnClickedBorrar(object sender, EventArgs e)
    {
        try
        {

            if (empleadoSeleccionado == null)
            {
                await DisplayAlert("Error", "Se debe seleccionar un empleado para modificar", "Aceptar");
                return;
            }

            if (!ComprobarEntrys())
            {
                bool respuesta = await DisplayAlert("Confirmar borrado", "¿Estás seguro de que deseas borrar este empleado?", "Sí", "No");

                if (respuesta)
                {

                    await empleadoService.EliminarEmpleado(empleadoSeleccionado);
                    await DisplayAlert("OK", "Empleado eliminado", "Aceptar");
                }

                //actualizar listview
                await CargarEmpleadosDeDepartamento();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private void OnClickedLimpiar(object sender, EventArgs e)
    {
        LimpiarPantalla();
    }

    private void LimpiarPantalla()
    {
        entryApellidos.Text = "";
        entryOficios.Text = "";
        entrySalarios.Text = "";
        entryComisión.Text = "";
        dateFechaAlta.Date = DateTime.Today;
        entradaBuscar.Text = "";

        Localizaciones.Clear();
        Empleados.Clear();
    }

    private async Task CargarEmpleadosDeDepartamento()
    {
        if (departamentoSeleccionado == null) return;

        var empleadosDepto = await empleadoService.GetEmpleadoPorDepartamento(departamentoSeleccionado.DeptNo);
        Empleados.Clear();
        foreach (var emp in empleadosDepto)
            Empleados.Add(emp);
    }
}
