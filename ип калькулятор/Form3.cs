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
    public partial class Form3 : Form
    {
        db db = new db();
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            CreateRows();
            RefreshDataGrid(resultDataGridView);
        }

        public void CreateRows()
        {
            resultDataGridView.Columns.Add("Сумма_займа", "Сумма займа");
            resultDataGridView.Columns.Add("Название_программы", "Название программы");
            resultDataGridView.Columns.Add("Процентная_ставка", "Процентная ставка");
            resultDataGridView.Columns.Add("Срок_кредита", "Срок кредита");
            resultDataGridView.Columns.Add("Ежемесячный_платеж", "Ежемесячный платеж");
            resultDataGridView.Columns.Add("Проценты", "Проценты");


        }

        public void AddRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDecimal(2), record.GetInt32(3), record.GetString(4), record.GetString(5));
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"select c.Сумма_займа,c.Название_программы,и.Процентная_ставка,c.Срок_кредита,c.Ежемесячный_платеж,c.Проценты from Сохранение c,Ипотечные_Программы и where c.Название_программы = и.Название_программы and Логин = '{User.username}'";

            SqlCommand cmd = new SqlCommand(query, db.con);

            db.con.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                AddRow(dgw, sqlDataReader);
            }
            sqlDataReader.Close();
            db.con.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        
    }
}
