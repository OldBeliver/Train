using System;
using System.Collections.Generic;

namespace Train
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal();
            terminal.Work();
        }
    }

    class Utils
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }

    class Direction
    {
        public Direction(string departure, string arrive)
        {
            Departure = departure;
            Arrive = arrive;
        }

        public string Departure { get; private set; }
        public string Arrive { get; private set; }
    }

    class Train
    {
        public Train()
        {
            Number = 0;
            Type = "нет";
            Direction = new Direction("пусто", "пусто");
            Track = 0;
            Status = "";
        }

        public Train(int number, string type, Direction direction, int track, string status)
        {
            Number = number;
            Type = type;
            Direction = direction;
            Track = track;
            Status = status;
        }

        public int Number { get; private set; }
        public string Type { get; private set; }
        public Direction Direction { get; private set; }
        public int Track { get; private set; }
        public string Status { get; private set; }

        public void SetStatus(string text)
        {
            Status = text;
        }

        public void SetNumber(int number)
        {
            Number = number;
        }

        public void SetType(string type)
        {
            Type = type;
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public void SetTrack(int track)
        {
            Track = track;
        }
    }

    class Terminal
    {
        private List<Train> _trains;
        private Renderer _renderer;

        public Terminal()
        {
            _trains = new List<Train>();
            _renderer = new Renderer();

            FillTrains();
        }

        public void Work()
        {
            bool isWork = true;

            while (isWork)
            {
                ShowDashboard();

                Console.WriteLine($"Нажмите любую клавишу для формирования нового поезда");
                Console.ReadKey();

                CreateNewTrain();

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowDashboard()
        {
            int shift = 2;
            int topPosition = 0;

            _renderer.PrintSeparator();
            _renderer.PrintHeader();
            _renderer.PrintSeparator();

            foreach (Train train in _trains)
            {
                _renderer.PrintTrain(train);
                _renderer.PrintSeparator();
            }

            for (int i = 0; i < _trains.Count; i++)
            {
                Console.ForegroundColor = SetTextColor(2);
                topPosition = (i + shift) * shift - 1;
                Console.SetCursorPosition(60, topPosition);
                Console.WriteLine($"{_trains[i].Status}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.SetCursorPosition(0, topPosition + shift);
        }

        private void CreateNewTrain()
        {
            Train train = new Train();
            int tracksCount = 10;

            string status = "формирование номера поезда";

            _renderer.PrintNewTrain(train, status);
            int number = ReadInt("номер поезда");
            train.SetNumber(number);


            status = "формирование типа поезда";
            _renderer.PrintNewTrain(train, status);
            train.SetType(GetWagonType());

            status = "формирование направления";
            _renderer.PrintNewTrain(train, status);
            train.SetDirection(GetDirection());


            status = "подготовка номера платформы";
            _renderer.PrintNewTrain(train, status);
            int track = Utils.GetRandomNumber(tracksCount);
            train.SetTrack(track);

            status = "сформирован";
            _renderer.PrintNewTrain(train, status);

            Console.WriteLine($"Нажмите любую клавишу, чтобы отправить поезд");
            train.SetStatus("отправлен");
            _trains.Add(train);
        }

        private ConsoleColor SetTextColor(int index)
        {
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green };

            return colors[index];
        }

        private void FillTrains()
        {
            _trains.Add(new Train(09, "фирм", new Direction("тудой", "сюдой"), 1, status: "отправлен"));
            _trains.Add(new Train(10, "фирм", new Direction("сюдой", "тудой"), 2, status: "отправлен"));
            _trains.Add(new Train(20, "литер", new Direction("тама", "здеся"), 3, status: "отправлен"));
            _trains.Add(new Train(120, "скорый", new Direction("ниоткуда", "в никуда"), 5, status: "отправлен"));
            _trains.Add(new Train(538, "пасс", new Direction("таракань", "кудыкины горы"), 7, status: "отправлен"));
        }

        private int ReadInt(string message)
        {
            int number = default;

            do
            {
                Console.WriteLine($"Введите {message}");
            }
            while (int.TryParse(Console.ReadLine(), out number) == false);

            return number;
        }

        private string GetWagonType()
        {
            string[] types = { "фирменный", "скорый", "пассажирский", "литерный", "электр" };
            int lineLength = 30;

            Console.WriteLine($"{new string('=', lineLength)}");
            Console.WriteLine($"Типы поездов:");
            Console.WriteLine($"{new string('=', lineLength)}");

            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            Console.WriteLine($"{new string('=', lineLength)}");

            int index = ReadInt("номер типа");

            if (index < 1 || index > types.Length)
            {
                Console.WriteLine($"выход за пределы диапазона");
                index = types.Length - 1;
                Console.WriteLine($"по умолчанию назначен тип поезда: {types[index]}");
            }

            return types[index - 1];
        }

        private Direction GetDirection()
        {
            string departure = string.Empty;
            string arrive = string.Empty;

            do
            {
                Console.WriteLine($"Введите пункт отправления");
                departure = Console.ReadLine();

                Console.WriteLine($"Введите пункт прибытия");
                arrive = Console.ReadLine();
            }
            while (departure == arrive);

            return new Direction(departure, arrive);
        }
    }

    class Renderer
    {
        public void PrintTrain(Train train)
        {
            Console.WriteLine($"{train.Number}\t{train.Type}\t\t{train.Track}\t{train.Direction.Departure} - {train.Direction.Arrive}");
        }

        public void PrintNewTrain(Train train, string status)
        {
            Console.Clear();
            PrintSeparator();
            PrintHeader();
            PrintSeparator();
            Console.WriteLine($"{train.Number}\t{train.Type}\t\t{train.Track}\t{train.Direction.Departure} - {train.Direction.Arrive}");
            Console.SetCursorPosition(60, 3);
            Console.WriteLine($"{status}");
        }

        public void PrintHeader()
        {
            string header = $"номер\tкатегория\tпуть\tназначение\t\t\tстатус";

            PrintGreenText(header);
        }

        public void PrintSeparator()
        {
            string separator = new string('=', 80);

            Console.WriteLine($"{separator}");
        }

        private void PrintGreenText(string text)
        {
            ConsoleColor defaultColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{text}");
            Console.ForegroundColor = defaultColor;
        }
    }

}
