using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Asp_Payments_Laba.FolderPdfGenerator;

public class PdfGenerator
{
    private readonly string _numInvoiceForPayment;
    private readonly string _paymentDate;
    private readonly string _buyerTypeBusiness;
    private readonly string _buyerName;
    private readonly string _buyerINN;
    private readonly string _buyerKPP;
    private readonly string _buyerAddress;
    private readonly string _productCostRub;
    private readonly string _productCostKop;
    private readonly string _productCostInWords;
    private readonly string _dateTripFrom;
    private readonly string _dateTripTo;
    //private readonly string currentDate;

    public PdfGenerator(
        string numInvoiceForPayment,
        string paymentDate,
        string buyerTypeBusiness,
        string buyerName,
        string buyerINN,
        string buyerKPP,
        string buyerAddress,
        string productCostRub,
        string productCostKop,
        string productCostInWords,
        string dateTripFrom,
        string dateTripTo)
        //string _currentDate)
    {
        _numInvoiceForPayment = numInvoiceForPayment;
        _paymentDate = paymentDate;
        _buyerTypeBusiness = buyerTypeBusiness;
        _buyerName = buyerName;
        _buyerINN = buyerINN;
        _buyerKPP = buyerKPP;
        _buyerAddress = buyerAddress;
        _productCostRub = productCostRub;
        _productCostKop = productCostKop;
        _productCostInWords = productCostInWords;
        _dateTripFrom = dateTripFrom;
        _dateTripTo = dateTripTo;
        //currentDate = _currentDate;
    }

    public byte[] GeneratePdf()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                // Заголовок
                page.Header()
                    .Text($"Счет на оплату № {_numInvoiceForPayment} от {_paymentDate}")
                    .SemiBold().FontSize(16).AlignCenter();

                // Поставщик
                page.Content()
                    .PaddingVertical(10)
                    .Column(column =>
                    {
                        column.Item()
                            .Text("Поставщик:")
                            .SemiBold().FontSize(14);

                        column.Item()
                            .PaddingBottom(10)
                            .Text("ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"<<НАЗВАНИЕ_КОМПАНИИ_ПОСТАВЩИКА>>\", <<ИНДЕКС_ПОСТАВЩИКА>>, <<АДРЕС_ПОСТАВЩИКА>>");

                        // Таблица с реквизитами поставщика
                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); // Первая колонка (ширина 1)
                                    columns.RelativeColumn(2);  // Вторая колонка
                                    columns.RelativeColumn(1); // 
                                    columns.RelativeColumn(2); // 
                                    columns.RelativeColumn(1); // 
                                    columns.RelativeColumn(4);  // пятая колонка
                                });

                                // Первая строка
                                table.Cell().ColumnSpan(4).Container().BorderTop(1)
                                    .BorderLeft(1).BorderRight(1).PaddingLeft(2).Text("ПАО СБЕРБАНК").SemiBold();
                                table.Cell().ColumnSpan(1).Container().Border(1)
                                    .Padding(8).PaddingTop(4).PaddingBottom(4).Text("БИК").SemiBold();
                                table.Cell().ColumnSpan(1).Container().Border(1)
                                    .AlignMiddle().PaddingLeft(20).Text("<<БИК_ПОСТАВЩИКА>>");

                                // Вторая строка
                                table.Cell().ColumnSpan(4).Container()
                                    .BorderLeft(1).BorderRight(1)
                                    .PaddingLeft(2).PaddingTop(10).PaddingBottom(10).Text("Москва");
                                table.Cell().ColumnSpan(1).RowSpan(2).Container().Border(1)
                                    .Padding(10).Text("Сч. №").SemiBold();
                                table.Cell().ColumnSpan(1).RowSpan(2).Container().Border(1)
                                    .AlignMiddle().AlignCenter().Text("<<НОМЕР_СЧЕТА_ПОСТАВЩИКА>>");

                                // Третья строка
                                table.Cell().ColumnSpan(4).Container().
                                BorderLeft(1).BorderRight(1).PaddingLeft(2).Text("Банк получателя").FontSize(10); // //FIX

                                // Четвертая строка
                                table.Cell().Container().Border(1)
                                    .Padding(4).AlignCenter().Text("ИНН").SemiBold();
                                table.Cell().Container().Border(1)
                                    .AlignMiddle().PaddingLeft(2).Text("9731051293");
                                table.Cell().Container().Border(1)
                                    .Padding(4).AlignCenter().Text("КПП").SemiBold();
                                table.Cell().Container().Border(1)
                                    .AlignMiddle().PaddingLeft(2).Text("773101001");
                                table.Cell().RowSpan(3).Container().Border(1)
                                    .AlignMiddle().AlignCenter().Text("Сч. №").SemiBold();
                                table.Cell().RowSpan(3).Container().Border(1)
                                    .AlignMiddle().AlignCenter().Text("40702810038000199456");

                                // Пятая строка
                                table.Cell().ColumnSpan(4).Container().BorderTop(1)
                                    .BorderLeft(1).BorderRight(1).PaddingLeft(5).PaddingTop(5)
                                    .PaddingBottom(10)
                                    .Text("ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \"<<НАЗВАНИЕ_КОМПАНИИ_ПОСТАВЩИКА>>\"").SemiBold();

                                // Шестая строка
                                table.Cell().ColumnSpan(4).Container().BorderBottom(1)
                                    .BorderLeft(1).BorderRight(1)
                                    .PaddingLeft(2).PaddingTop(3).Text("Получатель").FontSize(10);
                            });

                        // Покупатель
                        column.Item()
                            .PaddingTop(10)
                            .Text("Покупатель:")
                            .SemiBold().FontSize(14);

                        column.Item()
                            .PaddingBottom(10)
                            .Text($"{_buyerTypeBusiness} \"{_buyerName}\", ИНН: {_buyerINN}, КПП: {_buyerKPP}, Юридический адрес: {_buyerAddress}");

                        // Таблица с товарами
                        column.Item()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(8);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("№").SemiBold();
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("Наименование товаров, работ, услуг").SemiBold();
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("Кол-во").SemiBold();
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("Ед. изм.").SemiBold();
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("Цена").SemiBold();
                            header.Cell().Container().Border(1)
                                .PaddingLeft(2).AlignMiddle().AlignLeft().Text("Всего").SemiBold();
                        });

                        table.Cell().Container().Border(1).AlignMiddle().AlignLeft().Text("1");
                        table.Cell().Container().Border(1).AlignMiddle().AlignLeft().Text($"Туристическая путевка по договору № М-011124-1857 от {_paymentDate} г. Даты поездки {_dateTripFrom} - {_dateTripFrom}");
                        table.Cell().Container().Border(1).AlignMiddle().AlignLeft().Text("1");
                        table.Cell().Container().Border(1).AlignMiddle().AlignLeft().Text("шт.");
                        table.Cell().Container().Border(1).PaddingRight(1).AlignMiddle().AlignRight().Text($"{_productCostRub},\n{_productCostKop}");
                        table.Cell().Container().Border(1).PaddingRight(1).AlignMiddle().AlignRight().Text($"{_productCostRub},\n{_productCostKop}");

                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container().Text("Итого к оплате:");
                        table.Cell().Container().Border(1).AlignMiddle().AlignRight().Text($"{_productCostRub},\n{_productCostKop}");

                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container();
                        table.Cell().Container().Text("Без налога (НДС)");
                        table.Cell().Container().Border(1).AlignRight().Text("___");
                    });

                        // Итоговая сумма
                        column.Item()
                            .PaddingTop(10) // отступ сверху
                            .Text($"Всего наименований 1 на сумму {_productCostRub} {_productCostKop}");

                        column.Item()
                            .PaddingBottom(10) // отступ снизу
                            .Text(_productCostInWords);

                        // Подписи
                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem().Text("Директор").SemiBold();
                                row.RelativeItem().Text("_____________________").AlignRight();
                            });

                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem().Text("Бухгалтер").SemiBold();
                                row.RelativeItem().Text("_____________________").AlignRight();
                            });
                    });
            });
        });

        return document.GeneratePdf();
    }
}
