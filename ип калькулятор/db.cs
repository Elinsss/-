using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ип_калькулятор
{
    public class db
    {
        public string login = "";
        public string pass = "";
        public DateTime date = DateTime.Now;

        public SqlConnection con = new SqlConnection($@"Data Source=DESKTOP-UCA27K4\SQLSERVER; Initial Catalog=Ипотечный калькулятор;Integrated Security = True");
    }
}
