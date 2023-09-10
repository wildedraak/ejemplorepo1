using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
