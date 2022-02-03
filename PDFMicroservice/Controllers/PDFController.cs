using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDFMicroservice.Data;
using PDFMicroservice.Models;
using PDFMicroservice.ViewModels;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PDFMicroservice.Controllers
{
    public class PDFController : Controller
    {

        private ApplicationDbContext _dbContext;
        private readonly string FilePath = "/Users/gtaylor038/Downloads/";
        
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
        public async Task<IActionResult> Details(int id)
        {
            var pdf = _dbContext.PdfModels.Find(id);
            if(pdf == null)
            {
                return NotFound("No entry found for this id...");
            }
            else
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream($"{FilePath}{id}.pdf", FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, "application/pdf");
            }
        }
        
        [HttpPost]
        public ActionResult Details(DetailsFormViewModel viewModel)
        {
            var pdfs = _dbContext.PdfModels.Any();

            if (pdfs == false)
            {
                var nextPdf = 1;
                var pdf = new PdfModel
                {
                    TextInput = viewModel.TextInput,
                    FilePath = $"{FilePath}{nextPdf}.pdf",
                };
            
                _dbContext.PdfModels.Add(pdf);
                _dbContext.SaveChanges();
                GeneratePdf(viewModel.TextInput, nextPdf);
                return RedirectToAction("Details", "PDF");
            }
            else
            {
                var lastPdf = _dbContext.PdfModels.Max(p => p.Id);;
                var nextPdf = lastPdf++;
                var pdf = new PdfModel
                {
                    TextInput = viewModel.TextInput,
                    FilePath = $"{FilePath}{nextPdf}.pdf",
                };
            
                _dbContext.PdfModels.Add(pdf);
                _dbContext.SaveChanges();
            
                GeneratePdf(viewModel.TextInput, nextPdf);
                return RedirectToAction("Details", "PDF");
            }
        }

        public void GeneratePdf(string text, int nextPdf)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Calibri", 20, XFontStyle.Bold);
            
            gfx.DrawString(text, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            // string filename = "pdfGenerator.pdf";
            document.Save($"{FilePath}{nextPdf}.pdf");
        }
        
    }
}
