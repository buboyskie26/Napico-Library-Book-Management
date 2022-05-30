using Asset.Repository.Contract.ApiContract;
using Asset.ViewModel;
using Asset.ViewModel.CheckHistoryVM;
using Asset.ViewModel.HoldHistoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asset.Controllers.Api
{
    [Route("api/Checkout")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        private readonly ICheckoutApi _checkoutServ;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;

        public CheckoutApiController(ICheckoutApi appointmentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _checkoutServ = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
        [HttpGet]
        [Route("GetCheckoutData")]
        public IActionResult GetCheckoutData()
        {
            CommonResponse<List<CheckHistoryIndex>> commonResponse = new CommonResponse<List<CheckHistoryIndex>>();
            try
            {
                if (role == "Patron")
                {
                    commonResponse.dataenum = _checkoutServ.PatronCheckoutById(loginUserId);
                    commonResponse.status = 1;
                }
                
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = 0;
            }
            return Ok(commonResponse);
        }

    }
}
