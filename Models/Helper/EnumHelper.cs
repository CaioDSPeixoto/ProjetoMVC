using ProjetoMvc.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Models.Helper
{
    public class EnumHelper
    {
        public string ObterDisplay(TransactionTypeEnum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttribute = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                             .FirstOrDefault() as DisplayAttribute;
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
