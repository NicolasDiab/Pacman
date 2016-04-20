using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    class Sommet
    {
        public const int INFINI = 1000000;
        public int Potentiel;
        public bool Marque;
        public Coord Suivant;
        public Coord Pred;

        public Sommet()
        {
            Potentiel = INFINI;
            Marque = false;
            Pred = null;
        }

    }
}
