using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Services
{
    public class OportunidadApiService : IOportunidadApiService
    {
        private readonly HttpClient _httpClient;

        public OportunidadApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Oportunidad>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/oportunidades");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Oportunidad>>();
        }

        public async Task<Oportunidad> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/oportunidades/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Oportunidad>();
        }

        public async Task<bool> CreateAsync(Oportunidad oportunidad)
        {
            var response = await _httpClient.PostAsJsonAsync("api/oportunidades", oportunidad);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Oportunidad oportunidad)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/oportunidades/{oportunidad.Id}", oportunidad);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/oportunidades/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
