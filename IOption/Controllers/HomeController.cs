using IOption.Models;
using IOption.Models.ViewModels;
using IOption.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IOption.Controllers
{
    public class HomeController : Controller
    {
        private IsmsServices _smsServices;
        private KavehNegarViewModel _kavehNegarViewModel;
        private PasargadViewModel _PasargadViewModel;

        public HomeController(
            Func<SelectSmsPanel, IsmsServices> smsServices,
            IOptions<KavehNegarViewModel> kavehNegarOptions,
            IOptions<PasargadViewModel> pasargadOptions)
        {
            _smsServices = smsServices(SelectSmsPanel.KavehNegar);
            _kavehNegarViewModel = kavehNegarOptions.Value;
            _PasargadViewModel = pasargadOptions.Value;
        }

        public IActionResult Index()
        {
            ViewBag.KavehNegar = _kavehNegarViewModel.Api;
            ViewBag.Pasargad = _PasargadViewModel.TerminalId;
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.KavehNegarSms = _smsServices.SendSms();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
