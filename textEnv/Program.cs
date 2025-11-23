using ManczakSzybrua.KatalogProcesorow.DAO;
using ManczakSzybrua.KatalogProcesorow.CORE;
using ManczakSzybrua.KatalogProcesorow.INTERFACES;
using Microsoft.EntityFrameworkCore;


// PROSTY TEST BAZY DANYCH
Console.WriteLine("=== TEST BAZY SQLITE ===");

using (var db = new DataContext())
{
    Console.WriteLine("Tworzę bazę jeśli nie istnieje...");
    db.Database.EnsureCreated();

    // 1. Dodaj producenta jeśli brak
    if (!db.Manufacturers.Any())
    {
        Console.WriteLine("Dodaję producenta AMD...");

        db.Manufacturers.Add(new ManufacturerDO
        {
            Id = 1,
            Name = "AMD222"
        });

        db.SaveChanges();
    }

    // Pobierz producenta
    var amd = db.Manufacturers.First(m => m.Name == "AMD");

    // 2. Dodaj CPU jeśli brak
    if (!db.Cpus.Any())
    {
        Console.WriteLine("Dodaję CPU Ryzen 5 5600X...");

        db.Cpus.Add(new CPU_DO
        {
            Name = "Ryzen 5 5600X",
            Cores = 6,
            Threads = 12,
            BaseClockGHz = 3.7,
            Socket = SocketType.SocketAm4,
            ManufacturerId = amd.Id
        });

        db.SaveChanges();
    }

    // 3. Odczyt z bazy
    Console.WriteLine("\n=== LISTA CPU W BAZIE ===");

    var cpus = db.Cpus.ToList();

    foreach (var cpu in cpus)
    {
        Console.WriteLine($"{cpu.Id}: {cpu.Name} | {cpu.Cores}/{cpu.Threads} | {cpu.Socket} | ProdId={cpu.ManufacturerId}");
    }
}

Console.WriteLine("\nTEST ZAKOŃCZONY.");
