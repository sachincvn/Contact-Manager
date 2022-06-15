using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_Manager.Models
{
    public class WrapperModel<T> where T : class
    {
        public T Item { get; set; }
        public Operation Operation { get; set; }

        public WrapperModel(T item, Operation operation)
        {
            Operation = operation;
            Item = item;
        }
    }

    public enum Operation
    {
        Add,
        Update,
        Delete,
        Display,
    }
}
