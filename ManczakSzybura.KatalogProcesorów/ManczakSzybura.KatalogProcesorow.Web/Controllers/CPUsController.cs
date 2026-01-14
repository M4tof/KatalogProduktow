using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; // Wymagane
using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.Web.Controllers
{
    public class CPUsController : Controller
    {
        private readonly BL.BL _bl;

        // ASP.NET Core automatycznie poda tutaj konfigurację z appsettings.json
        public CPUsController(IConfiguration configuration)
        {
            _bl = BL.BL.GetInstance(configuration);
        }

        // GET: CPUs
        public IActionResult Index(string searchTerm, string filterByManufacturer, CPUSocketType? filterBySocket, int? filterByCores)
        {
            IEnumerable<ICPU> cpus = _bl.GetAllCPUs();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                cpus = _bl.SearchCPUByName(searchTerm);
            }

            if (!string.IsNullOrEmpty(filterByManufacturer))
            {
                cpus = cpus.Where(c => c.manufacturer.Name == filterByManufacturer);
            }

            if (filterBySocket.HasValue)
            {
                cpus = cpus.Where(c => c.SocketType == filterBySocket.Value);
            }

            if (filterByCores.HasValue)
            {
                cpus = cpus.Where(c => c.Cores == filterByCores.Value);
            }

            ViewBag.ManufacturerOptions = _bl.GetAllManufacturersNames();
            ViewBag.SocketOptions = Enum.GetValues(typeof(CPUSocketType)).Cast<CPUSocketType>();
            ViewBag.CoreOptions = _bl.GetUniqueCores();

            return View(cpus.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var cpu = _bl.GetCPUById(id.Value);
            if (cpu == null) return NotFound();
            return View(cpu);
        }

        public IActionResult Create()
        {
            ViewBag.Manufacturers = _bl.GetAllManufacturers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                var cpu = new Models.CPU();
                cpu.Name = collection["Name"];
                cpu.Cores = int.Parse(collection["Cores"]);
                cpu.Threads = int.Parse(collection["Threads"]);
                cpu.BaseClockGHz = double.Parse(collection["BaseClockGHz"], CultureInfo.InvariantCulture);
                cpu.SocketType = (CPUSocketType)Enum.Parse(typeof(CPUSocketType), collection["SocketType"]);
                cpu.manufacturer = _bl.GetManufacturerById(int.Parse(collection["ManufacturerId"]));

                _bl.CreateCPU(cpu);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Manufacturers = _bl.GetAllManufacturers();
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var cpu = _bl.GetCPUById(id.Value);
            if (cpu == null) return NotFound();

            ViewBag.Manufacturers = _bl.GetAllManufacturers();
            return View(cpu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var cpu = _bl.GetCPUById(id);
                cpu.Name = collection["Name"];
                cpu.Cores = int.Parse(collection["Cores"]);
                cpu.Threads = int.Parse(collection["Threads"]);
                cpu.BaseClockGHz = double.Parse(collection["BaseClockGHz"], CultureInfo.InvariantCulture);
                cpu.SocketType = (CPUSocketType)Enum.Parse(typeof(CPUSocketType), collection["SocketType"]);
                cpu.manufacturer = _bl.GetManufacturerById(int.Parse(collection["ManufacturerId"]));

                _bl.UpdateCPU(cpu);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Manufacturers = _bl.GetAllManufacturers();
                return View();
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var cpu = _bl.GetCPUById(id.Value);
            if (cpu == null) return NotFound();
            return View(cpu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bl.DeleteCPU(id);
            return RedirectToAction(nameof(Index));
        }
    }
}