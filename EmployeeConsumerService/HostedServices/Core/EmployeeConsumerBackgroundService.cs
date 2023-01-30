using Newtonsoft.Json;
using BusinessObjects.DTO;
using BusinessLayer.Interfaces;
using Common.ServiceBus.Interfaces;
using EmployeeConsumerService.HostedServices.Interfaces;

namespace EmployeeConsumerService.HostedServices.Core
{
    public class EmployeeConsumerBackgroundService : IHostedService, IDisposable
    {
        #region Declaration

        private readonly ITopicConsumer _topicConsumer;
        private readonly IEmployeeConsumerSignalRBroadcaster _employeeConsumerSignalRBroadcaster;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion Declaration

        #region Constructor

        public EmployeeConsumerBackgroundService(ITopicConsumer topicConsumer, 
            IEmployeeConsumerSignalRBroadcaster employeeConsumerSignalRBroadcaster,
            IEmployeeRepository employeeRepository)
        {
            _topicConsumer = topicConsumer;

            _employeeConsumerSignalRBroadcaster = employeeConsumerSignalRBroadcaster;

            _employeeRepository = employeeRepository;
        }

        public void Dispose()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _topicConsumer.Subscribe(ConsumeTopicMessage);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion Constructor

        #region Private Methods

        private async void ConsumeTopicMessage(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    var employee = JsonConvert.DeserializeObject<Employee>(message);

                    if (employee != null)
                    {
                        _employeeRepository.SaveEmployee(employee).Wait();

                        await _employeeConsumerSignalRBroadcaster.BroadcastMessage(employee);
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        #endregion Private Methods
    }
}
