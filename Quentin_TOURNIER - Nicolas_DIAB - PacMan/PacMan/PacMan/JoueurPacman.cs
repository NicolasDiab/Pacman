using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    class JoueurPacman : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        
        private string filename;
        private ObjetAnime pacman;
        private Vector2 position_initiale;
        private Vector2 vitesse_initiale;
        private const int maxY = 620, maxX = 540;
        private const int minY = 0, minX = 0;
        public const int VITESSE_PACMAN = 8;
        // Les 4 bool permettent de corriger un bug de décalage de
        // 10 pixels lors d'une collision
        private bool alreadyShiftUp = false;
        private bool alreadyShiftDown = false;
        private bool alreadyShiftRight = false;
        private bool alreadyShiftLeft = false;
        private double nextBlinkTime;
        private bool textureChange;
        private String textureOriginalString;
        private int posTextPacman;
        private bool pouvoir;

        public JoueurPacman(Game game, string filename, int vitesse, int posX, int posY)
            : base(game)
        {
            this.filename = filename;
            textureOriginalString = filename;
            this.vitesse_initiale = new Vector2(0, vitesse);
            this.position_initiale = new Vector2(posX, posY);
            this.Game.Components.Add(this);
            nextBlinkTime = 0;
            posTextPacman = 0;
            textureChange = true;
        }

        public ObjetAnime _pacman
        {
            get { return pacman; }
        }

        public bool Pouvoir
        {
            get
            {
                return pouvoir;
            }

            set
            {
                pouvoir = value;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            pacman = new ObjetAnime(Game.Content.Load<Texture2D>(@"images\" + filename),
                position_initiale, Vector2.Zero, vitesse_initiale);
            Vector2 taille;
            taille.X = pacman.Texture.Width;
            taille.Y = pacman.Texture.Height;
            pacman.Size = taille;
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pacman.Texture, pacman.Position, Color.Azure);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 p;

            // La classe Controls contient les constantes correspondantes aux contrôles définies sur la plate-forme
            // et des méthodes, pour chaque action possible dans le jeu, qui vérifient si les contrôles correspondants
            // ont été "enclenchés"
            if (Controls.CheckActionDown())
            {
                if (!Moteur2D.testCollision(this, "D"))
                {
                    // Est-ce qu'on est tout en bas ?
                    if (pacman.Position.Y + pacman.Size.X < maxY)
                    {
                        p = pacman.Position;
                        p.Y += pacman.Vitesse.Y;
                        pacman.Position = p;
                    }
                    else // arrive de l'autre côté de l'écran
                    {
                        p = pacman.Position;
                        p.Y = minY;
                        pacman.Position = p;
                    }
                    // Si l'utilisateur se déplace vers le bas,
                    // réinitialise le booléen de décalage
                    alreadyShiftDown = false;
                    // change texture
                    if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                    {
                        textureChange = !textureChange;
                        nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.2;
                    }
                    if (textureChange)
                    {
                        this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanBas1");
                    }
                    else
                    {
                        this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanBas0");
                    }
                }
                // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
                // on le décale vers le bas de 8px et le bool passe à true.
                else if ((pacman.Position.Y % 20 != 0) && (alreadyShiftDown == false))
                {
                    alreadyShiftDown = true;
                    p = pacman.Position;
                    p.Y += 8; // 8 et non 10 pour éviter l'effet "téléportation"
                    pacman.Position = p;
                }
                Boolean miam = Miam.testMiam(this);
            }
            else if (Controls.CheckActionUp())
            {
                if (!Moteur2D.testCollision(this, "U"))
                {
                    // Est-ce qu'on est tout en haut ?
                    if (pacman.Position.Y > minY)
                    {
                        p = pacman.Position;
                        p.Y -= pacman.Vitesse.Y;
                        pacman.Position = p;
                    }
                    else // arrive de l'autre côté de l'écran
                    {
                        p = pacman.Position;
                        p.Y = maxY - 20;
                        pacman.Position = p;
                    }
                    // Si l'utilisateur se déplace vers le haut,
                    // réinitialise le booléen de décalage
                    alreadyShiftUp = false;
                }
                // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
                // on le décale vers le haut de 8px et le bool passe à true.
                else if ((pacman.Position.Y % 20 != 0) && (alreadyShiftUp == false))
                {
                    alreadyShiftUp = true;
                    p = pacman.Position;
                    p.Y -= 8;
                    pacman.Position = p;
                }
                Boolean miam = Miam.testMiam(this);

                if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                {
                    textureChange = !textureChange;
                    nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.2;
                }
                if (textureChange)
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanHaut1");
                }
                else
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanHaut0");
                }
            }
            else if (Controls.CheckActionLeft())
            {
                if (!Moteur2D.testCollision(this, "L"))
                {
                    // Est-ce qu'on est tout à gauche ?
                    if (pacman.Position.X > minX)
                    {
                        p = pacman.Position;
                        p.X -= pacman.Vitesse.Y;
                        pacman.Position = p;
                    }
                    else // arrive de l'autre côté de l'écran
                    {
                        p = pacman.Position;
                        p.X = maxX;
                        pacman.Position = p;
                    }
                    // Si l'utilisateur se déplace vers la gauche,
                    // réinitialise le booléen de décalage
                    alreadyShiftLeft = false;
                }
                // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
                // on le décale vers la gauche de 8px et le bool passe à true.
                else if ((pacman.Position.X % 20 != 0) && (alreadyShiftLeft == false))
                {
                    alreadyShiftLeft = true;
                    p = pacman.Position;
                    p.X -= 8;
                    pacman.Position = p;
                }
                Boolean miam = Miam.testMiam(this);

                if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                {
                    textureChange = !textureChange;
                    nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.2;
                }
                if (textureChange)
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanGauche1");
                }
                else
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanGauche0");
                }
            }
            else if (Controls.CheckActionRight())
            {
                if (!Moteur2D.testCollision(this, "R"))
                {
                    // Est-ce qu'on est tout à droite ?
                    if (pacman.Position.X < maxX)
                    {
                        p = pacman.Position;
                        p.X += pacman.Vitesse.Y;
                        pacman.Position = p;
                    }
                    else // arrive de l'autre côté de l'écran
                    {
                        p = pacman.Position;
                        p.X = minX;
                        pacman.Position = p;
                    }
                    // Si l'utilisateur se déplace vers la droite,
                    // réinitialise le booléen de décalage
                    alreadyShiftRight = false;
                }
                // Si on a une colision et que l'utilisateur n'a pas encore été décalé,
                // on le décale vers la droite de 8px et le bool passe à true.
                else if ((pacman.Position.X % 20 != 0) && (alreadyShiftRight == false))
                {
                    alreadyShiftRight = true;
                    p = pacman.Position;
                    p.X += 8;
                    pacman.Position = p;
                }
                Boolean miam = Miam.testMiam(this);

                if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                {
                    textureChange = !textureChange;
                    nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.2;
                }
                if (textureChange)
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanDroite1");
                }
                else
                {
                    this._pacman.Texture = Game.Content.Load<Texture2D>(@"images\pacmanDroite0");
                }
            }

            base.Update(gameTime);
        }

        
    }
}
