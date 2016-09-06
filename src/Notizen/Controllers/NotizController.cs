using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var x = _context.Notizen.ToList().Select(u => new NotizModel(u)).ToList();
            return View(x);
        }

        public IActionResult Neu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Neu(NotizModel nm)
        {
            if (ModelState.IsValid)
            {
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
