﻿using Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator() { 
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("El nombre es requerido");
        }
    }
}
