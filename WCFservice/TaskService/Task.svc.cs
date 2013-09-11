using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService
{

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
            catch(Exception ex)
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
