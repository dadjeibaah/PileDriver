using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PileDriver
{
    public partial class GetNextChar : UserControl
    {
        public event EventHandler ButtonClick; //Handler to be used in the Event Bubbling
        public event EventHandler RstButtonClick;
        public GetNextChar()
        {
            InitializeComponent();
        }

        //Capturing the click event on the button
        protected void button1_Click(object sender, EventArgs e)
        {
            //This incaspulates the button click and sends it up to the parent control
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, e);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            if (this.RstButtonClick != null)
            {
                this.RstButtonClick(this, e);
            }

        }
    }
}
