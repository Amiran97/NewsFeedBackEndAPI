using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models.Dtos.Validators
{
    public class LoginCredentialsRequestValidator : AbstractValidator<LoginCredentialsRequest>
    {
        public LoginCredentialsRequestValidator() {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches(@"^[\S]+$").MinimumLength(8).MaximumLength(32);
        }
    }
}
