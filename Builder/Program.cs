using EmailValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
           
            do
            {
                Console.Write("Enter an email address: ");

                var input = Console.ReadLine();
                if (input == null)
                    break;

                input = input.Trim();
                Console.WriteLine("{0} is {1}!", input, EmailValidator.Validate(input) ? "valid" : "invalid");
            } while (true);

            Console.WriteLine();
        }
    }
}
