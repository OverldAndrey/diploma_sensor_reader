using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RobotSenderSample.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RobotSenderSample
{
    public class RobotClientScenarioEventArgs : EventArgs
    {
        public ScenarioStatusLogMessage scenarioMessage { get; set; }
    }
    public class RobotHubClient : IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<RobotHubClient> _logger;

        public event EventHandler<RobotClientScenarioEventArgs> scenarioStatusChanged;

        public RobotHubClient(string url)
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<RobotHubClient>();
            _connection = new HubConnectionBuilder()
                          .WithUrl(url)
                          .AddMessagePackProtocol()
                          .WithAutomaticReconnect()
                          .ConfigureLogging(logging =>
                          {
                              // Log to the Console
                              logging.AddConsole();
                          })
                          .Build();

            _cancellationTokenSource = new CancellationTokenSource();

            _connection.On<ScenarioStatusLogMessage>("ReceiveScenarioStatus", ReceiveScenarioStatus);
            _connection.On<ScenarioActivationLogMessage>("ReceiveChangeScenarioActivation",
                                                         ReceiveChangeScenarioActivation);
            _connection.On<ScenarioErrorLogMessage>("ReceiveErrorScenario", ReceiveErrorScenario);
            _connection.On<BmlStatusLogMessage>("ReceiveBmlStatus", ReceiveBmlStatus);
            _connection.On<BmlErrorLogMessage>("ReceiveErrorBml", ReceiveErrorBml);
            _connection.On<TagStatusLogMessage>("ReceiveTagStatus", ReceiveTagStatus);
            _connection.On<TagErrorLogMessage>("ReceiveErrorTag", ReceiveErrorTag);
        }

        public Task StartAsync()
        {
            return _connection.StartAsync(_cancellationTokenSource.Token);
        }

        public Task StopAsync()
        {
            return _connection.StopAsync(_cancellationTokenSource.Token);
        }

        #region Controll

        public Task Add(string scenarioStr)
        {
            return _connection.SendAsync("Add", scenarioStr, _cancellationTokenSource.Token);
        }

        public Task UpdateActivation(string scenarioName, double activation)
        {
            return _connection.SendAsync("UpdateActivation", scenarioName, activation, _cancellationTokenSource.Token);
        }

        public Task RemoveAll()
        {
            throw new NotImplementedException();
        }

        public Task RemoveScenario(string scenarioId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBmls(string scenarioId, IEnumerable<string> bmlIds)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Feedback

        private Task ReceiveErrorTag(TagErrorLogMessage tagError)
        {
            _logger.LogError("Receive: {Time}: Error for tag ({ScenarioId};{BmlId};{TagId}) - {Exception}",
                             tagError.Time, tagError.ScenarioId, tagError.BmlId, tagError.TagId, tagError.Exception);
            return Task.CompletedTask;
        }

        private Task ReceiveTagStatus(TagStatusLogMessage tagStatus)
        {
            _logger.LogInformation("Receive: {Time}: Tag ({ScenarioId};{BmlId};{TagId}) changed status to {Status}",
                                   tagStatus.Time, tagStatus.ScenarioId, tagStatus.BmlId,
                                   tagStatus.TagId, tagStatus.Status);
            return Task.CompletedTask;
        }

        private Task ReceiveErrorBml(BmlErrorLogMessage bmlError)
        {
            _logger.LogError("Receive: {Time}: Error for bml ({ScenarioId};{BmlId}) - {Exception}",
                             bmlError.Time, bmlError.ScenarioId, bmlError.BmlId, bmlError.Exception);
            return Task.CompletedTask;
        }

        private Task ReceiveBmlStatus(BmlStatusLogMessage bmlStatus)
        {
            _logger.LogInformation("Receive: {Time}: Bml ({ScenarioId};{BmlId}) changed status to {Status}",
                                   bmlStatus.Time, bmlStatus.ScenarioId, bmlStatus.BmlId, bmlStatus.Status);
            return Task.CompletedTask;
        }

        private Task ReceiveErrorScenario(ScenarioErrorLogMessage scenarioError)
        {
            _logger.LogError("Receive: {Time}: Error for scenario ({ScenarioId}) - {Exception}",
                             scenarioError.Time, scenarioError.ScenarioId, scenarioError.Exception);
            return Task.CompletedTask;
        }

        private Task ReceiveChangeScenarioActivation(ScenarioActivationLogMessage scenarioActivation)
        {
            _logger.LogInformation("Receive: {Time}: Scenario ({ScenarioId}) changed activation to {Activation}",
                                   scenarioActivation.Time, scenarioActivation.ScenarioId,
                                   scenarioActivation.NewActivation);
            return Task.CompletedTask;
        }

        private Task ReceiveScenarioStatus(ScenarioStatusLogMessage scenarioStatus)
        {
            _logger.LogInformation("Receive: {Time}: Scenario ({ScenarioId}) status changed to {Status}",
                                   scenarioStatus.Time, scenarioStatus.ScenarioId, scenarioStatus.Status);
            
            var scenarioStatusEventArgs = new RobotClientScenarioEventArgs();
            scenarioStatusEventArgs.scenarioMessage = scenarioStatus;
            scenarioStatusChanged.Invoke(this, scenarioStatusEventArgs);

            scenarioStatusEventArgs = null;
            
            return Task.CompletedTask;
        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
            await _connection.DisposeAsync();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}