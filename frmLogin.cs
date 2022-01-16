using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            pnlLogin.Visible = true;
            pnlCode.Visible = false;
            pnlLogin.Dock = DockStyle.Top;
            //pnlLogin.Location = new Point(42, 80);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            using (RNETEntities db = new RNETEntities())
            {
                var users = db.Users;
                User currUser = null; 
                bool flag = false;
                foreach (User u in users)
                {
                    if (u.UserName.ToUpper() == txtUser.Text.ToUpper() && u.UserPassword == txtPsw.Text)
                    {
                        flag = true;
                        currUser = u;
                        break;
                    }
                }
                if (!flag)
                    MessageBox.Show("User was not found");
                else
                {
                    if (pnlLogin.Visible)
                    {
                        Random rnd = new Random();
                        int otp = rnd.Next(1000, 10000);
                        currUser.OTP = otp.ToString();
                        db.SaveChanges();
                        string errMsg = String.Empty;
                        if(!SendMail(currUser.UserMail, otp.ToString(), out errMsg))
                        {
                            MessageBox.Show(errMsg);
                            return;
                        }
                        pnlLogin.Visible = false;
                        pnlCode.Visible = true;
                        pnlCode.Dock = DockStyle.Top;
                        //pnlCode.Location = new Point(48, 90);
                        //pnlCode.Width = 350;
                        //pnlCode.Height = 445;
                    }
                    else
                    {
                        if (currUser.OTP == txtOTP.Text)
                        {
                            currUser.OTP = string.Empty;
                            db.SaveChanges();
                            DialogResult = DialogResult.OK;
                        }
                        else
                            MessageBox.Show("Code is wrong");
                    }
                        
                }
            }


            

            //DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool SendMail(string email, string otp, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                MailAddress from = new MailAddress("EMailTester202201@gmail.com", "Test Project");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Check";
                m.Body = otp;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("EMailTester202201@gmail.com", "Aa12Bb&&");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
