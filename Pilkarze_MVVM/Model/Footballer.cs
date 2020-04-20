using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilkarze_MVVM.Model
{
    public class Footballer
    {
        #region Prop
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public uint Wiek { get; set; }
        public uint Waga { get; set; }
        #endregion

        #region constr
        public Footballer()
        {
        }
        public Footballer(string imie, string nazwisko, uint wiek, uint waga)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Wiek = wiek;
            Waga = waga;
        }
        #endregion

        #region methods

        //sprawdza czy obiekt ma ten sam stan co bieżąca instancja
        public bool isTheSame(Footballer footballer)
        {
            if (footballer.Nazwisko != Nazwisko) return false;
            if (footballer.Imie != Imie) return false;
            if (footballer.Wiek != Wiek) return false;
            if (footballer.Waga != Waga) return false;
            return true;
        }

        public override string ToString()
        {
            return $"{Nazwisko} {Imie} lat: {Wiek} waga: {Waga} kg";
        }
        #endregion
    }
}
