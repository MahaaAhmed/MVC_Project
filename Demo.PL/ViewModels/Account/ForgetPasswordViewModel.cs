using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required (ErrorMessage ="Email is Nessecary")]
        public string Email { get; set; }
    }
}
