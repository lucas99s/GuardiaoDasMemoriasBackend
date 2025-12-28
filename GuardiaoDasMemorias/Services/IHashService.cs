namespace GuardiaoDasMemorias.Services
{
    public interface IHashService
    {
        Task<string> GenerateUniqueHashAsync();
    }
}
