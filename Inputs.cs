public class Inputs {
    
    public static string GetString(string prompt) {
        Console.Write(prompt);
        
        string str_res = Console.ReadLine();

        if (str_res is null) {
            Console.WriteLine("Inputs.GetString(): String input cannot be null.");
        }
        
        return str_res;

    }

    public static int GetInt(string prompt) {
        Console.Write(prompt);
        int int_res = Convert.ToInt32(Console.ReadLine());

        return int_res;
    }
}

