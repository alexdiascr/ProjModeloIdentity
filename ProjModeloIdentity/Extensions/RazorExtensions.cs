using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ProjModeloIdentity.Extensions
{
    public static class RazorExtensions
    {
        //Caso se queira validar alguma coisa na view razor baseado em alguma claim
        public static bool IfClaim(this RazorPage page, string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(page.Context, claimName, claimValue);
        }

        //Caso se queira desabilitar algum botão através de uma validação de claims 
        public static string IfClaimShow(this RazorPage page, string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(page.Context, claimName, claimValue) ? "" : "disabled";
        }

        //No caso de um html content - Se por acaso, estiver escrevendo um link só poder ser exibido se o usuário tiver as claims
        public static IHtmlContent IfClaimShow(this IHtmlContent page, HttpContext context, string claimName, string claimValue)
        {
            return CustomAuthorization.ValidarClaimsUsuario(context, claimName, claimValue) ? page : null;
        }
    }
}
