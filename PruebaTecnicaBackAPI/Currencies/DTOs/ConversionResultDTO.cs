namespace PruebaTecnicaBackAPI.Currencies.DTOs
{
    public class ConversionResultDTO
    {
        public string FromCurrencyCode { get; set; } = string.Empty;
        public string ToCurrencyCode { get; set; } = string.Empty;
        public decimal OriginalAmount { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}
