using LabU.Core.Utils;
using System;
using System.Text;

namespace LabU.Tools.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            string cmd = "";
            while (true)
            {
                System.Console.WriteLine("Введите команду:");
                cmd = System.Console.ReadLine();
                switch (cmd)
                {
                    case "hash":
                        RunHasherCommad();
                        break;
                    case "exit":
                        return;
                }
                
            }
        }

        private static void RunHasherCommad()
        {
            System.Console.WriteLine("Введите пароль");
            var pswd = System.Console.ReadLine();
            if (!string.IsNullOrEmpty(pswd))
            {
                System.Console.WriteLine(Hasher.HashPassword(pswd));
            }
        }
    }
}
