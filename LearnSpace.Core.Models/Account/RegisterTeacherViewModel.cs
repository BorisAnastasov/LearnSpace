using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LearnSpace.Common.DataConstants.Teacher;

namespace LearnSpace.Core.Models.Account
{
	public class RegisterTeacherViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; } = string.Empty;

		[Required]
		[Display(Name = "First Name")]
		[StringLength(FirstNameMaxLen, MinimumLength = FirstNameMinLen)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Last Name")]
		[StringLength(LastNameMaxLen, MinimumLength = LastNameMinLen)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Spezialization")]
		[StringLength(SubjectSpecializationMaxLen, MinimumLength = SubjectSpecializationMinLen)]
		public string SubjectSpecialization { get; set; } = string.Empty;


		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; } = string.Empty;
	}
}
