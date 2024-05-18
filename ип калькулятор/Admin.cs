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

namespace ип_калькулятор
{
    public partial class Admin : Form
    {
        db db = new db();
        public Admin()
        {
            InitializeComponent();
            LoadFullRowsFromDatabase(listBox1);
        }

        private void LoadFullRowsFromDatabase(ListBox listBox)
        {
            // Создать подключение к базе данных
            // Открыть подключение
            db.con.Open();

            // Создать команду для выборки данных
            using (var command = db.con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Аккаунты";

                // Выполнить команду и получить результат
                using (var reader = command.ExecuteReader())
                {
                    // Перебрать строки из результата
                    while (reader.Read())
                    {
                        // Создать строку для хранения значений столбцов
                        string row = "";

                        // Перебрать все столбцы в текущей строке
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            // Получить значение столбца
                            string value = reader[i].ToString();

                            // Добавить значение в строку
                            row += $"{value} | ";
                        }

                        // Добавить строку в ListBox
                        listBox.Items.Add(row);
                    }
                }
            }
            db.con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
