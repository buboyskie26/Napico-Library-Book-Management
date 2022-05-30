using Asset.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel.ViewComponents
{
    public class HoldHistory : ViewComponent
    {
        private IHold _hold;
        public HoldHistory(IHold userService)
        {
            _hold = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _hold.GetAllHoldHistoryAsync();
            return View(users);
        }
    }
}
