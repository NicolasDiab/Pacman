using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PacMan
{
    class Miam
    {
        public static Boolean testMiam(JoueurPacman joueurPacman)
        {
            Boolean bmiam = false;
            // On récupère la position X/Y arrondie du pacman dans un tableau
            // On convertit la position x/y de pixel vers une case [i,j] de la matrice
            ObjetAnime objetAnime = joueurPacman._pacman;
            int[] position = Moteur2D.getPositionMatrice(objetAnime.Position.X, objetAnime.Position.Y);
            int j = position[0];
            int i = position[1];

            // Puis on test si le miam est possible
            byte[,] map = Pacman.getMap();
            if (map[i, j] == 1)
            {
                bmiam = true;
                map[i, j] = 10;
                Pacman.setMap(map);
                ajouterScore(5);
            }
            else if (map[i, j] == 3)
            {
                bmiam = true;
                map[i, j] = 2;
                Pacman.setMap(map);
                ajouterScore(100);
                // activer immortalité
                // fuite des fantome
                // chgt textures
            }
            else if (map[i, j] == 4)
            {
                bmiam = true;
                map[i, j] = 10;

                Pacman.setPouvoirBool(true);
                joueurPacman.Pouvoir = true;
                Pacman.setPouvoirTime(0);

                Pacman.setMap(map);
                ajouterScore(50);
            }

            return bmiam;
        }

        public static void testMiam(IAFantom iaFantom)
        {
            Boolean bmiam = false;
            // On récupère la position du fantom et du pacman
            Vector2 positionFantom = iaFantom._fantom.Position;
            Vector2 positionPacman = Pacman.getPositionPacman();

            double diffX = Math.Abs(positionFantom.X - positionPacman.X);
            double diffY = Math.Abs(positionFantom.Y - positionPacman.Y);
            bool pouvoir = Pacman.getPouvoirPacman();

            if (diffX <= 9 && diffY <= 9 && !pouvoir)
                bmiam = true;

            if (diffX <= 9 && diffY <= 9 && pouvoir)
            {
                ajouterScore(250);
                iaFantom.reset();
            }

            if (bmiam)
            {
                Pacman.tryAgain();
            }
        }

        public static void ajouterScore(int score)
        {
            // Test modulo 5000 pour l'ajout d'une vie bonus tous les 1000 de score.
            double actualScore = Pacman.getScore();
            double modulo1000Actu = Math.Truncate(actualScore / 1000);
            double modulo1000Futu = Math.Truncate((actualScore + score) / 1000);

            if (modulo1000Futu == modulo1000Actu + 1)
            {
                Pacman.setVies(Pacman.getVies() + 1);
            }

            Pacman.setScore(Pacman.getScore() + score);
        }
    }
}
