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
        private string lightlayoutBootstrap = "/lib/bootstrap/dist/css/bootstraplight.css";
       // private string darklayoutSite = "/css/sitedark.css";
       // private string lightlayoutSite = "/css/site.css";
        private Context _context;
        public NotizController(Context context)
        {
            _context = context;


        }
        //TODO "SASS" für Color.CSS erstellen anstatt einzelne CSS mit allem drin
        //TODO auslagern der im Controller nicht benötigten Methoden
        private void setzeStyle()
        {
            if (HttpContext.Session.GetString("StyleBootstrap") == null)
            {
                HttpContext.Session.SetString("StyleBootstrap", lightlayoutBootstrap);
            }

            ViewBag.StyleBootstrap =  HttpContext.Session.GetString("StyleBootstrap");

            
        }

        private void setzeSortierung()
        {
            if (HttpContext.Session.GetString("Sortierung") == null)
            {
                HttpContext.Session.SetString("Sortierung", SortierungsTyp.Wichtigkeit.ToString());
            }

            ViewBag.Sortierung = HttpContext.Session.GetString("Sortierung");


        }
        private void setzeFilter()
        {
            if (HttpContext.Session.GetString("FilterAbgeschlossen") == null)
            {
                HttpContext.Session.SetString("FilterAbgeschlossen", false.ToString());
            }

            ViewBag.FilterAbgeschlossen = HttpContext.Session.GetString("FilterAbgeschlossen");


        }
        public IActionResult WechsleStyle()
        {
            setzeStyle();
            HttpContext.Session.SetString("StyleBootstrap",
                HttpContext.Session.GetString("StyleBootstrap") == lightlayoutBootstrap
                    ? darklayoutBootstrap
                    : lightlayoutBootstrap);

            //TODO auslagern in eine Business Repo
            return  RedirectToAction("Liste");
        }
        public IActionResult WechsleSortierung(string Id)
        {

            setzeSortierung();
            if (Enum.IsDefined(typeof(SortierungsTyp), Id))
            {
                HttpContext.Session.SetString("Sortierung", Id);
            }
            //TODO auslagern in eine Business Repo
            return RedirectToAction("Liste");
        }

        public IActionResult WechsleFilter()
        {
            setzeFilter();
            HttpContext.Session.SetString("FilterAbgeschlossen",
                HttpContext.Session.GetString("FilterAbgeschlossen") == true.ToString()
                    ? false.ToString()
                    : true.ToString());
            //TODO auslagern in eine Business Repo
            // return ViewComponent("MyViewComponent", < arguments...>);
            return RedirectToAction("Liste");
        }

        public IActionResult Liste()
        {
            setzeStyle();
            setzeFilter();
            setzeSortierung();
            //TODO auslagern in eine Business Repo
            var x = _context.Notizen.ToList().Select(u => new NotizModelListe(u)).ToList();
            x = FilterListe(x);
            x = SortiereListe(x);
            return View(x);
        }

        private List<NotizModelListe> FilterListe(List<NotizModelListe> x)
        {
            if (HttpContext.Session.GetString("FilterAbgeschlossen") == true.ToString())
            {
                x = x.Where(c => !c.Abgeschlossen).ToList();
            }
            return x;
        }
        private List<NotizModelListe> SortiereListe(List<NotizModelListe> x)
        {
            if (HttpContext.Session.GetString("Sortierung") == SortierungsTyp.ErledigtBisDatum.ToString())
            {
                x = x.OrderBy(c => c.ErledigtBis).ToList();
            }
            else if (HttpContext.Session.GetString("Sortierung") == SortierungsTyp.Wichtigkeit.ToString())
            {
                x = x.OrderByDescending(c => c.Wichtigkeit).ToList();
            }
            else if (HttpContext.Session.GetString("Sortierung") == SortierungsTyp.Erstelldatum.ToString())
            {
                x = x.OrderBy(c => c.Erstelldatum).ToList();
            }
            return x;
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
                var zuErledigenbis = nm.ErledigtBisDatum;
                zuErledigenbis = zuErledigenbis.Add(nm.ErledigtBisZeit);
                var neueNotiz = new NotizDbModel
                {
                    Abgeschlossen = nm.Abgeschlossen,
                    Erstelldatum = DateTime.Now,
                    Beschreibung = nm.Beschreibung,
                    Wichtigkeit = nm.Wichtigkeit,
                    Titel = nm.Titel,
                    ErledigtBis = zuErledigenbis,
                    Id = nm.Id
                };
                //if (neueNotiz.Abgeschlossen)
                //{
                //    neueNotiz.AbgeschlossenDatum = DateTime.Now;
                //}
                //TODO auslagern in eine Business Repo
                _context.Notizen.Add(neueNotiz);
                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return View(nm);
        }

        public IActionResult Editieren(int Id)
        {
            setzeStyle();
            if (_context.Notizen.Any(c => c.Id == Id))
            {
                var x = new NotizModelEditieren(_context.Notizen.First(c => c.Id == Id));

                return View(x);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Editieren(NotizModelEditieren nm)
        {
            setzeStyle();
            if (ModelState.IsValid)
            {
                var zuErledigenbis = nm.ErledigtBisDatum;
                zuErledigenbis= zuErledigenbis.Add(nm.ErledigtBisZeit);
                var x = _context.Notizen.First(c => c.Id == nm.Id);
                //TODO auslagern in eine Business Repo
                //if (!nm.Abgeschlossen && x.Abgeschlossen)
                //{
                //    x.AbgeschlossenDatum = DateTime.Now;
                //}
                //if (nm.Abgeschlossen && !x.Abgeschlossen)
                //{
                //    x.AbgeschlossenDatum = null;
                //}
                x.Abgeschlossen = nm.Abgeschlossen;
                //x.Erstelldatum = nm.Erstelldatum;
                x.Beschreibung = nm.Beschreibung;
                x.Wichtigkeit = nm.Wichtigkeit;
                x.Titel = nm.Titel;
                x.ErledigtBis = zuErledigenbis;


                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return View(nm);
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
