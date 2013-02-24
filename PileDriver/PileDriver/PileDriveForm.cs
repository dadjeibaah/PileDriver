using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PileDriver
{
    public partial class PileDriveForm : Form
    {
        Facade fMain = new Facade();
        GetNextChar control = new GetNextChar(); // Used for the get next char window
        TabPage GNCTab; //Tab for the Get Next Char window
        
      
        public PileDriveForm()
        {
            
            InitializeComponent();
            //Remember this? this is the bubbled event from the GetNextChar user control
            this.control.ButtonClick+=new EventHandler(control_ButtonClick);
            this.control.RstButtonClick+=new EventHandler(control_RstButtonClick);
        }

        private void PileDriveForm_Load(object sender, EventArgs e)
        {

        }

        private void bTLoadSource_Click(object sender, EventArgs e)
        {
            tBSource.Enabled = true;
            //function to request source directory and file load with information
            tBSource.Text = fMain.FileLoad(DIR_TYPE.SRC_D);
        }

        private void bTLoadMASM_Click(object sender, EventArgs e)
        {
            tBMASM.Enabled = true;
            //Loading of the MASM directory and directory info return
            tBMASM.Text = fMain.FileLoad(DIR_TYPE.MA);
        }

        /*Method:   Loadbtn_Click
          * Pre:     Event has been passed in
          * Post:    The source code is added to the tbSourceCde textbox and the filename has
         *           been added to textbox1 
          * */
        private void Loadbtn_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            string[] strings = fMain.LoadSourceFile();
            tbSourceCde.Text = strings[0];
            textBox1.Text = strings[1];
        }//Loadbtn_Click

        private void getNextCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGNCTab(); //creating the tab for GetNextChar in the TabControl
        }

        /*Method:   control_ButtonClick
          * Pre:     Events have been passed in
          * Post:    textbox2 from GetNextChar has been update with the current line number.
          *          textbox1 from GetNextChar has been update with the next character from
          *          the source code
          * */
        void control_ButtonClick(object sender, System.EventArgs e)
        {
            string[] disp;
            disp = fMain.NextCharAndLine();
            control.Controls["textbox2"].Text = disp[0];
            control.Controls["textbox1"].Text = disp[1];
        }//control_ButtonClick

        void control_RstButtonClick(object sender, System.EventArgs e)
        {
            fMain.ResetGNC();
            control.Controls["textbox2"].Text = "";
            control.Controls["textbox1"].Text = "";
        }
        /*Method:   CreateGNCTab()
          * Pre:     N/A
          * Post:    The main tab has a new tab with the user control GetNextChar
          * */
        private void CreateGNCTab()
        {
            if (GNCTab == null)
            {
                GNCTab = new TabPage("Get Next Char");
                GNCTab.Name = "GNCTab";
                GNCTab.Controls.Add(control);
                tCMainTab.TabPages.Add(GNCTab);
                
            }
            tCMainTab.SelectedTab = tCMainTab.TabPages["GNCTab"];
        }//CreateGNCTab

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tCMainTab.TabPages.Remove(tCMainTab.SelectedTab);
        }

        private void functionTestButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tCMainTab.SelectedTab = tCMainTab.TabPages["tPToken"];
            fMain.DisplayTokens();
            dataGridView1.DataSource = fMain.DisplayTable();
        }

        private void dumbSymbolTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fMain.isOutSourceDirLoaded())
            {
                fMain.CreateSymTableFile();
            }
            else MessageBox.Show("Please ensure that a source file is loaded and then dump sym table");
        }
    }
}
