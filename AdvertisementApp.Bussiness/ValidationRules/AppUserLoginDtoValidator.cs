using AdvertisementApp.DTOs.AppUserDtos;
using FluentValidation;


namespace AdvertisementApp.Business.ValidationRules
{
    public class AppUserLoginDtoValidator:AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(X => X.Username).NotEmpty().WithMessage("Kullanıcı adı boş olamz");
            RuleFor(X => X.Password).NotEmpty().WithMessage("Parola boş olamaz");
        }
    }
}
