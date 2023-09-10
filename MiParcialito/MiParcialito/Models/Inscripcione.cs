using System;
using System.Collections.Generic;

namespace MiParcialito.Models
{
    public partial class Inscripcione
    {
        public int InscripcionesId { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }

        public virtual AspNetUser Estudiante { get; set; }
        public virtual Materium Materia { get; set; }
    }
}
