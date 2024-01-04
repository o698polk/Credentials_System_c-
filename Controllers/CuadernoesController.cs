using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using carnetutelvt.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;

namespace carnetutelvt.Controllers
{
    public class CuadernoesController : Controller
    {
        private readonly rgutelvtContext _context;
        private readonly rgutelvtContext _contetxuser;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _conter;

        public CuadernoesController(rgutelvtContext context, rgutelvtContext contextuser, IWebHostEnvironment environment, IHttpContextAccessor conter)
        {
            _context = context;
            _environment = environment;
            _conter = conter;
            _contetxuser = contextuser;
        }

        // GET: Cuadernoes
        public async Task<IActionResult> Index(string? nameC,int? Valor)
        {
             
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 )
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {

                int? id = Convert.ToInt32(_conter.HttpContext.Session.GetInt32("Id"));
                var datosst = await _context.Cuaderno.Where(u => u.Iduser == id).ToListAsync();
                if (datosst == null)
                {
                    var cuaderno = new Cuaderno();
                    cuaderno.NameC = "Default";
                    cuaderno.Iduser = id;
                    _context.Add(cuaderno);
                    await _context.SaveChangesAsync();
                }
              

                var datoss = await _context.Cuaderno.Where(u => u.Iduser == id).ToListAsync();
                ViewData["NumDatos"] = datoss.Count();
                if (nameC== null && Valor == 10000)
                {
                    var datos = await _context.Cuaderno.Where(u => u.Iduser == id).ToListAsync();
                    
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }
                else if (nameC != null)
                {

                    var datos = await _context.Cuaderno.Where(u => u.Iduser == id).ToListAsync();
                    var query = from d in datos
                                where d.NameC.Contains(nameC)
                                select d;

                    ViewData["NumDatosV"] = datos.Count();
                    return View(query);
                }
                else if (Valor != null)
                {
                    var datos = _context.Cuaderno.Where(u => u.Iduser == id).Take(Convert.ToInt32(Valor));
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }
                else
                {
                    var datos = _context.Cuaderno.Where(u => u.Iduser == id).Take(Convert.ToInt32(10));
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }


            }
        }

        // GET: Cuadernoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Cuaderno == null)
                {
                    return NotFound();
                }

                var cuaderno = await _context.Cuaderno
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cuaderno == null)
                {
                    return NotFound();
                }

                return View(cuaderno);
            }
        }

        // GET: Cuadernoes/Create
        public IActionResult Create()
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {

                return View();
            }
        }

        // POST: Cuadernoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameC,InfoC")] Cuaderno cuaderno)
        {
           
                if (ModelState.IsValid)
            {
                cuaderno.Dateupdate = DateTime.Now;
                cuaderno.Datecreate = DateTime.Now;
                cuaderno.Iduser = Convert.ToInt32(_conter.HttpContext.Session.GetInt32("Id"));
                _context.Add(cuaderno);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cuaderno);
            
        }

        // GET: Cuadernoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Cuaderno == null)
                {
                    return NotFound();
                }

                var cuaderno = await _context.Cuaderno.FindAsync(id);
                if (cuaderno == null)
                {
                    return NotFound();
                }
                return View(cuaderno);
            }
        }

        // POST: Cuadernoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameC,InfoC,Dateupdate")] Cuaderno cuaderno)
        {
            if (id != cuaderno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var miObjeto = _context.Cuaderno.FirstOrDefault(x => x.Id == id);

                    miObjeto.NameC = cuaderno.NameC;
                    miObjeto.InfoC = cuaderno.InfoC;
                    miObjeto.Dateupdate = cuaderno.Dateupdate;
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuadernoExists(cuaderno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cuaderno);
        }

        // GET: Cuadernoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Cuaderno == null)
                {
                    return NotFound();
                }

                var cuaderno = await _context.Cuaderno
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cuaderno == null)
                {
                    return NotFound();
                }

                return View(cuaderno);
            }
        }

        // POST: Cuadernoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cuaderno == null)
            {
                return Problem("Entity set 'rgutelvtContext.Cuaderno'  is null.");
            }
            var cuaderno = await _context.Cuaderno.FindAsync(id);
            if (cuaderno != null)
            {
                _context.Cuaderno.Remove(cuaderno);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuadernoExists(int id)
        {
          return (_context.Cuaderno?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
