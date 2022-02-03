using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFMicroservice.Data;

namespace PDFMicroservice.Controllers
{
    public class PDFController : Controller
    {

        private ApplicationDbContext _dbContext;

        // GET: PDFController
        public PDFController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: PDFController/Details/
        public ActionResult Details()
        {
            return View();
        }

        // GET: PDFController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PDFController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PDFController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PDFController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PDFController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: PDFController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
