using Microsoft.AspNetCore.Identity;
using GuardiaoDasMemorias.Entities.Cliente;

namespace GuardiaoDasMemorias.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Clientes? Cliente { get; set; }
}
