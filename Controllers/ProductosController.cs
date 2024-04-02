using MauiAppMVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.Controllers
{
    public class ProductosController
    {
        SQLiteAsyncConnection? _connection = null;


        public ProductosController() { Init(); }

        public async Task Init()
        {

            try
            {

                if (_connection is null)
                {
                    SQLite.SQLiteOpenFlags extensiones = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

                    if (string.IsNullOrEmpty(FileSystem.AppDataDirectory))
                    {
                        return;
                    }

                    _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "DBProductos.db3"), extensiones);

                    var creacion = await _connection.CreateTableAsync<Productos>();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error en el Init() {ex.Message}");
            }

        }

        // Crear los metodos Crud para la clase Personas
        // Create  
        public async Task<bool> storeProductos(Productos producto)
        {
            await Init();

            if (producto.Id == 0)
            {
                await _connection.InsertAsync(producto);
            }
            else
            {
                await _connection.UpdateAsync(producto);
            }

            return true;
        }


        // Update

        public async Task<int> updateProducto(Productos producto)
        {
            await Init();

            return await _connection.UpdateAsync(producto);
        }


        // Read 

        public async Task<List<Models.Productos>> GetListProductos()
        {
            await Init();
            return await _connection.Table<Productos>().ToListAsync();
        }

        // Read Element
        public async Task<Models.Productos> GetProductos(int pid)
        {
            await Init();
            return await _connection.Table<Productos>().Where(i => i.Id == pid).FirstOrDefaultAsync();
        }

        // Delete Element
        public async Task<int> deleteProductos(int itemID)
        {
            await Init();
            var itemToDelete = await GetProductos(itemID);

            if (itemToDelete != null)
            {

                return await _connection.DeleteAsync(itemToDelete);
            }
            return 0;
        }

        //Delete Alll

        public async Task<int> DeleteAllProductos()
        {
            await Init();
            return await _connection.DeleteAllAsync<Productos>();
        }


    }

}



