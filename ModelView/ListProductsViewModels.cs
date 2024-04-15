using MauiAppMVVM.Controllers;
using MauiAppMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiAppMVVM.ModelView
{
    public class ListProductsViewModels : BaseView
    {
        private ObservableCollection<Models.FireProd> _products;
        private FirebaseControl firebaseControl = new FirebaseControl();
               

        public ObservableCollection<Models.FireProd> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private Models.FireProd _selectedProduct;

        public Models.FireProd SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public ICommand GoToDetailsCommand { private set; get; }        

        public ICommand DeleteCommand { private set; get; }

        public ICommand NuevoProductoCommand{ private set; get; }

        public INavigation Navigation { get; set; }
        public ICommand SalirCommand { private set; get; }

        public ListProductsViewModels(INavigation navigation)
        {
            Navigation = navigation;
            GoToDetailsCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType, SelectedProduct));
            NuevoProductoCommand = new Command<Type>(async (pageType) => await NuevoProducto(pageType));
            DeleteCommand = new Command(async () => await DeleteProducto(SelectedProduct.Key));

            loadProductos();
        }

        async Task loadProductos()
        {
            List<FireProd> listProductos;

            Products = new ObservableCollection<FireProd>();

            try
            {
                listProductos = await firebaseControl.GetListProductos();
                foreach (var product in listProductos)
                {
                    FireProd productos = new FireProd
                    {
                        Key = product.Key,
                        Nombre = product.Nombre,
                        Precio = product.Precio,
                        Foto = product.Foto,
                    };

                    Products.Add(productos);
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "Se produjo un error al obtener los productos", "OK");
            }
        }

        async Task GoToDetails(Type pageType, FireProd selectedProduct)
        {
            if (selectedProduct != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);

                var Model = new ProductosViewModels();
                Model.SelectedProduct = selectedProduct;
                Model.Nombre = selectedProduct.Nombre;
                Model.Foto = selectedProduct.Foto;
                Model.Precio = selectedProduct.Precio;
                Model.Key = selectedProduct.Key;
                page.BindingContext = Model;

                await Navigation.PushAsync(page);
            }
        }

        async Task NuevoProducto(Type pageType)
        {
            var page = (Page)Activator.CreateInstance(pageType);

            var viewModel = new ProductosViewModels();
            viewModel.SelectedProduct = null;
            page.BindingContext = viewModel;
            await Navigation.PushAsync(page);
        }

        async Task DeleteProducto(string key)
        {
            if (SelectedProduct != null)
            {
                var tappedItem = Products.FirstOrDefault(item => item.Key == key);
                bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este producto?", "Si", "No");
                if (userConfirmed)
                {
                    try
                    {

                        if (firebaseControl != null)
                        {
                            bool success = await firebaseControl.deleteProducto(key);

                            if (success)
                            {
                                Products.Remove(tappedItem);
                                SelectedProduct = null;

                                await Application.Current.MainPage.DisplayAlert("Atención", "Producto Eliminado", "OK");
                            }
                        }

                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Atención", "No se pudo eliminar el producto", "OK");
                    }
                }

            }

        }

    }
}