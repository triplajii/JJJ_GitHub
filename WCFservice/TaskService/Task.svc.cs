using System;
using System.Collections.Generic;
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
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                    string file = Guid.NewGuid().ToString() + ".txt";
                    using (StreamWriter sw = new StreamWriter(path + file, false))
                    {
                        sw.Write("Task 1");
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
                    await sw.WriteAsync(task);
                    result = "";
                }
            }
            catch(Exception ex)
            {
                throw new Exception("AddTaskAsync(" + task + ")", ex);
            }
            return result;
        }


        public List<TodoTask> GetAllTasks()
        {
            try
            {
                return ReadFiles();
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllTasks()", ex);
            }
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
                await System.Threading.Tasks.Task.Factory.StartNew(() => DeleteFile(path + guid + ".txt"));
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteTaskAsync(" + guid + ")", ex);
            }
        }

        private void DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
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
                FileInfo[] files = di.GetFiles("*.txt");
                foreach (FileInfo file in files)
                {
                    using (StreamReader sr = new StreamReader(path + file.Name))
                    {
                        result.Add(new TodoTask
                        {
                            guid = file.Name.Split('.')[0],
                            task = sr.ReadToEnd()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ReadFiles()", ex);
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
        }

    }

}
