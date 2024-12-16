using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;
using System.Collections.ObjectModel;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class EditarEmpleado : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroempleados-4902d-default-rtdb.firebaseio.com/");
    public List<Cargo> Cargos { get; set; }
    public ObservableCollection<string> ListaCargos { get; set; }= new ObservableCollection<string>();
    private Empleado empleadoActual = new Empleado();
    private string empleadoId;
    public EditarEmpleado(string idEmpleado)
	{
		InitializeComponent();
        BindingContext = this;
        empleadoId = idEmpleado;
        CargarListaCargos();
        CargarEmpleado(empleadoId);  
	}

    private async void CargarListaCargos()
    {
        try
        {
            var cargos = await client.Child("Cargos").OnceAsync<Cargo>();
            ListaCargos.Clear();
            foreach (var cargo in cargos)
            {
                ListaCargos.Add(cargo.Object.Nombre);
            }
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", "Error:" + ex.Message, "OK");
        }

    }



    private async void CargarEmpleado(string idEmpleado)
    {
        var empleado = await client.Child("Empleados").Child(idEmpleado).OnceSingleAsync<Empleado>();

        if (empleado != null) 
        {
            EditPrimerNombreEntry.Text = empleado.PrimerNombre;
            EditSegundoNombreEntry.Text = empleado.SegundoNombre;
            EditPrimerApellidoEntry.Text = empleado.PrimerApellido;
            EditSegundoApellidoEntry.Text = empleado.SegundoApellido;
            EditCorreoEntry.Text = empleado.CorreoElectronico;
            EditSueldoEntry.Text = empleado.Sueldo.ToString();
            EditCargoPicker.SelectedItem = empleado.Cargo?.Nombre;
        }
    }

    private void ActualizarButton_Clicked(object sender, EventArgs e)
    {

    }
}