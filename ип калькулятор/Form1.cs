using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ип_калькулятор
{
    public partial class Form1 : Form
    {
        //Подключение бд
        db db = new db();
        User u = new User();
        private const string connectionString = @"Data Source=WIN-O0RH17IT5C5;Initial Catalog=Ипотечный калькулятор;Integrated Security=True";
        string login;
        int summ;
        string name_programm;
        int date;
        string monthpay;
        string percent;
        public Form1()
        {
            InitializeComponent();
            LoadPrograms();
        }

        private void LoadPrograms()
        {

            try
            {
                db.con.Open();

                string query = "SELECT Название_программы FROM Ипотечные_Программы";
                SqlDataAdapter adapter = new SqlDataAdapter(query, db.con);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                programComboBox.DataSource = dt;
                programComboBox.DisplayMember = "Название_программы";
                db.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке программ: {ex.Message}");
            }

        }
        //Расчет ипотеки
        private void calculateButton_Click(object sender, EventArgs e)
        {
            Conn();
        }

        public void Conn()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectedProgram = programComboBox.Text;
                    string query = $"SELECT * FROM Ипотечные_Программы WHERE Название_программы = '{selectedProgram}'";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        double interestRate = Convert.ToDouble(reader["Процентная_ставка"]);
                        int loanTerm = (int)numericUpDown1.Value;

                        double loanAmount = Convert.ToDouble(loanAmountTextBox.Text);
                        double monthlyPayment = CalculateMonthlyPayment(loanAmount, interestRate, loanTerm);


                        textBox1.Text = $"{monthlyPayment:C}";
                        textBox2.Text = $"{monthlyPayment * loanTerm:C}";
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при расчете кредита: {ex.Message}");
                }
            }
        }

        public double CalculateMonthlyPayment(double loanAmount, double interestRate, int loanTerm)
        {
            double monthlyInterestRate = interestRate / 100 / 12;
            int numberOfPayments = loanTerm * 12;


            double monthlyPayment = (loanAmount * monthlyInterestRate) / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
            return monthlyPayment;
        }


        //Отвечает за добавление всей информации в DataGridView
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateRows();
            RefreshDataGrid(resultDataGridView);
        }

        public void CreateRows()
        {
            resultDataGridView.Columns.Add("Название_программы", "Название программы");
            resultDataGridView.Columns.Add("Процентная_ставка", "Процентная ставка");
        }

        public void AddRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(1), record.GetDecimal(2));
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            dgw.Rows.Clear();

            string query = $"select * from Ипотечные_Программы";

            SqlCommand cmd = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                AddRow(dgw, sqlDataReader);
            }
            sqlDataReader.Close();
            connection.Close();
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 1)
            {
                label5.Text = "год";
            }
            if (numericUpDown1.Value == 2)
            {
                label5.Text = "года";
            }
            if (numericUpDown1.Value >= 5)
            {
                label5.Text = "лет";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
            linkLabel1.Text = User.username;
            if (linkLabel1.Text != "Войти")
            {
                label6.Visible = true;
                label6.Text = "Сохранения";
            }
            if (linkLabel1.Text == "admin" || linkLabel1.Text == "Admin")
            {
                label6.Visible = true;
                label6.Text = "Список";
            }
        }

        private void loanAmountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавиатуры - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }
        //ошибка при сохранении
        private void button1_Click(object sender, EventArgs e)
        {
            if (linkLabel1.Text == "Войти")
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт для сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else 
            {
                login = linkLabel1.Text;
                summ = Convert.ToInt32(loanAmountTextBox.Text);
                name_programm = programComboBox.Text;
                date = (int)numericUpDown1.Value;
                monthpay = textBox1.Text;
                percent = textBox2.Text;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(summ.ToString()) || string.IsNullOrWhiteSpace(name_programm)|| string.IsNullOrWhiteSpace(date.ToString())|| string.IsNullOrWhiteSpace(monthpay)|| string.IsNullOrWhiteSpace(percent))
                {
                    MessageBox.Show("Не все поля заполнены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Save();
            }
        }

        //сохранения
        public void Save()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            string query = $"select * from Сохранение";
            SqlCommand cmd = new SqlCommand(query, db.con);

            adapter.SelectCommand = cmd;

            adapter.Fill(Table);

            db.con.Open();
            SqlCommand insertCommand = new SqlCommand($"insert into Сохранение(Логин,Сумма_займа,Название_программы,Срок_кредита,Ежемесячный_платеж,Проценты) values ('{login}','{summ}','{name_programm}','{date}','{monthpay}','{percent}')", db.con);

            if (insertCommand.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Вы успешно сохранили результат", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            db.con.Close();
        }
      
        private void label6_Click(object sender, EventArgs e)
        {
            if (label6.Text == "Сохранения")
            {
            Form3 form = new Form3();
            form.ShowDialog();
            }
            else
            {
                Admin admin = new Admin();
                admin.ShowDialog();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
