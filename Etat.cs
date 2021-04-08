using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    class Etat : IComparable
    {
        public int borneInf;
        public List<int> alreadyVisited;
        public List<int> notVisited;

        public Etat(int borneInf, List<int> alreadyVisited, List<int> notVisited)
        {
            this.borneInf = borneInf;
            this.alreadyVisited = alreadyVisited;
            this.notVisited = notVisited;
        }

        public int CompareTo(object obj)
        {
            Etat other = obj as Etat;
            if (other == null)
                return -1;

            return borneInf.CompareTo(other.borneInf);
        }
    }
}
