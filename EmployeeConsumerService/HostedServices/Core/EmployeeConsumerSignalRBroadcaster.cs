using Microsoft.AspNetCore.SignalR;
using BusinessObjects.DTO;
using EmployeeConsumerService.Hubs;
using EmployeeConsumerService.HostedServices.Interfaces;

namespace EmployeeConsumerService.HostedServices.Core
{
    public class EmployeeConsumerSignalRBroadcaster : IEmployeeConsumerSignalRBroadcaster
    {
        #region Declaration

        private readonly IHubContext<EmployeeConsumerSignalHub> _hubContext;

        #endregion Declaration

        #region Constructor

        public EmployeeConsumerSignalRBroadcaster(IHubContext<EmployeeConsumerSignalHub> hubContext)
        {
            _hubContext = hubContext;
        }

        #endregion Constructor

        #region Public Methods

        public async Task BroadcastMessage(Employee employee)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveEmployeeData", employee);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Public Methods
    }
}
