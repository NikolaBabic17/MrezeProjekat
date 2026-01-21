using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class NewMain
    {
        public void Run()
        {
            Console.WriteLine("Hello, Server!");
            int karakter = Console.Read();
            Console.WriteLine($"{karakter} - {karakter}");
        }
    }
}
