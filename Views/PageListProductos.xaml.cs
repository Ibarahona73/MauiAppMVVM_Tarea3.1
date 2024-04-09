using Microsoft.Maui.Controls.PlatformConfiguration;
using System;

namespace MauiAppMVVM.Views;

public partial class PageListProductos : ContentPage
{
	public PageListProductos()
	{
		InitializeComponent();


        BindingContext = new ModelView.ListProductsViewModels(Navigation);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Current.Quit();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var navigation = Application.Current.MainPage.Navigation;
        await navigation.PushAsync(new Views.ListFirebase());
    }
}