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
            _logger.LogTrace("Usuário acessou a home!");
            return View();
        }
        
        public IActionResult Privacy()
        {
            throw new Exception("Erro");
            return View();
        }

        [Authorize(Roles = "Admin, Gestor")]
        public IActionResult Secrect()
        {
            try
            {
                throw new Exception("Algo horrivel ocorreu!");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
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

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}