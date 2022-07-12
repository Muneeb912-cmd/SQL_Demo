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

namespace Searching_From_SQL
{
    public partial class Form2 : Form
    {
        public Form2(string found_User,byte[] found_img)
        {
            InitializeComponent();
            Fill_Form(found_User, found_img);
        }

        public Image ConvertBytetoImage(byte[] found_img)
        {
            using(MemoryStream ms=new MemoryStream(found_img))
            {
                return Image.FromStream(ms);
            }
        }

        public void Fill_Form(string found_User, byte[] found_img)
        {
           label1.Text = "Welcome Back " + found_User;
            pictureBox1.Image= ConvertBytetoImage(found_img);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
