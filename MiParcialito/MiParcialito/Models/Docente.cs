using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class Docente
    {
        public Docente()
        {
            MateriaDocentes = new HashSet<MateriaDocente>();
        }

        public int Id { get; set; }
        public string DocenteNombre { get; set; }

        public virtual ICollection<MateriaDocente> MateriaDocentes { get; set; }
    }
}
