using Advertisement.UI.Models;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.DTOs.AppUserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Advertisement.UI.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IAppUserService appUserService;

        public AdvertisementController(IAppUserService appUserService)
        {
            this.appUserService = appUserService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Send(int advertisementId)
        {
            var userId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);
            var userResponse = await appUserService.GetByIdAsync<AppUserListDto>(userId);

            ViewBag.GenderId = userResponse.Data.GenderId;

            return View(new AdvertisementAppUserCreateModel()
            {
                AdvertisementId = advertisementId,
                AppUserId = userId,
            });

        }
    }
}
