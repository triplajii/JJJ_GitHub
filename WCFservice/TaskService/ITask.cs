using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TaskService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITask" in both code and config file together.
    [ServiceContract]
    public interface ITask
    {        
        [OperationContract]
        Task<string> AddTaskAsync(string task);

        [OperationContract]
        Task<string> AddAppointmentAsync(string title, DateTime date, string time);

        [OperationContract]
        Task<List<TaskService.Task.TodoTask>> GetAllTasksAsync();

        [OperationContract]
        System.Threading.Tasks.Task DeleteTaskAsync(string guid);

        [OperationContract]
        Task<int> GetTaskCount();
    }
}
