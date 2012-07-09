using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wonga.QA.Framework.Core;

namespace Program
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            string inputString = "";

            var opt = new ConsoleKeyInfo();
            var opt2 = new ConsoleKeyInfo();
            while (opt.KeyChar != '0')
            {
                Console.Clear();
                Console.WriteLine("Enter an option");
                Console.WriteLine("\t1. Encrypt a string");
                Console.WriteLine("\t2. Decrypt a string");
                opt = Console.ReadKey();
                switch (opt.KeyChar)
                {
                    case ('1'):
                        string encrypted = "";
                        Console.WriteLine();
                        Console.Write("Press 1 to paste string from clipboard or press 2 to enter it: ");
                        opt2 = Console.ReadKey();
                        switch (opt2.KeyChar)
                        {
                            case ('1'):
                                Console.WriteLine(Clipboard.GetText());
                                encrypted = Class.Encrypt(Clipboard.GetText());
                                break;

                            case ('2'):
                                Console.WriteLine();
                                Console.Write("String to encrypt: ");
                                inputString = Console.ReadLine();
                                encrypted = Class.Encrypt(inputString);
                                break;
                        }
                        Console.WriteLine(encrypted);
                        Clipboard.SetText(encrypted);
                        Console.WriteLine("Encrypted string copied to clipboard");
                        break;
                    case ('2'):
                        string decrypted = "";
                        Console.WriteLine();
                        Console.Write("Press 1 to paste string from clipboard or press 2 to enter it: ");
                        opt2 = Console.ReadKey();
                        switch (opt2.KeyChar)
                        {
                            case ('1'):
                                Console.WriteLine(Clipboard.GetText());
                                decrypted = Class.Decrypt(Clipboard.GetText());
                                break;

                            case ('2'):
                                Console.WriteLine();
                                Console.Write("String to decrypt: ");
                                inputString = Console.ReadLine();
                                decrypted = Class.Decrypt(inputString);
                                break;
                        }
                        Console.WriteLine(decrypted);
                        Clipboard.SetText(decrypted);
                        Console.WriteLine("Decrypted string copied to clipboard");
                        break;
                }
                Console.WriteLine("\nPress 0 to exit or other key to proceed");
                opt = Console.ReadKey();

            }
        }
    }
}
