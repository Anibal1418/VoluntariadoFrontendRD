using VoluntariadoConectadoRD.Models.DTOs;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public interface ITransparencyApiService
    {
        Task<ApiResponseDto<List<OrganizationTransparencyDto>>?> GetOrganizationsFinancialSummaryAsync(TransparencyFiltersDto? filters = null);
        Task<ApiResponseDto<OrganizationFinancialDetailsDto>?> GetOrganizationFinancialDetailsAsync(int organizationId);
        Task<ApiResponseDto<FinancialReportDetailDto>?> GetFinancialReportDetailsAsync(int reportId);
        Task<ApiResponseDto<List<int>>?> GetAvailableYearsAsync();
        Task<ApiResponseDto<List<string>>?> GetOrganizationTypesAsync();
        Task<ApiResponseDto<ChartDataDto>?> GetPlatformFinancialOverviewAsync();
    }
}