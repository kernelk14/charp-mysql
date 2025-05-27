using System;
using MySql.Data.MySqlClient;

namespace MainProgram
{
    class Program
    {
        public static void InsertData(MySqlCommand cmd) {
            string prod_name = Inputs.GetString("Enter a product name: ");
            int quantity = Inputs.GetInt($"Enter quantity for the product `{prod_name}`: ");
            int price = Inputs.GetInt($"Enter the starting price for the product `{prod_name}`: ");

            cmd.CommandText = @$"INSERT INTO products(name, quan, price) VALUES('{prod_name}', {quantity}, {price})";
            if (cmd.ExecuteNonQuery() > 0) {
                Console.WriteLine("Insert Executed");
            } else {
                Console.WriteLine(cmd.CommandText);
                throw new Exception("Cannot execute query.");
            }
        }
        public static int ReadData(List<string> prods, MySqlConnection con, MySqlCommand cmd) {
            cmd.CommandText = @"SELECT * FROM PRODUCTS";
            using var submit = new MySqlCommand(cmd.CommandText, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()) {
                string query_name = rdr.GetString(1);
                prods.Add(query_name);
            }
            if (prods.Count() > 0) {
                foreach (var names in prods) {
                    Console.WriteLine(names);
                }
            } else {
                Console.WriteLine("Database is empty.");
            }

            return prods.Count();
        }
        public static void Main(string[] args) 
        {
            List<string> prods = new List<string>();
            string cs = @"server=localhost;userid=root;password='';database=csharp_test";
            using var con = new MySqlConnection(cs);
            
            try
            {
                con.Open();   
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                throw new Exception("Cannot connect to MySQL");
            }

            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS products(id INT(11) PRIMARY KEY AUTO_INCREMENT, name VARCHAR(800) NOT NULL, quan INT(30) NOT NULL, price INT(30) NOT NULL)";
            if (cmd.ExecuteNonQuery() == 0) {
                Console.WriteLine("Command Executed.");
            } else {
                throw new Exception("Cannot execute MySQL Query.");
            }
            
            /*InsertData(cmd);*/
            if (ReadData(prods, con, cmd) <= 0) {
                InsertData(cmd);
            }
            
        }
    }
}
