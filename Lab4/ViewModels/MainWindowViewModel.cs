using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Utils;
using Lab2.Exceptions;
using Lab4.DataAccess;
using Lab4.Models;
using System.Collections.ObjectModel;

namespace Lab1.ViewModels
{
    public class PersonViewModel
    {
        private ICommand _saveCommand;
        private ICommand _resetCommand;
        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private PersonRepository _repository;
        private Person _personEntity = null;
        public PersonRecord PersonRecord { get; set; }

        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                    _resetCommand = new RelayCommand(param => ResetData(), null);

                return _resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveData(), null);

                return _saveCommand;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => EditData((int)param), null);

                return _editCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => DeleteStudent((int)param), null);

                return _deleteCommand;
            }
        }

        public PersonViewModel()
        {
            _personEntity = new Person();
            _repository = new PersonRepository();
            PersonRecord = new PersonRecord();
            GetAll();
        }

        public void ResetData()
        {
            PersonRecord.Name = string.Empty;
            PersonRecord.Id = 0;
            PersonRecord.Surname = string.Empty;
            PersonRecord.Email = string.Empty;
            PersonRecord.WesternZodiacSign = string.Empty;
            PersonRecord.EasternZodiacSign = string.Empty;
            PersonRecord.Birthday = DateTime.Now;
            PersonRecord.Age = 0;
        }

        private static void ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return;
            else
                throw new InvalidEmail(email + " is Invalid Email Address");
        }

        public void DeleteStudent(int id)
        {
            if (MessageBox.Show("Confirm delete of this record?", "Person", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.RemoveStudent(id);
                    MessageBox.Show("Record successfully deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    GetAll();
                }
            }
        }

        public void SaveData()
        {
            if (PersonRecord != null)
            {
                _personEntity = new Person();
                _personEntity.Name = PersonRecord.Name;
                _personEntity.Age = PersonRecord.Age;
                _personEntity.Surname = PersonRecord.Surname;
                _personEntity.Email = PersonRecord.Email;
                _personEntity.WesternZodiacSign = PersonRecord.WesternZodiacSign;
                _personEntity.EasternZodiacSign = PersonRecord.EasternZodiacSign;
                _personEntity.Birthday = PersonRecord.Birthday;

                try
                {
                    if (_personEntity.Birthday > _personEntity.today)
                    {
                        throw new InvalidFutureDate("Invalid future date");
                    }
                    _personEntity.CalculateAge();
                    if (_personEntity.Age > 135)
                    {
                        throw new InvalidPastDate("Invalid past date");
                    }
                    ValidateEmail(_personEntity.Email);

                    if (PersonRecord.Id <= 0)
                    {
                        _repository.AddPerson(_personEntity);
                        MessageBox.Show("New record successfully saved.");
                    }
                    else
                    {
                        _personEntity.ID = PersonRecord.Id;
                        _repository.UpdatePerson(_personEntity);
                        MessageBox.Show("Record successfully updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                finally
                {
                    GetAll();
                    ResetData();
                }
            }
        }

        public void EditData(int id)
        {
            var model = _repository.Get(id);
            PersonRecord.Id = model.ID;
            PersonRecord.Name = model.Name;
            PersonRecord.Email = model.Email;
            PersonRecord.Surname = model.Surname;
            PersonRecord.Age = (int)model.Age;
            PersonRecord.Birthday = model.Birthday;
            PersonRecord.WesternZodiacSign = model.WesternZodiacSign;
            PersonRecord.EasternZodiacSign = model.EasternZodiacSign;

        }

        public void GetAll()
        {
            PersonRecord.PersonRecords = new ObservableCollection<PersonRecord>();
            _repository.GetAll().ForEach(data => PersonRecord.PersonRecords.Add(new PersonRecord()
            {
                Id = data.ID,
                Name = data.Name,
                Email = data.Email,
                Birthday = data.Birthday,
                WesternZodiacSign = data.WesternZodiacSign,
                EasternZodiacSign = data.EasternZodiacSign,
                Age = Convert.ToInt32(data.Age),
                Surname = data.Surname
            }));
        }
    }
}
