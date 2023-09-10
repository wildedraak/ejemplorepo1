using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class Materium
    {
        public Materium()
        {
            Inscripciones = new HashSet<Inscripcione>();
            MateriaDocentes = new HashSet<MateriaDocente>();
        }

        public int Id { get; set; }
        public string MateriaNombre { get; set; }

        public virtual ICollection<Inscripcione> Inscripciones { get; set; }
        public virtual ICollection<MateriaDocente> MateriaDocentes { get; set; }
    }
}
