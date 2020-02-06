using Designer;
using Syriatel.NPM.Desktop.Models.Classes;
using Syriatel.NPM.Desktop.Models.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syriatel.NPM.Desktop.Designer
{
    public class BoDesigner
    {
        private Application application;
        private Universe universe;
        private string universe_name;
        private Class StartClass;


        public BoDesigner(string universe_name)
        {
            if (universe_name.ToUpper().Equals("H69"))
            {
                universe_name = "Huawei 6900 (R17)";
                //universe_name = "H6900 R17 TEST";
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

        public Boolean OpenApplication()
        {
            application = new Application();
            string UserName = System.Configuration.ConfigurationManager.AppSettings["DesignerUserName"];
            string Password = System.Configuration.ConfigurationManager.AppSettings["DesignerPassword"];
            string CmcName =  System.Configuration.ConfigurationManager.AppSettings["DesignerCmcName"];
            string Authentiation = System.Configuration.ConfigurationManager.AppSettings["DesignerAuthentiation"];


            application.Logon(UserName, Password, CmcName, Authentiation);
            Console.Write("Done");
            return true;
        }

        public Boolean OpenUniverse()
        {
            this.universe = application.Universes.OpenFromEnterprise("/New DB", universe_name, false);
            return true;
        }

        public Boolean CloseUniverse()
        {
            universe.Save();
            universe.SaveAs(@"\\TIS3\\Universes_test\\" + universe_name + ".unv");
            universe.Close();
            return true;
        }

        public Boolean ExportUniverse()
        {
            application.Universes.Export("/New DB", @"\\TIS3\\Universes_test\\" + universe_name + ".unv", true);
            return true;
        }

        public Boolean CloseApplication()
        {
            foreach (Process Proc in Process.GetProcesses())
            {
                if (Proc.ProcessName.Equals("designer"))  //Process designer?
                    Proc.Kill();
            }
            return true;
        }
        

        #region Hide Item  functions

        public void HideItem(string path, string ObjectName)
        {
            ResponseDesigner res = new ResponseDesigner();
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
                if (ob.Name.ToLower() == ObjectName.ToLower()) { ob.Show = false; break; }

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

        public void AddSubsetToBO(FileTypeAddSubset subsetModelView, string boTableName)
        {
            bool tableExist = CheckTableExistInBo(boTableName);

            if (!tableExist)
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

        private bool CheckTableExistInBo(string boTableName)
        {
            try
            {
                universe.Tables.get_Item(boTableName);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void CreateBoTable(string boTableName)
        {
            universe.Tables.Add(boTableName);
            Join TableJoin = universe.Joins.Add(boTableName + ".ID (+)=ME.ID");
            TableJoin.Cardinality = DsCardinality.dsManyToOneCardinality;
        }

        private Class CheckAndAddNewPath(string path)
        {
            string[] tokens = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(tokens);
            Class cls = null;
            Class clsTemp = null;
            try
            {
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
                    try
                    {
                        clsTemp = universe.Classes.get_FindClass(tokens[i]);
                    }
                    catch (Exception ex) { clsTemp = null; }

                    if (clsTemp == null)
                    {
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
    }
}
