using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.Models
{
    [SQLite.Table("productosDB")]
    public class Productos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(200), NotNull]
        public string Nombre { get; set; }
        [NotNull]
        public double Precio { get; set; }
        public string Foto { get; set; }
    }

}
