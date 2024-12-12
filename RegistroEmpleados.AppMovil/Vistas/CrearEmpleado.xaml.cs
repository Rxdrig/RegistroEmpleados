using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;
using System.Linq.Expressions;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class CrearEmpleado : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroempleados-4902d-default-rtdb.firebaseio.com/");

    public List<Cargo> Cargos { set; get; }

    public CrearEmpleado()
    {
        InitializeComponent();
        ListarCargos();
        BindingContext = this;
    }

    private void ListarCargos()
    {
        var cargos = client.Child("Cargos").OnceAsync<Cargo>();
        Cargos = cargos.Result.Select(x=>x.Object).ToList();
        
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        Cargo cargo = cargoPicker.SelectedItem as Cargo;

        var empleado = new Empleado
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoeEntry.Text,
            CorreoElectronico = correoEntry.Text,
            FechaInicio = fechaInicioPicker.Date,
            Sueldo = int.Parse(sueldoEntry.Text),
            Cargo = cargo
        };

        try
        {
            await client.Child("Empleados").PostAsync(empleado);
            await DisplayAlert("Exito", $"El empleado {empleado.PrimerNombre} {empleado.PrimerApellido} fue guardado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}