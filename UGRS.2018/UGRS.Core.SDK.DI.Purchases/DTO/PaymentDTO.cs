
namespace UGRS.Core.SDK.DI.Purchases.DTO
{
    public class PaymentDTO
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Folio { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string EmpId { get; set; }
        public string Employee { get; set; }
        public string Area { get; set; }
        public string Date { get; set; }
        public double ImpSol { get; set; }
        public double ImpComp { get; set; }
        public double ImpSob { get; set; }
        public double ImpFalt { get; set; }
        public double SaldoPen { get; set; }
        public double MQ_Credit { get; set; }
        public double MQ_Debit { get; set; }
    }
}
