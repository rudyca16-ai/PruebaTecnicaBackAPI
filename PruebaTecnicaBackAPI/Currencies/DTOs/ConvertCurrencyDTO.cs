using System.ComponentModel;

namespace PruebaTecnicaBackAPI.Currencies.DTOs
{
    public class ConvertCurrencyDTO
    {
        [DefaultValue("USD")]
        public string FromCurrencyCode { get; set; } = string.Empty;
        [DefaultValue("PYG")]
        public string ToCurrencyCode { get; set; } = string.Empty;
        [DefaultValue(100)]
        public decimal Amount { get; set; }
    }
}
