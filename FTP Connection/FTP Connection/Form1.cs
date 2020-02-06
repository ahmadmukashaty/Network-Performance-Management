using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using FTPConnection;

namespace FTP_Connection
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            
            InitializeComponent();
            IU2000FTPFileManager ftpClient = null;
            ftpClient = U200FtpMonitor.GetInstance();
            List<string> dims = null;
            //bool statue = ftpClient.FileIsExistInDirectory("MISDBA/Pulled", "67109391asdsa");
            //MessageBox.Show(statue.ToString());
            bool status = ftpClient.GetDimentionsFromDirectory("MISDBA/Pulled", "1526726684", ref dims);
            if (status == false)
            {
                MessageBox.Show("This subset isn't activated");
            }
            else
            {
                foreach (string dim in dims)
                    MessageBox.Show(dim);
            }

            //FTP ftpClient = null;

            //try
            //{
            //    ftpClient = new FTP("10.2.137.2", "tis", "Mm2000@123");
            //    List<string> dims = null;
            //    // subset example : 67109391
            //    bool status = ftpClient.GetDimentionsFromDirectory("MISDBA/Pulled", "112233", ref dims);
            //    if (status == false)
            //    {
            //        MessageBox.Show("This subset isn't activated");
            //    }
            //    else
            //    {
            //        foreach (string dim in dims)
            //            MessageBox.Show(dim);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //ftpClient = null;
        }

    }
}
