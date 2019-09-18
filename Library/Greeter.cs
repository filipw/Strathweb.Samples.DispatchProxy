using System;

namespace Library
{
    internal class Greeter
    {
        public static void Greet(IGreeting greeting)
        {
            Console.WriteLine(greeting.Message);
        }
    }

    internal interface IGreeting
    {
        string Message { get; }
    }
}
