using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.Helpers;

namespace CompanyName.ProjectName.Core.Abstractions.Repositories
{
    public interface ISettingsRepository : IBaseRepository<Setting>
    {
        Task<T> GetSettingValue<T>(string key, T defaultValue);
        Task<AsyncTryGetResult<T>> TryGetSettingValue<T>(string key);
        Task<bool> TryUpdateSettingValue<T>(string key, T value);
    }
}
