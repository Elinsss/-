using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace ип_калькулятор
{
    public partial class Form2 : Form
    {
        db db = new db();
        public string username;
        public Form2()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //ошибка при входе
        private void button1_Click(object sender, EventArgs e)
        {
            db.login = textBox1.Text;
            db.pass = textBox2.Text;
            db.date = DateTime.Now;

            if (db.login == "" || db.pass == "")
            {
                MessageBox.Show("Пожалуйста, введите логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            logins();
            Time();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else 
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
        //для админа
        private void logins()
        {
            User.username = textBox1.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            string query = $"select * from Аккаунты where Login='{db.login}' and Password='{db.pass}'";

            SqlCommand command = new SqlCommand(query, db.con);

            adapter.SelectCommand = command;

            adapter.Fill(Table);

            if (Table.Rows.Count == 1)
            {
                if (textBox1.Text == "admin") 
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
                else 
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }

                db.con.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                db.con.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Time()
        {
            string query = $"update Аккаунты set Time = '{db.date}' where Login = '{textBox1.Text}'";

            SqlCommand command = new SqlCommand(query, db.con);

            try
            {
                db.con.Open();

                int rowsAffected = command.ExecuteNonQuery();

                db.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            db.login = textBox1.Text;
            db.pass = textBox2.Text;

            if (db.login == ""  || db.pass == "")
            {
                MessageBox.Show("Пожалуйста, введите логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            registration();
            Time();
        }

        //регистрация
        public void registration()
        {
            User.username = textBox1.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            string query = $"select * from Аккаунты where Login='{db.login}'";

            SqlCommand cmd = new SqlCommand(query, db.con);

            adapter.SelectCommand = cmd;

            adapter.Fill(Table);

            db.con.Open();

            if (Table.Rows.Count == 0)
            {
                SqlCommand insertCommand = new SqlCommand($"insert into Аккаунты(Login,Password,Time) values ('{db.login}','{db.pass}','{db.date}')", db.con);

                if (insertCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Такой логин/почта уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            db.con.Close();
        }
    }
}
