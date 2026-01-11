using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.DBFile
{
    public class DAOFile : IDAO
    {
        private List<ManufacturerDBFile> manufacturers = new();
        private List<CPUDBFile> cPUs = new();
        private readonly string manPath = "manufacturers.json";
        private readonly string cpuPath = "cpus.json";

        public DAOFile()
        {
            string fullPath = Path.GetFullPath(cpuPath);
            Console.WriteLine($"[DAOFile] Data is being stored at: {fullPath}");

            Load();
        }

        private void Save()
        {
            foreach (var cpu in cPUs)
            {
                if (cpu.manufacturer != null) cpu.ManufacturerId = cpu.manufacturer.Id;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(manPath, JsonSerializer.Serialize(manufacturers, options));
            File.WriteAllText(cpuPath, JsonSerializer.Serialize(cPUs, options));
        }

        private void Load()
        {
            if (File.Exists(manPath) && File.Exists(cpuPath))
            {
                try
                {
                    manufacturers = JsonSerializer.Deserialize<List<ManufacturerDBFile>>(File.ReadAllText(manPath)) ?? new();
                    cPUs = JsonSerializer.Deserialize<List<CPUDBFile>>(File.ReadAllText(cpuPath)) ?? new();

                    foreach (var cpu in cPUs)
                    {
                        cpu.manufacturer = manufacturers.FirstOrDefault(m => m.Id == cpu.ManufacturerId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load error, creating defaults: " + ex.Message);
                    LoadDefaults();
                }
            }
            else
            {
                LoadDefaults();
            }
        }

        private void LoadDefaults()
        {
            var intel = new ManufacturerDBFile { Id = 1, Name = "Intel", Address = "USA" };
            manufacturers.Add(intel);
            cPUs.Add(new CPUDBFile { Id = 1, Name = "Core i7", manufacturer = intel, ManufacturerId = 1, Cores = 8 });
            Save();
        }

        public ICPU CreateNewCPU(ICPU cpu)
        {
            return InternalCreateCPU(cpu);
        }

        public void CreateCPU(ICPU cpu)
        {
            InternalCreateCPU(cpu);
        }

        private ICPU InternalCreateCPU(ICPU cpu)
        {
            var dbCpu = new CPUDBFile
            {
                Id = cpu.Id,
                Name = cpu.Name,
                Cores = cpu.Cores,
                Threads = cpu.Threads,
                BaseClockGHz = cpu.BaseClockGHz,
                SocketType = cpu.SocketType,
                ManufacturerId = cpu.manufacturer.Id,
                manufacturer = manufacturers.FirstOrDefault(m => m.Id == cpu.manufacturer.Id)
            };
            cPUs.Add(dbCpu);
            Save();
            return dbCpu;
        }


        public IEnumerable<ICPU> GetAllCPUs() => cPUs;
        public IEnumerable<IManufacturer> GetAllManufaturers() => manufacturers;

        public IManufacturer CreateNewManufacturer(IManufacturer m)
        {
            var dbMan = new ManufacturerDBFile { Id = m.Id, Name = m.Name, Address = m.Address };
            manufacturers.Add(dbMan);
            Save();
            return dbMan;
        }

        public void UpdateCPU(ICPU cpu)
        {
            var existing = cPUs.FirstOrDefault(c => c.Id == cpu.Id);
            if (existing != null)
            {
                existing.Name = cpu.Name;
                existing.Cores = cpu.Cores;
                existing.Threads = cpu.Threads;
                existing.BaseClockGHz = cpu.BaseClockGHz;
                existing.SocketType = cpu.SocketType;
                existing.ManufacturerId = cpu.manufacturer.Id;
                existing.manufacturer = manufacturers.FirstOrDefault(m => m.Id == cpu.manufacturer.Id);
                Save();
            }
        }

        public void UpdateManufacturer(IManufacturer m)
        {
            var existing = manufacturers.FirstOrDefault(x => x.Id == m.Id);
            if (existing != null)
            {
                existing.Name = m.Name;
                existing.Address = m.Address;
                Save();
            }
        }

        public void DeleteCPU(int id) { cPUs.RemoveAll(x => x.Id == id); Save(); }
        public void DeleteManufacturer(int id) { manufacturers.RemoveAll(x => x.Id == id); Save(); }

        public ICPU NewCPU() => new CPUDBFile();
    }
}