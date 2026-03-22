namespace PruebaTecnicaBackAPI.Currencies.DTOs
{
    public class CurrencyResponseDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal RateToBase { get; set; }
    }
}
