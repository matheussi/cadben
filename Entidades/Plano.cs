namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class Plano : EntidadeBase
    {
        public Plano(long id) : this() { ID = id; }
        public Plano() { Data = DateTime.Now; QuartoComum = true; }

        public virtual ContratoADM ContratoAdm { get; set; }

        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual DateTime Data { get; set; }

        public virtual bool QuartoComum { get; set; }
        public virtual string QuartoComumCodigo { get; set; }
        public virtual string QuartoComumCodigoANS { get; set; }
        public virtual string QuartoComumSubplano { get; set; }
        public virtual DateTime? QuartoComumInicio { get; set; }

        public virtual bool QuartoParticular { get; set; }
        public virtual string QuartoParticularCodigo { get; set; }
        public virtual string QuartoParticularCodigoANS { get; set; }
        public virtual string QuartoParticularSubplano { get; set; }
        public virtual DateTime? QuartoParticularInicio { get; set; }
    }
}
