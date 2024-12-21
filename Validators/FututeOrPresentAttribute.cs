using System.ComponentModel.DataAnnotations;

namespace ProjetoMvc.Validators
{
    public class FututeOrPresentAttribute : ValidationAttribute
    {
        public FututeOrPresentAttribute()
        {
            ErrorMessage = "O campo {0} deve ser uma data futura ou presente.";
        }

        public override bool IsValid(object? value)
        {
            if (value is null) // não faz sentido validar se null por que já é tratado null anteriormente
            {
                return true;
            }

            var date = (DateTime)value;
            return date >= DateTime.Now;
        }
    }
}
