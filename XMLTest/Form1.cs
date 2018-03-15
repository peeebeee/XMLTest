using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using QuickType;
using Newtonsoft.Json;


namespace XMLTest
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s;
            using (StreamReader sr = new StreamReader(@"c:\users\peter\documents\test.json"))
            {
                s = sr.ReadToEnd();
                sr.Close();
            }
            MessageBox.Show(s);
            var anprStuff = AnprStuff.FromJson(s);

        }
    }
}
