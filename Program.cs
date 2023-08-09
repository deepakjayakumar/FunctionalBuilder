using System.Runtime.CompilerServices;

namespace FunctionalBuilder
{

    public class Person
    {
        public string Name, Position;

        public override string? ToString()
        {
            return $"{nameof(Name)} : {Name} , {nameof(Position)} : {Position}";
        }
    }

    public abstract class FunctionBuilder<TSubject,Self>
        where Self : FunctionBuilder<TSubject,Self>
        where TSubject :new()

    {
        private readonly List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

        public Self Do(Action<TSubject> action)
        {
            AddAction(action);
            return (Self) this;
        }

        public Self Build()
        {
            actions.Aggregate(new TSubject(), (p, f) => f(p));
            return (Self) this;
        }
        private Self AddAction(Action<TSubject> action)
        {
            actions.Add(p =>
            {
                action(p); return p;
            });

            return (Self) this;
        }

    }


    public sealed class PersonBuilder : FunctionBuilder<Person,PersonBuilder>
    {
        public PersonBuilder Called(string name)
        {
            Do(p => p.Name = name);
            return this;
        }


    }
    //public sealed class PersonBuilder{

    //    private readonly List<Func<Person,Person>> actions = new List<Func<Person,Person>>();

    //    public PersonBuilder Called(string name)
    //    {
    //        Do(p => p.Name = name);
    //        return this;
    //    }
    //    public PersonBuilder Do(Action<Person> action)
    //    {
    //        AddAction(action);
    //        return this;
    //    }

    //    public PersonBuilder Build()
    //    {
    //        actions.Aggregate(new Person(),(p,f) => f(p));
    //        return this;
    //    }
    //    private PersonBuilder AddAction(Action<Person> action) {
    //        actions.Add(p =>
    //        {
    //            action(p); return p;
    //        });

    //        return this;
    //    }

    //    }

    public static class PersonBuilderExtension
    {
        public static PersonBuilder WorksAs
            (this PersonBuilder builder, string Position)
            => builder.Do(p => p.Position = Position);
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new PersonBuilder().Called("a").WorksAs("painter").Build();
            Console.WriteLine(person.ToString());
        }
    }
}