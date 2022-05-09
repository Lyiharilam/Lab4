using Lab1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab4.DataAccess
{
	public class PersonRepository
	{
		private List<Person> personContext = new List<Person>();

		public PersonRepository()
		{
			string fileContent = File.ReadAllText("../../../persons.json");

			if (fileContent.Length < 5)
            {
				GenerateInitialData();
				WriteDataToFile();
			}
            else
            {
				personContext = JsonSerializer.Deserialize<List<Person>>(fileContent);
			}
			
		}

		private void GenerateInitialData()
        {
			for (int i = 0; i < 50; ++i)
			{
				Person initialPerson = new Person() { ID = i, Name = $"SomeUserName {i}", Surname = $"SomeUserSurname {i}", Email = $"{i}email@example.com", Birthday = new DateTime(1970 + i, 3, 1, 7, 0, 0) };
				initialPerson.DetermineWesternZodiacSign();
				initialPerson.DetermineEasternZodiacSign();
				initialPerson.CalculateAge();
				personContext.Add(initialPerson);
			}
		}

		private void WriteDataToFile()
        {
			string jsonString = JsonSerializer.Serialize(personContext);
			File.WriteAllText("../../../persons.json", jsonString);
		}

		public Person Get(int id)
		{
			return personContext.Find(x => x.ID == id);
		}

		public List<Person> GetAll()
		{
			return personContext;
		}

		public void AddPerson(Person person)
		{
			if (person != null)
			{
				person.DetermineWesternZodiacSign();
				person.DetermineEasternZodiacSign();
				person.CalculateAge();
				Random rnd = new Random();
				int num = rnd.Next();
				person.ID = num;
				personContext.Add(person);
				WriteDataToFile();
			}
		}

		public async void UpdatePerson(Person person)
		{
			var personFind = this.Get(person.ID);
			if (personFind != null)
			{
				personFind.Birthday = person.Birthday;
				personFind.Name = person.Name;
				personFind.Surname = person.Surname;
				personFind.Email = person.Email;
				person.DetermineWesternZodiacSign();
				person.DetermineEasternZodiacSign();
				person.CalculateAge();
				personFind.WesternZodiacSign = person.WesternZodiacSign;
				personFind.EasternZodiacSign = person.EasternZodiacSign;
				personFind.Age = person.Age;
				WriteDataToFile();
			}
		}

		public void RemoveStudent(int id)
		{
			var personFind = this.Get(id);
			if (personFind != null)
			{
				personContext.Remove(personFind);
				WriteDataToFile();
			}
		}
	}
}
