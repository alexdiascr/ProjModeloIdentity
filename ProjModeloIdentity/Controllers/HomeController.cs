using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjModeloIdentity.Extensions;
using ProjModeloIdentity.Models;
using System.Diagnostics;
using static ProjModeloIdentity.Extensions.CustomAuthorization;

namespace ProjModeloIdentity.Controllers
{
    //Atributo diz que só iria aceitar usuários autenticados acesssar
    //essa parte da aplicação
    //Aplicando para toda a Controller e fechando toda a aplicação para somente
    //usuários autenticados
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Abrindo excessão
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Gestor")]
        public IActionResult Secrect()
        {
            return View();
        }

        [Authorize(Policy ="PodeExcluir")]
        public IActionResult SecrectClaim()
        {
            return View("Secrect");
        }

        [Authorize(Policy = "PodeEscrever")]
        public IActionResult SecrectClaimGravar()
        {
            return View("Secrect");
        }

        [ClaimsAuthorize("Produtos", "Ler")]
        public IActionResult ClaimsCustom()
        {
            return View("Secrect");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}