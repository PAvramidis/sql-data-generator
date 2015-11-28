using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    abstract class Encja
    {
        public abstract void Create(System.IO.StreamWriter file);
        public abstract void Randomize();
        public abstract void Insert(System.IO.StreamWriter file);

        public virtual void Update(System.IO.StreamWriter file)
        {
            return;
        }
    }
}
