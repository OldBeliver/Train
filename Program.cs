using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Train
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal terminal = new Terminal();
            terminal.Work();

            Console.ReadKey();
        }
    }

    class Utils
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber( int maxValue)
        {
            return s_random.Next(maxValue);
        } 

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }

        public static void PressEnter()
        {
            do
            {
                Console.WriteLine($"Нажмите Enter для продолжения");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Enter);
        }
    }

    class Terminal
    {
        private TrainCreator _creator = new TrainCreator();
        private List<Train> _trains = new List<Train>();

        public Terminal()
        {
            FillTrains();
        }

        public void Work()
        {
            bool isShowMustGoOn = true;

            while (isShowMustGoOn)
            {
                ShowDashbord();

                Train train = _creator.CreateNewTrain();

                Cartoon animation = new Cartoon();
                animation.AnimationTrain();

                _trains.Add(train);
            }
        }

        private void FillTrains()
        {
            _trains.Add(new Train(1, "литерный", new Direction("здеся", "тама"), new List<Wagon>(), 1, "задерживается"));
            _trains.Add(new Train(20, "скорый", new Direction("тудой", "сюдой"), new List<Wagon>(), 2, "прибывает"));
            _trains.Add(new Train(300, "пассажирский", new Direction("Кудыкины горы", "Тьма таракань"), new List<Wagon>(), 3, "отправляется"));
        }

        private void ShowDashbord()
        {
            (string, int)[] titles = new[]
            {
                ("номер", 7),
                ("тип", 13),
                ("наименование", 33),
                ("путь", 12),
                ("статус", 10)
            };

            string separator = new string('=', 80);
            string header = "";

            Console.Clear();

            for (int i = 0; i < titles.Length; i++)
            {
                int shift = titles[i].Item2;
                header += $"{titles[i].Item1.PadRight(shift)}";
            }

            Console.WriteLine($"{separator}");
            Console.WriteLine($"{header}");
            Console.WriteLine($"{separator}");

            for (int i = 0; i < _trains.Count; i++)
            {
                string line =
                    $"{_trains[i].Number.ToString().PadRight(7)}" +
                    $"{_trains[i].Type.PadRight(13)}" +
                    $"{_trains[i].Direction.Departure.PadRight(15)}{_trains[i].Direction.Arrive.PadRight(20)}" +
                    $"{_trains[i].Track.ToString().PadRight(10)}" +
                    $"{_trains[i].Status}";

                Console.WriteLine($"{line}");
                Console.WriteLine($"{separator}");
            }

            Utils.PressEnter();
        }
    }

    class Train
    {
        public Train(int number, string type, Direction direction, List<Wagon> wagons, int track, string status)
        {
            Number = number;
            Type = type;
            Direction = direction;
            Wagons = wagons;
            Track = track;
            Status = status;
        }

        public Train(Direction direction, int wagons):this(3, "", direction, new List<Wagon>(), 5, "отправлен")
        {
            Direction = direction;
            WagonsCount = wagons;
        }

        public int Number { get; }
        public string Type { get; }
        public Direction Direction { get; }
        public int WagonsCount { get; }
        public List<Wagon> Wagons { get; }
        public int Track { get; }
        public string Status { get; }
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

    class Wagon
    {
        public int Capacity => 23;
    }

    class TrainCreator
    {
        public Train CreateNewTrain()
        {
            ShowCurrentInfo();

            Direction direction = CreateDirection();
            ShowCurrentInfo(direction.Departure, direction.Arrive);

            int tickets = CreateTickets();
            ShowCurrentInfo(direction.Departure, direction.Arrive, tickets);

            int wagons = CalculateWagons(tickets);
            ShowCurrentInfo(direction.Departure, direction.Arrive, tickets, wagons);

            return new Train(direction, wagons);
        }

        private int CalculateWagons(int tickets)
        {
            int wagonCapacity = new Wagon().Capacity;

            int wagons = tickets / wagonCapacity;

            if (tickets % wagonCapacity != 0)
            {
                wagons++;
            }

            Console.WriteLine($"Добавлено {wagons} вагонов. В каждом вагоне {wagonCapacity} места");
            Console.ReadKey();
            return wagons;
        }

        private int CreateTickets()
        {
            int minTickets = 200;
            int maxTickets = 1200;
            int tickets = Utils.GetRandomNumber(minTickets, maxTickets);

            Console.WriteLine($"На данное направлении было продано {tickets} билетов");

            Console.ReadKey();

            return tickets;
        }

        private Direction CreateDirection()
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

        private void ShowCurrentInfo(string departure = "не указан", string arrive = "не указан", int tickets = 0, int wagons = 0)
        {
            string header = "Информация о формирующемся составе";
            string separator = new string('=', 40);

            Console.Clear();
            Console.WriteLine($"{separator}");
            Console.WriteLine($"{header}");
            Console.WriteLine($"{separator}");
            Console.WriteLine($"Пункт отправления: {departure}");
            Console.WriteLine($"Пункт прибытия: {arrive}");
            Console.WriteLine($"Продано билетов: {tickets}");
            Console.WriteLine($"Количество вагонов: {wagons}");
            Console.WriteLine($"{separator}");

            Utils.PressEnter();
        }
    }

    class Cartoon
    {
        public void AnimationTrain()
        {
            Console.CursorVisible = false;

            string[] train =
            {
                " 0 o O    ",
                "      *   ",
                " [O]__ST  ",
                " |======} ",
                " '000--o\\.",
            };

            string[] train2 =
                {
                "0 O o     ",
                "      o   ",
                " [O]__ST  ",
                " |======} ",
                " '000--o\\.",
            };

            int trainPositionLeft = 2;
            int trainPositionTop = 10;
            int trainMoveDistance = 50;
            int trainSizeWidth = train[0].Length;
            int trainSizeHeight = train.Length;

            int pathPositionLeft = trainPositionLeft;
            int pathPositionTop = trainPositionTop + trainSizeHeight - 1;
            int pathDistance = trainMoveDistance + trainSizeWidth;

            for (int i = 0; i < trainMoveDistance; i++)
            {
                string[] trainSprite = train2;

                if (i % 2 == 0)
                {
                    trainSprite = train;
                }

                DrawTrain(trainSprite, trainPositionLeft, trainPositionTop, ConsoleColor.Black);
                DrawSymbol('.', pathPositionLeft, pathPositionTop, ConsoleColor.White, pathDistance);
                trainPositionLeft++;
                DrawTrain(trainSprite, trainPositionLeft, trainPositionTop, ConsoleColor.White);

                Thread.Sleep(100);
            }

            Console.CursorVisible = true;
        }

        private void DrawTrain(string[] train, int positionLeft, int PositionTop, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            for (int i = 0; i < train.Length; i++)
            {
                string line = train[i];
                Console.SetCursorPosition(positionLeft, PositionTop + i);
                Console.Write(line);
            }
        }

        private void DrawSymbol(char symbol, int positionLeft, int positionTop, ConsoleColor color, int count)
        {
            Console.SetCursorPosition(positionLeft, positionTop);
            Console.ForegroundColor = color;

            for (int i = 0; i < count; i++)
            {
                Console.Write(symbol);
            }
        }
    }
}
