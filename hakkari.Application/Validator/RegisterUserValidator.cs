using FluentValidation;
using Pachakkari.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Validator
{
    public class RegisterUserValidator:AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
              RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must contain at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Enter a valid email address.");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
                .MaximumLength(128)
                .WithMessage("Password cannot exceed 128 characters.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain a number.")
                .Matches("[^a-zA-Z0-9\\s]")
                .WithMessage("Password must contain a special character.")
                .Matches(@"^\S+$")
                .WithMessage("Password cannot contain spaces.");


        }
    }
}
