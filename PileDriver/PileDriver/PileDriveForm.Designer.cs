namespace PileDriver
{
    partial class PileDriveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tCMainTab = new System.Windows.Forms.TabControl();
            this.TbOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tPSource = new System.Windows.Forms.TabPage();
            this.tbSourceCde = new System.Windows.Forms.TextBox();
            this.tPToken = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getNextCharacterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionTestButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tBSource = new System.Windows.Forms.TextBox();
            this.tBMASM = new System.Windows.Forms.TextBox();
            this.Loadbtn = new System.Windows.Forms.Button();
            this.bTLoadSource = new System.Windows.Forms.Button();
            this.bTLoadMASM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dumbSymbolTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tCMainTab.SuspendLayout();
            this.TbOptions.SuspendLayout();
            this.tPSource.SuspendLayout();
            this.tPToken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.TopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tCMainTab
            // 
            this.tCMainTab.ContextMenuStrip = this.TbOptions;
            this.tCMainTab.Controls.Add(this.tPSource);
            this.tCMainTab.Controls.Add(this.tPToken);
            this.tCMainTab.Controls.Add(this.tabPage3);
            this.tCMainTab.Location = new System.Drawing.Point(12, 30);
            this.tCMainTab.Name = "tCMainTab";
            this.tCMainTab.SelectedIndex = 0;
            this.tCMainTab.Size = new System.Drawing.Size(686, 288);
            this.tCMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tCMainTab.TabIndex = 0;
            // 
            // TbOptions
            // 
            this.TbOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeTabToolStripMenuItem});
            this.TbOptions.Name = "TbOptions";
            this.TbOptions.Size = new System.Drawing.Size(163, 26);
            // 
            // closeTabToolStripMenuItem
            // 
            this.closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
            this.closeTabToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.closeTabToolStripMenuItem.Text = "Close Active Tab";
            this.closeTabToolStripMenuItem.Click += new System.EventHandler(this.closeTabToolStripMenuItem_Click);
            // 
            // tPSource
            // 
            this.tPSource.Controls.Add(this.tbSourceCde);
            this.tPSource.Location = new System.Drawing.Point(4, 22);
            this.tPSource.Name = "tPSource";
            this.tPSource.Padding = new System.Windows.Forms.Padding(3);
            this.tPSource.Size = new System.Drawing.Size(678, 262);
            this.tPSource.TabIndex = 0;
            this.tPSource.Text = "Source Code";
            this.tPSource.UseVisualStyleBackColor = true;
            // 
            // tbSourceCde
            // 
            this.tbSourceCde.Location = new System.Drawing.Point(6, 6);
            this.tbSourceCde.Multiline = true;
            this.tbSourceCde.Name = "tbSourceCde";
            this.tbSourceCde.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSourceCde.Size = new System.Drawing.Size(666, 250);
            this.tbSourceCde.TabIndex = 0;
            // 
            // tPToken
            // 
            this.tPToken.Controls.Add(this.dataGridView1);
            this.tPToken.Location = new System.Drawing.Point(4, 22);
            this.tPToken.Name = "tPToken";
            this.tPToken.Padding = new System.Windows.Forms.Padding(3);
            this.tPToken.Size = new System.Drawing.Size(678, 262);
            this.tPToken.TabIndex = 1;
            this.tPToken.Text = "Token List";
            this.tPToken.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Size = new System.Drawing.Size(672, 256);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(678, 262);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Emmiter";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TopMenu
            // 
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.functionsToolStripMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(709, 24);
            this.TopMenu.TabIndex = 1;
            this.TopMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDirectoriesToolStripMenuItem,
            this.openSourceFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDirectoriesToolStripMenuItem
            // 
            this.openDirectoriesToolStripMenuItem.Name = "openDirectoriesToolStripMenuItem";
            this.openDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openDirectoriesToolStripMenuItem.Text = "Open Directories...";
            // 
            // openSourceFileToolStripMenuItem
            // 
            this.openSourceFileToolStripMenuItem.Name = "openSourceFileToolStripMenuItem";
            this.openSourceFileToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openSourceFileToolStripMenuItem.Text = "Open Source File...";
            // 
            // functionsToolStripMenuItem
            // 
            this.functionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getNextCharacterToolStripMenuItem,
            this.functionTestButtonToolStripMenuItem,
            this.dumbSymbolTableToolStripMenuItem});
            this.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            this.functionsToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.functionsToolStripMenuItem.Text = "Functions";
            // 
            // getNextCharacterToolStripMenuItem
            // 
            this.getNextCharacterToolStripMenuItem.Name = "getNextCharacterToolStripMenuItem";
            this.getNextCharacterToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.getNextCharacterToolStripMenuItem.Text = "Get Next Character...";
            this.getNextCharacterToolStripMenuItem.Click += new System.EventHandler(this.getNextCharacterToolStripMenuItem_Click);
            // 
            // functionTestButtonToolStripMenuItem
            // 
            this.functionTestButtonToolStripMenuItem.Name = "functionTestButtonToolStripMenuItem";
            this.functionTestButtonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.functionTestButtonToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.functionTestButtonToolStripMenuItem.Text = "Get Token List";
            this.functionTestButtonToolStripMenuItem.Click += new System.EventHandler(this.functionTestButtonToolStripMenuItem_Click);
            // 
            // tBSource
            // 
            this.tBSource.Enabled = false;
            this.tBSource.Location = new System.Drawing.Point(101, 378);
            this.tBSource.Name = "tBSource";
            this.tBSource.Size = new System.Drawing.Size(379, 20);
            this.tBSource.TabIndex = 2;
            // 
            // tBMASM
            // 
            this.tBMASM.Enabled = false;
            this.tBMASM.Location = new System.Drawing.Point(101, 416);
            this.tBMASM.Name = "tBMASM";
            this.tBMASM.Size = new System.Drawing.Size(379, 20);
            this.tBMASM.TabIndex = 3;
            // 
            // Loadbtn
            // 
            this.Loadbtn.Location = new System.Drawing.Point(498, 337);
            this.Loadbtn.Name = "Loadbtn";
            this.Loadbtn.Size = new System.Drawing.Size(75, 23);
            this.Loadbtn.TabIndex = 5;
            this.Loadbtn.Text = "Load";
            this.Loadbtn.UseVisualStyleBackColor = true;
            this.Loadbtn.Click += new System.EventHandler(this.Loadbtn_Click);
            // 
            // bTLoadSource
            // 
            this.bTLoadSource.Location = new System.Drawing.Point(498, 376);
            this.bTLoadSource.Name = "bTLoadSource";
            this.bTLoadSource.Size = new System.Drawing.Size(75, 23);
            this.bTLoadSource.TabIndex = 6;
            this.bTLoadSource.Text = "Browse";
            this.bTLoadSource.UseVisualStyleBackColor = true;
            this.bTLoadSource.Click += new System.EventHandler(this.bTLoadSource_Click);
            // 
            // bTLoadMASM
            // 
            this.bTLoadMASM.Location = new System.Drawing.Point(498, 416);
            this.bTLoadMASM.Name = "bTLoadMASM";
            this.bTLoadMASM.Size = new System.Drawing.Size(75, 23);
            this.bTLoadMASM.TabIndex = 7;
            this.bTLoadMASM.Text = "Browse";
            this.bTLoadMASM.UseVisualStyleBackColor = true;
            this.bTLoadMASM.Click += new System.EventHandler(this.bTLoadMASM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 385);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Source Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 421);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "MASM Directory";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(101, 339);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(379, 20);
            this.textBox1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 346);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Source";
            // 
            // dumbSymbolTableToolStripMenuItem
            // 
            this.dumbSymbolTableToolStripMenuItem.Name = "dumbSymbolTableToolStripMenuItem";
            this.dumbSymbolTableToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.dumbSymbolTableToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.dumbSymbolTableToolStripMenuItem.Text = "Dumb Symbol Table";
            this.dumbSymbolTableToolStripMenuItem.Click += new System.EventHandler(this.dumbSymbolTableToolStripMenuItem_Click);
            // 
            // PileDriveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 449);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bTLoadMASM);
            this.Controls.Add(this.bTLoadSource);
            this.Controls.Add(this.Loadbtn);
            this.Controls.Add(this.tBMASM);
            this.Controls.Add(this.tBSource);
            this.Controls.Add(this.tCMainTab);
            this.Controls.Add(this.TopMenu);
            this.MainMenuStrip = this.TopMenu;
            this.Name = "PileDriveForm";
            this.Text = "PileDriver";
            this.Load += new System.EventHandler(this.PileDriveForm_Load);
            this.tCMainTab.ResumeLayout(false);
            this.TbOptions.ResumeLayout(false);
            this.tPSource.ResumeLayout(false);
            this.tPSource.PerformLayout();
            this.tPToken.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.TabControl tCMainTab;
        private System.Windows.Forms.TabPage tPSource;
        private System.Windows.Forms.TabPage tPToken;
        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TextBox tBSource;
        private System.Windows.Forms.TextBox tBMASM;
        private System.Windows.Forms.Button Loadbtn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button bTLoadSource;
        private System.Windows.Forms.Button bTLoadMASM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSourceCde;
        private System.Windows.Forms.ToolStripMenuItem functionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getNextCharacterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSourceFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip TbOptions;
        private System.Windows.Forms.ToolStripMenuItem closeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionTestButtonToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem dumbSymbolTableToolStripMenuItem;
    }
}

