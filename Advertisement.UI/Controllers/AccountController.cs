using Advertisement.Common;
using Advertisement.Common.Enums;
using Advertisement.UI.Extensions;
using Advertisement.UI.Models;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Business.Services;
using AdvertisementApp.DTOs.AppUserDtos;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Advertisement.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenderService genderService;
        private readonly IValidator<UserCreateModel> userCreateModelValidator;
        private readonly IAppUserService appUserService;
        private readonly IMapper mapper;

        public AccountController(IGenderService genderService, IValidator<UserCreateModel> userCreateModelValidator, IAppUserService appUserService, IMapper mapper)
        {
            this.genderService = genderService;
            this.userCreateModelValidator = userCreateModelValidator;
            this.appUserService = appUserService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> SignUp()
        {
            var response = await genderService.GetAllAsync();
            var model = new UserCreateModel
            {
                Genders = new SelectList(response.Data, "Id", "Definition")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateModel model)
        {
            var result = userCreateModelValidator.Validate(model);

            if (result.IsValid)
            {
                var dto = mapper.Map<AppUserCreateDto>(model);
                var createResponse = await appUserService.CreateWithRoleAsync(dto, (int)RoleType.Member);
                return this.ResponseRedirectAction(createResponse, "SignIn");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            var response = await genderService.GetAllAsync();
            model.Genders = new SelectList(response.Data, "Id", "Definition", model.GenderId);

            return View(model);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AppUserLoginDto dto)
        {
            var result = await appUserService.CheckUserAsync(dto);
            if (result.ResponseType == Common.ResponseType.Success)
            {
                var roleResult = await appUserService.GetRolesByUserIdAsync(result.Data.Id);
                // ilgili kullanıcının rollerini çekmemiz.
                var claims = new List<Claim>();

                if (roleResult.ResponseType == ResponseType.Success)
                {
                    foreach (var role in roleResult.Data)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Definition));
                    }
                }

                claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()));

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = dto.RememberMe,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Kullanıcı adı veya şifre hatalı", result.Message);
            return View(dto);
        }

        public async IActionResult LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
