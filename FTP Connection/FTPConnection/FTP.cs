using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace FTPConnection
{
    class FTP
    {
        private string host = null;
        private string user = null;
        private string pass = null;

        /// <summary>
        /// Initialize the FTP Connection using host , username and password
        /// </summary>
        public FTP(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }


        /// <summary>
        /// Get files Number inside directory and it can take string as a condition
        /// to get only files contains that string or don't contain that string
        /// contain or not contain condition depends on contain param value
        /// </summary>
        public int GetFilesNumInDirectory(string directory)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = host,
                    UserName = user,
                    Password = pass,
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    //get Remote Directory
                    RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);

                    return remoteDirectory.Files.Count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public Dictionary<string, int> GetFilesNumInDirectory_(string directory, string[] condition, bool contain)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = host,
                    UserName = user,
                    Password = pass,
                };
                Dictionary<string, int> file_count = new Dictionary<string, int>();
                using (Session session = new Session())
                {

                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);
                    foreach (string c in condition)
                    {
                        int filesNumber = 0;
                        foreach (RemoteFileInfo fileInfo in remoteDirectory.Files)
                        {

                            if (fileInfo.Name.Contains(c))
                                filesNumber += (contain ? 1 : 0);
                            else
                                filesNumber += (contain ? 0 : 1);

                        }
                        file_count.Add(c, filesNumber);
                        //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                        //   fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                    }
                    return file_count;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetFilesNumInDirectory(string directory, string condition, bool contain)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = host,
                    UserName = user,
                    Password = pass,
                };

                using (Session session = new Session())
                {
                    int filesNumber = 0;
                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);

                    foreach (RemoteFileInfo fileInfo in remoteDirectory.Files)
                    {
                        if (!fileInfo.Name.Contains(".."))
                        {

                            if (!fileInfo.Name.Contains("_1440_") && (!fileInfo.Name.Contains("_recover_")))
                                //  if ((one<=two) && (one>=three))



                                if (fileInfo.Name.Contains(condition))
                                    filesNumber += (contain ? 1 : 0);
                                else
                                    filesNumber += (contain ? 0 : 1);


                            //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                            //   fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                        }
                    }
                    return filesNumber;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool FileIsExistInDirectory(string directory, string file)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = host,
                    UserName = user,
                    Password = pass,
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);

                    foreach (RemoteFileInfo fileInfo in remoteDirectory.Files)
                    {
                        if (!fileInfo.Name.Contains(".."))
                        {
                            if (fileInfo.Name.Contains(file))
                                return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetFilesNumInDirectoryByHour(string directory, string condition, bool contain)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = host,
                    UserName = user,
                    Password = pass,
                };

                using (Session session = new Session())
                {
                    int filesNumber = 0;
                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);

                    foreach (RemoteFileInfo fileInfo in remoteDirectory.Files)
                    {
                        if (!fileInfo.Name.Contains(".."))
                        {
                            string one = fileInfo.Name.Split('_').ElementAt(1) + "_" + fileInfo.Name.Split('_').ElementAt(2) + "_" + fileInfo.Name.Split('_').ElementAt(3).Substring(0, 12);
                            string two = condition + "00";
                            string three = condition.Substring(0, condition.Length - 2) + "0000";
                            int tt = one.CompareTo(two);
                            int tt1 = one.CompareTo(three);
                            /*
                           int one;
                           string test = fileInfo.Name.Split('_').ElementAt(3).Substring(0, 10);
                           string two1=condition+"00";
                           int two;
                            int three;
                          int.TryParse(fileInfo.Name.Split('_').ElementAt(3).Substring(0, 10), out  one);
                          int.TryParse(two1, out two);
                          int.TryParse(condition.Substring(0, 8)+two1, out three);
                          !fileInfo.Name.Contains("_1440_") && */

                            if ((!fileInfo.Name.Contains("_recover_")))

                                if ((tt <= 0) && (tt1 >= 0))



                                    //    if (fileInfo.Name.Contains(condition))
                                    filesNumber += (contain ? 1 : 0);
                                else
                                    filesNumber += (contain ? 0 : 1);


                            //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                            //   fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                        }
                    }
                    session.Close();
                    return filesNumber;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool GetDimentionsFromDirectory(string directory, string subsetID, ref List<string> dimentions)
        {
            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = host,
                UserName = user,
                Password = pass,
            };
            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);

                RemoteDirectoryInfo remoteDirectory = session.ListDirectory(directory);

                foreach (RemoteFileInfo fileInfo in remoteDirectory.Files)
                {
                    // Is it a file with .txt extension?
                    if (!fileInfo.IsDirectory && fileInfo.Name.Contains(subsetID))
                    {
                        string sourceLocation = session.EscapeFileMask(directory + "/" + fileInfo.Name);

                        var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                        path = path.Substring(6);

                        string distDirectory = path + @"\download\";

                        // copy gz file to serve112
                        session.GetFiles(sourceLocation, distDirectory).Check();

                        DirectoryInfo directorySelected = new DirectoryInfo(distDirectory);

                        foreach (FileInfo fileToDecompress in directorySelected.GetFiles(fileInfo.Name))
                        {
                            string decompressedFile = Decompress(fileToDecompress);
                            using (var reader = new StreamReader(decompressedFile))
                            {
                                int i = 0;
                                while (!reader.EndOfStream)
                                {
                                    i++;
                                    var line = reader.ReadLine();
                                    if (i == 3)
                                    {
                                        var values = line.Split(';');
                                        dimentions = ExtractDimentions(values[0]);
                                        break;
                                    }
                                }
                            }
                        }

                        ClearDirecotry(directorySelected);
                        return true;
                    }
                }

                dimentions = null;
                return false;
            }
        }

        private void ClearDirecotry(DirectoryInfo directorySelected)
        {
            foreach (FileInfo file in directorySelected.GetFiles("*"))
            {
                try
                {
                    File.Delete(directorySelected + file.Name);
                }
                catch (Exception ex) { }
            }
        }

        private static string Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                    return newFileName;
                }

            }
        }


        private List<string> ExtractDimentions(string line)
        {
            try
            {
                List<string> dims = new List<string>();
                string subtest = line.Split('\"')[1];
                string subtest2 = subtest.Split('/')[1];
                string firstname = subtest2.Split(':')[0];
                string restname = subtest2.Split(':')[1];
                string[] arrString = restname.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string st in arrString)
                {
                    string dim = new string(st.Split('=')[0].ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
                    dims.Add(firstname + ":" + dim);
                }
                return dims;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
    public class U200FtpMonitor : IU2000FTPFileManager
    {
        private static U200FtpMonitor instatnce = null;
        private FTP ftpClient = null;
        public static U200FtpMonitor GetInstance()
        {

            if (instatnce == null)
            {
                instatnce = new U200FtpMonitor();
            }
            return instatnce;

        }
        private U200FtpMonitor()//string ip, string userName, string password)
        {
            ftpClient = new FTP("10.2.137.2", "tis", "Mm2000@123");
        }

        public int GetFilesNumInDirectoryByHour(string directory, string fileName, bool Exists = true)
        {
            return ftpClient.GetFilesNumInDirectoryByHour(directory, fileName, Exists);
        }
        public bool GetDimentionsFromDirectory(string directory, string fileName, ref List<string> dimentions)
        {
            return ftpClient.GetDimentionsFromDirectory(directory, fileName, ref dimentions);
        }

        public bool FileIsExistInDirectory(string directory, string file)
        {
            return ftpClient.FileIsExistInDirectory(directory, file);
        }
    }
}

