using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2


namespace PacMan
{
    class ObjetAnime
    {
        private Texture2D _texture;    //  sprite texture 

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        private Vector2 _position;     //  sprite position on screen

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _size;         //  sprite size in pixels

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private Vector2 _vitesse;

        public Vector2 Vitesse
        {
            get { return _vitesse; }
            set { _vitesse = value; }
        }

        public ObjetAnime(Texture2D texture, Vector2 position, Vector2 size, Vector2 vitesse)
        {
            this._texture = texture;
            this._position = position;
            this._size = size;
            this._vitesse = vitesse;
        }
    }

}
