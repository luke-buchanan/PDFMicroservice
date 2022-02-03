using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFMicroservice.Data;
using PDFMicroservice.Models;
using PDFMicroservice.ViewModels;

namespace PDFMicroservice.Controllers
{
    public class PDFController : Controller
    {

        private ApplicationDbContext _dbContext;

        public PDFController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }
        
        [HttpGet("/api/{id}")]
        public IActionResult Details(int id)
        {
            var pdf = _dbContext.PdfModels.Find(id);
            if(pdf == null)
            {
                return NotFound("No entry found for this id...");
            }
            else
            {
                return Ok(pdf);
            }
        }
        
        [HttpPost]
        public ActionResult Details(DetailsFormViewModel viewModel)
        {
            var pdf = new PdfModel
            {
                TextInput = viewModel.TextInput,
                FilePath = "local-host://8080" 
            };
            
            
            _dbContext.PdfModels.Add(pdf);
            _dbContext.SaveChanges();
            
            return RedirectToAction("Details", "PDF");
        }
    }
}
