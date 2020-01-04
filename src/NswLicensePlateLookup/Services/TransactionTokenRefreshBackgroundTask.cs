using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NswLicensePlateLookup.Services
{
    [ExcludeFromCodeCoverage]
    public class TransactionTokenRefreshBackgroundTask : IHostedService, IDisposable
    {
        private Timer _timer;
        
        private IServiceNswTransactionTokenHelper _serviceNswTransactionTokenHelper;

        public TransactionTokenRefreshBackgroundTask(IServiceNswTransactionTokenHelper serviceNswTransactionTokenHelper)
        {
            _serviceNswTransactionTokenHelper = serviceNswTransactionTokenHelper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RefreshTransactionToken, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

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
            _serviceNswTransactionTokenHelper.ClearTransactionTokenCache();
            _serviceNswTransactionTokenHelper.GetTransactionToken().GetAwaiter().GetResult();
        }
    }
}