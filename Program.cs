// using System;
using MySql.Data.MySqlClient;

namespace MainProgram
{
    public class Program
    {
        public static long row_count(MySqlConnection con, string col)
        {
            string commandLine = @$"SELECT COUNT(*) FROM {col}";
            using (MySqlCommand cmd = new MySqlCommand(commandLine, con))
            {
                long count = (long)cmd.ExecuteScalar();

                return count - 1;
                cmd.Cancel();
            }
        }
        public static void InsertData(MySqlCommand cmd)
        {
            string prod_name = Inputs.GetString("Enter a product name: ");
            int quantity = Inputs.GetInt($"Enter quantity for the product `{prod_name}`: ");
            int price = Inputs.GetInt($"Enter the starting price for the product `{prod_name}`: ");

            cmd.CommandText = @$"INSERT INTO products(name, quan, price) VALUES('{prod_name}', {quantity}, {price})";
            if (cmd.ExecuteNonQuery() >= 0)
            {
                Console.WriteLine("Insert Executed");
            }
            else
            {
                Console.WriteLine(cmd.CommandText);
                throw new Exception("Cannot execute query.");
            }
        }

        public static int ReadData(List<string> prods, List<int> quants, List<int> prices, MySqlConnection con, MySqlCommand cmd)
        {
            long cnt = row_count(con, "products");

            cmd.CommandText = @"SELECT * FROM products";
            using var submit = new MySqlCommand(cmd.CommandText, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string query_name = rdr.GetString(1);
                int query_quan = rdr.GetInt32(2);
                int query_price = rdr.GetInt32(3);
                if (prods.Count <= cnt) prods.Add(query_name);
                if (quants.Count <= cnt) quants.Add(query_quan);
                if (prices.Count <= cnt) prices.Add(query_price);
            }
            if (prods.Count > 0)
            {
                int i;
                for (i = 1; i <= prods.Count; i++)
                {
                    int bundle_price = prices[i - 1] * quants[i - 1];
                    Console.WriteLine($"{i}. {prods[i - 1]} - {quants[i - 1]}pcs - ${bundle_price} total - ${prices[i - 1]}/pc");
                }
                i = 0;
            }
            else
            {
                Console.WriteLine("Database is empty.");
            }
            Console.WriteLine($"Current Count: {prods.Count}");
            return prods.Count;
        }

        public static void Choose(List<string> prods, List<int> quants, List<int> prices, MySqlConnection con, MySqlCommand cmd)
        {
            Console.WriteLine("Select an option:\n\t0. Exit\n\t1. Insert Data\n\t2. Read Data");
            int choice = Inputs.GetInt("Choice: ");

            if (choice == 0)
            {
                Console.WriteLine("Thank You!");
            }
            else if (choice == 1)
            {
                InsertData(cmd);
                Choose(prods, quants, prices, con, cmd);
                prods.Clear();
                quants.Clear();
                prices.Clear();
            }
            else if (choice == 2)
            {
                ReadData(prods, quants, prices, con, cmd);
                Choose(prods, quants, prices, con, cmd);
            }
            else
            {
                Console.WriteLine("Invalid Choice!");
                Choose(prods, quants, prices, con, cmd);
            }
        }

        public static void Main(string[] args)
        {
            List<string> prods = new List<string>();
            List<int> quants = new List<int>();
            List<int> prices = new List<int>();
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
            if (cmd.ExecuteNonQuery() == 0)
            {
                Console.WriteLine("Command Executed.");
            }
            else
            {
                throw new Exception("Cannot execute MySQL Query.");
            }

            Console.WriteLine("Welcome to MySQL Testing!");
            Choose(prods, quants, prices, con, cmd);
        }
    }
}
