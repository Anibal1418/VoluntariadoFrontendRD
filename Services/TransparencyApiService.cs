using VoluntariadoConectadoRD.Models.DTOs;
using VoluntariosConectadosRD.Models;
using VoluntariosConectadosRD.Models.DTOs;

namespace VoluntariosConectadosRD.Services
{
    public class TransparencyApiService : ITransparencyApiService
    {
        private readonly IBaseApiService _baseApiService;
        private readonly ILogger<TransparencyApiService> _logger;

        public TransparencyApiService(IBaseApiService baseApiService, ILogger<TransparencyApiService> logger)
        {
            _baseApiService = baseApiService;
            _logger = logger;
        }

        public async Task<ApiResponseDto<List<OrganizationTransparencyDto>>?> GetOrganizationsFinancialSummaryAsync(TransparencyFiltersDto? filters = null)
        {
            try
            {
                var queryParams = new List<string>();
                
                if (filters != null)
                {
                    if (filters.Año.HasValue)
                        queryParams.Add($"año={filters.Año}");
                    if (filters.Trimestre.HasValue)
                        queryParams.Add($"trimestre={filters.Trimestre}");
                    if (!string.IsNullOrEmpty(filters.TipoOrganizacion))
                        queryParams.Add($"tipoOrganizacion={Uri.EscapeDataString(filters.TipoOrganizacion)}");
                    if (filters.SoloVerificadas.HasValue)
                        queryParams.Add($"soloVerificadas={filters.SoloVerificadas}");
                    if (filters.MontoMinimo.HasValue)
                        queryParams.Add($"montoMinimo={filters.MontoMinimo}");
                    if (filters.MontoMaximo.HasValue)
                        queryParams.Add($"montoMaximo={filters.MontoMaximo}");
                    if (!string.IsNullOrEmpty(filters.OrdenPor))
                        queryParams.Add($"ordenPor={Uri.EscapeDataString(filters.OrdenPor)}");
                    if (filters.Descendente)
                        queryParams.Add($"descendente={filters.Descendente}");
                }

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                return await _baseApiService.GetAsync<ApiResponseDto<List<OrganizationTransparencyDto>>>($"transparency/organizations{queryString}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetOrganizationsFinancialSummaryAsync");
                return null;
            }
        }

        public async Task<ApiResponseDto<OrganizationFinancialDetailsDto>?> GetOrganizationFinancialDetailsAsync(int organizationId)
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<OrganizationFinancialDetailsDto>>($"transparency/organizations/{organizationId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetOrganizationFinancialDetailsAsync");
                return null;
            }
        }

        public async Task<ApiResponseDto<FinancialReportDetailDto>?> GetFinancialReportDetailsAsync(int reportId)
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<FinancialReportDetailDto>>($"transparency/reports/{reportId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetFinancialReportDetailsAsync");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<int>>?> GetAvailableYearsAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<List<int>>>("transparency/filters/years");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAvailableYearsAsync");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<string>>?> GetOrganizationTypesAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<List<string>>>("transparency/filters/organization-types");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetOrganizationTypesAsync");
                return null;
            }
        }

        public async Task<ApiResponseDto<ChartDataDto>?> GetPlatformFinancialOverviewAsync()
        {
            try
            {
                return await _baseApiService.GetAsync<ApiResponseDto<ChartDataDto>>("transparency/platform-overview");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetPlatformFinancialOverviewAsync");
                return null;
            }
        }
    }
}