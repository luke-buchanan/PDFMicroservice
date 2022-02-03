using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFMicroservice.Data;

namespace PDFMicroservice.Controllers
{
    public class PDFController : Controller
    {

        private ApplicationDbContext _dbContext;

        public PDFController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: PDFController/Details/
        public ActionResult Details()
        {
            return View();
        }

        // POST: PDFController/Create
        [HttpPost]
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
