using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zadanie8_Plarium
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase dataBase=new DataBase();//создаем БД
            Pogoda wether = new Pogoda();
            //добавление в делегат методов установки начальных значений
            AddStartValue addStartValue;
            addStartValue = Cleener;
            addStartValue += AddPeoples;
            addStartValue += AddRegion;
            addStartValue += AddWether;
            #region Обработка нажатия клавиши
            KeyEvent evnt = new KeyEvent();//событие нажатия клавиши
            evnt.KeyDown += async (sender, e) =>
            {
                switch (e.ch)
                {
                    case '1'://устанавливаем начальные значения
                        {
                            try
                            {

                                addStartValue();
                            }
                            catch
                            {

                            }

                            break;
                        }
                    case '2':
                        {
                            try
                            {
                                bool Test = false;
                                //проверяем файл SeriaState.txt, если состояние истино то востанавливаем из серриолизации, иначе из БД
                                using (StreamReader sr = new StreamReader("SeriaState.txt", System.Text.Encoding.Default))
                                {
                                    string line;

                                    while ((line = sr.ReadLine()) != null)
                                    {


                                        if (line == "True") Test = true;
                                    }

                                }
                                if (Test) dataBase = LoadFromBinaryFile("dataBase.dat");//рассериализация
                                else dataBase.GetToColection();//из БД
                            }
                            catch
                            {
                                Console.WriteLine($"База данных пуста");
                            }

                            break;
                        }
                    case '3':
                        {
                            wether.Notify += AddToReturnFile;//подписываемся на события вывода(если не подписаться в консоль метод ничего не выведет)
                            try
                            {
                                wether.GetPogoda(dataBase.pogodas, dataBase.regions[0]);//Вывести сведения о погоде в заданном регионе.
                            }
                            catch
                            {
                                Console.WriteLine($" Еще не установлены значения");
                            }

                            wether.Notify -= AddToReturnFile;//отписываемся от события
                            break;
                        }
                    case '4':
                        {
                            wether.Notify += AddToReturnFile;//подписываемся на событие
                            try
                            {
                                wether.GetData(dataBase.pogodas, dataBase.regions[1], "Снег", 0);// Вывести даты, когда в заданном регионе шел снег и температура была ниже заданной отрицательной.
                            }
                            catch
                            {
                                Console.WriteLine($" Еще не установлены значения");
                            }

                            wether.Notify -= AddToReturnFile;//отписываемся от события
                            break;
                        }
                    case '5':
                        {
                            wether.Notify += AddToReturnFile;//подписываемся на событие
                            try
                            {
                                wether.GetPogoda(dataBase.pogodas, "английский");//Вывести информацию о погоде за прошедшую неделю в регионах, жители которых общаются на заданном языке.
                            }
                            catch
                            {
                                Console.WriteLine($" Еще не установлены значения");
                            }

                            wether.Notify -= AddToReturnFile;//отписываемся от события
                            break;
                        }
                    case '6':
                        {
                            wether.Notify += AddToReturnFile;//подписываемся на событие
                            try
                            {
                                wether.GetTemp(dataBase.pogodas, 6000, dataBase.regions);//Вывести среднюю температуру за прошедшую неделю в регионах с площадью больше заданной.
                            }
                            catch
                            {
                                Console.WriteLine($" Еще не установлены значения");
                            }

                            wether.Notify -= AddToReturnFile;//отписываемся от события
                            break;
                        }
                    case '7'://серриализируем БД в файл
                        {
                            try
                            {
                                SaveBinaryFormat(dataBase, "dataBase.dat");
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter("SeriaState.txt"))
                                {

                                    file.WriteLine("True");

                                    file.Close();
                                }
                            }
                            catch
                            {
                                Console.WriteLine($"Ошибка");
                            }


                            break;
                        }
                    case '0'://завершение
                        {

                            Console.WriteLine($"Программа завершена");
                            break;
                        }
                    default://неизвестные значения
                        {
                            Console.WriteLine($"такого значения не предусмотрено");
                            break;
                        }
                }
            };

            Console.WriteLine($"Управляющие команды: \n" +
                $"1-Заполнить все исхордными данными\n" +
                $"2-Восстановить данные из базы данных\n" +
                $"3-Вывести сведения о погоде в заданном регионе\n" +
                $"4-Вывести даты, когда в заданном регионе шел снег и температура была ниже заданной отрицательной\n" +
                $"5-Вывести информацию о погоде за прошедшую неделю в регионах, жители которых общаются на заданном языке\n" +
                $"6-Вывести среднюю температуру за прошедшую неделю в регионах с площадью больше заданной\n" +
                $"7-сериализовать\n" +
                $"0-Выход\n");
            char ch;
            do//обработка пока не будет нажат выход(0)
            {
                Console.Write("Введите комманду: ");
                ConsoleKeyInfo key;
                key = Console.ReadKey();
                ch = key.KeyChar;
                Console.WriteLine("");
                evnt.OnKeyDown(key.KeyChar);

            }
            while (ch != '0');
            System.Console.ReadKey(true);
            #endregion
            #region Методы для добавления начальных значений
            void AddPeoples()
            { 
                dataBase.AddData(new People("Евреи", "иврит"));
                dataBase.AddData(new People("Англичане", "английский"));
            }
            void AddRegion()
            {
                dataBase.AddData(new Country("Британия", 7000, dataBase.peoples[1]));
                dataBase.AddData( new City("Израиль", 10000, dataBase.peoples[0]));
                dataBase.AddData( new Oblast("Шотландия", 4000, dataBase.peoples[1]));
            }
            void AddWether()
            {
                dataBase.AddData(new Pogoda(dataBase.regions[0], new DateTime(2021, 10, 21, 00, 00, 00), 3, "Дождь"));
                dataBase.AddData(new Pogoda(dataBase.regions[0], new DateTime(2021, 10, 26, 00, 00, 00), -3, "Снег"));
                dataBase.AddData(new Pogoda(dataBase.regions[0], new DateTime(2021, 10, 23, 00, 00, 00), 13, "Солнце"));

                dataBase.AddData(new Pogoda(dataBase.regions[1], new DateTime(2021, 10, 2, 00, 00, 00), -6, "Снег"));
                dataBase.AddData(new Pogoda(dataBase.regions[1], new DateTime(2021, 10, 25, 00, 00, 00), -3, "Снег"));
                dataBase.AddData(new Pogoda(dataBase.regions[1], new DateTime(2021, 10, 30, 00, 00, 00), 13, "Дождь"));

                dataBase.AddData(new Pogoda(dataBase.regions[2], new DateTime(2021, 10, 19, 00, 00, 00), 3, "Дождь"));
                dataBase.AddData(new Pogoda(dataBase.regions[2], new DateTime(2021, 10, 25, 00, 00, 00), 13, "Снег"));
                dataBase.AddData(new Pogoda(dataBase.regions[2], new DateTime(2021, 10, 30, 00, 00, 00), 13, "Солнце"));
            }
            #endregion
        }

        private static void DisplayMessage(string message) => Console.WriteLine(message);//метод для вывода который передаем в делегат события

        delegate void AddStartValue();//делегат стартовых значений
        private static void AddToReturnFile(string s)//метод для вывода в файл  который передаем в делегат события
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Return.txt",true))
            {

                file.WriteLine(s, true);

                file.Close();
            }
        }

        private static void Cleener()//метод очистки
        {
            System.IO.File.WriteAllBytes("People.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Region.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Pogoda.txt", new byte[0]);
            System.IO.File.WriteAllBytes("Return.txt", new byte[0]);
        }

        static void SaveBinaryFormat(object objGraph, string fileName)// метод сериализации
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, objGraph);
            }
            Console.WriteLine("--> Сохранение объекта в Binary format");
        }
        static DataBase LoadFromBinaryFile(string fileName)//метод десериализации
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            DataBase dataBaseerial;
            using (Stream fStream = File.OpenRead(fileName))
            {
                dataBaseerial =
                     (DataBase)binFormat.Deserialize(fStream);

            }
            return dataBaseerial;
        }
    }

}
