using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Task" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Task.svc or Task.svc.cs at the Solution Explorer and start debugging.
    public class Task : ITask
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\";

        public Task()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                    string file = Guid.NewGuid().ToString() + ".txt";
                    using (StreamWriter sw = new StreamWriter(path + file, false))
                    {
                        sw.Write("Test task 1");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Task()", ex);
            }

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

        //public List<TodoTask> GetAllTasks()
        //{
        //    try
        //    {
        //        return ReadFiles();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("GetAllTasks()", ex);
        //    }
        //}

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


        private void DeleteFile(string file)
        {
            try
            {
                if ( File.Exists(file + ".txt") )
                    File.Delete(file + ".txt");
                else if (File.Exists(file + ".dat"))
                    File.Delete(file + ".dat");
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteFile(" + file + ")", ex);
            }
        }


        private List<TodoTask> ReadFiles()
        {
            List<TodoTask> result = new List<TodoTask>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.*");
                foreach (FileInfo file in files)
                {
                    using (StreamReader sr = new StreamReader(path + file.Name))
                    {
                        TodoTask task = new TodoTask
                        {
                            guid = file.Name.Split('.')[0],
                            task = sr.ReadLine()
                        };
                        if (file.Extension == ".dat")
                        {
                            task.date = StringToDate(sr.ReadLine());
                            task.time = sr.ReadLine();
                            task.info = task.task + "(" + task.date.Value.Day.ToString() + "." + task.date.Value.Month.ToString() + ". " + task.time.Substring(0, 5);
                        }
                        else
                            task.info = task.task;
                        result.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ReadFiles()", ex);
            }
            return result;
        }

        private int ReadFileCount()
        {
            int result = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.*");
                result = files.Count();
            }
            catch (Exception ex)
            {
                throw new Exception("ReadFileCount()", ex);
            }
            return result;
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
            public string time { get; set; }
            [DataMember]
            public string info { get; set; }
        }





        private string TimeToString(DateTime time)
        {
            try
            {
                return time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0');
            }
            catch (Exception ex)
            {
                throw new Exception("TimeToString(" + time + ")", ex);
            }
        }

        private DateTime StringToDate(string date)
        {
            try
            {
                return new DateTime(
                    int.Parse(date.Substring(0, 4)),
                    int.Parse(date.Substring(4, 2)),
                    int.Parse(date.Substring(6, 2)));
            }
            catch (Exception ex)
            {
                throw new Exception("StringToDate(" + date + ")", ex);
            }
        }
    }

}
