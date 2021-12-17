using System.ComponentModel.DataAnnotations;

namespace Clientes.API.ViewModels
{
    public class CrearClienteViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }

        public string Email { get; set; }
        [Required]
        public int DNI { get; set; }

        public string Fecha_de_Creacion { get; set; }

        public string Usuario_creador { get; set; }
    }
}