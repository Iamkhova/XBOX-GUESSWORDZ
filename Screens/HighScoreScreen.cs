#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class HighScoreScreen : GameScreen
    {
        #region Fields

        const string GFX_HIGHSCORE_BG = "Backgrounds/highscore_bg";
        const string GFX_HIGHSCORE_TEXT ="Textures/highscores";
        const string GFX_NEWHIGHSCORE_TEXT = "Textures/newhighscore";
        const string GFX_TROPHYIMAGE = "Textures/trophy";
        const string GFX_ACONTINUE = "Textures/a-continue";
        const string GFX_BRETURNMENU = "Textures/b-returnmenu";

        ContentManager content;
        Texture2D gfx_highscore_bg;
        Texture2D gfx_highscore_text;
        Texture2D gfx_newhighscore_text;
        Texture2D gfx_trophy;
        Texture2D gfx_acontinue;
        Texture2D gfx_breturnmenu;
        HighScores highScore = new HighScores();
        bool scoreUpdated = false;
        bool scoreSaved = false;
        bool noHighScore = false;

        SpriteFont gameFont;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HighScoreScreen()
        {
         
            HighScores.LoadHighScores();
           
        
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            bool check = isHighScore();

            if (check == true)
            {

                GuessWurdz.oskb.Visible = true;
                GuessWurdz.oskb.Enabled = true;
                //GuessWurdz.oskb.Text = GuessWurdz.gamer.Gamertag;
                GuessWurdz.oskb.Prompt = "Enter Name for High Score";

            }
            else
            {
                noHighScore = true;
                GuessWurdz.gameState = GuessWurdz.GameState.GameEnd;
            }


        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("Fonts/highscore");
            gfx_highscore_bg = content.Load<Texture2D>(GFX_HIGHSCORE_BG);
            gfx_highscore_text = content.Load<Texture2D>(GFX_HIGHSCORE_TEXT);
            gfx_newhighscore_text = content.Load<Texture2D>(GFX_NEWHIGHSCORE_TEXT);
            gfx_trophy = content.Load<Texture2D>(GFX_TROPHYIMAGE);
            gfx_acontinue = content.Load<Texture2D>(GFX_ACONTINUE);
            gfx_breturnmenu = content.Load<Texture2D>(GFX_BRETURNMENU);
            

           // backgroundTexture = content.Load<Texture2D>("Backgrounds/background");
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

            if (GuessWurdz.gameState == GuessWurdz.GameState.HandelHighScore)
            {
                if (GuessWurdz.oskb.ExitState > 0 && !scoreSaved)
                {
                    GuessWurdz.oskb.Visible = false;
                    GuessWurdz.oskb.Enabled = false;

                    highScore.SaveHighScore(GuessWurdz.gscore, GuessWurdz.glevel, GuessWurdz.oskb.Text);
                    scoreSaved = true;

                }
            }
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(InputState input)
        {
            if (GuessWurdz.gameMode == GuessWurdz.GameMode.ViewingHighScore)
            {
                if (input.IsNewKeyPress(Keys.Escape) || input.IsNewButtonPress(Buttons.B))
                {
                    ScreenManager.AddScreen(new BackgroundScreen());
                }

            }else
                if (GuessWurdz.gameMode == GuessWurdz.GameMode.PlayingGame)
                {
                    if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                    {
                        ScreenManager.AddScreen(new BackgroundScreen());
                    }
                }

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;
            
            // Set Screen To White
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);
            // Draw Backgrond
            spriteBatch.Begin(SpriteBlendMode.None);
            spriteBatch.Draw(gfx_highscore_bg, fullscreen,
                             new Color(255 , 255, 255));
            spriteBatch.End();

            // Update Score Board

            if ((GuessWurdz.oskb.ExitState > 0 || noHighScore == true) && GuessWurdz.gameState == GuessWurdz.GameState.GameEnd)
            {
                if (!scoreUpdated)
                {
                    HighScores.LoadHighScores();
                    scoreUpdated = true;
                }
                
            }

            // Draw Score Board Screen

       

            if (noHighScore == true)
            {
                drawHighScore();
            }
            else
            {
                drawNewHighScore();    
            }

            if (GuessWurdz.gameMode == GuessWurdz.GameMode.PlayingGame)
            {
                displayAbutton(40, 650);
            }
            else
            {
                displayBbutton(40, 650);
            }

        }

        public void displayAbutton(int x, int y)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_acontinue, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();

        }

        public void displayBbutton(int x, int y)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_breturnmenu, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();

        }

        private void drawNewHighScore()
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            int count = 0;
            int position;

            spriteBatch.Begin();

            spriteBatch.Draw(gfx_newhighscore_text,
                new Rectangle(445, 150, 711, 68), new Color(255, 255, 255));
            spriteBatch.Draw(gfx_trophy,
                new Rectangle(65, 100, 353, 521), new Color(255, 255, 255));

            // Draw Scores
            for (int i = 0; i <= (GuessWurdz.theHighScore.Count - 1); i++)
            {
                position = (count * 70);
                if (GuessWurdz.gscore == GuessWurdz.theHighScore.Score[i])
                {
                    spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(455, 250 + position), Color.Red);
                    spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(980, 250 + position), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(455, 250 + position), Color.Black);
                    spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(980, 250 + position), Color.Black);
                
                }
                count++;
            }

            spriteBatch.End();




        }
 

        private void drawHighScore()
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            int count = 0;
            int position;

           spriteBatch.Begin();

           spriteBatch.Draw(gfx_highscore_text,
               new Rectangle(375, 140, 533, 62), new Color(255, 255, 255));

            // Draw Scores
           for (int i = 0; i <= (GuessWurdz.theHighScore.Count - 1); i++)
            {
                position = (count * 70);
                spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(295, 250 + position), Color.Black);
                spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(810 , 250 + position), Color.Black);

                count++;
            }

            spriteBatch.End();

      
         


        }


  
        private bool isHighScore()
        {
            HighScores.LoadHighScores();

            int scoreIndex = -1;
            bool response;
            for (int i = 0; i < (GuessWurdz.theHighScore.Count - 1); i++)
            {
                if (GuessWurdz.gscore > GuessWurdz.theHighScore.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex == -1) { response = false; } else { response = true; }

            return response;

        }


        #endregion
    }
}
