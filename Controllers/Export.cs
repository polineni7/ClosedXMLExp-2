using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClosedXMLExp.Controllers
{
    public class Export : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
