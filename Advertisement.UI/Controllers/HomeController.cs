using Advertisement.UI.Extensions;
using AdvertisementApp.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Advertisement.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProvidedServiceService providedServiceService;
        private readonly IAdvertisementService advertisementService;

        public HomeController(IProvidedServiceService providedServiceService, IAdvertisementService advertisementService)
        {
            this.providedServiceService = providedServiceService;
            this.advertisementService = advertisementService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await providedServiceService.GetAllAsync();
            return this.ResponseView(response);
        }

        public async Task<IActionResult> HumanResource()
        {
            var response = await advertisementService.GetActivesAsync();
            return this.ResponseView(response);
        }
    }
}
