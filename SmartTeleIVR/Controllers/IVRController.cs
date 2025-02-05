using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTeleIVR.Core.Application;

namespace SmartTeleIVR.Controllers
{
    [ApiController]
    [Route("api/ivr")]
    [AllowAnonymous]
    public class IVRController : ControllerBase
    {
        private readonly IIVRService _ivrService;

        public IVRController(IIVRService ivrService)
        {
            _ivrService = ivrService ?? throw new ArgumentNullException(nameof(ivrService));
        }
        [HttpPost("welcome")]
        public IActionResult GetWelcomeMessage([FromForm] string From)
        {
            var response = _ivrService.GetWelcomeMessage(From);
            return Content(response, "application/xml");
        }
        // Endpoint to handle menu options
        [HttpPost("menu")]
        public IActionResult HandleMenuOptions([FromForm] string? digits = null, [FromForm] string? From = null)
        {
                var result = _ivrService.HandleMenuOptions(digits, From);
                return Content(result, "application/xml"); 
        }

        [HttpPost("book-appointment")]
        public IActionResult HandleBookAppointment([FromForm] string? digits = null, [FromForm] string? From = null)
        {
                var result = _ivrService.HandleBookAppointment(digits, From);
                return Content(result, "application/xml"); 
        }

        // Optional: Uncomment and adjust if you plan to add a CheckBooking endpoint
        /*
        [HttpPost("check-booking")]
        public IActionResult CheckBooking([FromQuery] string callerPhoneNumber)
        {
            try
            {
                var result = _ivrService.HandleMenuOptions("2", callerPhoneNumber); // Option 2 to check booking
                return Content(result, "application/xml"); // Return as XML content for Twilio response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        */
    }
}
