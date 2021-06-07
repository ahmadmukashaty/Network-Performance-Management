using Syriatel.NPM.Desktop.Designer;
using Syriatel.NPM.Desktop.Models.Classes;
using Syriatel.NPM.Desktop.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syriatel.NPM.Desktop
{

    public partial class Form1 : Form
    {
        private BoFile Bofile = null;
        private string userAlias = null;
        private int? userID,actionID = null;
        private ResponseDesigner responseDesigner = null;

        public Form1()
        {
            
            //1275071629_12-12-2017-3:15-PM
            InitializeComponent();
            loadingImage.Visible = false;
        }

        private void importFile_btn_Click(object sender, EventArgs e)
        {
            //test thing
            loadingImage.Visible = true;
            BoDesigner boDesigner = new BoDesigner("H69");
            boDesigner.OpenApplication();
            boDesigner.OpenUniverse();
            boDesigner.CloseUniverse();
            boDesigner.ExportUniverse();
            boDesigner.CloseApplication();
            loadingImage.Visible = false;

            
            try
            {
                string fileName = fileName_box.Text;
                if (fileName.Length > 0)
                {
                    loadingImage.Visible = true;
                    //get user alias by windows authentication
                    
                    userAlias = GetUserAliasName();
                    userID = OracleHelper.GetUserAliasID(userAlias.ToLower());
                    if (userID == null)
                    {
                        MessageBox.Show(Constants.Messages.UNAUTHORIZED_USER_ERROR);
                    }       
                    else
                    {
                        
                        actionID = OracleHelper.GetActionID(Constants.INSERT_FILE_TO_BO_ACTION);
                        if (actionID == null)
                        {
                            MessageBox.Show(Constants.Messages.FAILED_ACTION_ERROR);
                        }  
                        else
                        {
                            MessageBox.Show("line 1");
                            Bofile = OracleHelper.GetBoFileData(fileName);
                            MessageBox.Show("line 2");
                            if (Bofile == null)
                            {
                                MessageBox.Show("line 3");
                                MessageBox.Show(Constants.Messages.FILE_NOT_FOUND_ERROR);
                            }
                            else
                            {
                                MessageBox.Show("line 4");
                                bool designerOpened = CheckDesignerOpened();
                                MessageBox.Show("line 5");
                                if (designerOpened)
                                {
                                    MessageBox.Show("line 6");
                                    DialogResult dialogResult = MessageBox.Show(Constants.Messages.DESIGNER_ALREADY_OPENED_ERROR, Constants.BUSINESS_OBJECTS, MessageBoxButtons.YesNo);
                                    MessageBox.Show("line 7");
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        CloseDesignerApplication();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("line 8");
                                    responseDesigner = Bofile.InsertDataToBusinesObjects();
                                    MessageBox.Show("line 9");
                                    
                                    if (!responseDesigner.successed)
                                    {
                                        if (responseDesigner.file_type == null)
                                        {
                                            //add to history log
                                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Bofile.fileID, Constants.NO, responseDesigner.errorMessage);
                                            MessageBox.Show(Constants.Messages.UNEXPECTED_ERROR);
                                        }
                                        else if (responseDesigner.file_type == Constants.DEACTIVATE_COUNTER_TYPE)
                                        {
                                            //add to history log
                                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Bofile.fileID, Constants.NO, responseDesigner.errorMessage);
                                            MessageBox.Show(Constants.Messages.FAILED_DEACTIVATE_COUNTER_ERROR);
                                        }
                                        else
                                        {
                                            //add to history log
                                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Bofile.fileID, Constants.NO, responseDesigner.errorMessage);
                                            MessageBox.Show(Constants.Messages.FAILED_ADD_SUBSET_ERROR);
                                        }
                                    }
                                    else
                                    {
                                        //add to history log
                                        OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Bofile.fileID, Constants.YES, Constants.Messages.EMPTY_MESSAGE);
                                        MessageBox.Show(Constants.Messages.SUCCESSFUL_MESSAGE);
                                    }
                                }
                            }
                        }
                    }
                    loadingImage.Visible = false;
                }
                else
                {
                    MessageBox.Show(Constants.Messages.NO_FILE_NAME_ERROR);
                }
            }catch(Exception ex)
            {
                loadingImage.Visible = false;
                //add to history log
                if (userID != null)
                {
                    if (actionID == null)
                    {
                        OracleHelper.InsertDataInHistoryFileLog((int)userID, Constants.NO, ex.Message);
                    }
                    else
                    {
                        if (Bofile == null)
                        {
                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Constants.NO, ex.Message);
                        }
                        else
                        {
                            OracleHelper.InsertDataInHistoryFileLog((int)userID, (int)actionID, Bofile.fileID, Constants.NO, ex.Message);
                        }
                    }
                }
                MessageBox.Show(Constants.Messages.UNEXPECTED_ERROR);
            }
             
        }

        private void CloseDesignerApplication()
        {
            foreach (Process Proc in Process.GetProcesses())
            {
                if (Proc.ProcessName.Equals(Constants.DESIGNER_APPLICATION_NAME))  //Process designer?
                    Proc.Kill();
            }
        }

        private bool CheckDesignerOpened()
        {
            foreach (Process Proc in Process.GetProcesses())
            {
                if (Proc.ProcessName.Equals(Constants.DESIGNER_APPLICATION_NAME))  //Process designer?
                {
                    return true;
                }
                    
            }
            return false;
        }

        private string GetUserAliasName()
        {
            return Environment.UserName;
        }
    }
}
