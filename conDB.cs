using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Windows.Forms;

namespace EmployeeManager {
    public class conDB {
        internal static MySqlConnection getConnection() {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server = localhost; Database = dsnhanvien; Port = 3306; User ID=root; Password="; //K1001011
            try {
                conn.Open();
                return conn;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
