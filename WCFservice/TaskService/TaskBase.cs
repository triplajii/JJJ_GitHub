using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace TaskService
{
    public class TaskBase
    {
        protected string path = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\";

        protected void Init()
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

        protected void DeleteFile(string file)
        {
            try
            {
                if (File.Exists(file + ".txt"))
                    File.Delete(file + ".txt");
                else if (File.Exists(file + ".dat"))
                    File.Delete(file + ".dat");
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteFile(" + file + ")", ex);
            }
        }


        protected List<TaskService.Task.TodoTask> ReadFiles()
        {
            List<TaskService.Task.TodoTask> result = new List<TaskService.Task.TodoTask>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles("*.*");
                foreach (FileInfo file in files)
                {
                    using (StreamReader sr = new StreamReader(path + file.Name))
                    {
                        TaskService.Task.TodoTask task = new TaskService.Task.TodoTask
                        {
                            guid = file.Name.Split('.')[0],
                            task = sr.ReadLine()
                        };
                        if (file.Extension == ".dat")
                        {
                            string date = sr.ReadLine();
                            task.date = StringToDate(date);
                            task.datestring = StringToUIDate(date);
                            task.time = sr.ReadLine();
                            
                        }
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

        protected int ReadFileCount()
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

        protected string TimeToString(DateTime time)
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

        protected DateTime StringToDate(string date)
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

        protected string StringToUIDate(string date)
        {
            try
            {
                return date.Substring(6, 2) + "." + date.Substring(4, 2) + "." + date.Substring(0, 4);
            }
            catch (Exception ex)
            {
                throw new Exception("StringToUIDate(" + date + ")", ex);
            }
        }

    }
}