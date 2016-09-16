using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notizen.DbModel;
using Notizen.DbModel.Notizen;
using Notizen.Model;

namespace Notizen.Controllers
{
    public class NotizController : Controller
    {
        private const string DarklayoutBootstrap = "/lib/bootstrap/dist/css/bootstrapdark.css";
        private const string LightlayoutBootstrap = "/lib/bootstrap/dist/css/bootstraplight.css";
        private const string Stylesheet = "Style";
        private const string Sortierung = "Sortierung";
        private const string FilterAbgeschlossen = "FilterAbgeschlossen";
        private readonly ApplicationDbContext _context;

        public NotizController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void SetzeStyle()
        {
            if (HttpContext.Session.GetString(Stylesheet) == null)
                HttpContext.Session.SetString(Stylesheet, LightlayoutBootstrap);
            ViewBag.StyleBootstrap = HttpContext.Session.GetString(Stylesheet);
        }

        private void SetzeSortierung()
        {
            if (HttpContext.Session.GetString(Sortierung) == null)
                HttpContext.Session.SetString(Sortierung, SortierungsTyp.Wichtigkeit.ToString());
            ViewBag.Sortierung = HttpContext.Session.GetString(Sortierung);
        }

        private void SetzeFilter()
        {
            if (HttpContext.Session.GetString(FilterAbgeschlossen) == null)
                HttpContext.Session.SetString(FilterAbgeschlossen, false.ToString());
            ViewBag.FilterAbgeschlossen = HttpContext.Session.GetString(FilterAbgeschlossen);
        }

        public IActionResult WechsleStyle()
        {
            SetzeStyle();
            HttpContext.Session.SetString(Stylesheet,
                HttpContext.Session.GetString(Stylesheet) == LightlayoutBootstrap
                    ? DarklayoutBootstrap
                    : LightlayoutBootstrap);
            return RedirectToAction("Liste");
        }

        public IActionResult WechsleSortierung(string id)
        {
            SetzeSortierung();
            if (Enum.IsDefined(typeof(SortierungsTyp), id))
                HttpContext.Session.SetString(Sortierung, id);
            return RedirectToAction("Liste");
        }

        public IActionResult WechsleFilter()
        {
            SetzeFilter();
            HttpContext.Session.SetString(FilterAbgeschlossen,
                HttpContext.Session.GetString(FilterAbgeschlossen) == true.ToString()
                    ? false.ToString()
                    : true.ToString());
            return RedirectToAction("Liste");
        }

        public IActionResult Liste()
        {
            SetzeStyle();
            SetzeFilter();
            SetzeSortierung();
            var x = _context.Notizen.ToList().Select(u => new NotizModelListe(u)).ToList();
            x = FilterListe(x);
            x = SortiereListe(x);
            return View(x);
        }

        private List<NotizModelListe> FilterListe(List<NotizModelListe> x)
        {
            if (HttpContext.Session.GetString(FilterAbgeschlossen) == true.ToString())
                x = x.Where(c => !c.Abgeschlossen).ToList();
            return x;
        }

        private List<NotizModelListe> SortiereListe(List<NotizModelListe> x)
        {
            if (HttpContext.Session.GetString(Sortierung) == SortierungsTyp.ErledigtBisDatum.ToString())
                x = x.OrderBy(c => c.ErledigtBis).ToList();
            else if (HttpContext.Session.GetString(Sortierung) == SortierungsTyp.Wichtigkeit.ToString())
                x = x.OrderByDescending(c => c.Wichtigkeit).ToList();
            else if (HttpContext.Session.GetString(Sortierung) == SortierungsTyp.Erstelldatum.ToString())
                x = x.OrderBy(c => c.Erstelldatum).ToList();
            return x;
        }

        public IActionResult Neu()
        {
            SetzeStyle();
            return View();
        }

        [HttpPost]
        public IActionResult Neu(NotizModelErstellen nm)
        {
            SetzeStyle();
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
                _context.Notizen.Add(neueNotiz);
                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return View(nm);
        }

        public IActionResult Editieren(int id)
        {
            SetzeStyle();
            if (_context.Notizen.Any(c => c.Id == id))
            {
                var x = new NotizModelEditieren(_context.Notizen.First(c => c.Id == id));

                return View(x);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Editieren(NotizModelEditieren nm)
        {
            SetzeStyle();
            if (ModelState.IsValid)
            {
                var zuErledigenbis = nm.ErledigtBisDatum;
                zuErledigenbis = zuErledigenbis.Add(nm.ErledigtBisZeit);
                var x = _context.Notizen.First(c => c.Id == nm.Id);
                x.Abgeschlossen = nm.Abgeschlossen;
                x.Beschreibung = nm.Beschreibung;
                x.Wichtigkeit = nm.Wichtigkeit;
                x.Titel = nm.Titel;
                x.ErledigtBis = zuErledigenbis;


                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return View(nm);
        }

        public IActionResult Error()
        {
            SetzeStyle();
            return View();
        }

        public IActionResult Loeschen(int id)
        {
            var x = _context.Notizen.First(c => c.Id == id);
            _context.Notizen.Remove(x);
            _context.SaveChanges();
            return RedirectToAction("Liste");
        }
    }
}