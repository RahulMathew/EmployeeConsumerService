using BusinessObjects.DTO;
using Microsoft.AspNetCore.SignalR;

namespace EmployeeConsumerService.Hubs
{
    public class EmployeeConsumerSignalHub : Hub
    {
        #region Public Methods

        public async Task SendMessage(Employee employee)
        {
            await Clients.All.SendAsync("ReceiveEmployeeData", employee);
        }

        #endregion Public Methods
    }
}
