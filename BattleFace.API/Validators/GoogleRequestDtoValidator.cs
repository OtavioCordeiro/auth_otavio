using BattleFace.Application.DTOs;
using FluentValidation;

namespace BattleFace.API.Validators
{
    public class GoogleRequestDtoValidator : AbstractValidator<GoogleRequestDto>
    {
        public GoogleRequestDtoValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        }
    }
}
