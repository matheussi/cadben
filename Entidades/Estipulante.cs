namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using cadben.Entidades.Enuns;

    public class Estipulante : EntidadeBaseData
    {
        public virtual string Nome { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual long ContratanteId { get; set; }
    }

    public class EstipulanteTaxa : EntidadeBaseData
    {
        public virtual long EstipulanteId { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual DateTime Vigencia { get; set; }
        public virtual TipoTaxa Tipo { get; set; }
    }
}
