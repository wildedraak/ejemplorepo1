using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            Inscripciones = new HashSet<Inscripcione>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int? Edad { get; set; }

        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<Inscripcione> Inscripciones { get; set; }
    }
}
