using System;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Abstractions.Repositories;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.Helpers;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Entities;

namespace CompanyName.ProjectName.Repository.Repositories.Settings
{
    public class SettingsRepository : BaseRepository<Setting, SettingEntity>, ISettingsRepository
    {
        public SettingsRepository(CompanyNameProjectNameContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<T> GetSettingValue<T>(string key, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            return setting == null ? defaultValue : (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        public async Task<AsyncTryGetResult<T>> TryGetSettingValue<T>(string key)
        {
            var result = new AsyncTryGetResult<T>
            {
                Successful = false
            };

            if (string.IsNullOrWhiteSpace(key))
            {
                return result;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            if (setting == null)
            {
                return result;
            }

            result.Value = (T)Convert.ChangeType(setting.Value, typeof(T));
            result.Successful = true;

            return result;
        }

        public async Task<bool> TryUpdateSettingValue<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            if (setting == null)
            {
                return false;
            }

            setting.Value = value.ToString();

            return true;
        }
    }
}
