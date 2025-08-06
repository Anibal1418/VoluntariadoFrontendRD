using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace VoluntariosConectadosRD.Services
{
    public class BaseApiService : IBaseApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BaseApiService> _logger;

        public BaseApiService(HttpClient httpClient, IConfiguration configuration, ILogger<BaseApiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            
            // Establecer dirección base desde la configuración
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api/";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar GET {Endpoint}", endpoint);
                return default;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST {Endpoint} returned {StatusCode}: {Response}", endpoint, response.StatusCode, responseContent);
                    
                    // Try to parse error response as ApiResponseDto
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return errorResponse;
                    }
                    catch
                    {
                        // If we can't parse as expected type, return default
                        return default;
                    }
                }
                
                return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar POST {Endpoint}", endpoint);
                return default;
            }
        }

        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar PUT {Endpoint}", endpoint);
                return default;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar DELETE {Endpoint}", endpoint);
                return false;
            }
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
} 