using GuardiaoDasMemorias.DTOs.Template;

namespace GuardiaoDasMemorias.Repository.Queries.Template
{
    public interface ITemplateQueries
    {
        Task<IEnumerable<TemplateDto>> GetAllAsync();
        Task<TemplateDto?> GetByIdAsync(int id);
        Task<IEnumerable<TemplateDto>> GetByTemaIdAsync(int temaId);
        Task<IEnumerable<TemplateDto>> GetAtivosAsync();
    }
}
