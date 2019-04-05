namespace UGRS.Core.SDK.DI.Transports.DTO
{
    public  class CommissionDebtDTO
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public decimal Importe { get; set; }
        public string Auxiliar { get; set; }

    }
}
