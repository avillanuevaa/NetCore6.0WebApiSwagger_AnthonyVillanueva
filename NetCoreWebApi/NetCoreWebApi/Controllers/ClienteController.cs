using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCoreWebApi.Models;
using System.Collections.Generic;
using System.Data;

namespace NetCoreWebApi.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase    {
        public readonly DataContext _context;

        public ClienteController(DataContext context) {
            _context = context;
        }


        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<Cliente>>>listar() {
            List<Cliente> lstCLientes;
            List<Cliente> lstEdad = new List<Cliente>();
            DateTime nacimiento = new DateTime();

            lstCLientes = (await _context.Cliente.ToListAsync());
            foreach (var item in lstCLientes)
            {
                nacimiento = item.fechaNac;
                item.edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                lstEdad.Add(item);
            }
            return Ok(lstEdad);
        }


        [HttpGet]
        [Route("listar3EdadMayores")]
        public async Task<ActionResult<List<Cliente>>> listar3EdadMayores()
        {
            List<Cliente> lstCLientes;
            List<Cliente> lstEdad = new List<Cliente>();
            DateTime nacimiento = new DateTime();


            lstCLientes = (await _context.Cliente.ToListAsync());     
            foreach (var item in lstCLientes)
            {
                nacimiento = item.fechaNac;
                item.edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;    
                lstEdad.Add(item);
            }
             IEnumerable<Cliente> lstUltimos = (from cliente in lstEdad orderby cliente.edad descending select  cliente).Take(3);
            return Ok(lstUltimos);
        }



        [HttpGet("{id}")]       
        public async Task<ActionResult<List<Cliente>>> listarxid(int id)
        {
            DateTime nacimiento = new DateTime();
            var clteFind = await _context.Cliente.FindAsync(id);
            Cliente cltEdad = new Cliente();


            if (clteFind == null) 
            return BadRequest("Dato No encontrado");

            cltEdad = clteFind;
            nacimiento = clteFind.fechaNac;
            cltEdad.edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

            return Ok(cltEdad);
        }


        [HttpPost]
        public async Task<ActionResult<List<Cliente>>> AddCliente(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
           
            return Ok(await _context.Cliente.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Cliente>>> UpdateCliente(Cliente request)
        {
            var dbClte = await _context.Cliente.FindAsync(request.id);
            if (dbClte == null)
                return BadRequest("Dato No encontrado");
            dbClte.nombres = request.nombres;
            dbClte.apellidos = request.apellidos;
            dbClte.fechaNac = request.fechaNac;

            await _context.SaveChangesAsync();
            return Ok(await _context.Cliente.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cliente>>> DeleteCliente(int id)
        {
            var dbClte = await _context.Cliente.FindAsync(id);
            if (dbClte == null)
                return BadRequest("Dato No encontrado");   
             _context.Cliente.Remove(dbClte);
            await _context.SaveChangesAsync();
            return Ok(await _context.Cliente.ToListAsync());
        }



    }
}
