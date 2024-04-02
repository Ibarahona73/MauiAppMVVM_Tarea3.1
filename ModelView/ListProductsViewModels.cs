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

        private ObservableCollection<Models.Productos> _products;
        private ProductosController productosController = new ProductosController();

        public ObservableCollection<Models.Productos> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private Models.Productos _selectedProduct;

        public Models.Productos SelectedProduct
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
            DeleteCommand = new Command(async () => await DeleteProduct(SelectedProduct.Id));

            SalirCommand = new Command(() =>
            {MessagingCenter.Send<object>(this, "CerrarApp");});

            

            CargarProductos();            
        }

        async Task CargarProductos()
        {
            List<Productos> listProductos;

            Products = new ObservableCollection<Productos>();

            try
            {
                listProductos = await productosController.GetListProductos();
                foreach (var product in listProductos)
                {
                    Productos productos = new Productos
                    {
                        Id = product.Id,
                        Nombre = product.Nombre,
                        Precio = product.Precio,
                        Foto = product.Foto,

                    };

                    Products.Add(productos);

                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Se Produjo Un Error Al Obtener los Productos", "OK");
            }

        }

        async Task GoToDetails(Type pageType, Models.Productos selectedProduct)
        {
            if (SelectedProduct != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);


                var VModel = new ProductosViewModels();

                VModel.SelectedProduct = SelectedProduct;                
                VModel.Nombre = SelectedProduct.Nombre;                                
                VModel.Foto = SelectedProduct.Foto;
                VModel.Precio = SelectedProduct.Precio;
                VModel.Id = SelectedProduct.Id;

                page.BindingContext = VModel;


                await Navigation.PushAsync(page);
            }
        }

        async Task NuevoProducto(Type pageType)
        {

            var page = (Page)Activator.CreateInstance(pageType);

            var VModel = new ProductosViewModels();
            VModel.SelectedProduct = null;
            page.BindingContext = VModel;
            await Navigation.PushAsync(page);


        }

        async Task DeleteProduct(int id)
        {
            if (SelectedProduct != null)
            {
                bool Confirmo = await Application.Current.MainPage.DisplayAlert("Confirmacion", "Esta Seguro Que Desea Borrar Este Registro?", "Si", "No");

                if (Confirmo)
                {
                    try
                    {
                        if (productosController != null)
                        {
                            int exito = await productosController.deleteProductos(id);

                            if (exito != 0)
                            {
                                Products.Remove(SelectedProduct);
                                await Application.Current.MainPage.DisplayAlert("Atencion", "Producto Eliminado", "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Atencion", $"No Se Pudo Borrar El Registro: \n{ex.Message}", "OK");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Seleccione un producto válido para eliminar.", "OK");
            }
        }

    }
}