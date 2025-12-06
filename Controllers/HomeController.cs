using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Asp_Payments_Laba.Models;
//using IronPdf;
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
    // public IActionResult GetPayment(long numInvoiceForPayment, long dateTripFrom, long dateTripTo, string paymentDate,
    // string buyerTypeBusiness, long buyerINN, long buyerKPP, string buyerName,
    //  string buyerAddress, string productCost, string lastPath)
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
        // if (lastPath != null)
        // {
        //     if (System.IO.File.Exists(lastPath))
        //     {
        //         System.IO.File.Delete(lastPath);
        //     }
        // }
        // if (numInvoiceForPayment < 0)
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyNumInvoiceForPayment);
        // }
        // if (paymentDate == null || paymentDate == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyPaymentDate);
        // }
        // if (productCost == null || productCost == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyProductCost);
        // }
        // if (buyerName == null || buyerName == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyBuyerName);
        // }
        // if (buyerAddress == null || buyerAddress == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyBuyerAddress);
        // }
        // if (buyerINN <= 0)
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyINN);
        // }
        // if (productName == null || productName == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyProductName);
        // }
        // if (productCost == null || productCost == "")
        // {
        //     return RedirectToAction("DoPayment", Errors.ErrEmptyProductCost);
        // }
        var spl = model.PaymentDate.Split("-");
        model.PaymentDate = $"{spl[2]}.{spl[1]}.{spl[0]}";

        spl = model.DateTripFrom.Split("-");
        model.DateTripFrom = $"{spl[2]}.{spl[1]}.{spl[0]}";

        spl = model.DateTripTo.Split("-");
        model.DateTripTo = $"{spl[2]}.{spl[1]}.{spl[0]}";

        var iRes = model.ProductCost.Split(".").ToList();
        if(iRes.Count == 1)
        {
            iRes.Add("0");
        }
        else if(iRes.Count == 0)
        {
            iRes.Add("0");
            iRes.Add("0");
        }
        // var splitted = productCost.Split(".");
        // string[] iRes = new string[2]{"", ""};
        // if(splitted.Length == 1)
        // {
        //     iRes[0] = splitted[0];
        //     iRes[1] = "0";
        // }
        // else if(splitted.Length == 0)
        // {
        //     iRes[0] = "0";
        //     iRes[1] = "0";
        // }
        // else
        // {
        //     iRes[0] = splitted[0];
        //     iRes[1] = splitted[1];

        // }
        
        var caseOfRub = NeedCaseOfRub(iRes[0]);
        var caseOfKop = NeedCaseOfKop(iRes[1]);
        model.ProductCostRub = $"{iRes[0]} {caseOfRub}";
        model.ProductCostKop = $"{iRes[1]} {caseOfKop}";
        model.ProductCostInWords = $"{RusNumber.Str(int.Parse(iRes[0]))} {caseOfRub} {RusNumber.Str(int.Parse(iRes[1]), false)} {caseOfKop}";

        //string htmlText = "";
        if (model.BuyerTypeBusiness == "ООО")
        {
            model.BuyerTypeBusiness = "ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ";
            // if (buyerKPP == 0)
            // {
            //     //return RedirectToAction("DoPayment", Errors.ErrEmptyKPP);
            //     return View();
            // }
            //htmlText = $"<!DOCTYPE html>\n<html lang=\"en\">\n  <head>\n    <meta charset=\"UTF-8\" />\n    <link rel=\"stylesheet\" href=\"get_payment_css.css\" />\n  </head>\n\n  <body>\n    <h3>Счет на оплату No {numInvoiceForPayment} от {paymentDate}</h3>\n\n    <div class=\"buyer-or-supplier\">\n      <h4>Поставщик:</h4>\n      <p>\n        ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"СОЗДАТЕЛИ МИРОВ\", 121596, г\n        Москва, ул Говорова, дом 15, кв 77\n      </p>\n    </div>\n    <table class=\"table-global-supplier\">\n      <tr>\n        <td class=\"name-bank\" colspan=\"4\">ПАО СБЕРБАНК</td>\n        <td class=\"label\">БИК</td>\n        <td class=\"num\">044525225</td>\n      </tr>\n      <tr>\n        <td class=\"name-city\" colspan=\"4\">Москва</td>\n        <td class=\"label\" rowspan=\"2\">Сч. №</td>\n        <td class=\"num\" rowspan=\"2\">30101810400000000225</td>\n      </tr>\n      <tr>\n        <td class=\"bank-beneficiary\" colspan=\"4\">\n          <p class=\"table-supplier-mini-text-bank\">Банк получателя</p>\n          <p></p>\n        </td>\n      </tr>\n      <tr>\n        <td class=\"label\">ИНН</td>\n        <td>9731051293</td>\n        <td class=\"label\">КПП</td>\n        <td>773101001</td>\n        <td class=\"label\" rowspan=\"3\">Сч. №</td>\n        <td class=\"label\" rowspan=\"3\">40702810038000199456</td>\n      </tr>\n      <tr>\n        <td class=\"table-name-company\" colspan=\"4\">\n          ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"СОЗДАТЕЛИ МИРОВ\"\n        </td>\n      </tr>\n      <tr>\n        <td class=\"table-recipient\" colspan=\"4\">\n          <p class=\"table-supplier-mini-text-recipient\">Получатель</p>\n        </td>\n      </tr>\n    </table>\n\n    <div class=\"buyer-or-supplier\">\n      <h4>Покупатель:</h4>\n      <p>\n        {buyerTypeBusiness} \"{buyerName}\", ИНН: {buyerINN}, КПП: {buyerKPP},\n        Юридический адрес: {buyerAddress}\n      </p>\n    </div>\n\n    <table class=\"table-buyer\">\n      <tr>\n        <td>№</td>\n        <td>Наименование товаров, работ, услуг</td>\n        <td>Кол-во</td>\n        <td>Ед. изм.</td>\n        <td>Цена</td>\n        <td>Всего</td>\n      </tr>\n      <tr>\n        <td>1</td>\n        <td>\n          Туристическая путевка по договору No М-011124-1857 от 01.11.2024 г.\n          Даты поездки 02.01.2025 - 08.01.2025\n        </td>\n        <td>1</td>\n        <td>шт.</td>\n        <td>39 500,00</td>\n        <td>39 500,00</td>\n      </tr>\n    </table>\n    <div>\n      <p>\n        Всего наименований 1 на сумму {productCostRub} {productCostKop}\n      </p>\n      <p>{productCostInWords}</p>\n    </div>\n    <div class=\"row-text\">\n      <p class=\"more-margin-right\">Директор</p>\n      <p>_____________________</p>\n    </div>\n    <div class=\"row-text\">\n      <p class=\"more-margin-right\">Бухгалтер</p>\n      <p>_____________________</p>\n    </div>\n  </body>\n</html>\n";
        }
        else
        {
            // if (buyerKPP != 0)
            // {
            //     // return RedirectToAction("DoPayment", Errors.ErrNotNeedKPP);
            //     return View();
            // }
            model.BuyerTypeBusiness = "ИНДИВИДУАЛЬНЫЙ ПРЕДПРИНИМАТЕЛЬ";
            //htmlText = $"<!DOCTYPE html>\n<html lang=\"en\">\n  <head>\n    <meta charset=\"UTF-8\" />\n    <link rel=\"stylesheet\" href=\"get_payment_css.css\" />\n  </head>\n\n  <body>\n    <h3>Счет на оплату No {numInvoiceForPayment} от {paymentDate}</h3>\n\n    <div class=\"buyer-or-supplier\">\n      <h4>Поставщик:</h4>\n      <p>\n        ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"СОЗДАТЕЛИ МИРОВ\", 121596, г\n        Москва, ул Говорова, дом 15, кв 77\n      </p>\n    </div>\n    <table class=\"table-global-supplier\">\n      <tr>\n        <td class=\"name-bank\" colspan=\"4\">ПАО СБЕРБАНК</td>\n        <td class=\"label\">БИК</td>\n        <td class=\"num\">044525225</td>\n      </tr>\n      <tr>\n        <td class=\"name-city\" colspan=\"4\">Москва</td>\n        <td class=\"label\" rowspan=\"2\">Сч. №</td>\n        <td class=\"num\" rowspan=\"2\">30101810400000000225</td>\n      </tr>\n      <tr>\n        <td class=\"bank-beneficiary\" colspan=\"4\">\n          <p class=\"table-supplier-mini-text-bank\">Банк получателя</p>\n          <p></p>\n        </td>\n      </tr>\n      <tr>\n        <td class=\"label\">ИНН</td>\n        <td>9731051293</td>\n        <td class=\"label\">КПП</td>\n        <td>773101001</td>\n        <td class=\"label\" rowspan=\"3\">Сч. №</td>\n        <td class=\"label\" rowspan=\"3\">40702810038000199456</td>\n      </tr>\n      <tr>\n        <td class=\"table-name-company\" colspan=\"4\">\n          ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"СОЗДАТЕЛИ МИРОВ\"\n        </td>\n      </tr>\n      <tr>\n        <td class=\"table-recipient\" colspan=\"4\">\n          <p class=\"table-supplier-mini-text-recipient\">Получатель</p>\n        </td>\n      </tr>\n    </table>\n\n    <div class=\"buyer-or-supplier\">\n      <h4>Покупатель:</h4>\n      <p>\n        {buyerTypeBusiness} \"{buyerName}\", ИНН: {buyerINN}\n        Юридический адрес: {buyerAddress}\n      </p>\n    </div>\n\n    <table class=\"table-buyer\">\n      <tr>\n        <td>№</td>\n        <td>Наименование товаров, работ, услуг</td>\n        <td>Кол-во</td>\n        <td>Ед. изм.</td>\n        <td>Цена</td>\n        \n<td>Всего</td>\n      </tr>\n      <tr>\n        <td>1</td>\n        <td>\n          Туристическая путевка по договору No М-011124-1857 от 01.11.2024 г.\n          Даты поездки 02.01.2025 - 08.01.2025\n        </td>\n        <td>1</td>\n        <td>шт.</td>\n        <td>39 500,00</td>\n        <td>39 500,00</td>\n      </tr>\n    </table>\n    <div>\n      <p>\n        Всего наименований 1 на сумму {productCostRub} {productCostKop}\n      </p>\n      <p>{productCostInWords}</p>\n    </div>\n    <div class=\"row-text\">\n      <p class=\"more-margin-right\">Директор</p>\n      <p>_____________________</p>\n    </div>\n    <div class=\"row-text\">\n      <p class=\"more-margin-right\">Бухгалтер</p>\n      <p>_____________________</p>\n    </div>\n  </body>\n</html>\n";
        }
        string nameFile = $"invoiceForPayment{model.NumInvoiceForPayment}.pdf";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "FolderForPdf", nameFile);

        CreatePdf(model, path);
        // Task.Run(async () =>
        // {
        //     await CreatePdf(htmlText, path);
        // });
        /*if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Console.WriteLine("Файл успешно удален.");
        }*/
        // Disable local disk access or cross-origin requests
        //Installation.EnableWebSecurity = true;
        // Instantiate Renderer
        //var renderer = new ChromePdfRenderer();
        // Create a PDF from a HTML string using C#
        //var pdf = renderer.RenderHtmlAsPdf(htmlText);
        // Export to a file or Stream
        //pdf.SaveAs(nameFile);

        return View(viewName: null, path);
    }

    // static void CreatePdf(string numInvoiceForPayment, string paymentDate,
    //     string buyerTypeBusiness, string buyerINN, string buyerKPP, string buyerName,
    //     string buyerAddress, string productName, string productCostRub,
    //     string productCostKop, string productCostInWords, string path)
    static void CreatePdf(InvoiceViewModel model, string path)
    {
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

    // static async Task CreatePdf(string htmlText, string path)
    // {
    //     try
    //     {
    //         /*var browser = await Puppeteer.LaunchAsync(new LaunchOptions
    //         {
    //             ExecutablePath = "/usr/bin/chromium-browser",  // для linux
    //             Headless = true,
    //             //ExecutablePath = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome" // Укажите путь к своему браузеру(для mac os)
    //         });*/
    //         var options = new ConnectOptions
    //         {
    //             BrowserURL = "ws://localhost:9222"
    //         };
    //         var browser = await Puppeteer.ConnectAsync(options);
            
    //         var page = await browser.NewPageAsync();

    //         // Устанавливаем HTML контент в страницу
    //         await page.SetContentAsync(htmlText);

    //         // Генерация PDF
    //         await page.PdfAsync(path);

    //         // Закрытие браузера
    //         await browser.CloseAsync();

    //         Console.WriteLine("PDF успешно создан!");
    //     }
    //     catch (Exception ex)
    //     {
    //         System.Console.WriteLine("puppeteerSharp has exception: " + ex.Message);
    //     }
    //     // Загружаем Chromium (если не загружен)
    //     //await new BrowserFetcher().DownloadAsync();

    //     // Запуск Chromium
    //     ///Applications/Google Chrome.app/Contents/MacOS

    // }
    public IActionResult DownloadFile(string path)
    {
        //var path = Path.Combine(Directory.GetCurrentDirectory(), nameFile);
        byte[] bytes = System.IO.File.ReadAllBytes(path);
        return File(bytes, "application/octet-stream", path.Split('/').LastOrDefault());
    }
    private static string NeedCaseOfKop(string num)
    {
        if(num == null)
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
