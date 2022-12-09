using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.API.Models.Dtos.Validators
{
    public class RegisterCredentialsRequestValidator : AbstractValidator<RegisterCredentialsRequest>
    {
        public RegisterCredentialsRequestValidator() {
            RuleFor(x => x.UserName).NotEmpty().Matches(@"^[\S]+$").MinimumLength(4).MaximumLength(32);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches(@"^[\S]+$").MinimumLength(8).MaximumLength(32);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Matches(@"^[\S]+$").MinimumLength(8).MaximumLength(32).Equal(x=>x.Password).WithMessage("Password and ConfirmPassword must be equel");
        }
    }
}
