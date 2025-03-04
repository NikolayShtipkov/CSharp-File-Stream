using System;
using System.Collections.Generic;
using System.Linq;

// Абстрактен клас за превозно средство
abstract class Vehicle
{
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public int MaxPassengers { get; private set; }

    protected Vehicle(string brand, string model, int year, int maxPassengers)
    {
        Brand = brand;
        Model = model;
        Year = year;
        MaxPassengers = maxPassengers;
    }

    public abstract decimal GetRentalPrice();

    public override string ToString()
    {
        return $"{Brand} {Model} ({Year}) - {MaxPassengers} пътници";
    }
}

// Конкретни класове за различните типове превозни средства
class Motorcycle : Vehicle
{
    private decimal price;
    public Motorcycle(string brand, string model, int year, int maxPassengers, decimal price)
        : base(brand, model, year, maxPassengers)
    {
        this.price = price;
    }

    public override decimal GetRentalPrice() => price;
}

class Car : Vehicle
{
    protected decimal price;
    public Car(string brand, string model, int year, int maxPassengers, decimal price)
        : base(brand, model, year, maxPassengers)
    {
        this.price = price;
    }

    public override decimal GetRentalPrice() => price;
}

class LuxuryCar : Car
{
    public LuxuryCar(string brand, string model, int year, int maxPassengers, decimal price)
        : base(brand, model, year, maxPassengers, price)
    {
    }

    public override decimal GetRentalPrice() => base.GetRentalPrice() + 200;
}

class Van : Vehicle
{
    private decimal price;
    public Van(string brand, string model, int year, int maxPassengers, decimal price)
        : base(brand, model, year, maxPassengers)
    {
        this.price = price;
    }

    public override decimal GetRentalPrice() => price;
}

class LuxuryVan : Van
{
    public LuxuryVan(string brand, string model, int year, int maxPassengers, decimal price)
        : base(brand, model, year, maxPassengers, price)
    {
    }

    public override decimal GetRentalPrice() => base.GetRentalPrice() + 200;
}

class Bus : Vehicle
{
    private decimal standardSeatPrice;
    public Bus(string brand, string model, int year, int maxPassengers, decimal standardSeatPrice)
        : base(brand, model, year, maxPassengers)
    {
        this.standardSeatPrice = standardSeatPrice;
    }

    public override decimal GetRentalPrice()
    {
        int luxurySeats = (int)(MaxPassengers * 0.2);
        int standardSeats = MaxPassengers - luxurySeats;
        decimal luxurySeatPrice = standardSeatPrice * 1.3m;

        return (standardSeats * standardSeatPrice) + (luxurySeats * luxurySeatPrice);
    }
}

class Program
{
    static void Main()
    {
        List<Vehicle> vehicles = new List<Vehicle>
        {
            new Motorcycle("Yamaha", "MT-07", 2022, 2, 50),
            new Car("Toyota", "Corolla", 2021, 5, 100),
            new LuxuryCar("BMW", "5 Series", 2022, 5, 150),
            new Van("Ford", "Transit", 2020, 9, 120),
            new LuxuryVan("Mercedes", "V-Class", 2023, 7, 180),
            new Bus("Volvo", "9400", 2019, 50, 10)
        };

        Console.WriteLine("Налични превозни средства:");
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine($"{vehicle} - Цена на наем: {vehicle.GetRentalPrice()} лв.");
        }

        int totalPassengers = vehicles.Sum(v => v.MaxPassengers);
        decimal totalRentalPrice = vehicles.Sum(v => v.GetRentalPrice());

        Console.WriteLine($"\nОбщ брой пътници: {totalPassengers}");
        Console.WriteLine($"Обща сума от транспортни цени: {totalRentalPrice} лв.");
    }
}
