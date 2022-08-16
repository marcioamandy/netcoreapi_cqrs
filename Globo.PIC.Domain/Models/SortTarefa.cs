using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{
    public class SortTarefa
    {
        //public Nivel Nivel { get; set; }
        List<Tarefa> _tarefas = new List<Tarefa>();
        public List<Tarefa> Tarefas
        {
            get
            {
                return _tarefas;
            }
            set
            {
                _tarefas = value;
            }
        }

    }
}
