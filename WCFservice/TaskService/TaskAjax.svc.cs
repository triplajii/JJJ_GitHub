using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace TaskService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TaskAjax : TaskBase
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public string Test()
        {
            //Init();
            // Add your operation implementation here
            return "OK";
        }

        [OperationContract]
        public TaskService.Task.TodoTask[] GetAllTasks()
        {
            try
            {
                return ReadFiles().ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllTasks()", ex);
            }
        }

        [OperationContract]
        public string AddTask(string task)
        {
            string result = "kesken";
            try
            {
                string file = Guid.NewGuid().ToString() + ".txt";
                using (StreamWriter sw = new StreamWriter(path + file, false))
                {
                    sw.WriteLine(task);
                    sw.Flush();
                    result = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddTask(" + task + ")", ex);
            }
            return result;
        }


        [OperationContract]
        public bool DeleteTask(string guid)
        {
            bool result = false;
            try
            {
                DeleteFile(path + guid);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception("DeleteTask(" + guid + ")", ex);
            }
            return result;
        }
    }
}
