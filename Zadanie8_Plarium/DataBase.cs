using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zadanie8_Plarium
{
    [Serializable]
    class DataBase
    {
        [XmlIgnore]
        private int iPeople= 0;
        [XmlIgnore]
        private int iRegion = 0;
        [XmlIgnore]
        private int iPogoda = 0;
        [XmlArray("CollectionPeople"), XmlArrayItem("Item")]
        public List<People> peoples = new List<People>();
        [XmlArray("CollectionRegion"), XmlArrayItem("Item")]
        public List<Region> regions = new List<Region>();
        [XmlIgnore]
        public Pogoda wether = new Pogoda();
        [XmlArray("CollectionPogoda"), XmlArrayItem("Item")]
        public List<Pogoda> pogodas = new List<Pogoda>();

        public DataBase()
        {


        }
    
        #region AddData
        public void AddData(People people)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("People.txt", true))
            {
                file.WriteLine($"#{iPeople++}\nЛюди:\n{people.Nazvanie}\nЯзык:\n{people.Langue}\n");
                file.Close();
            }
            peoples.Add(people);
        }

        public void AddData(Region region)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Region.txt", true))
            {

                file.WriteLine($"#{iRegion++}\nРегион:\n{region.Nazva}\nПлощадь:\n{region.Plochad}\nЛюди:\n{region.people.Nazvanie}\n");

                file.Close();
            }
            regions.Add(region);
        }

        public void AddData(Pogoda pogoda)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Pogoda.txt", true))
            {
                file.WriteLine($"#{iPogoda++}\n"+pogoda.ToString());
                file.Close();
            }
            pogodas.Add(pogoda);
        }
        #endregion

        #region removeData
       
        public void Cleener()
        {
            System.IO.File.WriteAllBytes("People.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Region.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Pogoda.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Return.txt", new byte[0]);
        }
        #endregion


        public void GetToColection()
        {
            
            using (StreamReader sr = new StreamReader("People.txt", System.Text.Encoding.Default))
            { 
                bool Test = false;
                string lines = "0";
                string line;
                string peopl="", lang="";
                    while ((line = sr.ReadLine()) != null)
                    {

                    switch (lines)
                    {
                        case "Люди:":
                            {
                                peopl = line;
                                break;
                            }
                        case "Язык:":
                            {
                                lang = line;
                                Test = true;
                                break;
                            }
                        case "\n":
                            {
                                
                                break;
                            }
                        default:
                            {
                              
                                break;
                            }
                    }

                    if (Test) { peoples.Add(new People(peopl, lang)); Test = false; }
                        lines = line;
                    }
                
            }

            using (StreamReader sr = new StreamReader("Region.txt", System.Text.Encoding.Default))
            {
                bool Test = false;
                string lines = "0";
                string line;
                int pl = 0, index=0;
                string peopl = "";
                while ((line = sr.ReadLine()) != null)
                {

                    switch (lines)
                    {
                        case "Регион:":
                            {
                                peopl = line;
                                break;
                            }
                        case "Площадь:":
                            {
                                pl = Convert.ToInt32(line);
                                break;
                            }
                        case "Люди:":
                            {
                                index = peoples.Select((item, i) => new { Item = item, Index = i })
                 .First(x => x.Item.Nazvanie == line).Index;
Test = true;
                                break;
                            }
                        case "\n":
                            {
                                
                                break;
                            }
                        default:
                            {

                                break;
                            }
                    }

                    if (Test) { regions.Add(new Country(peopl, pl, peoples[index])); Test = false; }
                    lines = line;
                }

            }

            using (StreamReader sr = new StreamReader("Pogoda.txt", System.Text.Encoding.Default))
            {
                bool Test = false;
                string lines = "0";
                string line;
                string peopl = "";
                int  index = 0;
                decimal t=0;
                DateTime date = new DateTime();
                while ((line = sr.ReadLine()) != null)
                {

                    switch (lines)
                    {
                        case "Регион:":
                            {
                                index = regions.Select((item, i) => new { Item = item, Index = i })
                 .First(x => x.Item.Nazva == line).Index;
                               
                                break;
                            }
                        case "Дата:":
                            {
                                date = Convert.ToDateTime(line);

                                break;
                            }
                        case "Температура:":
                            {
                                t = Convert.ToDecimal(line);
                              
                                break;
                            }
                        case "Осодки:":
                            {
                                peopl = line;
  Test = true;
                                break;
                            }
                        case "\n":
                            {
                              
                                break;
                            }
                        default:
                            {

                                break;
                            }
                    }

                    if (Test) { pogodas.Add(new Pogoda(regions[index],date,t,peopl)); Test = false; }
                    lines = line;
                }

            }
        }



        public void Serialize(DataBase dataBase)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
            using (FileStream fs = new FileStream("DataBase.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, dataBase);
            }
        }
        public DataBase DeSerialize(DataBase dataBase)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataBase));
            using (FileStream fs = new FileStream("DataBase.xml", FileMode.OpenOrCreate))
            {
                dataBase = (DataBase)serializer.Deserialize(fs);
            }
            return dataBase;
        }

       
    }
}
