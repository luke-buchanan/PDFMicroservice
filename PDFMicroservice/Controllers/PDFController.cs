using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PDFMicroservice.Data;
using PDFMicroservice.Models;
using PDFMicroservice.ViewModels;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;


namespace PDFMicroservice.Controllers
{
    public class PDFController : Controller
    {

        private ApplicationDbContext _dbContext;
        private static readonly string FilePath = "/Users/gtaylor038/Downloads/";
        
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
                TempData["Message"] = "PDF Saved";
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

        static void GeneratePdf(string text, int nextPdf)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
            
            gfx.DrawString(text, font, XBrushes.Black, new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);
            // CreatePage(document, text);
            document.Save($"{FilePath}{nextPdf}.pdf");
        }

        // Uses migraDoc which doesnt currently work on Mac
        // static void CreatePage(PdfDocument document, String text)
        // {
        //     PdfPage page = document.AddPage();
        //     XGraphics gfx = XGraphics.FromPdfPage(page);
        //     gfx.MUH = PdfFontEncoding.Unicode;
        //     
        //     // XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
        //     //
        //     // gfx.DrawString(
        //     //     text, 
        //     //     font, 
        //     //     XBrushes.Black, 
        //     //     new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center
        //     //     );
        //
        //     Document doc = new Document();
        //     BitVector32.Section sec = doc.AddSection();
        //
        //     Paragraph para = sec.AddParagraph();
        //     para.Format.Alignment = ParagraphAlignment.Justify;
        //     para.Format.Font.Size = 12;
        //     para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
        //     para.AddText(text);
        //     para.Format.Borders.Distance = "5pt";
        //     para.Format.Borders.Color = Colors.Gold;
        //
        //     MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
        //     docRenderer.PrepareDocument();
        //     docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", para);
        // }
        
    }
}
