using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    class IAFantom : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        private string filename;
        private ObjetAnime fantom;
        private Vector2 position_initiale;
        private Vector2 vitesse_initiale;
        private const int maxY = 610, maxX = 540;
        private const int minY = 0, minX = 0;
        // Les 4 bool permettent de corriger un bug de décalage de
        // 10 pixels lors d'une collision
        private bool alreadyShiftUp = false;
        private bool alreadyShiftDown = false;
        private bool alreadyShiftRight = false;
        private bool alreadyShiftLeft = false;
        private bool firstMove;
        private bool alreadyCollision;
        private int depla;
        private string textureOriginaleString;

        public IAFantom(Game game, string filename, int vitesse, int posX, int posY)
            : base(game)
        {
            this.filename = filename;
            textureOriginaleString = filename;
            this.vitesse_initiale = new Vector2(0, vitesse);
            this.position_initiale = new Vector2(posX, posY);
            this.Game.Components.Add(this);
            this.FirstMove = true;
            this.alreadyCollision = true;
        }

        public ObjetAnime _fantom
        {
            get { return fantom; }
        }

        public bool FirstMove
        {
            get
            {
                return firstMove;
            }

            set
            {
                firstMove = value;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fantom = new ObjetAnime(Game.Content.Load<Texture2D>(@"images\" + filename),
                position_initiale, Vector2.Zero, vitesse_initiale);
            Vector2 taille;
            taille.X = fantom.Texture.Width;
            taille.Y = fantom.Texture.Height;
            fantom.Size = taille;
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(fantom.Texture, fantom.Position, Color.Azure);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Est-ce qu'on est dans la zone de départ ?
            if (this.FirstMove)
            {
                this.goUpFirst();
            }
            else
            {
                // Une fois sorti de la zone de départ
                // Déplacement aléatoire : 1=D, 2=U, 3=R, 4=L
                if (alreadyCollision)
                {
                    this.depla = obtenirDeplaRandom();
                }
                switch (depla)
                {
                    case 1:
                        this.goDown();
                        break;
                    case 2:
                        this.goUp();
                        break;
                    case 3:
                        this.goRight();
                        break;
                    case 4:
                        this.goLeft();
                        break;
                }
                Miam.testMiam(this);
            }

            base.Update(gameTime);
        }

        public void goUpFirst()
        {
            Vector2 p;

            if (!Moteur2D.testCollision(this, "U"))
            {
                // Est-ce qu'on est tout en haut ?
                if (fantom.Position.Y > minY)
                {
                    p = fantom.Position;
                    p.Y -= fantom.Vitesse.Y;
                    fantom.Position = p;
                }
                else // arrive de l'autre côté de l'écran
                {
                    p = fantom.Position;
                    p.Y = maxY;
                    fantom.Position = p;
                }
                // Si l'utilisateur se déplace vers le haut,
                // réinitialise le booléen de décalage
                alreadyShiftUp = false;
            }
            // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
            // on le décale vers le haut de 8px et le bool passe à true.
            else if ((fantom.Position.Y % 20 != 0) && (alreadyShiftUp == false))
            {
                alreadyShiftUp = true;
                p = fantom.Position;
                p.Y -= 8;
                fantom.Position = p;
                this.alreadyCollision = true;
                this.FirstMove = false;
            }
            // Effectue un nouveau déplacement si une collision apparait ou un chemin lattérale.
            // Fin du premier mouvement
            else if (Moteur2D.testCollision(this, "U") || !Moteur2D.testCollision(this, "R") || !Moteur2D.testCollision(this, "L"))
            {
                this.FirstMove = false;
                this.alreadyCollision = true;
            }
        }

        public void goDown()
        {
            Vector2 p;

            if (!Moteur2D.testCollisionFantom(this, "D"))
            {
                // Est-ce qu'on est tout en bas ?
                if (fantom.Position.Y < maxY)
                {
                    p = fantom.Position;
                    p.Y += fantom.Vitesse.Y;
                    fantom.Position = p;
                }
                else // arrive de l'autre côté de l'écran
                {
                    p = fantom.Position;
                    p.Y = minY;
                    fantom.Position = p;
                }
                // Si l'utilisateur se déplace vers le bas,
                // réinitialise le booléen de décalage
                alreadyShiftDown = false;
            }
            // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
            // on le décale vers le bas de 8px et le bool passe à true.
            else if ((fantom.Position.Y % 20 != 0) && (alreadyShiftDown == false))
            {
                alreadyShiftDown = true;
                p = fantom.Position;
                p.Y += 8; // 8 et non 10 pour éviter l'effet "téléportation"
                fantom.Position = p;
                this.alreadyCollision = true;
            }
            else // bloqué
            {
                alreadyCollision = true;
            }
            // Effectue un nouveau déplacement si une collision apparait ou un chemin lattérale.
            if (fantom.Position.Y % 20 == 0 && (!Moteur2D.testCollisionFantom(this, "R") || !Moteur2D.testCollisionFantom(this, "L")))
            {
                this.alreadyCollision = true;
            }
        }

        public void goRight()
        {
            Vector2 p;

            if (!Moteur2D.testCollisionFantom(this, "R"))
            {
                // Est-ce qu'on est tout à droite ?
                if (fantom.Position.X < maxX)
                {
                    p = fantom.Position;
                    p.X += fantom.Vitesse.Y;
                    fantom.Position = p;
                }
                else // arrive de l'autre côté de l'écran
                {
                    p = fantom.Position;
                    p.X = minX;
                    fantom.Position = p;
                }
                // Si l'utilisateur se déplace vers la droite,
                // réinitialise le booléen de décalage
                alreadyShiftRight = false;
            }
            // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
            // on le décale vers la droite de 8px et le bool passe à true.
            else if ((fantom.Position.X % 20 != 0) && (alreadyShiftRight == false))
            {
                alreadyShiftRight = true;
                p = fantom.Position;
                p.X += 8;
                fantom.Position = p;
                this.alreadyCollision = true;
            }
            else // bloqué
            {
                alreadyCollision = true;
            }
            // Effectue un nouveau déplacement si une collision apparait ou un chemin lattérale
            if (fantom.Position.X % 20 == 0 && (!Moteur2D.testCollisionFantom(this, "U") || !Moteur2D.testCollisionFantom(this, "D")))
            {
                this.alreadyCollision = true;
            }
        }

        public void goUp()
        {
            Vector2 p;

            if (!Moteur2D.testCollisionFantom(this, "U"))
            {
                // Est-ce qu'on est tout en haut ?
                if (fantom.Position.Y > minY)
                {
                    p = fantom.Position;
                    p.Y -= fantom.Vitesse.Y;
                    fantom.Position = p;
                }
                else // arrive de l'autre côté de l'écran
                {
                    p = fantom.Position;
                    p.Y = maxY - 20;
                    fantom.Position = p;
                }
                // Si l'utilisateur se déplace vers le haut,
                // réinitialise le booléen de décalage
                alreadyShiftUp = false;
            }
            // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
            // on le décale vers le haut de 8px et le bool passe à true.
            else if ((fantom.Position.Y % 20 != 0) && (alreadyShiftUp == false))
            {
                alreadyShiftUp = true;
                p = fantom.Position;
                p.Y -= 8;
                fantom.Position = p;
                this.alreadyCollision = true;
            }
            else // bloqué
            {
                alreadyCollision = true;
            }
            // Effectue un nouveau déplacement si une collision apparait ou un chemin lattérale
            if (fantom.Position.Y % 20 == 0 && (!Moteur2D.testCollisionFantom(this, "R") || !Moteur2D.testCollisionFantom(this, "L")))
            {
                this.alreadyCollision = true;
            }
        }

        public void goLeft()
        {
            Vector2 p;

            if (!Moteur2D.testCollisionFantom(this, "L"))
            {
                // Est-ce qu'on est tout à gauche ?
                if (fantom.Position.X > minX)
                {
                    p = fantom.Position;
                    p.X -= fantom.Vitesse.Y;
                    fantom.Position = p;
                }
                else // arrive de l'autre côté de l'écran
                {
                    p = fantom.Position;
                    p.X = maxX;
                    fantom.Position = p;
                }
                // Si l'utilisateur se déplace vers la gauche,
                // réinitialise le booléen de décalage
                alreadyShiftLeft = false;
            }
            // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
            // on le décale vers la gauche de 8px et le bool passe à true.
            else if ((fantom.Position.X % 20 != 0) && (alreadyShiftLeft == false))
            {
                alreadyShiftLeft = true;
                p = fantom.Position;
                p.X -= 8;
                fantom.Position = p;
                this.alreadyCollision = true;
            }
            else // bloqué
            {
                alreadyCollision = true;
            }
           // Effectue un nouveau déplacement si une collision apparait ou un chemin lattérale
            if (fantom.Position.X % 20 == 0 && (!Moteur2D.testCollisionFantom(this, "U") || !Moteur2D.testCollisionFantom(this, "D")))
            {
                this.alreadyCollision = true;
            }
        }

        // Déplacement aléatoire : 1=D, 2=U, 3=R, 4=L
        public int obtenirDeplaRandom()
        {
            Random random = new Random();
            int depla = random.Next(1, 5);
            this.alreadyCollision = false;

            return depla;
        }

        public void reset()
        {
            Vector2 p = this._fantom.Position;
            p.X = 280;
            p.Y = 280;
            this._fantom.Position = p;
            this.firstMove = true;
        }

        public void texturePouvoir()
        {
           // filename = "fan_mangeable.png";
            this._fantom.Texture = Game.Content.Load<Texture2D>(@"images\fan_mangeable");
        }

        public void textureOriginale()
        {
          //  filename = textureOriginaleString;
            this._fantom.Texture = Game.Content.Load<Texture2D>(@"images\" + textureOriginaleString);
        }

        public void updatePosition()
        {
            Vector2 p = this._fantom.Position;

            if(p.X>maxX)
            {
                p.X = minX;
            }

            if (p.X < minX)
            {
                p.X = maxX;
            }

            if (p.Y > maxY)
            {
                p.Y = minY;
            }

            if (p.Y < minY)
            {
                p.Y = maxY;    
            }
            this._fantom.Position = p;
        }
    }
}
