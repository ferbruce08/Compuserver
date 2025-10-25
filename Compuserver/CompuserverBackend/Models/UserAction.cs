using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompuserverBackend.Models
{
    public class UserAction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ActionType { get; set; } = string.Empty; // Ejemplo: "Creó un pedido", "Pagó un recibo"

        public DateTime ActionDate { get; set; } = DateTime.Now;

        // Relación con el usuario
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public Data.ApplicationUser? User { get; set; }
    }
}
