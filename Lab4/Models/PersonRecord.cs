using Lab4.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class PersonRecord : ViewModelBase
    {
        #region Fields
        private DateTime _birthday = DateTime.Today;
        private string _name = "";
        private string _surname = "";
        private string _email = "";
        private string _easternZodiacSign = "";
        private string _westernZodiacSign = "";
        private int _age = 0;
        public readonly DateTime today = DateTime.Today;
        private bool _isFormFilled = false;
        #endregion

        #region Properties
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public string EasternZodiacSign
        {
            get { return _easternZodiacSign; }
            set 
            { 
                _easternZodiacSign = value;
                OnPropertyChanged("EasternSodiacSign");
            }
        }

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged("Name");
                ValidateForm();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set 
            { 
                _surname = value;
                OnPropertyChanged("Surname");
                ValidateForm();
            }
        }

        public string Email
        {
            get { return _email; }
            set 
            { 
                _email = value;
                OnPropertyChanged("Email");
                ValidateForm();
            }
        }

        public string WesternZodiacSign
        {
            get { return _westernZodiacSign; }
            set 
            { 
                _westernZodiacSign = value;
                OnPropertyChanged("WesternSodiacSign");
            }
        }

        public int Age
        {
            get { return _age; }
            set 
            { 
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public bool IsFormFilled
        {
            get { return _isFormFilled; }
            set
            {
                _isFormFilled = value;
                OnPropertyChanged("IsFormFilled");
            }
        }

        public bool IsAdult => _age > 18;

        public string SunSign => _westernZodiacSign;

        public string ChineseSign => _easternZodiacSign;

        public bool IsBirthday => (Birthday.Day == DateTime.Today.Day && Birthday.Month == DateTime.Today.Month);

        private ObservableCollection<PersonRecord> _personRecords;
        public ObservableCollection<PersonRecord> PersonRecords
        {
            get
            {
                return _personRecords;
            }
            set
            {
                _personRecords = value;
                OnPropertyChanged("PersonRecords");
            }
        }

        private void PersonModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("PersonRecords");
        }

        private void ValidateForm()
        {
            IsFormFilled =
                !(
                String.IsNullOrWhiteSpace(Name)
                || String.IsNullOrWhiteSpace(Email)
                || String.IsNullOrWhiteSpace(Surname)
                );
        }
        #endregion

    }
}
