namespace Asp_Payments_Laba.Models;

public class Buyer
{
    public long NumInvoiceForPayment { get; set; }
    public string PaymentDate {get;set;}
    public string BuyerTypeBusiness {get;set;}
    public long BuyerINN {get;set;}
    public long BuyerKPP {get;set;}
    public string BuyerName {get;set;}
    public string BuyerAddress {get;set;}
    public string ProductName {get;set;}
    public double ProductCost {get;set;}

}