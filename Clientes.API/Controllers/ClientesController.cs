using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Clientes.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Clientes.API.ViewModels;
using Clientes.DAL.Entities;

//uncomment all the [Authorize]
namespace Clientes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context){
            _context = context;
        }
        
        // GET api/clientes
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            //return new string[] { "value1", "value2" };
            return Ok(clientes);
        }
        // GET api/cliente/5
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetCliente (int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(cliente);
        }

        // POST api/clientes/crear
        [HttpPost("crear")]
        //[Authorize]
        public async Task<IActionResult> Crear(CrearClienteViewModel model)
        {
            var fecha = DateTime.Now.ToString("dd/MM/yyyy");
            var cliente = new Cliente
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                DNI = model.DNI,
                Email = model.Email,
                Fecha_de_Creacion = fecha,
                Usuario_creador = model.Usuario_creador
            };
            if(cliente.Email == null){
                cliente.Email = "";
            };
            if(cliente.Usuario_creador == null){
                cliente.Usuario_creador = "admin";
            };
            _context.Clientes.Add(cliente);
            var result = await _context.SaveChangesAsync();
            return Ok(result);
        }

        // PUT api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Cambiar(int id, Cliente model)
        {
            if(model.Email == null){
                model.Email = "";
            };
            if(model.Usuario_creador == null){
                model.Usuario_creador = "admin";
            };
            if(model.Fecha_de_Creacion == null){
                model.Fecha_de_Creacion = DateTime.Now.ToString("dd/MM/yyyy");
            };
            _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null){
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ClienteExiste(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
