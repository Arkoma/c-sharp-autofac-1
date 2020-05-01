using Autofac;
using System;
using System.Collections.Generic;

namespace AutofacSample
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail}: {message}");
        }
    }
    public class Engine
    {
        private ILog log;
        private int id;
        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }
        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class Car
    {
        private ILog log;
        private Engine engine;
        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();

        }
        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }
        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward ...");
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterType<EmailLog>().As<ILog>();
            builder.RegisterType<ConsoleLog>().As<ILog>().PreserveExistingDefaults();
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>()
                .UsingConstructor(typeof(Engine));

            IContainer container = builder.Build();

            var car = container.Resolve<Car>();

            car.Go();
        }
    }
}
