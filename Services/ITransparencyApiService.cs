using VoluntariadoConectadoRD.Models.DTOs;
using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Services
{
    public interface ITransparencyApiService
    {
        Task<ApiResponse<List<OrganizationTransparencyDto>>?> GetOrganizationsFinancialSummaryAsync(TransparencyFiltersDto? filters = null);
        Task<ApiResponse<OrganizationFinancialDetailsDto>?> GetOrganizationFinancialDetailsAsync(int organizationId);
        Task<ApiResponse<FinancialReportDetailDto>?> GetFinancialReportDetailsAsync(int reportId);
        Task<ApiResponse<List<int>>?> GetAvailableYearsAsync();
        Task<ApiResponse<List<string>>?> GetOrganizationTypesAsync();
        Task<ApiResponse<ChartDataDto>?> GetPlatformFinancialOverviewAsync();
    }
}