using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.Interfaces;

namespace Pachakkari.API.Controllers
{
    [ApiController]
    [Route("api/admin/dasboard")]
    [Authorize(Roles="Admin")]
    public class AdminDashboardController:ControllerBase
    {
        private readonly IAdminDashboardService _adminDashBoardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _adminDashBoardService = dashboardService;
        }
        [HttpGet("summary")]
        public async Task<IActionResult>GetSummary()
        {
            var summary = await _adminDashBoardService.GetSummaryAsync();
            return Ok(summary);
        }

        [HttpGet("monthly-orders")]
        public async Task<IActionResult> GetMonthlyOrders(
    [FromQuery] int? year)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;

            var monthlyOrders =
                await _adminDashBoardService.GetMonthlyOrdersAsync(selectedYear);

            return Ok(monthlyOrders);
        }

    }
}
