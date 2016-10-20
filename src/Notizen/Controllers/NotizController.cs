using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notizen.DbModel.Notizen;
using Notizen.Model;
using Notizen.Repository;

namespace Notizen.Controllers
{
    public class NotizController : Controller
    {
        //private const string DarklayoutBootstrap = "/lib/bootstrap/dist/css/bootstrapdark.css";
        //private const string LightlayoutBootstrap = "/lib/bootstrap/dist/css/bootstraplight.css";
        private const string Stylesheet = "Style";
        private const string Sortierung = "Sortierung";
        private const string FilterAbgeschlossen = "FilterAbgeschlossen";
        //private readonly ApplicationDbContext _context;

        private readonly NotizRepository _notizRepository;

        public NotizController(ApplicationDbContext context)
        {
            _notizRepository = new NotizRepository(context);

        }
        
        private void SetzeStyle()
        {
            if (HttpContext.Session.GetString(Stylesheet) == null)
                HttpContext.Session.SetString(Stylesheet, "true");
            ViewBag.StyleBootstrap = HttpContext.Session.GetString("false");
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
            HttpContext.Session.SetString(Stylesheet, Convert.ToBoolean(HttpContext.Session.GetString(Stylesheet))
                    ? "false"
                    : "true");
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
            return View(_notizRepository.GetListe(Convert.ToBoolean(HttpContext.Session.GetString(FilterAbgeschlossen)), HttpContext.Session.GetString(Sortierung)));
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
                _notizRepository.FuegeHinzu(nm);
                return RedirectToAction("Liste");
            }
            return View(nm);
        }

        public IActionResult Editieren(int id)
        {
            SetzeStyle();
            return _notizRepository.Existiert(id)
                ? (IActionResult) View(_notizRepository.GetAsNotizModelEditieren(id))
                : NotFound();
        }

        [HttpPost]
        public IActionResult Editieren(NotizModelEditieren nm)
        {
            SetzeStyle();
            if (ModelState.IsValid)
            {
                _notizRepository.Aktualisiere(nm);
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
            _notizRepository.Loesche(id);
            return RedirectToAction("Liste");
        }
    }
}