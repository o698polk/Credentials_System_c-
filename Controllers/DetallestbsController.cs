using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using carnetutelvt.Models;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Plugins;
using System.Diagnostics.Metrics;

namespace carnetutelvt.Controllers
{
    public class DetallestbsController : Controller
    {
        private readonly rgutelvtContext _context;
		private readonly rgutelvtContext _contetxuser;
		private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _conter;
        public DetallestbsController(rgutelvtContext context, rgutelvtContext contextuser, IWebHostEnvironment environment, IHttpContextAccessor conter)
        {
            _context = context;
            _environment = environment;
            _conter = conter;
            _contetxuser = contextuser;

		}

        // GET: Detallestbs
        public async Task<IActionResult> Index(string? Ci, int? Valor)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")
            {
                return RedirectToAction( "Login", "Usertbs");
            }
            else
            {
				ViewData["NumDatos"] = await _context.Usertbs.CountAsync();
				if (Ci == null && Valor == 10000)
				{
					var rgutelvtContext = _context.Detallestbs.Include(d => d.IduserNavigation);
                    var datos = await rgutelvtContext.ToListAsync();
					ViewData["NumDatosV"] = datos.Count();
					return View(datos);
				}
				else if(Ci!=null)
				{
                  
                    var datos = await _context.Detallestbs.Where(u => u.Ci.Contains(Ci)).ToListAsync();
					ViewData["NumDatosV"] = datos.Count();
					return View(datos);
				}
                else if (Valor!=null)
                {
					var datos = _context.Detallestbs.Take(Convert.ToInt32(Valor));
					ViewData["NumDatosV"] = datos.Count();
					return View(datos);
				}
                else
                {
					var datos = _context.Detallestbs.OrderByDescending(x => x.Iduser).Take(Convert.ToInt32(10));
					ViewData["NumDatosV"] = datos.Count();
					return View(datos);
				}

				
            }
        }

        // GET: Detallestbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Detallestbs == null)
                {
                    return NotFound();
                }

                var detallestb = await _context.Detallestbs
                    .Include(d => d.IduserNavigation)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (detallestb == null)
                {
                    return NotFound();
                }

                return View(detallestb);
            }
        }
        // GET: Detallestbs/Estudiantes/5
        public async Task<IActionResult> Estudiantes(int? id)
        {
            
                if (id == null || _context.Detallestbs == null)
                {
                    return NotFound();
                }

                var detallestb = await _context.Detallestbs
                    .Include(d => d.IduserNavigation)
                    .FirstOrDefaultAsync(m => m.Iduser == id);
             var usertbdd = await _contetxuser.Usertbs.FirstOrDefaultAsync(m => m.Id == id);
			if (detallestb == null)
                {
                    return NotFound();
                }
            ViewData["type"] = usertbdd.Typess;

				return View(detallestb);
            
        }

        // GET: Detallestbs/Create
        public IActionResult Create()
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id");
                return View();
            }
        }

        // POST: Detallestbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Surnames,Specialty,Faculty,Ci,Iduser,Bloodtype")] Detallestb detallestb, IFormFile Imgcarnett)
        {
            if (detallestb.Iduser ==0)
            {
                return NotFound();
            }

            detallestb.Fullname = detallestb.Fullname.ToUpper();
                detallestb.Surnames = detallestb.Surnames.ToUpper();
                detallestb.Dateupdate= DateTime.Now;
                detallestb.Datecreate= DateTime.Now;

                if (Imgcarnett != null)
                {
                        detallestb.Imgcarnet = await subirimg(Imgcarnett);
                }


                _context.Add(detallestb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id", detallestb.Iduser);
            return View(detallestb);
        }

        // GET: Detallestbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Detallestbs == null)
                {
                    return NotFound();
                }

                var detallestb = await _context.Detallestbs.FindAsync(id);
                if (detallestb == null)
                {
                    return NotFound();
                }
                ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id", detallestb.Iduser);
                ViewData["Datecreate"] = detallestb.Datecreate;
                ViewData["Imgcarnet"] = detallestb.Imgcarnet;
                return View(detallestb);
            }
        }

        // POST: Detallestbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fullname,Surnames,Specialty,Faculty,Ci,Imgcarnet,Bloodtype ")] Detallestb detallestb, IFormFile Imgcarnett)
        {
         
            if (id != detallestb.Id)
            {
                return NotFound();
            }

          
                try
                {
                    var miObjeto = _context.Detallestbs.FirstOrDefault(x => x.Id == id);

             
                    miObjeto.Fullname = detallestb.Fullname.ToUpper();
                    miObjeto.Surnames = detallestb.Surnames.ToUpper();
                    miObjeto.Specialty = detallestb.Specialty;
                    miObjeto.Faculty = detallestb.Faculty;
                    miObjeto.Ci = detallestb.Ci;
                   miObjeto.Bloodtype = detallestb.Bloodtype;

                    miObjeto.Dateupdate = DateTime.Now;

                    


                    if (Imgcarnett != null)
                    {
                        if (detallestb.Imgcarnet != null)
                        {
                            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + detallestb.Imgcarnet);
                            System.IO.File.Delete(uploads);
                        }

                        miObjeto.Imgcarnet = await subirimg(Imgcarnett);
					}
                   
                
                
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallestbExists(detallestb.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
         
            ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id", detallestb.Iduser);
            return View(detallestb);
        }

        
        // Funcion de subir imagen
        public async Task<string> subirimg(IFormFile archivo)
        {
             var fileName="";
                if (archivo != null && archivo.Length > 0)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                     fileName = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                       await archivo.CopyToAsync(fileStream);
                    }

                     
                }



            return Path.Combine( fileName); ;
        }
        // GET: Detallestbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")
            {
                return RedirectToAction("Login", "Usertbs");
            }
            else
            {
                if (id == null || _context.Detallestbs == null)
                {
                    return NotFound();
                }

                var detallestb = await _context.Detallestbs
                    .Include(d => d.IduserNavigation)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (detallestb == null)
                {
                    return NotFound();
                }

                return View(detallestb);
            }
        }

        // POST: Detallestbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Detallestbs == null)
            {
                return Problem("Entity set 'rgutelvtContext.Detallestbs'  is null.");
            }
            var detallestb = await _context.Detallestbs.FindAsync(id);
            if (detallestb != null)
            {
                if (detallestb.Imgcarnet != null)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + detallestb.Imgcarnet);
                    System.IO.File.Delete(uploads);
                }
                _context.Detallestbs.Remove(detallestb);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetallestbExists(int id)
        {
          return _context.Detallestbs.Any(e => e.Id == id);
        }
    }
}
