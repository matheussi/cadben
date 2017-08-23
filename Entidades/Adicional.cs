namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    [Serializable]
    public class Adicional : EntidadeBaseData
    {
        public virtual Operadora Operadora { get; set; }

        public virtual string Descricao { get; set; }
        public virtual string Codigo { get; set; }
        public virtual bool ParaTodaProposta { get; set; }
        public virtual bool Ativo { get; set; }
    }

    [Serializable]
    public class AdicionalFaixa : EntidadeBase
    {
        public virtual Adicional Adicional { get; set; }

        public virtual DateTime Vigencia { get; set; }
        public virtual int IdadeInicio { get; set; }
        public virtual int IdadeFim { get; set; }
        public virtual decimal Valor { get; set; }
    }
}