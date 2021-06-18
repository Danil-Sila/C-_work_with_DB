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
    public partial class FMain : Form
    {
        public int i, dos; //dos переменная доступа в работе программы
        SqlConnection sqlConnection;
        public string conString = @"Data Source=ДАНИЛ-ПК\MYSQLEXPRESS;Initial Catalog=DBTourist;Integrated Security=True";
        public string Rol; //роль пользователя
        public string ID; //ID пользователя
        public int s; //скидка
        public int q; //число для суммирования скидки
        public static FMain main { get; set; }
        public FMain()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
        }

        //Запись роли из авторизации
        public string tb
        {
            get {
                    return tbRole.Text;
                }
            set {
                    tbRole.Text = value;   
                }
        }

        //Загрузка параметров скидки и кол-ва
        public void downloadSale()
        {
            string sale, quant;
            try
            {
                SqlCommand com = new SqlCommand("Select sale FROM Sale WHERE SaleID ='1'", sqlConnection);
                sale = com.ExecuteScalar().ToString();
                com = new SqlCommand("Select quant FROM Sale  WHERE SaleID ='1'", sqlConnection);
                quant = com.ExecuteScalar().ToString();
                s = Convert.ToInt32(sale);
                q = Convert.ToInt32(quant);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
            }
        }

        //Загрузка данных о маршрутах
        public void Load_Route()
        {
            dataGridViewRoute.Rows.Clear();
            SqlDataReader sqlReader_Route = null;
            SqlCommand command_Route = new SqlCommand("SELECT * FROM [Route]", sqlConnection);
            try
            {
                sqlReader_Route = command_Route.ExecuteReader();
                while (sqlReader_Route.Read())
                {
                    dataGridViewRoute.Rows.Add(sqlReader_Route[0].ToString(), sqlReader_Route[1].ToString(), sqlReader_Route[2].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader_Route != null) sqlReader_Route.Close();
            }
        }

        //зугрузка данных об отелях
        public void Load_Hotel()
        {
            dataGridViewHotel.Rows.Clear();
            SqlDataReader sqlReader_Hotel = null;
            SqlCommand command_Hotel = new SqlCommand("SELECT Hotel.HotelId, Hotel.HotelName, Hotel.HCost, Hotel.HDuaration, Route.RCountry FROM Hotel, Route WHERE Hotel.HRouteId = Route.RouteId", sqlConnection);
            try
            {
                sqlReader_Hotel = command_Hotel.ExecuteReader();
                while (sqlReader_Hotel.Read())
                {
                    dataGridViewHotel.Rows.Add(sqlReader_Hotel[0].ToString(), sqlReader_Hotel[1].ToString(), sqlReader_Hotel[2].ToString(), sqlReader_Hotel[3].ToString(), sqlReader_Hotel[4].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader_Hotel != null) sqlReader_Hotel.Close();
            }
        }

        //загрузка данных о клиентах
        public void Load_Klient()
        {
            dataGridViewKlient.Rows.Clear();
            SqlDataReader sqlReader_Klient = null;
            SqlCommand command_Klient = new SqlCommand("SELECT * FROM [Klient]", sqlConnection);
            try
            {
                sqlReader_Klient = command_Klient.ExecuteReader();
                while (sqlReader_Klient.Read())
                {
                    dataGridViewKlient.Rows.Add(sqlReader_Klient[0].ToString(), sqlReader_Klient[1].ToString(), sqlReader_Klient[2].ToString(), sqlReader_Klient[3].ToString(), sqlReader_Klient[4].ToString(), sqlReader_Klient[5].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader_Klient != null) sqlReader_Klient.Close();
            }
        }

        //загузка данных о путёвках
        public void Load_Voucher()
        {
            dataGridViewVoucher.Rows.Clear();
            SqlDataReader sqlReader_Voucher = null;
            SqlCommand command_Voucher = new SqlCommand("SELECT Voucher.VoucherId, Voucher.VKlientId, Klient.KSurname, Hotel.HotelID, Hotel.HotelName, Voucher.VDateOtpr, Voucher.VSales, Voucher.VQuantity FROM Voucher, Klient, Hotel WHERE Voucher.VKlientID = Klient.KLientId and Voucher.VHotelId = Hotel.HotelId", sqlConnection);
            try
            {
                sqlReader_Voucher = command_Voucher.ExecuteReader();
                while (sqlReader_Voucher.Read())
                {
                    dataGridViewVoucher.Rows.Add(sqlReader_Voucher[0].ToString(), sqlReader_Voucher[1].ToString(), sqlReader_Voucher[2].ToString(), sqlReader_Voucher[3].ToString(), sqlReader_Voucher[4].ToString(), sqlReader_Voucher[5].ToString(), sqlReader_Voucher[7].ToString(), sqlReader_Voucher[6]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader_Voucher != null) sqlReader_Voucher.Close();
            }
        }

        //размер формы 
        public void size()
        {
            i = 0;
            this.Size = new Size(625, 500);
            button1.Text = "Добавление";
        }

        //закрытие GB для добавления
        public void gbVisible()
        {
            groupBoxRoute.Visible = false;
            groupBoxHotel.Visible = false;
            groupBoxKlient.Visible = false;
            groupBoxVoucher.Visible = false;
        }

        //расчёт цены
        public double cost(double c, int d)
        {
            double sum;
            sum = c * d * 7;
            return sum;
        }

        //показ gb 
        public void gbShow()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    groupBoxVoucher.Visible = true;
                    groupBoxVoucher.Size = new Size(254, 347);
                    groupBoxVoucher.Location = new Point(613, 32);
                    if (dos == 3)
                    {
                        button3.Enabled = true;
                        button1.Enabled = true;
                    }
                    if (dos == 4)
                    {
                        button3.Enabled = false;
                        button1.Enabled = false;
                    }
                    break;
                case 1:
                    groupBoxKlient.Visible = true;
                    groupBoxKlient.Size = new Size(254, 347);
                    groupBoxKlient.Location = new Point(613, 32);
                    if (dos == 3 || dos ==4)    
                    {
                        button3.Enabled = true;
                        button1.Enabled = true;
                    }
                    if (dos == 4)
                    {
                        button3.Enabled = false;
                        button1.Enabled = false;
                    }
                    break;
                case 2:
                    groupBoxHotel.Visible = true;
                    groupBoxHotel.Size = new Size(254, 347);
                    groupBoxHotel.Location = new Point(613, 32);
                    if (dos == 3 || dos ==4)
                    {
                        button3.Enabled = false;
                        button1.Enabled = false;
                    }
                    break;
                case 3:
                    groupBoxRoute.Visible = true;
                    groupBoxRoute.Size = new Size(254, 347);
                    groupBoxRoute.Location = new Point(613, 32);
                    if (dos == 3 || dos ==4)
                    {
                        button3.Enabled = false;
                        button1.Enabled = false;
                    }
                    break;

            }
        }

        //расчёт скидки
        public double Sale(int q, int pr, double s) {
            double sale;
            pr++;
            int ch;
            ch = q / pr;
            sale = s * ch;
           // MessageBox.Show(sale.ToString());
            return sale;
        }



        private void FMain_Load(object sender, EventArgs e)
        {
            size();
            //Добавление данных о клиентах
            Load_Klient();
            // добавление маршрутов
            Load_Route();
            //добавление данных о отелях
            Load_Hotel();
            //добавление данных о путёвках
            Load_Voucher();
            dos = 0;
            tbRole.Enabled = false;
            //Предоставление доступа для работы
            FAuthor avt = new FAuthor();
            if (tbRole.Text == "ОПЕРАТОР")
            {
                администрированиеToolStripMenuItem.Enabled = false;
            }
            if (tbRole.Text == "МЕНЕДЖЕР")
            {
                администрированиеToolStripMenuItem.Enabled = false;
                настройкиToolStripMenuItem.Enabled = false;
                dos = 3;
                gbShow();
            }
            if (tbRole.Text == "ОБЗОРЩИК")
            {               
                администрированиеToolStripMenuItem.Enabled = false;
                настройкиToolStripMenuItem.Enabled = false;
                dos = 4;
                gbShow();
            }
        }
        

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
            Form ifrm = Application.OpenForms[0];
            ifrm.Close();
        }

        private void dBTouristDataSetBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        //кнопка добавление: изменение размера формы + поля для добавления записей
        private void button1_Click(object sender, EventArgs e)
        {
            if (i == 0) {
                this.Size = new Size(890, 495);
                i = 1;
            }
            gbVisible();            //отображение Gropbox Vsible: fasle
            gbShow();               //показ группы полей для добавления
            button4.Enabled = true;
            
        }

        private void FMain_Activated(object sender, EventArgs e)
        {

        }

        //изменение записей
        private void button4_Click(object sender, EventArgs e)
        {
            double sum;
            downloadSale(); //получение параметров скидка-число
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    double sal;
                    if (dataGridViewVoucher.SelectedRows.Count > 0)
                    {                       
                        try
                        {
                            sal = Sale(Convert.ToInt32(tbVquant.Text), q, s); // расчёт скидки
                            int VoucherID = int.Parse(dataGridViewVoucher[0, dataGridViewVoucher.SelectedRows[0].Index].Value.ToString());
                            SqlCommand comUpVoucher = new SqlCommand("UPDATE Voucher SET  VKlientId='" + cbVid.Text + "', VHotelId = '" + cbVhotel.Text + "', VDateOtpr = '" + dtpVdate.Value.ToString("dd.MM.yyyy") + "', VSales = '" + sal.ToString() + "', VQuantity = '" + tbVquant.Text + "', VUserID = '" + ID + "' WHERE VoucherId = " + VoucherID + "", sqlConnection);
                            comUpVoucher.ExecuteNonQuery();
                            Load_Voucher();
                            MessageBox.Show("Запись изменена!","Информация",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK);
                        }                      
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для изменения.", "Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 1:
                    if (dataGridViewKlient.SelectedRows.Count > 0)
                    {
                        try
                        {
                            int KlientId = int.Parse(dataGridViewKlient[0, dataGridViewKlient.SelectedRows[0].Index].Value.ToString());
                            SqlCommand comUpKlient = new SqlCommand("UPDATE Klient SET KSurname = '" + tbKFamily.Text + "', KName = '" + tbKName.Text + "', KOtchestvo ='" + tbKOtches.Text + "', KAddress = '" + tbKAdres.Text + "', KPhone = '" + tbKPhone.Text + "', KUserID = '" + ID + "' WHERE KlientId = " + KlientId + "", sqlConnection);
                            comUpKlient.ExecuteNonQuery();
                            Load_Klient();
                            MessageBox.Show("Запись изменена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для изменения.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);                       
                    }                    
                    break;

                case 2:
                    
                    if (dataGridViewHotel.SelectedRows.Count > 0)
                    {
                        try
                        {
                            SqlCommand HRouteId = new SqlCommand("SELECT RouteId FROM Route WHERE RCountry ='" + cbHCountry.Text + "'", sqlConnection);
                            string RouteId = HRouteId.ExecuteScalar().ToString();
                            int HotelId = int.Parse(dataGridViewHotel[0, dataGridViewHotel.SelectedRows[0].Index].Value.ToString());
                            sum = cost(Convert.ToDouble(tbHCost.Text), Convert.ToInt32(cbHDuration.Text));
                            SqlCommand comUpHotel = new SqlCommand("UPDATE Hotel SET HotelName = '" + tbHName.Text + "', HCost = '" + sum.ToString() + "', HDuaration = '" + cbHDuration.Text + "', HRouteId = '" + RouteId + "', HUserID = '" + ID + "' WHERE HotelId = " + HotelId + "", sqlConnection);
                            comUpHotel.ExecuteNonQuery();
                            Load_Hotel();
                            MessageBox.Show("Запись изменена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для изменения.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                    
                case 3:
                    if (dataGridViewRoute.SelectedRows.Count > 0)
                    {
                        try
                        { 
                            int RouteId = int.Parse(dataGridViewRoute[0, dataGridViewRoute.SelectedRows[0].Index].Value.ToString());
                            SqlCommand comUpRoute = new SqlCommand("UPDATE Route SET RCountry = '" + tbRCountry.Text + "', RClimate = '" + cbRClimate.Text + "', RUserID = '" + ID + "' WHERE RouteId = " + RouteId + "", sqlConnection);
                            comUpRoute.ExecuteNonQuery();
                            Load_Route();
                            MessageBox.Show("Запись изменена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для изменения.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        //Администрирование - форма с пользователями
        private void button2_Click(object sender, EventArgs e)
        {
            FUsers obj = new FUsers();
            obj.Show();

        }
        //удаление записей
        private void button3_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    if (dataGridViewVoucher.SelectedRows.Count > 0 )
                    {
                        var rez = MessageBox.Show("Удаление может повлиять на работу программы! Вы действительно хотите удалить запись?","Внимание!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                        if (rez == DialogResult.Yes)
                        {
                            try
                            {
                                int VoucherId = int.Parse(dataGridViewVoucher[0, dataGridViewVoucher.SelectedRows[0].Index].Value.ToString());
                                SqlCommand delVoucher = new SqlCommand("DELETE FROM Voucher WHERE VoucherId = " + VoucherId + "", sqlConnection);
                                delVoucher.ExecuteNonQuery();
                                Load_Voucher();
                                MessageBox.Show("Запись удалена!","Информация",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
                            }
                        }       
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для удаления.", "Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 1:
                    if (dataGridViewKlient.SelectedRows.Count > 0)
                    {
                        var rez = MessageBox.Show("Удаление может повлиять на работу программы! Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rez == DialogResult.Yes)
                        {
                            try
                            {
                                int KlientId = int.Parse(dataGridViewKlient[0, dataGridViewKlient.SelectedRows[0].Index].Value.ToString());
                                SqlCommand delKlient = new SqlCommand("DELETE FROM Klient WHERE KlientId = " + KlientId + "", sqlConnection);
                                delKlient.ExecuteNonQuery();
                                Load_Klient();
                                MessageBox.Show("Запись удалена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для удаления.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case 2:
                    if (dataGridViewHotel.SelectedRows.Count > 0)
                    {
                        var rez = MessageBox.Show("Удаление может повлиять на работу программы! Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rez == DialogResult.Yes)
                        {
                            try
                            {
                                int HotelId = int.Parse(dataGridViewHotel[0, dataGridViewHotel.SelectedRows[0].Index].Value.ToString());
                                SqlCommand delHotel = new SqlCommand("DELETE FROM Hotel WHERE HotelId=" + HotelId + "", sqlConnection);
                                delHotel.ExecuteNonQuery();
                                Load_Hotel();
                                MessageBox.Show("Запись удалена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для удаления.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case 3:
                    if (dataGridViewRoute.SelectedRows.Count > 0)
                    {
                        var rez = MessageBox.Show("Удаление может повлиять на работу программы! Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rez == DialogResult.Yes)
                        {
                            try
                            {
                                int RouteId = int.Parse(dataGridViewRoute[0, dataGridViewRoute.SelectedRows[0].Index].Value.ToString());
                                SqlCommand del_Route = new SqlCommand("DELETE FROM Route WHERE RouteId=" + RouteId + "", sqlConnection);
                                del_Route.ExecuteNonQuery();
                                Load_Route();
                                MessageBox.Show("Запись удалена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для удаления.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }           
        }
        //добавление записей
        private void button6_Click(object sender, EventArgs e)
        {
            downloadSale(); //получение параметров скидка-число
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    double sal;
                    if (!string.IsNullOrEmpty(cbVid.Text) && !string.IsNullOrWhiteSpace(cbVid.Text) &&
                        !string.IsNullOrEmpty(cbVhotel.Text) && !string.IsNullOrWhiteSpace(cbVhotel.Text) &&
                        !string.IsNullOrEmpty(dtpVdate.Text) && !string.IsNullOrWhiteSpace(dtpVdate.Text) &&                        
                        !string.IsNullOrEmpty(tbVquant.Text) && !string.IsNullOrWhiteSpace(tbVquant.Text))
                    {
                        try
                        {
                            SqlCommand comAddVoucher = new SqlCommand("INSERT INTO [VOUCHER] (VKlientId, VHotelId, VDateOtpr, VSales, VQuantity, VUserID)VALUES(@KlientId, @HotelId, @Date, @Sale, @Quant, @ID)", sqlConnection);
                            comAddVoucher.Parameters.AddWithValue("KlientId", cbVid.Text);
                            comAddVoucher.Parameters.AddWithValue("HotelId", cbVhotel.Text);
                            comAddVoucher.Parameters.AddWithValue("Date", dtpVdate.Value.ToString("dd.MM.yyyy"));
                            sal = Sale(Convert.ToInt32(tbVquant.Text), q, s); //скидка намана
                            comAddVoucher.Parameters.AddWithValue("Sale", sal.ToString());
                            comAddVoucher.Parameters.AddWithValue("Quant", tbVquant.Text);
                            comAddVoucher.Parameters.AddWithValue("ID", ID);
                            comAddVoucher.ExecuteNonQuery();
                            Load_Voucher();
                            MessageBox.Show("Запись добавлена!","Информация",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(),ex.Source.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(tbKFamily.Text) && !string.IsNullOrWhiteSpace(tbKFamily.Text) &&
                        !string.IsNullOrEmpty(tbKName.Text) && !string.IsNullOrWhiteSpace(tbKName.Text) &&
                        !string.IsNullOrEmpty(tbKOtches.Text) && !string.IsNullOrWhiteSpace(tbKOtches.Text) &&
                        !string.IsNullOrEmpty(tbKAdres.Text) && !string.IsNullOrWhiteSpace(tbKAdres.Text) &&
                        !string.IsNullOrEmpty(tbKPhone.Text) && !string.IsNullOrWhiteSpace(tbKPhone.Text))
                    {
                        try
                        {                        
                            SqlCommand comAddKlient = new SqlCommand("INSERT INTO [Klient] (KSurname, KName, KOtchestvo, KAddress, KPhone, KUserID)VALUES(@Fam, @Name, @Otches, @Adr, @Phone, @ID)", sqlConnection);
                            comAddKlient.Parameters.AddWithValue("Fam", tbKFamily.Text);
                            comAddKlient.Parameters.AddWithValue("Name", tbKName.Text);
                            comAddKlient.Parameters.AddWithValue("Adr", tbKAdres.Text);
                            comAddKlient.Parameters.AddWithValue("Phone", tbKPhone.Text);
                            comAddKlient.Parameters.AddWithValue("Otches", tbKOtches.Text);
                            comAddKlient.Parameters.AddWithValue("ID", ID);
                            comAddKlient.ExecuteNonQuery();
                            tbKFamily.Text = "";
                            tbKName.Text = "";
                            tbKAdres.Text = "";
                            tbKPhone.Text = "";
                            tbKOtches.Text = "";
                            Load_Klient();
                            MessageBox.Show("Запись добавлена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case 2:

                    double sum;
                    if (!string.IsNullOrEmpty(tbHName.Text) && !string.IsNullOrWhiteSpace(tbHName.Text) &&
                        !string.IsNullOrEmpty(tbHCost.Text) && !string.IsNullOrWhiteSpace(tbHCost.Text) &&
                        !string.IsNullOrEmpty(cbHCountry.Text) && !string.IsNullOrWhiteSpace(cbHCountry.Text) &&
                        !string.IsNullOrEmpty(cbHDuration.Text) && !string.IsNullOrWhiteSpace(cbHDuration.Text))
                    {
                        try
                        { 
                            SqlCommand HRouteId = new SqlCommand("SELECT RouteId FROM Route WHERE RCountry ='" + cbHCountry.Text + "'", sqlConnection);
                            string RouteId = HRouteId.ExecuteScalar().ToString();
                            sum = cost(Convert.ToDouble(tbHCost.Text), Convert.ToInt32(cbHDuration.Text));
                            SqlCommand comAddHotel = new SqlCommand("INSERT INTO [Hotel] (HotelName, HCost, HDuaration, HRouteId)VALUES(@HotelName, @HCost, @HDuration, @HRouteId)", sqlConnection);
                            comAddHotel.Parameters.AddWithValue("HotelName", tbHName.Text);
                            comAddHotel.Parameters.AddWithValue("HCost", sum.ToString());
                            comAddHotel.Parameters.AddWithValue("HDuration", cbHDuration.Text);
                            comAddHotel.Parameters.AddWithValue("HRouteId", RouteId);
                            comAddHotel.ExecuteNonQuery();
                            tbHCost.Text = "";
                            tbHName.Text = "";
                            cbHDuration.Text = "";
                            cbHCountry.Text = "";
                            Load_Hotel();
                            MessageBox.Show("Запись добавлена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case 3:                   
                    if (!string.IsNullOrEmpty(tbRCountry.Text) && !string.IsNullOrWhiteSpace(tbRCountry.Text) &&
                        !string.IsNullOrEmpty(cbRClimate.Text) && !string.IsNullOrWhiteSpace(cbRClimate.Text))
                    {
                        try
                        { 
                            SqlCommand command = new SqlCommand("INSERT INTO [Route] (RCountry, RClimate, RUserID)VALUES(@RCountry,@RClimate,@ID)", sqlConnection);
                            command.Parameters.AddWithValue("RCountry", tbRCountry.Text);
                            command.Parameters.AddWithValue("RClimate", cbRClimate.Text);
                            command.Parameters.AddWithValue("ID", ID);
                            command.ExecuteNonQuery();
                            tbRCountry.Text = "";
                            cbRClimate.Text = "";
                            Load_Route();
                            MessageBox.Show("Запись добавлена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        //кнопка скрытия полей для добавления/изменения записей
        private void button7_Click(object sender, EventArgs e)
        {
            size();
            button4.Enabled = false;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {

        }
        //заполнение текстом TB для изменения
        private void dataGridViewRoute_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.SelectedRows.Count > 0)
            {
                tbRCountry.Text = "" + dataGridViewRoute.Rows[dataGridViewRoute.SelectedRows[0].Index].Cells[1].Value;
                cbRClimate.Text = "" + dataGridViewRoute.Rows[dataGridViewRoute.SelectedRows[0].Index].Cells[2].Value;
            }
            else
            {
                tbRCountry.Text = "";
                cbRClimate.Text = "";
            }
        }

        private void cbHCountry_Click(object sender, EventArgs e)
        {
            cbHCountry.Items.Clear();
            SqlDataReader reader_Country = null;                      
            SqlCommand Country = new SqlCommand("SELECT RCountry FROM Route",sqlConnection);
            try
            {
                reader_Country = Country.ExecuteReader();
                while (reader_Country.Read())
                {
                    cbHCountry.Items.Add(reader_Country[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reader_Country != null) reader_Country.Close();
            }                   
        }

        private void dataGridViewHotel_Click(object sender, EventArgs e)
        {
            if (dataGridViewHotel.SelectedRows.Count > 0)
            {
                tbHName.Text = "" + dataGridViewHotel.Rows[dataGridViewHotel.SelectedRows[0].Index].Cells[1].Value;
                tbHCost.Text = "" + dataGridViewHotel.Rows[dataGridViewHotel.SelectedRows[0].Index].Cells[2].Value;
                cbHDuration.Text = "" + dataGridViewHotel.Rows[dataGridViewHotel.SelectedRows[0].Index].Cells[3].Value;
                cbHCountry.Text = "" + dataGridViewHotel.Rows[dataGridViewHotel.SelectedRows[0].Index].Cells[4].Value;
            }
            else
            {
                tbHName.Text = "";
                tbHCost.Text = "";
                cbHDuration.Text = "";
                cbHDuration.Text = "";
            }
        }

        private void dataGridViewVoucher_Click(object sender, EventArgs e)
        {
            if (dataGridViewVoucher.SelectedRows.Count > 0)
            {
                cbVid.Text = "" + dataGridViewVoucher.Rows[dataGridViewVoucher.SelectedRows[0].Index].Cells[1].Value;
                cbVhotel.Text = "" + dataGridViewVoucher.Rows[dataGridViewVoucher.SelectedRows[0].Index].Cells[3].Value;
                dtpVdate.Text = "" + dataGridViewVoucher.Rows[dataGridViewVoucher.SelectedRows[0].Index].Cells[5].Value;
                tbVquant.Text = "" + dataGridViewVoucher.Rows[dataGridViewVoucher.SelectedRows[0].Index].Cells[6].Value;
                tbVhotel.Text = "" + dataGridViewVoucher.Rows[dataGridViewVoucher.SelectedRows[0].Index].Cells[7].Value;
            }
            else
            {
                cbVid.Text = "";
                cbVhotel.Text = "";
                dtpVdate.Text = "";
                tbVquant.Text = "";
                tbVhotel.Text = "";
            }
        }

        private void dataGridViewKlient_Click(object sender, EventArgs e)
        {
            if (dataGridViewKlient.SelectedRows.Count > 0)
            {
                tbKFamily.Text = "" + dataGridViewKlient.Rows[dataGridViewKlient.SelectedRows[0].Index].Cells[1].Value;
                tbKName.Text = "" + dataGridViewKlient.Rows[dataGridViewKlient.SelectedRows[0].Index].Cells[2].Value;
                tbKOtches.Text = "" + dataGridViewKlient.Rows[dataGridViewKlient.SelectedRows[0].Index].Cells[3].Value;
                tbKAdres.Text = "" + dataGridViewKlient.Rows[dataGridViewKlient.SelectedRows[0].Index].Cells[4].Value;
                tbKPhone.Text = "" + dataGridViewKlient.Rows[dataGridViewKlient.SelectedRows[0].Index].Cells[5].Value;
            }
            else
            {
                tbKFamily.Text = "";
                tbKName.Text = "";
                tbKOtches.Text = "";
                tbKAdres.Text = "";
                tbKPhone.Text = "";
            }
        }

        //отображение в зависимости от выбранного tabPage
        private void tabControl1_Click(object sender, EventArgs e)
        {
            gbVisible();
            gbShow();
        }

        //заполенение comboBox данными из таблиц
        private void cbVid_Click(object sender, EventArgs e)
        {
            cbVid.Items.Clear();
            SqlDataReader reader_Klient = null;
            SqlCommand KlientId = new SqlCommand("SELECT KlientId FROM Klient", sqlConnection);      
            try
            {
                reader_Klient = KlientId.ExecuteReader();
                while (reader_Klient.Read())
                {
                    cbVid.Items.Add(reader_Klient[0].ToString());
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reader_Klient != null) reader_Klient.Close();          
            }          
        }

        private void cbVhotel_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void cbVid_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void cbVid_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbVid.Text))
            {
                tbVklient.Clear();
                SqlCommand KlientName = new SqlCommand("SELECT KSurname FROM Klient WHERE KlientId=" + cbVid.Text + "", sqlConnection);
                SqlDataReader reader_KlientName = null;
                try
                {
                    reader_KlientName = KlientName.ExecuteReader();
                    while (reader_KlientName.Read())
                    {
                        tbVklient.Text = (reader_KlientName[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (reader_KlientName != null) reader_KlientName.Close();
                }
            }
            else tbVklient.Text = "";          
        }

        private void cbVhotel_Click(object sender, EventArgs e)
        {
            cbVhotel.Items.Clear();
            SqlDataReader reader_Hotel = null;
            SqlCommand HotelId = new SqlCommand("SELECT HotelID FROM Hotel", sqlConnection);
            try
            {
                reader_Hotel = HotelId.ExecuteReader();
                while (reader_Hotel.Read())
                {
                    cbVhotel.Items.Add(reader_Hotel[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reader_Hotel != null) reader_Hotel.Close();
            }
        }
        // заполнение CB полями для поиска
        private void comboBox1_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    cbFind.Items.Clear();
                    cbFind.Items.Add("ID Клиента");
                    cbFind.Items.Add("Клиент");
                    cbFind.Items.Add("Отель");
                    cbFind.Items.Add("Дата отправления");
                    cbFind.Items.Add("Количество");
                    cbFind.Items.Add("Скидка");
                    break;

                case 1:
                    cbFind.Items.Clear();
                    cbFind.Items.Add("ID Клиента");
                    cbFind.Items.Add("Фамилия");
                    cbFind.Items.Add("Имя");
                    cbFind.Items.Add("Адрес");
                    cbFind.Items.Add("Телефон");       
                    break;
                case 2:
                    cbFind.Items.Clear();
                    cbFind.Items.Add("Название отеля");
                    cbFind.Items.Add("Стоимость");
                    cbFind.Items.Add("Длительность");
                    cbFind.Items.Add("Страна");
                    break;
                case 3:
                    cbFind.Items.Clear();
                    cbFind.Items.Add("Страна");
                    cbFind.Items.Add("Климат");
                    break;
            }
        }

        //поиск
        private void button2_Click_1(object sender, EventArgs e)
        {
            i = 0;
            switch (tabControl1.SelectedIndex)
            {

                case 0:
                    if (cbFind.Text != "" && tbFind.Text != "")
                    {
                        dataGridViewVoucher.Rows.Clear();
                        if (cbFind.Text == "Клиент")
                        {
                            SqlCommand fKlient = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND k.KSurname LIKE '" + tbFind.Text+"%'",sqlConnection);
                            SqlDataReader readerKlient = null; 
                            try
                            {      
                                readerKlient = fKlient.ExecuteReader();
                                while (readerKlient.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerKlient[0].ToString(), readerKlient[1].ToString(), readerKlient[2].ToString(), readerKlient[3].ToString(), readerKlient[4].ToString(), readerKlient[5].ToString(), readerKlient[7].ToString(), readerKlient[6].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerKlient != null) readerKlient.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Отель")
                        {
                            SqlCommand fHotel = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND h.HotelName LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerHotel = null;
                            try
                            {
                                readerHotel = fHotel.ExecuteReader();
                                while (readerHotel.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerHotel[0].ToString(), readerHotel[1].ToString(), readerHotel[2].ToString(), readerHotel[3].ToString(), readerHotel[4].ToString(), readerHotel[5].ToString(), readerHotel[7].ToString(), readerHotel[6].ToString());
                                    i = 1;
                                }  
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerHotel != null) readerHotel.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Скидка")
                        {
                            SqlCommand fSales = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND v.VSales LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerSales = null;
                            try
                            {
                                readerSales = fSales.ExecuteReader();
                                while (readerSales.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerSales[0].ToString(), readerSales[1].ToString(), readerSales[2].ToString(), readerSales[3].ToString(), readerSales[4].ToString(), readerSales[5].ToString(), readerSales[7].ToString(), readerSales[6].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerSales != null) readerSales.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Количество")
                        {
                            SqlCommand fQuant = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND v.VQuantity LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerQuant = null;
                            try
                            {
                                readerQuant = fQuant.ExecuteReader();
                                while (readerQuant.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerQuant[0].ToString(), readerQuant[1].ToString(), readerQuant[2].ToString(), readerQuant[3].ToString(), readerQuant[4].ToString(), readerQuant[6].ToString(), readerQuant[5].ToString(), readerQuant[7].ToString(), readerQuant[6].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerQuant != null) readerQuant.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "ID Клиента")
                        {
                            SqlCommand fID = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND v.VKlientId LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerID = null;
                            try
                            {
                                readerID = fID.ExecuteReader();
                                while (readerID.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerID[0].ToString(), readerID[1].ToString(), readerID[2].ToString(), readerID[3].ToString(), readerID[4].ToString(), readerID[6].ToString(), readerID[5].ToString(), readerID[7].ToString(), readerID[6].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerID != null) readerID.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Дата отправления")
                        {
                           SqlCommand com = new SqlCommand("SELECT VDateOtpr FROM Voucher", sqlConnection);
                           string d = com.ExecuteScalar().ToString();                         
                           SqlCommand fDate = new SqlCommand("SELECT v.VoucherId, v.VKlientId, k.KSurname, h.HotelID, h.HotelName, v.VDateOtpr, v.VSales, v.VQuantity FROM  Voucher v, Hotel h, Klient k WHERE v.VKlientId = k.KlientId AND v.VHotelId = h.HotelId AND VDateOtpr LIKE '" + tbFind.Text + "%'", sqlConnection);
                           SqlDataReader readerDate = null;
                            try
                            {
                                readerDate = fDate.ExecuteReader();
                                while (readerDate.Read())
                                {
                                    dataGridViewVoucher.Rows.Add(readerDate[0].ToString(), readerDate[1].ToString(), readerDate[2].ToString(), readerDate[3].ToString(), readerDate[4].ToString(), readerDate[5].ToString(), readerDate[7].ToString(), readerDate[6].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerDate != null) readerDate.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля для поиска!","Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 1:
                    if (cbFind.Text != "" && tbFind.Text != "")
                    {
                        dataGridViewKlient.Rows.Clear();
                        if (cbFind.Text == "Фамилия")
                        {
                            SqlCommand fFam = new SqlCommand("SELECT * FROM Klient WHERE KSurname LIKE '"+tbFind.Text+"%'",sqlConnection);
                            SqlDataReader readerFam = null;
                            try
                            {
                                fFam.ExecuteReader();
                                while (readerFam.Read())
                                {
                                    i = 1;
                                    dataGridViewKlient.Rows.Add(readerFam[0].ToString(), readerFam[1].ToString(), readerFam[2].ToString(), readerFam[3].ToString(), readerFam[4].ToString(), readerFam[5].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerFam != null) readerFam.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "ID Клиента")
                        {
                            SqlCommand fID = new SqlCommand("SELECT * FROM Klient WHERE KlientID LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerID = null;
                            try
                            {
                                readerID = fID.ExecuteReader();
                                while (readerID.Read())
                                {
                                    dataGridViewKlient.Rows.Add(readerID[0].ToString(), readerID[1].ToString(), readerID[2].ToString(), readerID[3].ToString(), readerID[4].ToString(), readerID[5].ToString());
                                    i = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerID != null) readerID.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Имя")
                        {
                            SqlCommand fName = new SqlCommand("SELECT * FROM Klient WHERE KName LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerName = null;
                            try
                            {
                                readerName = fName.ExecuteReader();
                                while (readerName.Read())
                                {
                                    i = 1;
                                    dataGridViewKlient.Rows.Add(readerName[0].ToString(), readerName[1].ToString(), readerName[2].ToString(), readerName[3].ToString(), readerName[4].ToString(), readerName[5].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerName != null) readerName.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Адрес")
                        {
                            SqlCommand fAdr = new SqlCommand("SELECT * FROM Klient WHERE KAddress LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerAdr = null;
                            try
                            {
                                readerAdr = fAdr.ExecuteReader();
                                while (readerAdr.Read())
                                {
                                    i = 1;
                                    dataGridViewKlient.Rows.Add(readerAdr[0].ToString(), readerAdr[1].ToString(), readerAdr[2].ToString(), readerAdr[3].ToString(), readerAdr[4].ToString(), readerAdr[5].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerAdr != null) readerAdr.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Телефон")
                        {                           
                            SqlCommand fPhone = new SqlCommand("SELECT * FROM Klient WHERE KPhone LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerPhone = null;
                            try
                            {
                                readerPhone = fPhone.ExecuteReader();
                                while (readerPhone.Read())
                                {
                                    i = 1;
                                    dataGridViewKlient.Rows.Add(readerPhone[0].ToString(), readerPhone[1].ToString(), readerPhone[2].ToString(), readerPhone[3].ToString(), readerPhone[4].ToString(), readerPhone[5].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerPhone != null) readerPhone.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Заполните поля для поиска!","Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 2:
                    if (cbFind.Text != "" && tbFind.Text != "")
                    {
                        dataGridViewHotel.Rows.Clear();
                        if (cbFind.Text == "Название отеля")
                        {
                            SqlCommand fName = new SqlCommand("SELECT h.HotelId, h.HotelName, h.HCost, h.HDuaration, r.RCountry FROM Hotel h, Route r WHERE h.HRouteId = r.RouteId AND HotelName LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerName = null;
                            try
                            {
                                readerName = fName.ExecuteReader();
                                while (readerName.Read())
                                {
                                    i = 1;
                                    dataGridViewHotel.Rows.Add(readerName[0].ToString(), readerName[1].ToString(), readerName[2].ToString(), readerName[3].ToString(), readerName[4].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerName != null) readerName.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Стоимость")
                        {
                            SqlCommand fCost = new SqlCommand("SELECT h.HotelId, h.HotelName, h.HCost, h.HDuaration, r.RCountry FROM Hotel h, Route r WHERE h.HRouteId = r.RouteId AND HCost LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerCost = null;
                            try
                            {
                                readerCost = fCost.ExecuteReader();
                                while (readerCost.Read())
                                {
                                    i = 1;
                                    dataGridViewHotel.Rows.Add(readerCost[0].ToString(), readerCost[1].ToString(), readerCost[2].ToString(), readerCost[3].ToString(), readerCost[4].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerCost != null) readerCost.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Длительность")
                        {
                            SqlCommand fDuaration = new SqlCommand("SELECT h.HotelId, h.HotelName, h.HCost, h.HDuaration, r.RCountry FROM Hotel h, Route r WHERE h.HRouteId = r.RouteId AND HDuaration LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerDuaration = null;
                            try
                            {
                                readerDuaration = fDuaration.ExecuteReader();
                                while (readerDuaration.Read())
                                {
                                    i = 1;
                                    dataGridViewHotel.Rows.Add(readerDuaration[0].ToString(), readerDuaration[1].ToString(), readerDuaration[2].ToString(), readerDuaration[3].ToString(), readerDuaration[4].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerDuaration != null) readerDuaration.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (cbFind.Text == "Страна")
                        {
                            SqlCommand fCountry = new SqlCommand("SELECT h.HotelId, h.HotelName, h.HCost, h.HDuaration, r.RCountry FROM Hotel h, Route r WHERE h.HRouteId = r.RouteId AND r.RCountry LIKE'" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerCountry = null;
                            try
                            {
                                readerCountry = fCountry.ExecuteReader();
                                while (readerCountry.Read())
                                {
                                    i = 1;
                                    dataGridViewHotel.Rows.Add(readerCountry[0].ToString(), readerCountry[1].ToString(), readerCountry[2].ToString(), readerCountry[3].ToString(), readerCountry[4].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerCountry != null) readerCountry.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Заполните поля для поиска!","Ошибка.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;

                case 3:
                    if (cbFind.Text != "" && tbFind.Text != "")
                    {
                        dataGridViewRoute.Rows.Clear();
                        if (cbFind.Text == "Страна")
                        {
                            SqlCommand fRCountry = new SqlCommand("SELECT * FROM Route WHERE RCountry LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerRCountry = null;
                            try
                            {
                                readerRCountry = fRCountry.ExecuteReader();
                                while (readerRCountry.Read())
                                {
                                    i = 1;
                                    dataGridViewRoute.Rows.Add(readerRCountry[0].ToString(), readerRCountry[1].ToString(), readerRCountry[2].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerRCountry != null) readerRCountry.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            SqlCommand fClimate = new SqlCommand("SELECT * FROM Route WHERE RClimate LIKE '" + tbFind.Text + "%'", sqlConnection);
                            SqlDataReader readerRClimate = null;
                            try
                            {
                                readerRClimate = fClimate.ExecuteReader();
                                while (readerRClimate.Read())
                                {
                                    i = 1;
                                    dataGridViewRoute.Rows.Add(readerRClimate[0].ToString(), readerRClimate[1].ToString(), readerRClimate[2].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                if (readerRClimate != null) readerRClimate.Close();
                            }
                            if (i == 0) MessageBox.Show("Поиск не дал результатов!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните поля поиска!", "Ошибка.", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    break;
            }                 
        }
        //сброс результатов поиска
        private void button5_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {

                case 0:
                    Load_Voucher();
                    break;

                case 1:
                    Load_Klient();
                    break;

                case 2:
                    Load_Hotel();
                    break;

                case 3:
                    Load_Route();
                    break;
            }
        }
        //отображение подсказки при поиске по дате
        private void cbFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFind.Text == "Дата отправления")
            {
                label19.Visible = true;
                label22.Visible = true;
            }
            else
            {
                label19.Visible = false;
                label22.Visible = false;
            }
        }

        private void администрированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FUsers users = new FUsers();
            users.Show();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSettings set = new FSettings();
            set.ID = ID;
            set.Show();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rez = MessageBox.Show("Вы действительно хотите сменить пользователя?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rez == DialogResult.Yes)
            {
                Hide();
                FAuthor frm = new FAuthor();
                frm.Show();
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FInform frm = new FInform();
            frm.Show();
        }

        //заполнение CB данными о названии отелей
        private void cbVhotel_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbVhotel.Text))
            {
                SqlCommand com = new SqlCommand("SELECT HotelName FROM Hotel WHERE HotelID ='" +cbVhotel.Text+"'",sqlConnection);
                try
                {
                    tbVhotel.Text = com.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),ex.Source.ToString());
                }
            }
        }
    }
}
