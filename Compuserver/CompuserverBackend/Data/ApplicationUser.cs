using Microsoft.AspNetCore.Identity;

namespace CompuserverBackend.Data
{
    // Clase que representa a los usuarios del sistema (hereda de IdentityUser)
    public class ApplicationUser : IdentityUser
    {
        // Aquí puedes agregar propiedades personalizadas del usuario
        // Ejemplo: Nombre completo, fecha de registro, etc.
        public string? FullName { get; set; }
    }
}
