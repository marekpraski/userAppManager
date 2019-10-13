using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public class Form2EventArgs : EventArgs
    {
        public string text { get; set; }
    }

    public partial class Form2 : Form
    {
       
        public delegate void textTypedEventHandler(object sender, Form2EventArgs args);

        public event textTypedEventHandler ttEvent;

        public Form2()
        {
            InitializeComponent();
        }

        public void OnttEvent(object sender, EventArgs args)
        {
            if(ttEvent != null)
            {
                Form2EventArgs arg = new Form2EventArgs();
                arg.text = this.textBox1.Text;
                ttEvent(this, arg);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            OnttEvent(sender, e);
        }
    }
}
