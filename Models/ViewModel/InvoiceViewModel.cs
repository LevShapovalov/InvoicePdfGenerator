using System;

namespace Asp_Payments_Laba.Models.ViewModel;

public class InvoiceViewModel
{
    public string NumInvoiceForPayment{ get; set; }
    public string PaymentDate{ get; set; }
    public string BuyerTypeBusiness{ get; set; }
    public string BuyerName{ get; set; }
    public string BuyerINN{ get; set; }
    public string BuyerKPP{ get; set; }
    public string BuyerAddress{ get; set; }
    public string ProductCostRub{ get; set; }
    public string ProductCostKop{ get; set; }
    public string ProductCostInWords{ get; set; }
    public string DateTripFrom{ get; set; }
    public string DateTripTo{ get; set; }
    public string ProductCost { get; set; }
    //public string ProductName { get; set; }
}
