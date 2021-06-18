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
    public partial class FSettings : Form
    {
        public SqlConnection sqlConnection;
        public string conString = @"Data Source=ДАНИЛ-ПК\MYSQLEXPRESS;Initial Catalog=DBTourist;Integrated Security=True";
        public string ID;
        public FSettings()
        {           
            InitializeComponent();
            sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
        }

        public void refresh()
        {
            try
            {
                SqlCommand com = new SqlCommand("Select sale FROM Sale WHERE SaleID ='1'", sqlConnection);
                tbSale.Text = com.ExecuteScalar().ToString();
                com = new SqlCommand("Select quant FROM Sale WHERE SaleID ='1'", sqlConnection);
                tbQuant.Text = com.ExecuteScalar().ToString();           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
            }
        }

        private void FSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
        }

        private void FSettings_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSale.Text) && !string.IsNullOrWhiteSpace(tbSale.Text)
                && !string.IsNullOrEmpty(tbQuant.Text) && !string.IsNullOrWhiteSpace(tbQuant.Text))
            {
                var rez = MessageBox.Show("Сохранение настроек повлекут изменения! Вы действительно хотите сохранить новые параметры?","Внимание", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (rez == DialogResult.Yes)
                {
                    try
                    {
                        SqlCommand com = new SqlCommand("UPDATE Sale SET sale='" + tbSale.Text + "', quant='" + tbQuant.Text + "', UserId = '" + ID + "' WHERE SaleID='1'", sqlConnection);
                        com.ExecuteNonQuery();
                        MessageBox.Show("Настройки сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
                    }
                }               
            }
            else
            {
                MessageBox.Show("Заполние поля скидка и количество.","ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        // сменить пользователя
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
