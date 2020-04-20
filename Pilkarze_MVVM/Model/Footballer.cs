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

        public string ToFileFormat()
        {
            return $"{Nazwisko}|{Imie}|{Wiek}|{Waga}";
        }

        public static Footballer CreateFromString(string sPilkarz)
        {
            string imie, nazwisko;
            uint wiek, waga;
            var pola = sPilkarz.Split('|');
            if (pola.Length == 4)
            {
                nazwisko = pola[0];
                imie = pola[1];
                wiek = uint.Parse(pola[2]);
                waga = uint.Parse(pola[3]);
                return new Footballer(imie, nazwisko, wiek, waga);
            }
            throw new Exception("Błędny format danych z pliku");
        }
        #endregion
    }
}
