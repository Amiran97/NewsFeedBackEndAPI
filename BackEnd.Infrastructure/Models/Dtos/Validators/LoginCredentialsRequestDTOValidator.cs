using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models.Dtos.Validators
{
    public class LoginCredentialsRequestDTOValidator : AbstractValidator<LoginCredentialsRequestDTO>
    {
        public LoginCredentialsRequestDTOValidator() {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
