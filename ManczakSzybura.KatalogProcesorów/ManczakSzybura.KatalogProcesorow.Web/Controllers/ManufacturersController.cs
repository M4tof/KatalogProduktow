using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.Web.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly BL.BL _bl;

        public ManufacturersController()
        {
            string libraryName = System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"]!;
            _bl = BL.BL.GetInstance(libraryName);
        }

        // GET: Manufacturers
        public IActionResult Index(string searchTerm, string filterByAddress)
        {
            // Start with the full list
            IEnumerable<IManufacturer> manufacturers = _bl.GetAllManufacturers();

            // Chain the filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                manufacturers = manufacturers.Where(m => m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filterByAddress))
            {
                manufacturers = manufacturers.Where(m => m.Address == filterByAddress);
            }

            ViewBag.UniqueAddresses = _bl.GetUniqueAddresses();
            return View(manufacturers.ToList());
        }

        // GET: Manufacturers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var manufacturer = _bl.GetManufacturerById(id.Value);
            if (manufacturer == null) return NotFound();
            return View(manufacturer);
        }

        // GET: Manufacturers/Create
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
                int newId = _bl.GetAllManufacturers().Any() ? _bl.GetAllManufacturers().Max(m => m.Id) + 1 : 1;
                var manufacturer = new Models.Manufacturer();
                manufacturer.Id = newId;
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

        // GET: Manufacturers/Edit/5
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
            return View(manufacturer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bl.DeleteManufacturer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}