using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zadanie8_Plarium
{
    [Serializable]
    class Pogoda
    {
        [XmlAttribute("Region")]
        public Region reg { get; set; }
        [XmlAttribute("Data")]
        public DateTime date;
        [XmlAttribute("Temperatyre")]
        public decimal temp;
        [XmlElement("Osad")]
        public string osad;

        public delegate void AccountHandler(string message);
        private event AccountHandler _notify;
        public event AccountHandler Notify
        {
            add
            {
                _notify += value;
                Console.WriteLine($"{value.Method.Name} добавлен");
            }
            remove
            {
                _notify -= value;
                Console.WriteLine($"{value.Method.Name} удален");
            }
        }

        public Pogoda(Region region, DateTime Date, decimal T, string Osad)
        {
            reg = region;
            date = Date;
            temp = T;
            osad = Osad;
        }
            
        public Pogoda()
        {
           

        }
        public void GetPogoda(List<Pogoda> vezers, Region region)
        {

            IEnumerable<Pogoda> results = vezers.Where(s => s.reg.Nazva == region.Nazva);

            foreach (Pogoda pogoda in results)
               
                {
                    _notify?.Invoke($"{pogoda.reg.GetInfo()} {pogoda.date} числа, температура {pogoda.temp + "°C"} , осадки:{pogoda.osad}");
                }

        }
        public void GetData(List<Pogoda> vezers, Region region, string osadki, decimal zTemp)
        {

            IEnumerable<Pogoda> results = from s in vezers
                                                     where s.reg.Nazva == region.Nazva && s.osad == osadki && zTemp > s.temp
                                          select s;
            foreach (Pogoda pogoda in results)
                    _notify?.Invoke($" {pogoda.date} числа {pogoda.reg.GetInfo()}, температура {pogoda.temp + "°C"} была меньше заданной {zTemp + "°C"}, и были заданные осадки:{pogoda.osad}");
        }
        public void GetPogoda(List<Pogoda> vezers, string Lang)
        {
            IEnumerable<Pogoda> results = from s in vezers
                                          where s.reg.people.Langue == Lang && s.date.AddDays(7) >= DateTime.Today
                                          select s;
            foreach (Pogoda pogoda in results)
            _notify?.Invoke($"{pogoda.reg.GetInfo()} люди говорят на языке {Lang} {pogoda.date} числа, температура {pogoda.temp + "°C"}, осадки:{pogoda.osad}");
               
            
        }
        public void GetTemp(List<Pogoda> vezers, int Zplochad, List< Region> regions)
        {
            decimal srTemp = 0;
            foreach (Region region in regions)
            {
                try
                {
                    if (region.Plochad > Zplochad)
                        foreach (Pogoda pogoda in vezers)
                            if (pogoda.reg.Nazva == region.Nazva && pogoda.date.AddDays(7) >= DateTime.Today)
                                srTemp += pogoda.temp;

                    _notify?.Invoke($"{region.GetInfo()} средняя температура {srTemp + "°C"}");
                    srTemp = 0;
                }
                catch { _notify?.Invoke($"возникла ошибка, выход за пределы"); }

            }

        }
        public override string ToString()
        {
            return $"Регион:\n{reg.GetInfo()}\nДата:\n{date}\nТемпература:\n{temp}\nОсодки:\n{osad}\n";
        }
  
        public void Cleener()
        {
            System.IO.File.WriteAllBytes("Pogoda.txt", new byte[0]);
        }
    }
}
