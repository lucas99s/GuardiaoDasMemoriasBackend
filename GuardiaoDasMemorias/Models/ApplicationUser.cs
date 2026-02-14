using Microsoft.AspNetCore.Identity;
using GuardiaoDasMemorias.Entities;

namespace GuardiaoDasMemorias.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Cliente? Cliente { get; set; }
}
