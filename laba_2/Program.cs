using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Copier
{
    class CopyInfo
    {
        public string DirectoryName { get; set; }                                       
        public string Filename { get; set; }                                            
        public string Filename2 { get; set; }                                       
        public string DirectoryName2 { get; set; }                                      

        public string Destination
        {

            get
            {
                return Path.Combine(DirectoryName, Filename);                           
            }
        }
        public string Source
        {
            get
            {
                return Path.Combine(DirectoryName2, Filename2);
            }
        }
    }

    class Program
    {
        private static string Directory1;
        private static string Directory2;
       
        private static ConcurrentBag<Task<bool>> tasks = new ConcurrentBag<Task<bool>>(); 

        static void Main(string[] args)
        {
            
            Directory1 = Console.ReadLine();                                           
            Directory2 = Console.ReadLine();                                          
            CopyInfo copyInfo = new CopyInfo()                                            
            {
                DirectoryName = Directory2,
                Filename = string.Empty,
                Filename2 = string.Empty,
                DirectoryName2 = Directory1
            };

            if (Directory.Exists(copyInfo.Source))                                        
            {
                CreateDirectory(copyInfo.DirectoryName);
                RunTask(Copy, copyInfo);
            }
            else
            {
                Console.WriteLine("Данной директории не существует");
                Console.Read();
                Environment.Exit(0);
            }            
            string[] names = Directory.GetFiles(Directory1, "*.*", SearchOption.AllDirectories); 
            Console.WriteLine("Copied files: " + names.Length);                           
            Console.Read();
        }

        private static void CreateDirectory(string path)                                  
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static bool IsDirectory(string path)        
        {
            return Directory.Exists(path);
        }

        private static bool Copy(object state)
        {
            CopyInfo copyInfo = state as CopyInfo;
            if (IsDirectory(copyInfo.Destination))
            {
                RunTask(CopyDirectory, copyInfo);
            }
            else
            {
                RunTask(CopyFile, copyInfo);
            }
            return true;
        }

        private static bool CopyDirectory(object state)
        {
            CopyInfo copyInfo = state as CopyInfo;
            CreateDirectory(copyInfo.Destination);
            foreach (string dir in Directory.GetDirectories(copyInfo.Source))
            {
                string d = dir.Replace(copyInfo.DirectoryName2, string.Empty);   
                d = d.Substring(1);                                                       
                CopyInfo newCopyInfo = new CopyInfo()
                {
                    DirectoryName = copyInfo.Destination,
                    Filename = d,
                    Filename2 = string.Empty,
                    DirectoryName2 = dir
                };
                RunTask(CopyDirectory, newCopyInfo);
            }
            foreach (string file in Directory.GetFiles(copyInfo.Source))
            {
                string f = file.Replace(copyInfo.DirectoryName2, string.Empty);
                f = f.Substring(1);
                CopyInfo newCopyInfo = new CopyInfo()
                {
                    DirectoryName = copyInfo.Destination,
                    Filename = f,
                    Filename2 = f,
                    DirectoryName2 = copyInfo.DirectoryName2
                };
                RunTask(Copy, newCopyInfo);
            }
            return true;
        }

        private static bool CopyFile(object state)
        {
            CopyInfo copyInfo = state as CopyInfo;
            File.Copy(copyInfo.Source, copyInfo.Destination);
            return true;
        }

        private static void RunTask(Func<object, bool> func, object param)
        {
            Task<bool> task = new Task<bool>(func, param);
            task.Start();
            tasks.Add(task);
        }
    }
}
