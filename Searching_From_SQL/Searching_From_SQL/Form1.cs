using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Searching_From_SQL
{
    public partial class Form1 : Form
    {
        const int HT_CAPTION = 0x2;
        const int WM_NCLBUTTONDOWN = 0xA1;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public Form1()
        {
            InitializeComponent();            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }       
        

        public byte[] ConvertImageToByte(Image img)
        {
            using (MemoryStream ms=new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        
        public void SaveData()
        {
            string full_name = FullName.Text;
            string email = Email.Text;
            string password = Password.Text;
            byte[] img = ConvertImageToByte(DisplayPic.Image);
            var con = Sql_Configuration.getInstance().getConnection();            
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("Insert Into Login_Demo(Full_Name,Email,Password,Image) Values('" + full_name + "','" + email + "','" + password + "',@Image)", con);
            cmd.Parameters.AddWithValue("@Image", SqlDbType.Image).Value = img;
            cmd.CommandType=CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Sign Up Successfull!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveData();
            FullName.Text="";
            Email.Text="";
            Password.Text="";
        }

        public void Search_User()
        {
            string email = login_email.Text;
            string password = login_password.Text;
            string found_user = "";
            byte[] found_img = null;
            var con = Sql_Configuration.getInstance().getConnection();
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select Full_Name,Image From Login_Demo Where Email='"+email+"' AND Password='"+password+"'",con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader sdr=cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    found_user = sdr["Full_Name"].ToString();
                    found_img = (byte[])sdr["Image"];
                }                
                Form2 f2 = new Form2(found_user,found_img);
                this.Hide();
                f2.Show();
            }
            else
            {
                MessageBox.Show("Email ID or Password is Incorrect");
                //return false;
            }
            //return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Search_User();
        }       

        public void OpenBrowseDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                DisplayPic.Image = new Bitmap(open.FileName);
                FilePath.Text = open.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenBrowseDialog();
        }
    }
}
