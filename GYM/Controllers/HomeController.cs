using System.Diagnostics;
using GYM.BLL.Interfaces;
using GYM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Controllers
{
    [Authorize]
    public class HomeController(IAnalyticsService _analyticsService) : Controller
    {
        public IActionResult Index()
        {
            var Data = _analyticsService.GetAllAnalytics();
            return View(Data);
        }
    }
}
