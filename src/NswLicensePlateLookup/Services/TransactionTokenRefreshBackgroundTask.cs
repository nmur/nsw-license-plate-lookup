using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NswLicensePlateLookup.Services
{
    public class TransactionTokenRefreshBackgroundTask : IHostedService, IDisposable
    {
        private Timer _timer;

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
            Console.WriteLine($"Refreshing transaction token at: {DateTime.Now.ToString()}");
        }
    }
}