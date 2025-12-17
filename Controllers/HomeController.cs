using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Asp_Payments_Laba.Models;
using System.Text;
using Asp_Payments_Laba.Library;
using PuppeteerSharp;
using Asp_Payments_Laba.FolderPdfGenerator;
using Asp_Payments_Laba.Models.ViewModel;

namespace Asp_Payments_Laba.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult DoPayment()
    {
        return View();
    }
    public IActionResult GetPayment()
    {
        return View();
    }
    [HttpPost]
    public IActionResult GetPayment(InvoiceViewModel model)
    {
        if (model == null ||
            string.IsNullOrWhiteSpace(model.PaymentDate) ||
            string.IsNullOrWhiteSpace(model.DateTripFrom) ||
            string.IsNullOrWhiteSpace(model.DateTripTo) ||
            string.IsNullOrWhiteSpace(model.ProductCost))
        {
            ModelState.AddModelError(string.Empty, "Заполните обязательные поля для формирования счёта.");
            return View("DoPayment", model);
        }
        var spl = model.PaymentDate.Split("-");
        model.PaymentDate = $"{spl[2]}.{spl[1]}.{spl[0]}";

        spl = model.DateTripFrom.Split("-");
        model.DateTripFrom = $"{spl[2]}.{spl[1]}.{spl[0]}";

        spl = model.DateTripTo.Split("-");
        model.DateTripTo = $"{spl[2]}.{spl[1]}.{spl[0]}";

        var iRes = model.ProductCost.Split(".").ToList();
        if (iRes.Count == 1)
        {
            iRes.Add("0");
        }
        else if (iRes.Count == 0)
        {
            iRes.Add("0");
            iRes.Add("0");
        }


        var caseOfRub = NeedCaseOfRub(iRes[0]);
        var caseOfKop = NeedCaseOfKop(iRes[1]);
        model.ProductCostRub = $"{iRes[0]} {caseOfRub}";
        model.ProductCostKop = $"{iRes[1]} {caseOfKop}";
        model.ProductCostInWords = $"{RusNumber.Str(int.Parse(iRes[0]))} {caseOfRub} {RusNumber.Str(int.Parse(iRes[1]), false)} {caseOfKop}";

        //string htmlText = "";
        if (model.BuyerTypeBusiness == "ООО")
        {
            model.BuyerTypeBusiness = "ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ";
        }
        else
        {
            model.BuyerTypeBusiness = "ИНДИВИДУАЛЬНЫЙ ПРЕДПРИНИМАТЕЛЬ";
        }
        string nameFile = $"invoiceForPayment{model.NumInvoiceForPayment}.pdf";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "FolderForPdf", nameFile);

        CreatePdf(model, path);


        return View(viewName: null, path);
    }
    static void CreatePdf(InvoiceViewModel model, string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var generator = new PdfGenerator(
            numInvoiceForPayment: model.NumInvoiceForPayment,
            dateTripFrom: model.DateTripFrom,
            dateTripTo: model.DateTripTo,
            paymentDate: model.PaymentDate,
            buyerTypeBusiness: model.BuyerTypeBusiness,
            buyerName: model.BuyerName,
            buyerINN: model.BuyerINN,
            buyerKPP: model.BuyerKPP,
            buyerAddress: model.BuyerAddress,
            productCostRub: model.ProductCostRub,
            productCostKop: model.ProductCostKop,
            productCostInWords: model.ProductCostInWords
        );

        var pdfBytes = generator.GeneratePdf();
        System.IO.File.WriteAllBytes(path, pdfBytes);
    }

    public IActionResult DownloadFile(string path)
    {
        //var path = Path.Combine(Directory.GetCurrentDirectory(), nameFile);
        byte[] bytes = System.IO.File.ReadAllBytes(path);
        return File(bytes, "application/octet-stream", path.Split('/').LastOrDefault());
    }
    private static string NeedCaseOfKop(string num)
    {
        if (num == null)
        {
            return "копеек";
        }
        var iRes = num[num.Length - 1].ToString();
        if (iRes == "1")
        {
            num = "копейка";
        }
        else if (iRes == "2" || iRes == "3" || iRes == "4")
        {
            num = "копейки";
        }
        else
        {
            num = "копеек";
        }
        return num;
    }
    private string NeedCaseOfRub(string num)
    {
        var iRes = num[num.Length - 1].ToString();
        if (iRes == "1")
        {
            num = "рубль";

        }
        else if (iRes == "2" || iRes == "3" || iRes == "4")
        {
            num = "рубля";
        }
        else
        {
            num = "рублей";
        }
        return num;
    }
}
