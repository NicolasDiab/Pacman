using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PacMan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pacman : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Vector2 positionScore;
        private Vector2 positionVies;
        private Vector2 positionVictoire;
        private SpriteFont textFont;
        private SpriteFont textFontVictoire;
        private ObjetAnime mur;
        private ObjetAnime bean;
        private ObjetAnime beanGros;
        private ObjetAnime pouvoir;
        private IAFantom fantomeCyan;
        private IAFantom fantomeOrange;
        private IAFantom fantomeRouge;
        private IAFantom fantomeRose;
        private static JoueurPacman joueurPacman;
        private const int VX = 31;
        private const int VY = 28;
        private static byte[,] map;
        private Song ambiance;
        private static int score;
        private static int vies;
        private Boolean victoire;
        private static bool gameOver;
        private static bool _tryAgain;
        private double nextBlinkTime;
        private static double pouvoirTime;
        private static bool pouvoirBool;
        private Boolean msgVictoireVisibility;
        private bool msgGameOverVisibility;

        public Pacman()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            map = new byte[VX, VY]{
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 2, 2, 2, 2, 2, 2, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
            {0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            {0, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };
        }

        // Méthode d'accès à la map pour le test des collisions dans Moteur2D
        public static byte[,] getMap()
        {
            return map;
        }
        public static void setMap(byte[,] _map)
        {
            map = _map;
        }
        public static int getVX()
        {
            return VX;
        }
        public static int getVY()
        {
            return VY;
        }
        public static int getScore()
        {
            return score;
        }
        public static void setScore(int _score)
        {
            score = _score;
        }
        public static int getVies()
        {
            return vies;
        }
        public static void setVies(int _vies)
        {
            vies = _vies;
        }
        public static void setGameOver(bool gameOver2)
        {
            gameOver = gameOver2;
        }
        public static void setTryAgain(bool tryAgain2)
        {
            _tryAgain = tryAgain2;
        }

        public static bool getPouvoirPacman()
        {
            return joueurPacman.Pouvoir;
        }

        public static Vector2 getPositionPacman()
        {
            return joueurPacman._pacman.Position;
        }

        public static bool getPouvoirBool()
        {
            return pouvoirBool;
        }

        public static void setPouvoirBool(bool pouv)
        {
            pouvoirBool = pouv;
        }

        public static void setPouvoirTime(double time)
        {
            pouvoirTime = time;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            positionScore.X = 620;
            positionScore.Y = 200;

            positionVies.X = 620;
            positionVies.Y = 300;

            positionVictoire.X = this.GraphicsDevice.Viewport.Width / 2 - 390;
            positionVictoire.Y = this.GraphicsDevice.Viewport.Height / 2;

            joueurPacman = new JoueurPacman(this, "pacman", 3, 120, 400);

            fantomeCyan = new IAFantom(this, "fantome_cyan", 2, 281, 280);
            fantomeOrange = new IAFantom(this, "fantome_orange", 2, 282, 280);
            fantomeRose = new IAFantom(this, "fantome_rose", 2, 280, 280);
            fantomeRouge = new IAFantom(this, "fantome_rouge", 2, 279, 280);

            score = 0;
            vies = 3;

            nextBlinkTime = 0;
            gameOver = false;
            _tryAgain = false;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Font
            this.textFont = Content.Load<SpriteFont>("fonts\\TolkienFont");
            this.textFontVictoire = Content.Load<SpriteFont>("fonts\\TolkienFontVictoire");
            // Résolution
            graphics.PreferredBackBufferWidth = 810;
            graphics.PreferredBackBufferHeight = 620;
            graphics.ApplyChanges();
            // on charge un objet mur 
            mur = new ObjetAnime(Content.Load<Texture2D>("images\\mur"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Vector2(0, 0));
            bean = new ObjetAnime(Content.Load<Texture2D>("images\\bean"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Vector2(0, 0));
            beanGros = new ObjetAnime(Content.Load<Texture2D>("images\\gros_bean"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Vector2(0, 0));
            pouvoir = new ObjetAnime(Content.Load<Texture2D>("images\\pouvoir"), new Vector2(0f, 0f), new Vector2(20f, 20f), new Vector2(0, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            // Scores
            String stringScore = "Score : " + score;
            String stringVies = "Vie(s) : " + vies;
            spriteBatch.DrawString(this.textFont, stringScore, positionScore, Color.Yellow);
            spriteBatch.DrawString(this.textFont, stringVies, positionVies, Color.Yellow);

            // Labyrinthe
            victoire = true;
            for (int x = 0; x < VX; x++)
            {
                for (int y = 0; y < VY; y++)
                {
                    if (map[x, y] == 0)
                    {
                        int xpos, ypos;
                        xpos = x * 20;
                        ypos = y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(mur.Texture, pos, Color.White);
                    }
                    else if (map[x, y] == 1)
                    {
                        victoire = false;
                        int xpos, ypos;
                        xpos = x * 20;
                        ypos = y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(bean.Texture, pos, Color.White);
                    }
                    else if (map[x, y] == 3)
                    {
                        victoire = false;
                        int xpos, ypos;
                        xpos = x * 20 + 4;
                        ypos = y * 20 + 4;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(pouvoir.Texture, pos, Color.White);
                    }
                    else if (map[x, y] == 4)
                    {
                        victoire = false;
                        int xpos, ypos;
                        xpos = x * 20;
                        ypos = y * 20;
                        Vector2 pos = new Vector2(ypos, xpos);
                        spriteBatch.Draw(beanGros.Texture, pos, Color.White);
                    }
                }
            }

            // Victoire
            if (victoire && !gameOver)
            {
                // message
                if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                {
                    msgVictoireVisibility = !msgVictoireVisibility;
                    nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.5;
                }
                if (msgVictoireVisibility)
                {
                    String stringVictoire2 = "Felicitation ! Vous avez gagne !! Score final : " + score;
                    spriteBatch.DrawString(this.textFontVictoire, stringVictoire2, positionVictoire, Color.Yellow);
                }
                // stop fantomes
                // prochain niveau
            }

            if (_tryAgain && !victoire)
            {
                // reset position fantomes
                Vector2 p = new Vector2(280, 280);
                fantomeCyan._fantom.Position = p;
                fantomeCyan.FirstMove = true;
                fantomeOrange._fantom.Position = p;
                fantomeOrange.FirstMove = true;
                p = new Vector2(281, 280);
                fantomeRose._fantom.Position = p;
                fantomeRose.FirstMove = true;
                fantomeRouge._fantom.Position = p;
                fantomeRouge.FirstMove = true;
                // pacman
                joueurPacman._pacman.Position = new Vector2(120, 400);
                _tryAgain = false;
            }

            if (gameOver && !victoire)
            {
                // message
                if (gameTime.TotalGameTime.TotalSeconds >= nextBlinkTime)
                {
                    msgGameOverVisibility = !msgGameOverVisibility;
                    nextBlinkTime = gameTime.TotalGameTime.TotalSeconds + 0.5;
                }
                if (msgGameOverVisibility)
                {
                    String stringGameOver = "Perdu ! Vous avez ete mange !!";
                    spriteBatch.DrawString(this.textFontVictoire, stringGameOver, positionVictoire, Color.Yellow);
                    // petite danse des familles des fantomes
                    fantomeRouge.FirstMove = true;
                    fantomeRose.FirstMove = true;
                    fantomeOrange.FirstMove = true;
                    fantomeCyan.FirstMove = true;
                }
            }

            if (joueurPacman.Pouvoir && gameTime.TotalGameTime.TotalSeconds >= pouvoirTime)
            {
                pouvoirBool = !pouvoirBool;
                pouvoirTime = gameTime.TotalGameTime.TotalSeconds + 5;

                fantomeCyan.texturePouvoir();
                fantomeOrange.texturePouvoir();
                fantomeRose.texturePouvoir();
                fantomeRouge.texturePouvoir();
            }
            if (pouvoirBool)
            {
                stopPouvoir();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void tryAgain()
        {
            if (Pacman.getVies() > 0)
            {
                vies -= 1;
                _tryAgain = true;
            }
            else
                gameOver = true;
        }

       /* public void startPouvoir()
        {
            pouvoirTime = 0;
            pouvoirBool = true;
            joueurPacman.Pouvoir = true;
            fantomeCyan.texturePouvoir();
            fantomeOrange.texturePouvoir();
            fantomeRose.texturePouvoir();
            fantomeRouge.texturePouvoir();
        }*/

        public void stopPouvoir()
        {
            joueurPacman.Pouvoir = false;
            pouvoirTime = 0;
            fantomeCyan.textureOriginale();
            fantomeOrange.textureOriginale();
            fantomeRose.textureOriginale();
            fantomeRouge.textureOriginale();
        }
    }
}
