using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zadanie8_Plarium
{
    [Serializable]
    class People
    {
        [XmlElement("Name")]
        public string Nazvanie;
        [XmlAttribute("Value")]
        public string Langue;
        
        public People(string Name, string langue)
        {
            Nazvanie = Name;
            Langue = langue;
       

        }
        public People(People other)
        {
            Nazvanie = other.Nazvanie;
            Langue = other.Langue;
        }
       
    }
}
