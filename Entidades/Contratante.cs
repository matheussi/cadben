namespace cadben.Entidades
{
    using System;
    using System.Text;
    using System.Data;
    using System.Configuration;
    using System.Collections.Generic;

    public class Contratante
    {
        public Contratante() { DataCadastro = DateTime.Now; Ativo = true; }

        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Alias { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataAlteracao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
