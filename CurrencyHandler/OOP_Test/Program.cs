using System;

namespace OOP_Test
{
    class Base
    {
        public Base()
        {
            Console.WriteLine(Name);
        }

        public virtual string Name { get; set; } = "Base";
    }

    class Derived : Base
    {
        public override string Name { get; set; } = "Derived";
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Derived();

        }
    }
}
