using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pilkarze_MVVM.ViewModel
{
    using Model;
    using BaseClass;
    using System.Windows;
    using System.Windows.Data;
    using System.Xml.Serialization;
    using System.IO;

    internal class Footballing : ViewModelBase
    {
        public Footballing()
        {
            ReadList();
        }

        #region Interfejs publiczny 
        private string _firstArg = null;
        public string FirstArg {
            get 
            {
                return _firstArg;
            }
            set
            {
                _firstArg = value;
                onPropertyChanged(nameof(FirstArg));
            }
        }
        private string _secondArg = null;
        public string SecondArg {
            get
            {
                return _secondArg;
            }
            set
            {
                _secondArg = value;
                onPropertyChanged(nameof(SecondArg));
            }
        }
        private uint _age = 20;
        public uint Age { 
            get 
            {
                return _age;
            }
            set 
            {
                _age = value;
                onPropertyChanged(nameof(Age));
            } 
        }
        private uint _weight = 80;
        public uint Weight {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                onPropertyChanged(nameof(Weight));
            }
        }
        private Footballer _selected = null;
        public Footballer Selected 
        {
            get 
            {
                return _selected;
            }
            set 
            {
                _selected = value;
                if (value != null)
                {
                    FirstArg = _selected.Imie;
                    SecondArg = _selected.Nazwisko;
                    Age = _selected.Wiek;
                    Weight = _selected.Waga;
                }
                onPropertyChanged(nameof(Selected));
            }
        }
        private ObservableCollection<Footballer> _list = new ObservableCollection<Footballer>();
        public ObservableCollection<Footballer> List
        {
            get
            {
                return _list;
            }
            set
            {
                onPropertyChanged(nameof(List));
            }
        }

        #endregion

        #region Polecenia

        #region polecenie dodania Pilkarza

        private ICommand _add = null;
        public ICommand Add
        {
            get
            {
                if (_add == null)
                {
                    _add = new RelayCommand(
                        arg => { 
                            var tempFootballer = new Footballer(FirstArg.Trim(), SecondArg.Trim(), Age, Weight); 
                            List.Add(tempFootballer); Clear(); 
                        },
                        arg => {
                            var czyJuzJestNaLiscie = false;
                            if (List.Count != 0 && (!string.IsNullOrEmpty(FirstArg)) && (!string.IsNullOrEmpty(SecondArg)))
                            {
                                var tempFootballer = new Footballer(FirstArg.Trim(), SecondArg.Trim(), Age, Weight);
                                foreach (var p in List)
                                {
                                    var pilkarz = p as Footballer;
                                    if (pilkarz.isTheSame(tempFootballer))
                                    {
                                        czyJuzJestNaLiscie = true;
                                        break;
                                    }
                                }
                                return (!string.IsNullOrEmpty(FirstArg)) && (!string.IsNullOrEmpty(SecondArg) && !czyJuzJestNaLiscie);
                            }
                            else
                                return (!string.IsNullOrEmpty(FirstArg)) && (!string.IsNullOrEmpty(SecondArg));
                        }
                     );
                }
                return _add;
            }
        }
        #endregion
        #region polecenie odpoiwedzialne za edycje
        private ICommand _edit = null;
        public ICommand Edit
        {
            get
            {
                if (_edit == null)
                {
                    _edit = new RelayCommand(
                        arg => {
                                var pilkarz = Selected as Footballer;
                                pilkarz.Nazwisko = FirstArg.Trim();
                                pilkarz.Imie = SecondArg.Trim();
                                pilkarz.Waga = Weight;
                                pilkarz.Wiek = Age;
                                CollectionViewSource.GetDefaultView(List).Refresh();
                                Clear();  
                        },
                        arg => {
                            var czyJuzJestNaLiscie = false;
                            if (List.Count != 0 && (!string.IsNullOrEmpty(FirstArg)) && (!string.IsNullOrEmpty(SecondArg)))
                            {
                                var tempFootballer = new Footballer(FirstArg.Trim(), SecondArg.Trim(), Age, Weight);
                                foreach (var p in List)
                                {
                                    var footballer = p as Footballer;
                                    if (footballer.isTheSame(tempFootballer))
                                    {
                                        czyJuzJestNaLiscie = true;
                                        break;
                                    }
                                }
                                return (Selected != null) && (!string.IsNullOrEmpty(FirstArg)) && (!string.IsNullOrEmpty(SecondArg)) && (!czyJuzJestNaLiscie);
                            }
                            else
                                return false;
                        }
                        );
                }
                return _edit;
            }
        }
        #endregion
        #region polecenie odpoiwedzialne za usuwanie  
        private ICommand _delete = null;
        public ICommand Delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new RelayCommand(
                        arg => {
                            var dialog = MessageBox.Show($"Czy na pewno chcesz usunąć zaznaczonego piłkarza?", "Uwaga", MessageBoxButton.OKCancel);
                            if (dialog == MessageBoxResult.OK)
                            {
                                List.Remove(Selected);
                                Clear();
                            }
                        },
                        arg => (Selected != null)
                        );
                }
                return _delete;
            }
        }
        #endregion
        #region polecenie odpoiwedzialne za serializacje
        private ICommand _serialize = null;
        public ICommand Serialize
        {
            get
            {
                if (_serialize == null)
                {
                    _serialize = new RelayCommand(
                        arg => {
                            WriteList();
                        },
                        arg => true
                        );
                }
                return _serialize;
            }
        }
        #endregion
        #region polecenie odpoiwedzialne za czyszczenie okna
        private void Clear()
        {
            FirstArg = "";
            SecondArg = "";
            Age = 20;
            Weight = 80;
        }
        #endregion
        #region metody odpowiedzialne za serializacje
        private void WriteList()
        {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Footballer>));
                Stream writer = File.Create("baza.xml");
                serializer.Serialize(writer, List);
                writer.Close();
        }
        private void ReadList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Footballer>));
            Stream fs = File.OpenRead("baza.xml");
            _list = (ObservableCollection<Footballer>)serializer.Deserialize(fs);
        }
        #endregion

        #endregion
    }
}
