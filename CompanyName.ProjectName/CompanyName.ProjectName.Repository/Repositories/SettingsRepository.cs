using System;
using System.Threading.Tasks;
using AutoMapper;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Entities;

namespace CompanyName.ProjectName.Repository.Repositories.Settings
{
    public class SettingsRepository : BaseRepository<Setting, SettingEntity>
    {
        public SettingsRepository(CompanyNameProjectNameContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<T> GetSetting<T>(string key, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            var setting = await FirstOrDefaultAsync(x => x.Key == key);

            return setting == null ? defaultValue : (T)Convert.ChangeType(setting.Value, typeof(T));
        }
    }
}
