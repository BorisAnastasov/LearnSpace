using System.ComponentModel.DataAnnotations;

namespace LearnSpace.Core.Models.Account
{
    public class LogInViewModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
