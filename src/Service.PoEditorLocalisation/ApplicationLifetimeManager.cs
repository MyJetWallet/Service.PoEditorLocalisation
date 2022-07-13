using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;

namespace Service.PoEditorLocalisation
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly MyNoSqlClientLifeTime _nosqlLifeTime;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime, ILogger<ApplicationLifetimeManager> logger, MyNoSqlClientLifeTime nosqlLifeTime)
            : base(appLifetime)
        {
	        _logger = logger;
	        _nosqlLifeTime = nosqlLifeTime;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _nosqlLifeTime.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _nosqlLifeTime.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
