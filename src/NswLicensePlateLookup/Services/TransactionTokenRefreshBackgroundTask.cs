using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NswLicensePlateLookup.Services
{
    [ExcludeFromCodeCoverage]
    public class TransactionTokenRefreshBackgroundTask : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly ILogger _logger;

        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;

        public TransactionTokenRefreshBackgroundTask(ILogger<TransactionTokenRefreshBackgroundTask> logger, IServiceNswTransactionTokenHelper serviceNswTransactionTokenHelper)
        {
            _logger = logger;
            _serviceNswTransactionTokenHelper = serviceNswTransactionTokenHelper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RefreshTransactionToken, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void RefreshTransactionToken(object state)
        {
            _logger.LogDebug("Clearing transaction token cache");
            _serviceNswTransactionTokenHelper.ClearTransactionTokenCache();
            _logger.LogDebug("Getting a fresh transaction token");
            _serviceNswTransactionTokenHelper.GetTransactionToken().GetAwaiter().GetResult();
        }
    }
}