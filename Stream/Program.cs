using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PersonIO
{
    public class Person
    {
        public Person()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
    public class PersonService
    {
        private readonly string path = @"persons";
        public PersonService()
        {
            path = Path.Combine(path, "test.txt");
        }
        public void Create(Person person)
        {
            using FileStream fstream = new(path, FileMode.Append);
            byte[] buffer = Encoding.Default.GetBytes(person.Id.ToString() + "\n"
                + person.Age.ToString() + "\n"
                + person.LastName + "\n"
                + person.FirstName + "\n");
            fstream.Write(buffer, 0, buffer.Length);
        }
        public static List<Person> ConvertToPerson(string[] persons)
        {
            List<Person> people = new();
            for (int i = 0; i < persons.Length; i += 4)
            {
                try
                {
                    people.Add(new Person()
                    {
                        Id = Guid.Parse(persons[i]),
                        Age = Convert.ToInt16(persons[i + 1]),
                        LastName = persons[i + 2],
                        FirstName = persons[i + 3]
                    });
                }
                catch
                {

                }
            }
            return people;
        }
        public string[] Read()
        {
            string[] persons;
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                persons = textFromFile.Split('\n');
            }
            return persons;
        }
        public static void Print(List<Person> people)
        {
            foreach (var person in people)
            {
                Console.WriteLine($"ID: {person.Id.ToString()}\n" +
                    $"AGE: {person.Age}\n" +
                    $"LAST NAME: {person.LastName}\n" +
                    $"FIRST NAME: {person.FirstName}\n" +
                    "************\n");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            PersonService personService = new();
            List<Person> people = new();
            personService.Create(new Person { Age = 11, LastName = "AAA", FirstName = "BBB" });
            personService.Create(new Person { Age = 22, LastName = "CCC", FirstName = "DDD" });
            personService.Create(new Person { Age = 33, LastName = "EEE", FirstName = "FFF" });
            people = PersonService.ConvertToPerson(personService.Read());
            PersonService.Print(people);
        }
    }
}
