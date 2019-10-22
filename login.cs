using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManager {
    public partial class login : Form {
        public login() {
            InitializeComponent();
        }
        public Boolean checkUser() {
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            bool checkUser = false, checkPw = false;
            if (txtUser.Text != "admin") {
                lblWUser.Visible = true;
            } else {
                checkUser = true;
            }
            if (txtPw.Text != "admin") {
                    lblWPw.Visible = true;
            } else {
                checkPw = true;
            }
            if (checkUser && checkPw) {
                Visible = false;
                ShowInTaskbar = false;
                main mainForm = new main();
                mainForm.Activate();
                mainForm.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void txtUser_TextChanged(object sender, EventArgs e) {
            txtUser.SelectAll();
            lblWUser.Visible = false;
        }

        private void txtUser_Click(object sender, EventArgs e) {
            txtUser.SelectAll();
            lblWUser.Visible = false;
        }

        private void txtPw_Click(object sender, EventArgs e) {
            txtPw.Text = "";
            lblWPw.Visible = false;
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
