using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notizen.Model;

namespace Notizen.Controllers
{
    public class NotizController : Controller
    {
        public IActionResult Liste()
        {
            var x = new Collection<NotizModel>();
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
