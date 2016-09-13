using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notizen.DbModel;
using Notizen.DbModel.Notizen;
using Notizen.Model;

namespace Notizen.Controllers
{
    public class NotizController : Controller
    {
        //TODO: Helper Klasse für Styles
        private string darklayoutBootstrap = "/lib/bootstrap/dist/css/bootstrapdark.css";
        private string lightlayoutBootstrap = "/lib/bootstrap/dist/css/bootstrap.css";
       // private string darklayoutSite = "/css/sitedark.css";
       // private string lightlayoutSite = "/css/site.css";
        private Context _context;
        public NotizController(Context context)
        {
            _context = context;


        }
        //TODO "SASS" für Color.CSS erstellen anstatt einzelne Klassen
        private void setzeStyle()
        {
            if (HttpContext.Session.GetString("StyleBootstrap") == null)
            {
                HttpContext.Session.SetString("StyleBootstrap", lightlayoutBootstrap);
            }

            ViewBag.StyleBootstrap =  HttpContext.Session.GetString("StyleBootstrap");

            
        }
        public IActionResult WechsleStyle()
        {
            
            setzeStyle();
            if (HttpContext.Session.GetString("StyleBootstrap") == null)
            {
                HttpContext.Session.SetString("StyleBootstrap", lightlayoutBootstrap);
            }
            HttpContext.Session.SetString("StyleBootstrap",
                HttpContext.Session.GetString("StyleBootstrap") == lightlayoutBootstrap
                    ? darklayoutBootstrap
                    : lightlayoutBootstrap);
            //TODO auslagern in eine Business Repo
            // return ViewComponent("MyViewComponent", < arguments...>);
            return  RedirectToAction("Liste");
        }

        public IActionResult Liste()
        {
            setzeStyle();

            //TODO auslagern in eine Business Repo
            var x = _context.Notizen.ToList().Select(u => new NotizModelListe(u)).ToList();
            return View(x);
        }
        public IActionResult Neu(Guid id)
        {
            setzeStyle();

            return View();
        }
        [HttpPost]
        public IActionResult Neu(NotizModelErstellen nm)
        {
            setzeStyle();
            if (ModelState.IsValid)
            {
                //TODO auslagern in eine Business Repo
                _context.Notizen.Add(new NotizDbModel
                {
                    Abgeschlossen = nm.Abgeschlossen,
                    Erstelldatum = nm.Erstelldatum,
                    Beschreibung = nm.Beschreibung,
                    Wichtigkeit = nm.Wichtigkeit,
                    Titel = nm.Titel,
                    ErledigtBis = nm.ErledigtBis,
                    Id = nm.Id
                });
                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return BadRequest(nm);
        }

        public IActionResult Editieren(int Id)
        {
            setzeStyle();
            var x = new NotizModelEditieren(_context.Notizen.First(c => c.Id == Id));

            return View(x);
        }
        [HttpPost]
        public IActionResult Editieren(NotizModelEditieren nm)
        {
            setzeStyle();
            if (ModelState.IsValid)
            {
                var x = _context.Notizen.First(c => c.Id == nm.Id);
                //TODO auslagern in eine Business Repo
                x.Abgeschlossen = nm.Abgeschlossen;
                    x.Erstelldatum = nm.Erstelldatum;
                x.Beschreibung = nm.Beschreibung;
                x.Wichtigkeit = nm.Wichtigkeit;
                x.Titel = nm.Titel;
                x.ErledigtBis = nm.ErledigtBis;
               
                
                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return BadRequest(nm);
        }


        //TODO nicht in Notiz controller belassen
        public IActionResult Error()
        {
            setzeStyle();
            return View();
        }

        public IActionResult Loeschen(int Id)
        {

            var x = _context.Notizen.First(c => c.Id == Id);
            _context.Notizen.Remove(x);
            _context.SaveChanges();
            return RedirectToAction("Liste");
        }

    }
}
