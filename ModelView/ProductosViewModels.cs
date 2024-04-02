using MauiAppMVVM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiAppMVVM.ModelView
{
    public class ProductosViewModels : BaseView
    {
        private ProductosController productosController = new ProductosController();
        private int productId;        
        private string _nombre;                
        private double _precio;
        private string _foto;
        private int _id;
        private bool _visibilityCreate;
        private bool _visibilityUpdate;
        private Models.Productos _selectedProduct;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged();}
        }

        public double Precio
        {
            get { return _precio; }
            set { _precio = value; OnPropertyChanged(); }
        }

        public string Foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }

        public bool VisibilityCreate
        {
            get { return _visibilityCreate; }
            set { _visibilityCreate = value; OnPropertyChanged(); }
        }

        public bool VisibilityUpdate
        {
            get { return _visibilityUpdate; }
            set { _visibilityUpdate = value; OnPropertyChanged(); }
        }

        public Models.Productos SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            {
                _selectedProduct = value;
                OnPropertyChanged();

                Console.WriteLine($"SelectedProduct changed to: {_selectedProduct?.Nombre}");

                ShowProductStatusAlert();
            }
        }

        public ProductosViewModels()
        {
            CleanCommand = new Command(Cleaner);
            FotoCommand = new Command(() => TomarFoto());
            if (productId == 0) { }
            CreateCommand = new Command(async () => await CreateData());
            UpdateCommand = new Command(async () => await UpdateProducto(productId));
        }

        public ICommand CleanCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand FotoCommand { get; private set; }


        private void Cleaner()
        {
            Nombre = string.Empty;
            Precio = 0;
            Foto = string.Empty;
        }

        void ShowProductStatusAlert()
        {
            if(SelectedProduct != null)
            {
                productId = SelectedProduct.Id;
                VisibilityCreate = false;
                VisibilityUpdate = true;
            }
            else
            {
                VisibilityCreate = true;
                VisibilityUpdate = false;
            }
        }

                                                                                                              //CRUD 


        /*async Task CreateData()
        {
            try{

                var Productos = new Models.Productos{

                    Nombre = Nombre,
                    Precio = Precio,
                    Foto = Foto,
                };

                // Ejemplo de lo que quiere 
                // await DBNull.insert(product);

                // crear un objeto que maneje la bd 
            }

            catch(Exception ex){


            }
        } */

        async Task CreateData()
        {
            if(Nombre == null)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Porfavor Ingrese una Descripcion para el producto", "OK");
                return;
            }
            else if (Precio == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Porfavor Ingrese un Precio para el producto", "OK");
                return;
            }

            try
            {

                var Product = new Models.Productos
                {

                    Nombre = Nombre,
                    Precio = Precio,
                    Foto = Foto,
                };

                if(productosController != null)
                {
                    bool añadido = await productosController.storeProductos(Product);

                    if (añadido)
                    {
                        await Application.Current.MainPage.DisplayAlert("Atencion", "Producto Creado", "OK");
                        var navigation = Application.Current.MainPage.Navigation;
                        await navigation.PushAsync(new Views.PageListProductos());
                    }
                }
       
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se Puedo Crear El Producto", "OK");

            }

        }

        async Task UpdateProducto(int id)
        {
            if(Nombre == null)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Ingrese Un Nombre Para El Producto", "OK");
                return;
            }
            else if(Precio == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Ingrese Un Precio Para El Producto", "OK");
                return;
            }

            try
            {
                var Product = new Models.Productos
                {
                    Id = id,
                    Nombre = Nombre,
                    Precio = Precio,
                    Foto = Foto,
                };

                if (productosController != null)
                {
                    bool añadido = await productosController.storeProductos(Product);

                    if (añadido)
                    {
                        await Application.Current.MainPage.DisplayAlert("Atencion", "Producto Modificado", "OK");
                        var navigation = Application.Current.MainPage.Navigation;
                        await navigation.PushAsync(new Views.PageListProductos());
                    }
                }

            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Atencion", "Ingrese Un Precio Para El Producto", "OK");
            }
        }

        async void TomarFoto()
        {
            FileResult photo = await MediaPicker.CapturePhotoAsync();

            if(photo != null)
            {
                string photoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (Stream sourcephoto = await photo.OpenReadAsync())
                using (FileStream streamlocal = File.OpenWrite(photoPath))
                {
                    await sourcephoto.CopyToAsync(streamlocal);

                    Foto = Convertir.PhotoHelper.GetImg64(photo);
                }
            }
        }
    }
}
