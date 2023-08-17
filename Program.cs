using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Train
{
    class Program
    {
        static void Main(string[] args)
        {
            

            RailwayDepot depot = new RailwayDepot();

            depot.Work();
        }
    }

    class Direction
    {
        private string _departurePoint;
        private string _arrivePoint;

        public Direction(string departure, string arrive)
        {
            _departurePoint = departure;
            _arrivePoint = arrive;
        }

        public string Departure => _departurePoint;
        public string Arrive => _arrivePoint;
    }

    class Wagon
    {
        public int Seats { get; private set; } = 20;
    }

    class RailwayDepot
    {
        private static Random s_random = new Random();

        private Direction _direction = new Direction("не задано", "не задано");
        private int _tickets = default;
        private int _wagonsCount = default;
        private string _trainStatus;

        public void Work()
        {
            ShowTrainStatus();

            CreateDirection();
            ShowTrainStatus();

            SellTickets();
            ShowTrainStatus();

            CalculateWagonsCount(_tickets);
            _trainStatus = "отправлен";
            ShowTrainStatus();
        }

        private void ResetData()
        {
            _direction = new Direction("не задано", "не задано");
            _tickets = default;
            _wagonsCount = default;
            _trainStatus = "нет данных";
        }

        private void ShowTrainStatus()
        {
            Console.WriteLine($"Железнодорожный магнат");
            Console.WriteLine($"======================");
            Console.WriteLine($"Поезд: {_trainStatus}");
            Console.WriteLine($"Направление: {_direction.Departure} - {_direction.Arrive}");
            Console.WriteLine($"Количество проданных билетов {_tickets}");
            Console.WriteLine($"Количество вагонов {_wagonsCount}");

            Console.ReadKey();
            Console.Clear();
        }

        private void CreateDirection()
        {
            string departure;
            string arrive;

            do
            {
                Console.WriteLine($"Введите пункт отправления");
                departure = Console.ReadLine();

                Console.WriteLine($"Введите пункт прибытия");
                arrive = Console.ReadLine();
            }
            while (departure == arrive);

            _direction = new Direction(departure, arrive);
            _trainStatus = "формируется";
        }

        private void SellTickets()
        {
            int minTicketsCount = 200;
            int maxTicketsCount = 1000;

            _tickets = s_random.Next(minTicketsCount, maxTicketsCount + 1);
        }

        private void CalculateWagonsCount(int tickets)
        {
            Wagon wagon = new Wagon();
            int wagonSeats = wagon.Seats;

            _wagonsCount = (tickets / wagonSeats == 0) ? tickets / wagonSeats : tickets / wagonSeats + 1;
        }
    }

}
