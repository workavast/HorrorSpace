using System.Threading;
using System.Threading.Tasks;
using Avastrad.Bootstraps;
using Avastrad.Settings;
using VContainer;
using VContainer.Unity;

namespace App.Settings
{
    public class SettingsBootstrap : Bootstrap
    {
        [Inject] private SettingsRepository _settingsRepository;
        
        protected override Task SelfInitialization(CancellationToken cancellationToken, LifetimeScope lifeTimeScope)
        {
            _settingsRepository.TryLoad();
            _settingsRepository.Apply(true);
            return Task.CompletedTask;
        }
    }
}