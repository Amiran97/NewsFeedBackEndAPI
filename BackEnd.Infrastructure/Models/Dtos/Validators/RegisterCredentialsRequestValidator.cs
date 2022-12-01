using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models.Dtos.Validators
{
    public class RegisterCredentialsRequestValidator : AbstractValidator<RegisterCredentialsRequest>
    {
        public RegisterCredentialsRequestValidator() {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Password and ConfirmPassword must be equel");
        }
    }
}
