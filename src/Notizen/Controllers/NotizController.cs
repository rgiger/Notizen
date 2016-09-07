using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notizen.DbModel;
using Notizen.Model;
using Notizen.Notizen;

namespace Notizen.Controllers
{
    public class NotizController : Controller
    {
        private Context _context;
        public NotizController(Context context)
        {
            _context = context;
        }

        public IActionResult Liste()
        {
            //TODO auslagern in eine Business Repo
            var x = _context.Notizen.ToList().Select(u => new NotizModelList(u)).ToList();
            return View(x);
        }

        public IActionResult Neu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Neu(NotizModelCreate nm)
        {
            if (ModelState.IsValid)
            {
                //TODO auslagern in eine Business Repo
                _context.Notizen.Add(new NotizDbModel
                {
                    Abgeschlossen = nm.Abgeschlossen,
                    Erstelldatum = nm.Erstelldatum,
                    Beschreibung = nm.Beschreibung,
                    Wichtigkeit = nm.Wichtigkeit,
                    Title = nm.Title,
                    ErledigtBis = nm.ErledigtBis,
                    Id = nm.Id
                });
                _context.SaveChanges();
                return RedirectToAction("Liste");
            }
            return View(nm);
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
