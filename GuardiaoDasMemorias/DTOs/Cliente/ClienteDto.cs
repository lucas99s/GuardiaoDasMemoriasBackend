namespace GuardiaoDasMemorias.DTOs.Cliente
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
    }
}
