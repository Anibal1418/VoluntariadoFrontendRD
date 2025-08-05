using VoluntariosConectadosRD.Models;

namespace VoluntariosConectadosRD.Services
{
    public interface IOportunidadApiService
    {
        Task<List<Oportunidad>> GetAllAsync();
        Task<Oportunidad?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Oportunidad oportunidad);
        Task<bool> UpdateAsync(Oportunidad oportunidad);
        Task<bool> DeleteAsync(int id);
    }
}
