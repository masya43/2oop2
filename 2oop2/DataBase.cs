using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace _2oop2
{
    internal class DataBase
    {
        SqlConnection _connection = new SqlConnection(@"Data Source=DESKTOP-27UFTBE\MSSQLSERVER01;Database=dbForMotors;Integrated Security=True;");
        private void openConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
        private void closeConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        private SqlConnection GetConnection() 
        { 
            return _connection; 
        }
        public DataTable requestQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand(query, GetConnection());
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                closeConnection();
            }
            return dataTable;
        }
        public int requestQueryWithOutput(string query)
        {
            int result = 0;
            try
            {
                openConnection();
                SqlCommand command = new SqlCommand(query, GetConnection());
                result = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                closeConnection();
            }
            return result;
        }
    }
}
