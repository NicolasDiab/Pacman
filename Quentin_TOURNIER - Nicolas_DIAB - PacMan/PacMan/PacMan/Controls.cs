using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    class Controls
    {
        public const Keys UP = Keys.Z;
        public const Keys LEFT = Keys.Q;
        public const Keys DOWN = Keys.S;
        public const Keys RIGHT = Keys.D;

        // Vérifie si le joueur a effectué l'action "z"
        public static Boolean CheckActionUp()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(UP);
        }

        // Vérifie si le joueur passé en paramètre a effectué l'action "q"
        public static Boolean CheckActionLeft()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(LEFT);
        }

        // Vérifie si le joueur passé en paramètre a effectué l'action "s"
        public static Boolean CheckActionDown()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(DOWN);
        }

        // Vérifie si le joueur passé en paramètre a effectué l'action "d"
        public static Boolean CheckActionRight()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(RIGHT);
        }
    }
}