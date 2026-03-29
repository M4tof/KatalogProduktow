using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; // Wymagane
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.Web.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly BL.BL _bl;

        public ManufacturersController(IConfiguration configuration)
        {
            _bl = BL.BL.GetInstance(configuration);
        }

        // GET: Manufacturers
        public IActionResult Index(string searchTerm, string filterByAddress)
        {
            IEnumerable<IManufacturer> manufacturers = _bl.GetAllManufacturers();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                manufacturers = _bl.SearchProducerByName(searchTerm);
            }

            if (!string.IsNullOrEmpty(filterByAddress))
            {
                manufacturers = _bl.FilterProducerByAddress(filterByAddress);
            }

            ViewBag.UniqueAddresses = _bl.GetUniqueAddresses();
            return View(manufacturers.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var manufacturer = _bl.GetManufacturerById(id.Value);
            if (manufacturer == null) return NotFound();
            return View(manufacturer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                var manufacturer = new Models.Manufacturer();
                manufacturer.Name = collection["Name"];
                manufacturer.Address = collection["Address"];

                _bl.CreateManufacturer(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var manufacturer = _bl.GetManufacturerById(id.Value);
            if (manufacturer == null) return NotFound();
            return View(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var manufacturer = _bl.GetManufacturerById(id);
                manufacturer.Name = collection["Name"];
                manufacturer.Address = collection["Address"];

                _bl.UpdateManufacturer(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var manufacturer = _bl.GetManufacturerById(id.Value);
            if (manufacturer == null) return NotFound();

            // Check if any CPU is linked to this manufacturer
            bool hasLinkedCPUs = _bl.GetAllCPUs()
                .Any(cpu => cpu.manufacturer != null && cpu.manufacturer.Id == id.Value);

            if (hasLinkedCPUs)
            {
                // Store error message in TempData to show it on the Index page
                TempData["Error"] = $"Cannot delete '{manufacturer.Name}' because it is assigned to one or more CPUs.";
                return RedirectToAction(nameof(Index));
            }

            return View(manufacturer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Safety check again in the POST method
            bool hasLinkedCPUs = _bl.GetAllCPUs()
                .Any(cpu => cpu.manufacturer != null && cpu.manufacturer.Id == id);

            if (hasLinkedCPUs)
            {
                TempData["Error"] = "Deletion failed. CPUs are still assigned to this manufacturer.";
                return RedirectToAction(nameof(Index));
            }

            _bl.DeleteManufacturer(id);
            TempData["Success"] = "Manufacturer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}