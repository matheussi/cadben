namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class Operadora : EntidadeBase
    {
        public Operadora() { }
        public Operadora(long id) { ID = id; DataCadastro = DateTime.Now; }

        public virtual long ContratanteId { get; set; }

        public virtual string Nome { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string Email { get; set; }

        //public virtual string DDD1 { get; set; }
        //public virtual string Telefone1 { get; set; }
        //public virtual string DDD2 { get; set; }
        //public virtual string Telefone2 { get; set; }

        //public virtual string DDDCel1 { get; set; }
        //public virtual string Celular1 { get; set; }
        //public virtual string OperadoraCel1 { get; set; }

        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataAlteracao { get; set; }
        public virtual bool Inativa { get; set; }
    }
}
