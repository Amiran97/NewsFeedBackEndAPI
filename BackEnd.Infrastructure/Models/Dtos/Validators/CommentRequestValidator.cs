using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models.Dtos.Validators
{
    public class CommentRequestValidator : AbstractValidator<CommentRequest>
    {
        public CommentRequestValidator()
        {
            RuleFor(p => p.Message).NotEmpty().MaximumLength(255);
        }
    }
}
