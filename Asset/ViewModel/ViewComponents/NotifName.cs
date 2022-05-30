using Asset.Data.Model;
using Asset.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asset.ViewModel.ViewComponents
{
    public class NotifName : ViewComponent
    {
        private readonly IHold _hold;
        private readonly UserManager<Patron> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;

        public NotifName(IHold cart, UserManager<Patron> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _hold = cart;
        }
        public IViewComponentResult Invoke()
        {
            var items = _hold.NotificationName(loginUserId);
            return View("Default", items);
        }
    }
}
