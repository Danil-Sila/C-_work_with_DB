using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBProject
{
    public partial class FAuthor : Form
    {
        SqlConnection sqlConnection;
        public string conString = @"Data Source=ДАНИЛ-ПК\MYSQLEXPRESS;Initial Catalog=DBTourist;Integrated Security=True";
        public string role;
        public string UserID;

        public FAuthor()
        {
            InitializeComponent();           
            sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
        } 

        public void button1_Click(object sender, EventArgs e)
        {
            int f = 0;
            SqlCommand com = new SqlCommand("SELECT u.ULogin, u.UPassword, r.RName, u.UsersID FROM Users u, Roles r WHERE u.URolesId = r.RolesId AND ULogin = '" + tbLog.Text+"' AND u.UPassword = '" + tbPas.Text + "'", sqlConnection);
            SqlDataReader readerCom = null;
            if (tbLog.Text != "" && tbPas.Text != "")
            {
                try
                {
                    readerCom = com.ExecuteReader();
                    while (readerCom.Read())
                    {
                        f = 1;
                        string login = readerCom[0].ToString();
                        string pass = readerCom[1].ToString();
                        role = readerCom[2].ToString();
                        UserID = readerCom[3].ToString();
                        MessageBox.Show("Здравствуйте  " + role, "Информация", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        tbLog.Text = "";
                        tbPas.Text = "";
                        Hide();  
                        FMain obj = new FMain();
                        obj.tb = role.ToUpper().Trim();
                        obj.ID = UserID;
                        obj.Show();
                        
                    }
                    if (f == 0) MessageBox.Show("Логин и пароль введены не верно!", "Внимание!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (readerCom != null) readerCom.Close();
                }
            }
            else
            {
                MessageBox.Show("Заполните поля Логин и Пароль!","ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }

        private void FAuthor_Load(object sender, EventArgs e)
        {

        }

        private void FAuthor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                tbPas.UseSystemPasswordChar = false;
            else tbPas.UseSystemPasswordChar = true;
        }
    }
}
