using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using carnetutelvt.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using carnetutelvt.Controllers;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace carnetutelvt.Controllers
{
    public class UsertbsController : Controller
    {
        private readonly rgutelvtContext _context;
        private readonly rgutelvtContext _contextdeta;
        private const string rp0 = "0",rp1="1",dominio="";
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _conter;
        private IConfiguration _configuration;


        public UsertbsController(rgutelvtContext context, rgutelvtContext contextdeta, IWebHostEnvironment env, IHttpContextAccessor conter, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _conter = conter;
            _contextdeta = contextdeta;
            _configuration= configuration ;

        }

        // GET: Usertbs
        public async Task<IActionResult> Index(string? Email, int? Valor)
        {
           
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol")!="1")

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {
                ViewData["NumDatos"] = await _context.Usertbs.CountAsync();
                if (Email == null && Valor==10000)
				{
                    var datos = await _context.Usertbs.ToListAsync();
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }
                else if (Valor != null)
                {
                    var datos = _context.Usertbs.Take(Convert.ToInt32(Valor));
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }
                else if (Email != null )
				{
                    var datos = await _context.Usertbs.Where(u => u.Email.Contains(Email)).ToListAsync();
                    ViewData["NumDatosV"] = datos.Count();
                    return View(datos);
                }
                else
                {
					var datos =  _context.Usertbs.OrderByDescending(x => x.Id).Take(Convert.ToInt32(10));
					ViewData["NumDatosV"] = datos.Count();
					return View(datos);
				}
				
            }
           
        }

        public  IActionResult Chatbot()
        {

            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 )

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {
            
            
                    return View();
             
                
            }

        }

        // Vista para editar Foto Carnet y Contraseña
      

        public async Task<IActionResult> Modifuser(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 )

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {

                if (id == 0 )
                {
                    return NotFound();
                }

                var usertb = await _context.Usertbs.FindAsync(id);
                if (usertb == null)
                {
                    return NotFound();
                }
                ViewData["Datecreate"] = usertb.Datecreate;
                return View(usertb);
            }
        }
        //Editar Contraseña usuarios


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPass(int id,[Bind("Id,Passwords")] Usertb usertb)
        {
            if (usertb.Passwords == null)
            {
                return NotFound();
            }
                if (id != usertb.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var miObjeto = _context.Usertbs.FirstOrDefault(x => x.Id == id);
                        if (!string.IsNullOrEmpty(usertb.Passwords))
                        {
                            miObjeto.Passwords = EncrypData(usertb.Passwords);
                        }

                   
                        miObjeto.Dateupdate = DateTime.Now;

                        await _context.SaveChangesAsync();

                    ViewData["Error"] = rp1;
                    return RedirectToAction(nameof(Tablero));
                }
                    catch (DbUpdateConcurrencyException)
                    {
                    ViewData["Error"] = rp0;
                    return RedirectToAction(nameof(Tablero));

                }

            }
            ViewData["Error"] = rp0;
            return RedirectToAction(nameof(Tablero));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFotoPerfil(int id,[Bind("Id")] Detallestb detallestb,  IFormFile Imgcarnett)
        {
            var dell = new DetallestbsController(_contextdeta, _contextdeta, _env, _conter);
            if (id==0)
            {
                return NotFound();
            }


            try
            {
                var miObjeto = _contextdeta.Detallestbs.FirstOrDefault(x => x.Iduser == id);


                if (miObjeto != null)
                {
                    miObjeto.Dateupdate = DateTime.Now;
             
                if (Imgcarnett != null)
                {
                    if (miObjeto.Imgcarnet != null)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "uploads\\" + miObjeto.Imgcarnet);
                        System.IO.File.Delete(uploads);
                    }

                    miObjeto.Imgcarnet = await dell.subirimg(Imgcarnett);
                }
                    await _context.SaveChangesAsync();
                }



                ViewData["Error"] = rp1;
                return RedirectToAction(nameof(Tablero));
            }
            catch (DbUpdateConcurrencyException)
            {
                ViewData["Error"] = rp1;
                return RedirectToAction(nameof(Tablero));
            }


           

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTypeBlo(int id, [Bind("Id")] Detallestb detallestb, string Bloodtype)
        {
            var dell = new DetallestbsController(_contextdeta, _contextdeta, _env, _conter);
            if (id == 0)
            {
                return NotFound();
            }


            try
            {
                var miObjeto = _contextdeta.Detallestbs.FirstOrDefault(x => x.Iduser == id);


                if (miObjeto != null)
                {
                    miObjeto.Dateupdate = DateTime.Now;

                   
                   if (Bloodtype != null)
                    {
                        miObjeto.Bloodtype = Bloodtype.ToString();
                    }
                    await _context.SaveChangesAsync();
                }



                ViewData["Error"] = rp1;
                return RedirectToAction(nameof(Tablero));
            }
            catch (DbUpdateConcurrencyException)
            {
                ViewData["Error"] = rp1;
                return RedirectToAction(nameof(Tablero));
            }




        }







        //Generar Codigo QR


        public void GenerarQr(string webtex)
        {
            QRCodeEncoder code = new QRCodeEncoder();
            Bitmap img = code.Encode(webtex);
            System.Drawing.Image Qr = (System.Drawing.Image)img;
            using (MemoryStream mem = new MemoryStream())
            {
                Qr.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imgbyte = mem.ToArray();
                ViewData["qr"] = "data:image/git;base64,"+Convert.ToBase64String(imgbyte);
            }
        }


        // GET: Usertbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {

                if (_conter.HttpContext.Session.GetInt32("Id") != null)

                {
                    var detallestb = new Detallestb();
                    var userdato = new Usertb();
                    if (_conter.HttpContext.Session.GetInt32("Id") > 0)
                    {
                      
                            int? ids = id;
                        
                        

                        detallestb = _contextdeta.Detallestbs.Where(e => e.Iduser == ids).FirstOrDefault();
                        userdato = _context.Usertbs.Where(e => e.Id == ids).FirstOrDefault();
                        if (detallestb == null)
                        {
                            return NotFound();
                        }
                    }

                    ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id", detallestb.Iduser);
                    ViewData["Datecreate"] = detallestb.Datecreate;
                    ViewData["Imgcarnet"] = detallestb.Imgcarnet;

                    ViewData["Fullname"] = detallestb.Fullname;
                    ViewData["Surnames"] = detallestb.Surnames;

                    ViewData["Specialty"] = detallestb.Specialty;
                    ViewData["Bloodtype"] = detallestb.Bloodtype;
                    ViewData["Faculty"] = detallestb.Faculty;
                    ViewData["Ci"] = detallestb.Ci;
                    ViewData["email"] = userdato.Email;
                    ViewData["types"] = userdato.Typess;
                    //  GenerarQr("https://localhost:7288/Detallestbs/Estudiantes/"+detallestb.Iduser);
                    GenerarQr(_configuration["dominio"] + "/Detallestbs/Estudiantes/" + detallestb.Iduser);
                    return View(detallestb);

                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }
        }
      

        // GET: Usertbs/Create
        public IActionResult Create()
        {

            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {


                return View();
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (_conter.HttpContext.Session.GetInt32("Id") != null )

            {

                return RedirectToAction(nameof(Tablero));

            }
            else
            {

                ViewData["Error"] = "";
                return View();
            }
        }
   
        public IActionResult Logout()
        {

            _conter.HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Tablero()
        {

            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0)

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {

                if (_conter.HttpContext.Session.GetInt32("Id") != null)

                {
                    var detallestb = new Detallestb();
                    if (_conter.HttpContext.Session.GetInt32("Id") > 0)
                    {
                        int ids = Convert.ToInt32(_conter.HttpContext.Session.GetInt32("Id"));

                        detallestb = _contextdeta.Detallestbs.Where(e => e.Iduser == ids).FirstOrDefault();
                        if (detallestb == null)
                        {
                            return NotFound();
                        }
                    }

                    ViewData["Iduser"] = new SelectList(_context.Usertbs, "Id", "Id", detallestb.Iduser);
                    ViewData["Datecreate"] = detallestb.Datecreate;
                    ViewData["Imgcarnet"] = detallestb.Imgcarnet;
                 
                    ViewData["Fullname"] = detallestb.Fullname;
                    ViewData["Surnames"] = detallestb.Surnames;
                  
                    ViewData["Specialty"] = detallestb.Specialty;
                    ViewData["Bloodtype"] = detallestb.Bloodtype;
                    ViewData["Faculty"] = detallestb.Faculty;
                    ViewData["Ci"] = detallestb.Ci;
                    ViewData["email"] = _conter.HttpContext.Session.GetString("email");
                    ViewData["types"] = _conter.HttpContext.Session.GetString("Types");
                    //  GenerarQr("https://localhost:7288/Detallestbs/Estudiantes/"+detallestb.Iduser);
                    GenerarQr(_configuration["dominio"] + "/Detallestbs/Estudiantes/"+detallestb.Iduser);
                    return View(detallestb);

                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }
        }
        // VERIFICAR SI UN STRING ES ENCRIPTADO
        public bool IsEncrypted(string inputString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hash = sha256.ComputeHash(bytes);
            string hashedString = Encoding.UTF8.GetString(hash);
            return (hashedString == inputString);
        }
        // Encriptar datos
        public string EncrypData(string pass)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(pass);
            HashAlgorithm algorithm = CryptoConfig.CreateFromName("SHA256") as HashAlgorithm;
            byte[] hash = algorithm.ComputeHash(passwordBytes);

            return BitConverter.ToString(hash).Replace("-", "");
        }

        // Enviar Correos
 
        
        public dynamic EnviarCorreo(string receptor, string asunto, string texto)
        {
          
            try
            {
                  HelperMail helpermail= new HelperMail(_configuration) ;
                 string mensajefinal = "<h1>UTELVT, VERIFICA TU CUENTA DE USUARURIO,PARA GENERAR TU CARNET/ <h1/><h4> Click al Enlace:" + texto + " <h4/>"
                                    ;
                 helpermail.SendMail(receptor, asunto, mensajefinal);
                ViewData["MENSAJE"] = "Mensaje enviado a '" + receptor + "'";
             
            }
            catch (Exception ex)
            {
                return new
                {
                    response = "error: " + ex
                };
            }



            return new
            {
                response = "enviado"
            };
        }

        // POST: Usertbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,Passwords,Typess")] Usertb usertb)
        {
            if (usertb.Email != null && usertb.Passwords != null && usertb.Typess != null)
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Usertbs.Where(u => u.Email == usertb.Email).FirstOrDefault();
                    if (user != null)
                    {
                        ViewData["Error"] = rp0;
                        return View();
                    }
                    else
                    {

                        Random rnd = new Random();
                        long ran = rnd.Next(5000870, 8000008);
                        usertb.Passwords = EncrypData(usertb.Passwords);
                        usertb.Datecreate = DateTime.Now;
                        usertb.Dateupdate = DateTime.Now;
                        usertb.Numberverify = ran.ToString();
                        usertb.Verifyuser = 0;
                        usertb.Rol = "0";
                       
                        Detallestb dll = new Detallestb();
                        _context.Add(usertb);

                        await _context.SaveChangesAsync();
                        var usernw = _context.Usertbs.Where(u => u.Email == usertb.Email).FirstOrDefault();
                        if (usernw != null)
                        {
                           
                            dll.Iduser = usernw.Id;
                            dll.Datecreate = DateTime.Now;
                            dll.Dateupdate = DateTime.Now;
                            _contextdeta.Add(dll);
                            await _contextdeta.SaveChangesAsync();
                             EnviarCorreo(usertb.Email, "UTELVT CARNET, VERIFICAR", _configuration["dominio"] + "/Usertbs/Verificar/" + usertb.Numberverify);
                           // EnviarCorreo(usertb.Email, "UTELVT CARNET, VERIFICAR", "https://localhost:7288/Usertbs/Verificar/" + usertb.Numberverify);
                        
                        }
                        ViewData["Error"] = rp1;
                        return View();
                        //  return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["Error"] = rp0;
            return View();
        }

        //Funcion de verificacion
        public async Task<IActionResult> Verificar(int? id)
        {

            if (id == null || _context.Usertbs == null)
            {
                return NotFound();
            }
            else
            {

            

            var usertb = await _context.Usertbs.FirstOrDefaultAsync(m => m.Numberverify == id.ToString());
            
            if (usertb == null)
            {
                return NotFound();
            }
            else
            {
                    usertb.Verifyuser =1 ;
                    usertb.Dateupdate = DateTime.Now;
                    _context.Update(usertb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
             }
               
            }
         

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Passwords,Rol,Typess")] Usertb usertb)
        {
            
            if (ModelState.IsValid)
            {
                var user = _context.Usertbs.Where(u => u.Email == usertb.Email).FirstOrDefault();
                if (user != null)
                {
                    ViewData["Error"] = rp0;
                    return View();
                }
                else
                {



                    Random rnd = new Random();
                    long ran = rnd.Next(5000870,8000008);
                    usertb.Passwords = EncrypData(usertb.Passwords);
                    usertb.Datecreate = DateTime.Now;
                    usertb.Dateupdate = DateTime.Now;
                    usertb.Numberverify = ran.ToString();
                    usertb.Verifyuser = 1;
                    Detallestb dll = new Detallestb();
                   
                    _context.Add(usertb);
                    await _context.SaveChangesAsync();

                    var usernw = _context.Usertbs.Where(u => u.Email == usertb.Email).FirstOrDefault();
                    if (usernw != null)
                    {
                        dll.Iduser = usernw.Id;
                        dll.Datecreate = DateTime.Now;
                        dll.Dateupdate = DateTime.Now;
                        _contextdeta.Add(dll);
                        await _contextdeta.SaveChangesAsync();
                    }
                   
                    ViewData["Error"] = rp1;
                    //return View();
                     return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Usertbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {

                if (id == null || _context.Usertbs == null)
                {
                    return NotFound();
                }

                var usertb = await _context.Usertbs.FindAsync(id);
                if (usertb == null)
                {
                    return NotFound();
                }
                ViewData["Datecreate"] = usertb.Datecreate;
                return View(usertb);
            }
        }

        // POST: Usertbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Passwords,Verifyuser,Rol,Typess")] Usertb usertb)
        {
            
            if (id != usertb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var miObjeto = _context.Usertbs.FirstOrDefault(x => x.Id == id);
                    if (!string.IsNullOrEmpty(usertb.Passwords))
                       {
                        miObjeto.Passwords = EncrypData(usertb.Passwords);
                    }

                    if (!string.IsNullOrEmpty(usertb.Email))
                    {
                        miObjeto.Email = usertb.Email;
                    }

                   
                    miObjeto.Verifyuser = usertb.Verifyuser;
                   
                    miObjeto.Rol = usertb.Rol;
					miObjeto.Typess = usertb.Typess;

					miObjeto.Dateupdate = DateTime.Now;
                      
                       await _context.SaveChangesAsync();
                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsertbExists(usertb.Id))
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
            return View(usertb);
        }
       
        //lOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Passwords")] Usertb userdt)
        {
           
            if (userdt.Email == null || userdt.Passwords == null)
            {
                ViewData["Error"] = rp0;
                return View();
            }
           
                if (ModelState.IsValid)
            {
                try
                {
                    userdt.Passwords = EncrypData(userdt.Passwords);
                    var user = _context.Usertbs.Where(u => u.Email == userdt.Email && u.Passwords == userdt.Passwords).FirstOrDefault();
                    if (user == null)
                    {
                        ViewData["Error"] = rp0;
                        return View();
                    }
                    else
                    {
                        if (user.Verifyuser==1)
                        { string tipos = "0";
                            string roles = "0";
                                if (user.Typess != null) {
                                tipos = user.Typess;
                                }
                            if (user.Rol != null)
                            {
                                roles = user.Rol;
                            }
                            var detall = _contextdeta.Detallestbs.Where(u => u.Iduser == user.Id ).FirstOrDefault();
                            string[] cadena = user.Email.Split("@");
                        _conter.HttpContext.Session.SetString("name", cadena[0]);
                        _conter.HttpContext.Session.SetString("email", user.Email);
                        _conter.HttpContext.Session.SetInt32("Id", user.Id);
                        _conter.HttpContext.Session.SetString("Rol",roles);
                        _conter.HttpContext.Session.SetString("Types",tipos);
                        _conter.HttpContext.Session.SetString("imgperfil", "/uploads\\\\" + detall.Imgcarnet);   
                       
                            return RedirectToAction(nameof(Tablero));
                        }
                        else
                        {
                             EnviarCorreo(user.Email, "UTELVT CARNET, VERIFICAR", _configuration["dominio"] + "/Usertbs/Verificar/" + user.Numberverify);
                           // EnviarCorreo(user.Email, "UTELVT CARNET, VERIFICAR", "https://localhost:7288/Usertbs/Verificar/" + user.Numberverify);
                            ViewData["Error"] = rp0;
                            return View();
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserdtExists(userdt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(userdt);
        }
        private bool UserdtExists(int id)
        {
            return _context.Usertbs.Any(e => e.Id == id);
        }
        // GET: Usertbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_conter.HttpContext.Session.GetInt32("Id") == null || _conter.HttpContext.Session.GetInt32("Id") < 0 || _conter.HttpContext.Session.GetString("Rol") != "1")

            {

                return RedirectToAction(nameof(Login));

            }
            else
            {

                if (id == null || _context.Usertbs == null)
                {
                    return NotFound();
                }

                var usertb = await _context.Usertbs
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (usertb == null)
                {
                    return NotFound();
                }

                return View(usertb);
            }
        }

        // POST: Usertbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          
            if (_context.Usertbs == null)
            {
                return Problem("Entity set 'rgutelvtContext.Usertbs'  is null.");
            }
            var detallestb =  _contextdeta.Detallestbs.Where(e=> e.Iduser==id).FirstOrDefault();
            if (detallestb != null)
            {
     
                        if (detallestb.Imgcarnet != null)
                        {
                            var uploads = Path.Combine(_env.WebRootPath, "uploads\\" + detallestb.Imgcarnet);
                            System.IO.File.Delete(uploads);
                        }

                       
                 
                    _contextdeta.Detallestbs.Remove(detallestb);
            }
            var usertb = await _context.Usertbs.FindAsync(id);
            if (usertb != null)
            {
               
                _context.Usertbs.Remove(usertb);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

       
        private bool UsertbExists(int id)
        {
          return _context.Usertbs.Any(e => e.Id == id);
        }
    }
}
