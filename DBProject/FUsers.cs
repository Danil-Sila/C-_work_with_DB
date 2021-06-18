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
    public partial class FUsers : Form
    {
        public SqlConnection sqlConnection;
        public string conString = @"Data Source=ДАНИЛ-ПК\MYSQLEXPRESS;Initial Catalog=DBTourist;Integrated Security=True";
        public FUsers()
        {           
            InitializeComponent();
            sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
        }

        //Загрузка Пользователей
        public void Load_Users()
        {
            dataGridViewUsers.Rows.Clear();
            SqlCommand conUsers = new SqlCommand("SELECT u.UsersID, u.Ufio, u.ULogin, u.UPassword, r.RName  FROM Users u, Roles r WHERE u.URolesId = r.RolesId",sqlConnection);
            SqlDataReader readerUsers = null;
            try
            {
                readerUsers = conUsers.ExecuteReader();
                while (readerUsers.Read())
                {
                    dataGridViewUsers.Rows.Add(readerUsers[0].ToString(), readerUsers[1].ToString(), readerUsers[2].ToString(), readerUsers[3].ToString(), readerUsers[4].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (readerUsers != null) readerUsers.Close();
            }
        }
      

        private void FAddRoute_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
        }

        private void FUsers_Load(object sender, EventArgs e)
        {
            Load_Users(); //загрузка пользователей
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFIO.Text) && !string.IsNullOrWhiteSpace(tbFIO.Text) &&
                !string.IsNullOrEmpty(tbLogin.Text) && !string.IsNullOrWhiteSpace(tbLogin.Text) &&
                !string.IsNullOrEmpty(tbPassword.Text) && !string.IsNullOrWhiteSpace(tbPassword.Text) &&
                !string.IsNullOrEmpty(cbRoles.Text) && !string.IsNullOrWhiteSpace(cbRoles.Text))
            {
                SqlCommand URName = new SqlCommand("SELECT RolesID FROM Roles WHERE RName ='" + cbRoles.Text + "'", sqlConnection);
                string RolesId = URName.ExecuteScalar().ToString();
                SqlCommand comAddUser = new SqlCommand("INSERT INTO [Users] (Ufio, ULogin, UPassword, URolesId)VALUES(@fio, @log, @pas, @roles)", sqlConnection);
                comAddUser.Parameters.AddWithValue("fio", tbFIO.Text);
                comAddUser.Parameters.AddWithValue("log", tbLogin.Text);
                comAddUser.Parameters.AddWithValue("pas", tbPassword.Text);
                comAddUser.Parameters.AddWithValue("roles",RolesId);          
                comAddUser.ExecuteNonQuery();
                Load_Users();
                MessageBox.Show("Запись добавлена!","Информация",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Заполните поля!","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbRoles_Click(object sender, EventArgs e)
        {
            cbRoles.Items.Clear();
            SqlCommand comRole = new SqlCommand("SELECT RName FROM Roles",sqlConnection);
            SqlDataReader readerRole = null;
            try
            {
                readerRole = comRole.ExecuteReader();
                while (readerRole.Read())
                {
                    cbRoles.Items.Add(readerRole[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (readerRole != null) readerRole.Close();
            }
        }

        private void dataGridViewUsers_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                tbFIO.Text = "" + dataGridViewUsers.Rows[dataGridViewUsers.SelectedRows[0].Index].Cells[1].Value;
                tbLogin.Text = "" + dataGridViewUsers.Rows[dataGridViewUsers.SelectedRows[0].Index].Cells[2].Value;
                tbPassword.Text = "" + dataGridViewUsers.Rows[dataGridViewUsers.SelectedRows[0].Index].Cells[3].Value;
                cbRoles.Text = "" + dataGridViewUsers.Rows[dataGridViewUsers.SelectedRows[0].Index].Cells[4].Value;
            }
            else
            {
                tbFIO.Text = "";
                tbLogin.Text = "";
                tbPassword.Text = "";
                cbRoles.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var rez = MessageBox.Show("Удаление записи может повлиять на работу программы. Вы действительно хотите удалить запись?", "Вопрос?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (rez == DialogResult.Yes)
                {
                    int UserId = int.Parse(dataGridViewUsers[0, dataGridViewUsers.SelectedRows[0].Index].Value.ToString());
                    SqlCommand del_User = new SqlCommand("DELETE FROM Users WHERE UsersID=" + UserId + "", sqlConnection);
                    del_User.ExecuteNonQuery();
                    Load_Users();
                    MessageBox.Show("Запись удалена!","Информация",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }               
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var rez = MessageBox.Show("Изменение записи может повлиять на работу программы. Вы действительно хотите изменить запись?", "Вопрос?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rez == DialogResult.Yes)
                {
                    SqlCommand com = new SqlCommand("SELECT RolesID FROM Roles WHERE RName = '" + cbRoles.Text + "'", sqlConnection);
                    string RolesId = com.ExecuteScalar().ToString();
                    int UsrID = int.Parse(dataGridViewUsers[0, dataGridViewUsers.SelectedRows[0].Index].Value.ToString());
                    //SqlCommand comUsr = new SqlCommand("UPDATE Users SET Ufio = '" + tbFIO.Text + "', ULogin = '" + tbLogin.Text + "', UPassword = '" + tbPassword + "', URolesId = '" + RolesId + "' WHERE UsersID = " + 7 + "", sqlConnection);
                    //comUsr.ExecuteNonQuery();
                    SqlCommand comUpUser = new SqlCommand("UPDATE Users SET Ufio = '" + tbFIO.Text + "', ULogin = '" + tbLogin.Text + "', UPassword = '" + tbPassword.Text + "', URolesId = '" + RolesId + "' WHERE UsersId = " + UsrID + "", sqlConnection);
                    comUpUser.ExecuteNonQuery();
                    Load_Users();
                    MessageBox.Show("Запись изменена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }               
            }
            else
            {
                MessageBox.Show("Выберите строку для изменения","Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
