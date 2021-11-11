using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zadanie8_Plarium
{
    [Serializable]
    abstract class Region
    {
        [XmlElement("Name")]
        public string Nazva;
        [XmlAttribute("Value")]
        public int Plochad;
        [XmlAttribute("People")]
        public People people;

        public Region(string Name, int plochad, People person)
        {
            Nazva = Name;
            Plochad = plochad;
            people = person;
         


        }

      

        public abstract string GetInfo();

    }

    class Oblast : Region
    {
        public Oblast(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        }
        public override string GetInfo()
        {
            return $"{Nazva}";
        }
    }

    class Country : Region
    {
        public Country(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        

        }
        
        public override string GetInfo()
        {
            return $"{Nazva}";
        }
    }

    class City : Region
    {
        public City(string Name, int plochad, People person) : base(Name, plochad, person)
        {
        }

        public override string GetInfo()
        {
            return $"{Nazva}";
        }
    }
}
