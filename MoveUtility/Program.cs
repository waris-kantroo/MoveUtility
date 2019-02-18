using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveUtility
{
    class Program
    {
       static int filesMovedCount = 0;
        static int filesDeletedCount = 0;
        static void Main(string[] args)
        {
            try
            {
                string sourcepath = @ConfigurationManager.AppSettings["sourcepath"];
                string destinationPath = @ConfigurationManager.AppSettings["destinationPath"];
                DirectoryCopy(sourcepath, destinationPath);
                Console.WriteLine($"{filesMovedCount} number of files Moved");
                DeleteOldFies(destinationPath);
                Console.WriteLine($"{filesDeletedCount} number of files deleted");
                Console.WriteLine("Press any key to complete the operation");
                Console.Read();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DirectoryCopy(string strSource, string Copy_dest)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(strSource);

            DirectoryInfo[] directories = dirInfo.GetDirectories();

            FileInfo[] files = dirInfo.GetFiles();

            foreach (DirectoryInfo tempdir in directories)
            {
                Console.WriteLine(strSource + "/" + tempdir);

                Directory.CreateDirectory(Copy_dest + "/" + tempdir.Name);// creating the Directory   

                var ext = System.IO.Path.GetExtension(tempdir.Name);

                if (System.IO.Path.HasExtension(ext))
                {
                    foreach (FileInfo tempfile in files)
                    {
                        tempfile.CopyTo(Path.Combine(strSource + "/" + tempfile.Name, Copy_dest + "/" + tempfile.Name));

                    }
                }
                DirectoryCopy(strSource + "/" + tempdir.Name, Copy_dest + "/" + tempdir.Name);
            }

            FileInfo[] files1 = dirInfo.GetFiles();

            foreach (FileInfo tempfile in files1)
            {
                Directory.CreateDirectory(Copy_dest); // creating the Directory  if doesn't exist ... will create the ***empty directory***
                var fileTimeCreation = tempfile.CreationTime;
                if (tempfile.Extension.Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
                    {
                  //  fileTimeCreation = DateTime.Parse("8/28/2018 7:09:30 PM"); // not required in the real time
                    var months = Math.Round(DateTime.Now.Subtract(fileTimeCreation).Days / (365.25 / 12));
                    if (months >= 6)
                    {
                        if (!File.Exists(Path.Combine(Copy_dest, tempfile.Name)))
                        {
                            tempfile.MoveTo(Path.Combine(Copy_dest, tempfile.Name));
                            filesMovedCount++;
                        }
                        
                    }
                }
               
            }
        }

        public static void DeleteOldFies(string destPath)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(destPath);

            foreach (FileInfo file in di.GetFiles())
            {
                var fileTimeCreation = file.CreationTime;
                if (file.Extension.Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
                {
                    var months = Math.Round(DateTime.Now.Subtract(fileTimeCreation).Days / (365.25 / 12));
                    if (months >= 6)
                    {
                        file.Delete();
                        filesDeletedCount++;
                    }
                }
            }


            foreach (DirectoryInfo subfolder in di.GetDirectories())
            {
                foreach (FileInfo file in subfolder.GetFiles())
                {
                    var fileTimeCreation = file.CreationTime;
                    if (file.Extension.Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var months = Math.Round(DateTime.Now.Subtract(fileTimeCreation).Days / (365.25 / 12));
                        if (months >= 6)
                        {
                            file.Delete();
                            filesDeletedCount += filesDeletedCount;
                        }
                    }
                }

            }

        }

    }
}
