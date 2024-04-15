using Firebase.Database;
using Firebase.Database.Query;
using MauiAppMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.Controllers
{
    public class FirebaseControl
    {
        //Cliente de Firebase
        private FirebaseClient client = new FirebaseClient("https://mvvm-741a5-default-rtdb.firebaseio.com/");

        //Constructor Vacio
        public FirebaseControl() { }

        //Metodo para crear/agregar/update un nuevo producto
        public async Task<bool> CrearProducto(FireProd producto)
        {
            if (string.IsNullOrEmpty(producto.Key))
            {
                try
                {
                    var productos = client.Child("Productos").OnceAsync<FireProd>();
                    if (productos.Result.Count == 0)
                    {
                        await client.Child("Productos").PostAsync(new FireProd
                        {
                            Nombre = producto.Nombre,
                            Precio = producto.Precio,
                            Foto = producto.Foto,
                        });

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    await client.Child("Productos").Child(producto.Key).PutAsync(producto);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }

            return false;
        }

        //Metodo para Lectura de elementos
        public async Task<List<FireProd>> GetListProductos()
        {
            var productos = await client.Child("Productos").OnceSingleAsync<Dictionary<string, FireProd>>();

            return productos.Select(x => new FireProd
            {
                Key = x.Key,
                Nombre = x.Value.Nombre,
                Precio = x.Value.Precio,
                Foto = x.Value.Foto
            }).ToList();
        }

        //Delete
        public async Task<bool> deleteProducto(string key)
        {
            try
            {
                await client.Child("Productos").Child(key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}