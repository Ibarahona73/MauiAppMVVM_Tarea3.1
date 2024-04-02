namespace MauiAppMVVM.Views;

public partial class PageListProductos : ContentPage
{
	public PageListProductos()
	{
		InitializeComponent();

        BindingContext = new ModelView.ListProductsViewModels(Navigation);
    }

	 
}