using System.Net.Http.Headers;

namespace VoluntariosConectadosRD.Services
{
    public interface IBaseApiService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object data);
        Task<T?> PutAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
        void SetAuthToken(string token);
    }
} 