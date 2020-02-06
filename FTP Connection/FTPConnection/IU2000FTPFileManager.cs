using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTPConnection
{
    public interface IU2000FTPFileManager
    {
        //  int number = ftpClient.GetFilesNumInDirectoryByHour("MISDBA/Pulled", filename, true)
        int GetFilesNumInDirectoryByHour(string directory, string fileName, bool Exists = true);

        bool GetDimentionsFromDirectory(string directory, string fileName, ref List<string> dimentions);

        bool FileIsExistInDirectory(string directory, string file);
    }

}
