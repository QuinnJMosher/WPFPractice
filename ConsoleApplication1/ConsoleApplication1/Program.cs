using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public static string folder = "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\";
        public static string[] files = {
                            "flat_medal1.png",
                            "flat_medal2.png",
                            "flat_medal3.png"
                         };

        static void Main(string[] args)
        {
            Console.WriteLine("file image test:");

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(folder + files[i]);
            }
            Console.ReadLine();
        }
    }
}
