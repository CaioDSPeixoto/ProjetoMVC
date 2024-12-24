using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ProjetoMvc.Models.Helper
{
    public static class MessageHelper
    {
        // Configuração de mensagens
        public static void Error(ITempDataDictionary tempData, string message)
        {
            tempData["ErrorMessage"] = message;
        }

        public static void Success(ITempDataDictionary tempData, string message)
        {
            tempData["SuccessMessage"] = message;
        }

        public static void Warning(ITempDataDictionary tempData, string message)
        {
            tempData["WarningMessage"] = message;
        }

        // Renderização de mensagens
        public static string RenderAlert(this IHtmlHelper htmlHelper, ITempDataDictionary tempData)
        {
            if (tempData["ErrorMessage"] == null && tempData["SuccessMessage"] == null)
                return string.Empty;

            var alertType = tempData["ErrorMessage"] != null ? "alert-danger" : "alert-success";
            var message = tempData["ErrorMessage"] ?? tempData["SuccessMessage"];

            return $@"
            <div id='timer-alert' class='alert {alertType}'>
                {message}
            </div>";
        }
    }
}
