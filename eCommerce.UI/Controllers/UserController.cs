using eCommerce.UI.Models.Response;
using eCommerce.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.UI.Controllers
{
    public class UserController : Controller  // controller name
    {
        private readonly IHttpClientFactory clientFactory;

        public UserController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()  /// Action Method
        {
            return View();   // UI (show to end user ... cshtml --> cs + html)
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)  /// Action Method
        {

            var client = clientFactory.CreateClient("eCommerceApi");
            await  client.PostAsJsonAsync("/api/Token", viewModel);

            return View();  
        }




        public IActionResult Register()
        {
            return View();
        }


    }
}
