using MauiAppMVVM.Models;


namespace MauiAppMVVM.Views;

[QueryProperty(nameof(Detalle), "Detalle")]
public partial class VerProductPage : ContentPage
{
	Productos detalle;
    public Productos Productos { get; set; }

    public Productos Detalle {  

		get => detalle; 
		set {
				detalle = value;
				OnPropertyChanged();
			}
		}

	public VerProductPage()
	{
		InitializeComponent();
		BindingContext = this;        
    }

    public VerProductPage(Productos productos)
    {
        InitializeComponent();
        Detalle = productos;
        BindingContext = this;

    }
}