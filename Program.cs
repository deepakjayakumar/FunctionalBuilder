namespace FunctionalBuilder
{

    public class Person
    {
        public string Name, Position;
    }

    public sealed class PersonBuilder{

        private readonly List<Func<Person,Person>> actions = new List<Func<Person,Person>>();

        public PersonBuilder Called(string name)
        {
            Do(p => p.Name = name);
            return this;
        }
        public PersonBuilder Do(Action<Person> action)
        {
            AddAction(action);
            return this;
        }

        public PersonBuilder Build()
        {
            actions.Aggregate(new Person(),(p,f) => f(p));
            return this;
        }
        private PersonBuilder AddAction(Action<Person> action) {
            actions.Add(p =>
            {
                action(p); return p;
            });

            return this;
        }

        }

    
    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new PersonBuilder().Called("a").Build();
            Console.WriteLine("Hello, World!");
        }
    }
}