namespace CarritoAPI.DTOs
{
    public class ResumenCompraDTO
    {
        public decimal Total { get; set; }
        public int CantidadProductosDistintos { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
