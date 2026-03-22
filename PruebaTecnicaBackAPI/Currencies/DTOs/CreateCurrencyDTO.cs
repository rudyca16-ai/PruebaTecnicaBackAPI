using System.ComponentModel;

namespace PruebaTecnicaBackAPI.Currencies.DTOs
{
    public class CreateCurrencyDTO
    {
        [DefaultValue("USD")]
        public string Code { get; set; } = string.Empty;
        [DefaultValue("Dólar Estadounidense")]
        public string Name { get; set; } = string.Empty;
        [DefaultValue(1.0)]
        public decimal RateToBase { get; set; }
    }
}
