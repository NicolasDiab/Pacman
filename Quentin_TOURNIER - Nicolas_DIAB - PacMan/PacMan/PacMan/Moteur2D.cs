using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacMan
{
    class Moteur2D
    {
        // Return true si un déplacement ferait déplacer un pacman ou un fantome
        // vers un mur du labyrinthe.
        public static Boolean testCollision(Object obj1, String direction)
        {
            // On initialise une collision à false par défaut
            bool collision = false;
            // On récupère la position X/Y arrondie du pacman/fantome dans un tableau
            // On convertit la position x/y de pixel vers une case [i,j] de la matrice
            ObjetAnime objetAnime = null;
            if (obj1 is JoueurPacman)
            {
                JoueurPacman joueurPacman = (JoueurPacman)obj1;
                objetAnime = joueurPacman._pacman;
            }
            else if (obj1 is IAFantom)
            {
                IAFantom iaFantom = (IAFantom)obj1;
                objetAnime = iaFantom._fantom;
            }
            int[] position = getPositionMatrice(objetAnime.Position.X, objetAnime.Position.Y);
            int j = position[0];
            int i = position[1];

            // Puis on test si le déplacement est possible selon le déplacement
            // On récupère la carte
            byte[,] map = Pacman.getMap();
            int VX = Pacman.getVX();
            int VY = Pacman.getVY();

            // Si la case suivante est pas un mur
            // il y a collision
            // R L U D pour Right Left Up Down
            if (direction == "R")
            {
                if (j >= VY - 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    j = -1;
                if (map[i, j+1] == 0)
                    collision = true;
            }
            else if (direction == "L")
            {
                if (j <= 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    j = VY;
                if (map[i, j-1] == 0)
                    collision = true;
            }
            else if (direction == "U")
            {
                if (i <= 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    i = VX;
                if (map[i-1, j] == 0)
                    collision = true;
            }
            else if (direction == "D")
            {
                if (i >= VX - 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    i = -1;
                if (map[i+1, j] == 0)
                    collision = true;
            }

            return collision;
        }

        // Même méthode que testCollision mais spécifique aux fantomes.
        // Permet de considérer la zone de départ (map i,j =2) comme une collision.
        public static Boolean testCollisionFantom(IAFantom iaFantom, String direction)
        {
            // On initialise une collision à false par défaut
            bool collision = false;
            // On récupère la position X/Y arrondie du pacman/fantome dans un tableau
            // On convertit la position x/y de pixel vers une case [i,j] de la matrice
            ObjetAnime objetAnime = iaFantom._fantom;
            
            int[] position = getPositionMatrice(objetAnime.Position.X, objetAnime.Position.Y);
            int j = position[0];
            int i = position[1];

            // Puis on test si le déplacement est possible selon le déplacement
            // On récupère la carte
            byte[,] map = Pacman.getMap();
            int VX = Pacman.getVX();
            int VY = Pacman.getVY();

            // on met à jour la position du fantôme en cas de hors limite
       
            iaFantom.updatePosition();
            // Si la case suivante est pas un mur
            // il y a collision
            // R L U D pour Right Left Up Down
            
            if (direction == "R")
            {
                if (j >= VY - 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    j = -1;
                if (map[i, j + 1] == 0 || map[i, j + 1] == 2)
                    collision = true;
            }
            else if (direction == "L")
            {
                if (j <= 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    j = VY;
                if (map[i, j - 1] == 0 || map[i, j - 1] == 2)
                    collision = true;
            }
            else if (direction == "U")
            {
                if (i <= 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    i = VX;
                if (map[i - 1, j] == 0 || map[i - 1, j] == 2)
                    collision = true;
            }
            else if (direction == "D")
            {
                if (i >= VX - 1) // Si dépasse de la matrice, amène de l'autre côté de l'écran
                    i = -1;
                if (map[i + 1, j] == 0 || map[i + 1, j] == 2)
                    collision = true;
            }

            return collision;
        }

        // Convertit la position x/y de pixel vers une case [i,j] de la matrice
        public static int[] getPositionMatrice(double x, double y)
        {
            x = Math.Round(x, 0);
            y = Math.Round(y, 0);
            // On convertit la position x/y de pixel vers une case [i,j] de la matrice
            x = Math.Round(x / 20, 0);
            y = Math.Round(y / 20, 0);
            int[] position = { (int)x, (int)y };

            return position;
        }
    }
}