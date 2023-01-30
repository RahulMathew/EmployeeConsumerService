using BusinessObjects.DTO;

namespace EmployeeConsumerService.HostedServices.Interfaces
{
    public interface IEmployeeConsumerSignalRBroadcaster
    {
        Task BroadcastMessage(Employee employee);
    }
}
