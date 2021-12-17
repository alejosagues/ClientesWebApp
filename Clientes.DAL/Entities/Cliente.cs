using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clientes.DAL.Entities
{
    public class Cliente
    {
        public int _id { get; set; }

        public int Id { get; set; }
        
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int DNI { get; set; }

        public string Email { get; set; }

        public string Fecha_de_Creacion { get; set; }

        public string Usuario_creador { get; set; }
    }
}