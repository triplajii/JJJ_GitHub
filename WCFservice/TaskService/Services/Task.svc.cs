using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TaskService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Task" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Task.svc or Task.svc.cs at the Solution Explorer and start debugging.
    public class Task : TaskBase, ITask
    {
        //string path = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\";

        public Task()
        {
            Init();
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            //try
            //{
            //    DirectoryInfo di = new DirectoryInfo(path);
            //    if (!di.Exists)
            //    {
            //        di.Create();
            //        string file = Guid.NewGuid().ToString() + ".txt";
            //        using (StreamWriter sw = new StreamWriter(path + file, false))
            //        {
            //            sw.Write("Test task 1");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Task()", ex);
            //}

        }

        public async Task<string> AddTaskAsync(string task)
        {
            string result = "kesken";
            try
            {
                string file = Guid.NewGuid().ToString() + ".txt";
                using (StreamWriter sw = new StreamWriter(path + file, false))
                {
                    await sw.WriteLineAsync(task);
                    await sw.FlushAsync();
                    result = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddTaskAsync(" + task + ")", ex);
            }
            return result;
        }

        public async Task<string> AddAppointmentAsync(string title, DateTime date, string time)
        {
            string result = "kesken";
            try
            {
                string file = Guid.NewGuid().ToString() + ".dat";
                using (StreamWriter sw = new StreamWriter(path + file, false))
                {
                    await sw.WriteLineAsync(title);
                    await sw.WriteLineAsync(TimeToString(date));
                    await sw.WriteLineAsync(time);
                    await sw.FlushAsync();
                    result = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddAppointmentAsync(" + title + "," + date + "," + time + ")", ex);
            }
            return result;
        }


        public async Task<List<TodoTask>> GetAllTasksAsync()
        {
            try
            {
                return await System.Threading.Tasks.Task.Factory.StartNew(() => ReadFiles());
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllTasksAsync()", ex);
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(string guid)
        {
            try
            {
                await System.Threading.Tasks.Task.Factory.StartNew(() => DeleteFile(path + guid));
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteTaskAsync(" + guid + ")", ex);
            }
        }

        public async Task<int> GetTaskCount()
        {
            return await System.Threading.Tasks.Task.Factory.StartNew(() => ReadFileCount());
        }



        [DataContract]
        public class TodoTask
        {
            [DataMember]
            public string guid { get; set; }
            [DataMember]
            public string task { get; set; }
            [DataMember]
            public DateTime? date { get; set; }
            [DataMember]
            public string datestring { get; set; }
            [DataMember]
            public string time { get; set; }
        }

    }
}
