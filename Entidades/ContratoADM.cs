namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class ContratoADM : EntidadeBase
    {
        public ContratoADM() { Ativo = true; }
        public ContratoADM(long id) : this() { ID = id; }

        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual Operadora Operadora { get; set; }
        public virtual AssociadoPJ AssociadoPJ { get; set; }

        public virtual string Numero { get; set; }
        public virtual string ContratoSaude { get; set; }
        public virtual string ContratoDental { get; set; }

        public virtual string CodigoFilial { get; set; }
        public virtual string CodigoUnidade { get; set; }
        public virtual string CodigoAdministradora { get; set; }

        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataAlteracao { get; set; }
    }
}
