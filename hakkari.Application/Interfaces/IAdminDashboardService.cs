using Pachakkari.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<List<MonthlyOrdersDto>>GetMonthlyOrdersAsync(int year);
    }
}
