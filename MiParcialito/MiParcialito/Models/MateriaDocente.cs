using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class MateriaDocente
    {
        public int Id { get; set; }
        public int MateriaId { get; set; }
        public int DocenteId { get; set; }

        public virtual Docente Docente { get; set; }
        public virtual Materium Materia { get; set; }
    }
}
