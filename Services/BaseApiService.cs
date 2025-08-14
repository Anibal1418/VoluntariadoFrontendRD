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
            string? content = null;
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                
                content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("GET {Endpoint} response: {Content}", endpoint, content?.Substring(0, Math.Min(500, content?.Length ?? 0)));
                
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar GET {Endpoint}. Response content: {Content}", endpoint, content ?? "null");
                return default;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                _logger.LogInformation("POST {Endpoint} - Sending JSON: {Json}", endpoint, json);
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

        public async Task<T?> PostFormDataAsync<T>(string endpoint, MultipartFormDataContent formData, string? token = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                
                var response = await _httpClient.PostAsync(endpoint, formData);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST FormData {Endpoint} returned {StatusCode}: {Response}", endpoint, response.StatusCode, responseContent);
                    
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
                _logger.LogError(ex, "Error al llamar POST FormData {Endpoint}", endpoint);
                return default;
            }
        }

        public async Task<T?> DeleteAsync<T>(string endpoint, string? token = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                
                var response = await _httpClient.DeleteAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("DELETE {Endpoint} returned {StatusCode}: {Response}", endpoint, response.StatusCode, responseContent);
                    return default;
                }
                
                return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar DELETE {Endpoint}", endpoint);
                return default;
            }
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
} 