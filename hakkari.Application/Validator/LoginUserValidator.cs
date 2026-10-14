using FluentValidation;
using Pachakkari.Application.DTOs;
using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Validator
{
    public class LoginUserValidator : AbstractValidator<LoginDto>
    {
        public LoginUserValidator()
        {


            RuleFor(user => user.Email)
                    .NotEmpty()
                    .WithMessage("Email is required.")
                    .EmailAddress()
                    .WithMessage("Enter a valid email address.");

            RuleFor(user => user.Password)
                    .NotEmpty()
                    .WithMessage("Password is required.")
                    .MaximumLength(128)
                    .WithMessage("Password cannot exceed 128 characters.");
        }
    }
}
