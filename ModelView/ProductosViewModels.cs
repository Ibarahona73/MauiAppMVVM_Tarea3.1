using MauiAppMVVM.Controllers;
using MauiAppMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using System.Windows.Input;

namespace MauiAppMVVM.ModelView
{
    public class ProductosViewModels : BaseView
    {
        private FirebaseControl Firebasecontrol = new FirebaseControl();
        private ProductosController productosController = new ProductosController();
        private string productKey;
        private string _nombre;
        private double _precio;
        private string _foto;
        private string _key;
        private bool _visibilityCreate;
        private bool _visibilityUpdate;
        private Models.FireProd _selectedProduct;

        FirebaseClient client = new FirebaseClient("https://mvvm-741a5-default-rtdb.firebaseio.com/");
        public List<Productos> Productas { get; set; }

        public string Key
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged(); }
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

        public Models.FireProd SelectedProduct
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
            CreateCommand = new Command(async () => await CreateData());
            UpdateCommand = new Command(async () => await UpdateProducto(productKey));
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
                productKey = SelectedProduct.Key;
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


        async Task CreateData()
        {
            if (Nombre == null)
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "Por favor ingrese una descripcion para el producto", "OK");
                return;
            }
            else if (Precio == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "Por favor ingrese un precio para el producto", "OK");
                return;
            }

            try
            {
                var product = new Models.FireProd
                {
                    Nombre = Nombre,
                    Precio = Precio,
                    Foto = Foto
                };

                if (Firebasecontrol != null)
                {
                    bool addedSuccessfully = await Firebasecontrol.CrearProducto(product);

                    if (addedSuccessfully)
                    {
                        await Application.Current.MainPage.DisplayAlert("Atención", "Producto Creado", "OK");
                        var navigation = Application.Current.MainPage.Navigation;
                        await navigation.PushAsync(new Views.PageListProductos());
                    }
                }

            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "No se pudo crear el producto", "OK");
            }
        }

        async Task UpdateProducto(string key)
        {
            if (Nombre == null)
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "Por favor ingrese una descripcion para el producto", "OK");
                return;
            }
            else if (Precio == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "Por favor ingrese un precio para el producto", "OK");
                return;
            }

            try
            {
                var product = new Models.FireProd
                {
                    Key = key,
                    Nombre = Nombre,
                    Precio = Precio,
                    Foto = Foto
                };

                if (Firebasecontrol != null)
                {
                    bool addedSuccessfully = await Firebasecontrol.CrearProducto(product);

                    if (addedSuccessfully)
                    {
                        await Application.Current.MainPage.DisplayAlert("Atención", "Producto Actualizado", "OK");
                        var navigation = Application.Current.MainPage.Navigation;
                        await navigation.PushAsync(new Views.PageListProductos());
                    }
                }

            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Atención", "No se pudo actualizar el producto", "OK");
            }
        }

        //Tomar Foto
        async void TomarFoto()
        {
            FileResult photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
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