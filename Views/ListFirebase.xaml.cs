namespace MauiAppMVVM.Views;
using MauiAppMVVM.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

public partial class ListFirebase : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://mvvm-741a5-default-rtdb.firebaseio.com/");
	public ObservableCollection<Productos> Lista { get; set; } = new ObservableCollection<Productos>();

	public ListFirebase()
	{
		InitializeComponent();
		BindingContext = this;
		CargarList();

    }

	public void CargarList()
	{
		client.Child("Productos")
			.AsObservable<Productos>()
			.Subscribe((producto) =>
			{

				if(producto.Object != null)
				{
					Lista.Add(producto.Object);
				}

			});
	}

    private async void NewButton_Clicked(object sender, EventArgs e)
    {
        var navigation = Application.Current.MainPage.Navigation;
        await navigation.PushAsync(new Views.PageListProductos());
    }

    private void filtroEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
		string filtro = filtroEntry.Text.ToLower();

		if(filtro.Length >0 )
		{
			listaProducts.ItemsSource = Lista.Where(x => x.Nombre.ToLower().Contains(filtro));

		}
		else
		{
			listaProducts.ItemsSource = Lista;

        }
		

    }

    private async void listaProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        /*Productos productos = e.CurrentSelection.FirstOrDefault() as Productos;

        var parametro = new Dictionary<string, object>(){

           ["Detalle"] = productos

       };

       var navigation = Application.Current.MainPage.Navigation;
       await navigation.PushAsync(new VerProductPage(parametro)); */


        try
        {
            Productos productos = e.CurrentSelection.FirstOrDefault() as Productos;

            if (productos != null)
            {
                await Navigation.PushAsync(new VerProductPage(productos));
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepci�n de acuerdo a tus necesidades
            Console.WriteLine("Error al navegar a VerProductPage: " + ex.Message);
        }
    }

}