using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models.Dtos.Validators
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(p=>p.Title).NotEmpty().Matches(@"^[\S]+$").MaximumLength(100);
            RuleFor(p=>p.Content).NotEmpty().Matches(@"^[\S]+$").MaximumLength(255);
        }
    }
}
