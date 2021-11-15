using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zadanie8_Plarium
{
    [Serializable]
    abstract class Region//абстрактный класс регион
    {
        public string Nazva;//название региона
        public int Plochad;// площадь региона
        public People people;// люди которые живут в регионе

        public Region(string Name, int plochad, People person)
        {
            Nazva = Name;
            Plochad = plochad;
            people = person;
         


        }

        public abstract string GetInfo();//абстрактный метод вывода информации о регионе

    }
    [Serializable]
    class Oblast : Region//наследник от Region класс области
    {
        public Oblast(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        }
        public override string GetInfo()//переопределенный метод для области
        {
            return $"{Nazva}";
        }
    }
    [Serializable]
    class Country : Region//наследник от Region класс Страна
    {
        public Country(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        

        }
        
        public override string GetInfo()//переопределенный метод для страны
        {
            return $"{Nazva}";
        }
    }
    [Serializable]
    class City : Region//наследник от Region класс города
    {
        public City(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        }

        public override string GetInfo()//переопределенный метод для города
        {
            return $"{Nazva}";
        }
    }
}
