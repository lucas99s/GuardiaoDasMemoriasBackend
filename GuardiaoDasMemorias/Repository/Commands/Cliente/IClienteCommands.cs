namespace GuardiaoDasMemorias.Repository.Commands.Cliente
{
    public interface IClienteCommands
    {
        Task<int> CreateAsync(Entities.Cliente.Clientes cliente);
        Task<bool> UpdateAsync(Entities.Cliente.Clientes cliente);
        Task<bool> DeleteAsync(int id);
    }
}
