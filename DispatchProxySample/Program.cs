using System;
using System.Reflection;
using Library;

namespace DispatchProxySample
{
    class Program
    {
        static void Main(string[] args)
        {
            var internalType = Assembly.Load("Library").GetType("Library.Greeter");
            var greetMethod = internalType.GetMethod("Greet", BindingFlags.Public | BindingFlags.Static);

            var proxy = GreetingFactory.Create();
            Console.WriteLine(greetMethod.Invoke(null, new[] { proxy }));
        }
    }

    public class Greeting : DispatchProxy
    {
        private GreetingImpl _impl;

        public Greeting()
        {
            _impl = new GreetingImpl();
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
        }

        private class GreetingImpl // : IGreeting - doesn't implement IGreeting but mimics it
        {
            public string Message => "hello world";
        }
    }

    public class GreetingFactory
    {
        public static object Create()
        {
            var internalType = Assembly.Load("Library").GetType("Library.IGreeting");
            return typeof(DispatchProxy).GetMethod(nameof(DispatchProxy.Create)).MakeGenericMethod(internalType, typeof(Greeting)).Invoke(null, null);
        }
    }
}
