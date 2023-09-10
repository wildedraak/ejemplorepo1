namespace MiParcialito.Models
{
    public class UsuarioTemp
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int? Edad { get; set; }

        public int? RolId { get; set; }
    }
}
