using Designer;
using Syriatel.NPM.WebAPI.Models.BoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Syriatel.NPM.BoManager.Designer
{
    public class BODesigner
    {
        public Application application;
        public Universe universe;
        private string universe_name;
        private Class StartClass;

        //close window
        private const int WM_CLOSE = 16;
        private const int BN_CLICKED = 245;
        private int hwnd = 0;
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public BODesigner(string universe_name)
        {
            if (universe_name.ToUpper().Equals("H69"))
            {
                universe_name = "Huawei 6900 (R17)";
            }

            if (universe_name.ToUpper().Equals("GSN"))
            {
                universe_name = "Huawei GSN R14";
            }

            if (universe_name.ToUpper().Equals("NSS"))
            {
                universe_name = "HuaweiNSS R10";
            }
            this.universe_name = universe_name;
        }

        public void Open()
        {
            OpenApplication();
            OpenUniverse();
        }

        public void Close()
        {
            CloseUniverse();
            ExportUniverse();
            CloseApplication();
        }

        private void OpenApplication()
        {
            application = new Application();
            application.Visible = false;
            string UserName = System.Configuration.ConfigurationManager.AppSettings["DesignerUserName"];
            string Password = System.Configuration.ConfigurationManager.AppSettings["DesignerPassword"];
            string CmcName = System.Configuration.ConfigurationManager.AppSettings["DesignerCmcName"];
            string Authentiation = System.Configuration.ConfigurationManager.AppSettings["DesignerAuthentiation"];


            application.Logon(UserName, Password, CmcName, Authentiation);
            
        }

        private void OpenUniverse()
        {
            Thread thread = new Thread(DeleteImportUniverseMessage);
            thread.Start();
            this.universe = application.Universes.OpenFromEnterprise("/New DB", universe_name, true);
            //DeleteImportUniverseMessage();
            thread.Abort();
            //DeleteImportUniverseMessage();
        }

        private void CloseUniverse()
        {
            universe.Save();
            universe.SaveAs(@"\\TIS3\\Universes_test\\" + universe_name + ".unv");
            universe.Close();
        }

        private void ExportUniverse()
        {
            Thread thread = new Thread(DeleteConfirmExportUniverseMessage);
            thread.Start();
            application.Universes.Export("/New DB", @"\\TIS3\\Universes_test\\" + universe_name + ".unv", true);
            thread.Abort();
            //DeleteConfirmExportUniverseMessage();
        }


        private void CloseApplication()
        {
            application.Quit();
        }


        #region Hide Item  functions

        public void HideItem(string path, string ObjectName)
        {
            string stratClassName = GetFirstClassFromPath(path);
            path = RemoveFirstClassFromPath(path);
            StartClass = universe.Classes.get_FindClass(stratClassName);
            GetObject(StartClass, path, ObjectName);
        }

        private void GetObject(Class cls, string path, string ObjectName)
        {
            if (path != null)
            {
                string[] ClassesPath = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ClassesPath)
                {
                    Class CC = cls.Classes.get_FindClass(s);
                    cls = CC;
                }
            }
            foreach (IObject ob in cls.Objects)
            {
                if (ob.Name == ObjectName) { ob.Show = false; break; }

            }

        }

        #endregion


        #region support functions

        private string GetFirstClassFromPath(string path)
        {
            if (path != null)
            {
                string[] words = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return words[0];
            }
            return null;
        }

        private string RemoveFirstClassFromPath(string path)
        {
            string[] words = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1)
            {
                string newPath = "";
                for (int i = 1; i < words.Length; i++)
                {
                    newPath += (i == 1) ? (words[i]) : ("/" + words[i]);
                }
                return newPath;
            }
            return null;
        }

        private Class CreateNewPath(string[] tokens, int startIndex, Class cls)
        {
            if (startIndex == 0)
            {
                cls = universe.Classes.Add(tokens[startIndex]);
                startIndex++;
            }
            for (int i = startIndex; i < tokens.Length; i++)
            {
                cls = cls.Classes.Add(tokens[i]);
            }
            return cls;
        }

        #endregion


        #region Add Subset function

        public void AddSubsetToBO(SubsetModelView subsetModelView, bool newSubset, string boTableName)
        {
            if(newSubset)
            {
                boTableName = "M" + subsetModelView.subsetID;
                CreateBoTable(boTableName);
            }
            if (subsetModelView.measurements != null)
            {
                foreach (CounterModelView cmv in subsetModelView.measurements)
                {
                    Class cls = CheckAndAddNewPath(cmv.path);
                    CreateMeasurement(cls, cmv, boTableName);

                }
            }
            if (subsetModelView.dimentions != null)
            {
                foreach (DimantionModelView dim in subsetModelView.dimentions)
                {
                    Class cls = CheckAndAddNewPath(dim.path);
                    CreateDimention(cls, dim, boTableName);

                }
            }
        }

        private void CreateBoTable(string boTableName)
        {
            universe.Tables.Add(boTableName);
            Join TableJoin = universe.Joins.Add(boTableName + ".ID (+)=ME_SUB_LEV.ID");
            TableJoin.Cardinality = DsCardinality.dsManyToOneCardinality;
        }

        private Class CheckAndAddNewPath(string path)
        {
            string[] tokens = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Class cls = null;
            Class clsTemp = null;
            try { 
                clsTemp = universe.Classes.get_FindClass(tokens[0]);
            }
            catch (Exception ex) { clsTemp = null; }

            if (clsTemp == null)
            {
                cls = CreateNewPath(tokens, 0, cls);
            }
            else
            {
                cls = clsTemp;
                for (int i = 1; i < tokens.Length; i++)
                {
                    try{
                        clsTemp = universe.Classes.get_FindClass(tokens[i]);
                    }
                    catch (Exception ex) { clsTemp = null; }

                    if (clsTemp == null) { 
                        cls = CreateNewPath(tokens, i, cls);
                        break;
                    }
                    else { cls = clsTemp; }
                }
            }
            return cls;
        }

        private void CreateMeasurement(Class cls, CounterModelView counter, string boTableName)
        {
            IObject ob = cls.Objects.Add(counter.counterName);
            ob.Select = boTableName + "." + counter.shortName;
            ob.Qualification = DsObjectQualification.dsMeasureObject;
            ob.AggregateFunction = DsObjectAggregate.dsAggregateBySumObject;
            ob.HasListOfValues = true;
            ob.Type = DsObjectType.dsNumericObject;
            //string referance = ob.ListOfValues.Name;
            //System.Windows.Forms.MessageBox.Show(referance);
        }

        private void CreateDimention(Class cls, DimantionModelView dims, string boTableName)
        {
            IObject ob = cls.Objects.Add(dims.name);
            ob.Select = boTableName + "." + dims.name.Split(':')[1];
            ob.Qualification = DsObjectQualification.dsDimensionObject;
            ob.HasListOfValues = true;
            ob.Type = DsObjectType.dsCharacterObject;
            //string referance = ob.ListOfValues.Name;
            //System.Windows.Forms.MessageBox.Show(referance);
        }

        #endregion

#region Remove info messages

        private void DeleteImportUniverseMessage()
        {
            hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            do
            {
                hwnd = FindWindow(null, "Import Universe");
            } while (hwnd == 0);

            do
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "OK");
            } while (hwndChild == IntPtr.Zero);


            while (hwndChild != IntPtr.Zero)
            {
                SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "OK");

            }

            hwnd = 0;
            do
            {
                hwnd = FindWindow(null, "Import Universe");
            } while (hwnd == 0);

            while (hwnd != 0)
            {
                SendMessage(hwnd, WM_CLOSE, 0, IntPtr.Zero);
            }
            /*
            while (hwnd != 0)
            {
                SendMessage(hwnd, WM_CLOSE, 0, IntPtr.Zero);
            }*/
        }

        private void DeleteConfirmExportUniverseMessage()
        {
            hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            do
            {
                hwnd = FindWindow(null, "Warning");
            } while (hwnd == 0);

            do
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "&Yes");
            } while (hwndChild == IntPtr.Zero);


            while (hwndChild != IntPtr.Zero)
            {
                SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "&Yes");

            }
            
            //Thread.Sleep(10000);
            hwnd = 0;
            do
            {
                hwnd = FindWindow(null, "Export Universe");
            } while (hwnd == 0);

            while (hwnd != 0)
            {
                SendMessage(hwnd, WM_CLOSE, 0, IntPtr.Zero);
            }
        }




#endregion
    }
}
