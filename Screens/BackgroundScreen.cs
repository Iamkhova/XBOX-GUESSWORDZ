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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        const int MOVE_UP = 1;
        const int MOVE_DOWN = 2;
        const int MOVE_SELECT = 3;

        public const string GFX_HTP_BACKGROUND = "Backgrounds/howtoplay";
        public const string GFX_BRETURN = "Textures/b-returnmenu";

        public const string GFX_CREDITS_BACKGROUND = "Textures/credits";

        const string GFX_HIGHSCORE_BG = "Backgrounds/highscore_bg";
        const string GFX_HIGHSCORE_TEXT = "Textures/highscores";
        const string GFX_NEWHIGHSCORE_TEXT = "Textures/newhighscore";
        const string GFX_TROPHYIMAGE = "Textures/trophy";
        const string GFX_ACONTINUE = "Textures/a-continue";
        
        public const string GFX_TITLE_LOGO = "Textures/guess_wurdz-title";
        public const string GFX_TITLE_BACKGROUND = "Backgrounds/title_background";

        public const string GFX_START_GUESSING_ACTIVE = "Textures/a_start_guessing";
        public const string GFX_EXIT_GUESSWURDZ_ACTIVE = "Textures/a_exit_guesswurdz";
        public const string GFX_HIGH_SCORES_ACTIVE = "Textures/a_high_scores";
        public const string GFX_HOW_TO_PLAY_ACTIVE = "Textures/a_how_to_play";
        public const string GFX_VIEWCREDITS_ACTIVE = "Textures/a_view_credits";

        public const string GFX_START_GUESSING = "Textures/start_guessing";
        public const string GFX_EXIT_GUESSWURDZ = "Textures/exit_guesswurdz";
        public const string GFX_HIGH_SCORES = "Textures/high_scores";
        public const string GFX_HOW_TO_PLAY = "Textures/how_to_play";
        public const string GFX_VIEWCREDITS = "Textures/view_credits";

        public const string GFX_TRIAL_PLAY_ACTIVE = "Textures/play_trial";
        public const string GFX_TRIAL_PLAY = "Textures/play_trial2";

        public const string GFX_TRIAL_HOWTOPLAY_ACTIVE = "Textures/thowtoplay";
        public const string GFX_TRIAL_HOWTOPLAY = "Textures/thowtoplay2";

        public const string GFX_TRIAL_PURCHASE_ACTIVE = "Textures/tpurchasegame";
        public const string GFX_TRIAL_PURCHASE = "Textures/tpurchasegame2";

        public const string GFX_TRIAL_EXIT_ACTIVE = "Textures/texitgame";
        public const string GFX_TRIAL_EXIT = "Textures/texitgame2";

     


        ContentManager content;
        //Texture2D backgroundTexture;
        Texture2D titleLogo;
        Texture2D titleBackground;
        Texture2D htpBackground;
        Texture2D gfx_credit_bg;
        Texture2D gfx_highscore_bg;
        Texture2D gfx_highscore_text;
        Texture2D gfx_newhighscore_text;
        Texture2D gfx_trophy;
        Texture2D gfx_breturn;
        Texture2D gfx_acontinue;
        Texture2D[] menu_option = new Texture2D[6];
        Texture2D[] menu_option_active = new Texture2D[6];

        Texture2D[] tmenu_option = new Texture2D[6];
        Texture2D[] tmenu_option_active = new Texture2D[6];
        public static StorageDevice StorageDevice = null;
 
        SpriteFont menuFont;
        SpriteFont gameFont;
        int menuPosition = 1;
        protected float scale = 1f;
        int direction = 0;
        float timer = 0f;
        float interval = 60.0f; // 60 milliseconds
        bool musicStart = true;
        bool highScoreLoaded = false;
       

  
      

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
        
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
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

            //backgroundTexture = content.Load<Texture2D>("Backgrounds/background");
            titleLogo = content.Load<Texture2D>(GFX_TITLE_LOGO);
            titleBackground = content.Load<Texture2D>(GFX_TITLE_BACKGROUND);
            menu_option_active[1] = content.Load<Texture2D>(GFX_START_GUESSING_ACTIVE);
            menu_option_active[2] = content.Load<Texture2D>(GFX_HOW_TO_PLAY_ACTIVE);
            menu_option_active[3] = content.Load<Texture2D>(GFX_HIGH_SCORES_ACTIVE);
            menu_option_active[4] = content.Load<Texture2D>(GFX_VIEWCREDITS_ACTIVE);
            menu_option_active[5] = content.Load<Texture2D>(GFX_EXIT_GUESSWURDZ_ACTIVE);

            menu_option[1] = content.Load<Texture2D>(GFX_START_GUESSING);
            menu_option[2] = content.Load<Texture2D>(GFX_HOW_TO_PLAY);
            menu_option[3] = content.Load<Texture2D>(GFX_HIGH_SCORES);
            menu_option[4] = content.Load<Texture2D>(GFX_VIEWCREDITS);
            menu_option[5] = content.Load<Texture2D>(GFX_EXIT_GUESSWURDZ);

            tmenu_option_active[1] = content.Load<Texture2D>(GFX_TRIAL_PLAY_ACTIVE);
            tmenu_option_active[2] = content.Load<Texture2D>(GFX_TRIAL_HOWTOPLAY_ACTIVE);
            tmenu_option_active[3] = content.Load<Texture2D>(GFX_TRIAL_PURCHASE_ACTIVE);
            tmenu_option_active[4] = content.Load<Texture2D>(GFX_TRIAL_EXIT);

            tmenu_option[1] = content.Load<Texture2D>(GFX_TRIAL_PLAY);
            tmenu_option[2] = content.Load<Texture2D>(GFX_TRIAL_HOWTOPLAY);
            tmenu_option[3] = content.Load<Texture2D>(GFX_TRIAL_PURCHASE);
            tmenu_option[4] = content.Load<Texture2D>(GFX_TRIAL_EXIT_ACTIVE);


            htpBackground = content.Load<Texture2D>(GFX_HTP_BACKGROUND);
            gfx_breturn = content.Load<Texture2D>(GFX_BRETURN);

            gfx_highscore_bg = content.Load<Texture2D>(GFX_HIGHSCORE_BG);
            gfx_highscore_text = content.Load<Texture2D>(GFX_HIGHSCORE_TEXT);
            gfx_newhighscore_text = content.Load<Texture2D>(GFX_NEWHIGHSCORE_TEXT);
            gfx_trophy = content.Load<Texture2D>(GFX_TROPHYIMAGE);
            gfx_acontinue = content.Load<Texture2D>(GFX_ACONTINUE);

            gfx_credit_bg = content.Load<Texture2D>(GFX_CREDITS_BACKGROUND);
          

            menuFont = content.Load<SpriteFont>("Fonts/gamefont");
            gameFont = content.Load<SpriteFont>("Fonts/highscore");

            if (musicStart)
            {
                GuessWurdz.cue = Sound.Play(SoundEntry.IntroMusic);
                musicStart = false;
            }

            
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

            switch (GuessWurdz.gameState)
            {
                case GuessWurdz.GameState.StartMenu:
                    {
                        
                        // The time since Update was called last.
                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timer > interval)
                        {

                            // TODO: Add your game logic here.
                            if (scale > 1.1) { direction = 2; }
                            if (scale < .9) { direction = 1; }

                            if (direction == 1)
                            {
                                scale += elapsed;
                                scale = scale % 6;
                            }
                            else
                            {
                                scale -= elapsed;
                                scale = scale % 6;
                            }
                            timer = 0f;
                        }

                    }
                    break;
                case GuessWurdz.GameState.ViewHighScores:
                    {
                        if (!highScoreLoaded)
                        {
                                HighScores.LoadHighScores();
                            
                            highScoreLoaded = true;
                        }

                    }
                    break;

                default:
                    break;
            }
           
             
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            switch (GuessWurdz.gameState)
            {
                case GuessWurdz.GameState.StartMenu:
                    {
                        spriteBatch.Begin(SpriteBlendMode.None);
                        // Set Screen To White
                        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                                           Color.White, 0, 0);

                        spriteBatch.End();
                        drawTitleScreen();

                    }
                    break;
                case GuessWurdz.GameState.HowToPlayMenu:
                    {
                        spriteBatch.Begin(SpriteBlendMode.None);
                        // Set Screen To White
                        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                                           Color.White, 0, 0);

                        spriteBatch.Draw(htpBackground, fullscreen,
                             new Color(fade, fade, fade));

                        spriteBatch.End();
                        displayBbutton(100, 650);
                    }
                    break;
                case GuessWurdz.GameState.ViewHighScores:
                    {
                        drawHighScore();
                    }
                    break;

                case GuessWurdz.GameState.ViewCredits:
                    {
                        spriteBatch.Begin(SpriteBlendMode.None);
                        // Set Screen To White
                        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                                           Color.White, 0, 0);

                        spriteBatch.Draw(gfx_credit_bg, fullscreen,
                            new Color(fade, fade, fade));

                        spriteBatch.End();
                        displayBbutton(100, 650);

                    }
                    break;

                default:
                    break;
            }

        }

        public override void HandleInput(InputState input)
        {
            switch (GuessWurdz.gameState)
            {
                case GuessWurdz.GameState.StartMenu:
                    {
                        if (input.IsNewKeyPress(Keys.Down) || input.IsNewButtonPress(Buttons.LeftThumbstickDown) || input.IsNewButtonPress(Buttons.DPadDown))
                        {
                      
                            moveMenu(MOVE_DOWN);

                        }
                        else if (input.IsNewKeyPress(Keys.Up) || input.IsNewButtonPress(Buttons.LeftThumbstickUp) || input.IsNewButtonPress(Buttons.DPadUp))
                        {
                            moveMenu(MOVE_UP);

                        }

                        if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                        {
                            moveMenu(MOVE_SELECT);
                        }


                    }
                    break;
                case GuessWurdz.GameState.HowToPlayMenu:
                    {
                        if (input.IsNewKeyPress(Keys.Escape) || input.IsNewButtonPress(Buttons.B))
                        {
                            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
                        }
                    }
                    break;
                case GuessWurdz.GameState.ViewHighScores:
                    {
                        if (input.IsNewKeyPress(Keys.Escape) || input.IsNewButtonPress(Buttons.B))
                        {
                            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
                        }

                    } break;
                case GuessWurdz.GameState.ViewCredits:
                    {
                        if (input.IsNewKeyPress(Keys.Escape) || input.IsNewButtonPress(Buttons.B))
                        {
                            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
                        }

                    } break;
                default:
                    break;
            }

            

        }

        private void moveMenu(int position)
        {
            int temp_move;
            int max_move;

            if (Guide.IsTrialMode)
            {
                max_move = 4;
            }
            else { max_move = 5; }

            if (position == MOVE_UP)
            {
                temp_move = menuPosition;
                temp_move -= 1;
                if (temp_move < 1) { temp_move = 1; };

                menuPosition = temp_move;

            }

            if (position == MOVE_DOWN)
            {
                temp_move = menuPosition;
                temp_move += 1;
                if (temp_move > max_move) { temp_move = max_move; };

                menuPosition = temp_move;
            }
            Sound.Play(SoundEntry.SFX_Click);
            if (position == MOVE_SELECT)
            {

                switch (menuPosition)
                {
                    case 1 :
                        if (Guide.IsTrialMode) // Play Game
                        {
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            GuessWurdz.gameType = GuessWurdz.GAMETYPE_NORMAL;
                            GuessWurdz.gameMode = GuessWurdz.GameMode.PlayingGame;
                            GuessWurdz.startOfGame = true;
                            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
                            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
                        }
                        else
                        {

                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            GuessWurdz.gameType = GuessWurdz.GAMETYPE_NORMAL;
                            GuessWurdz.gameMode = GuessWurdz.GameMode.PlayingGame;
                            GuessWurdz.startOfGame = true;
                            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
                            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
                        }
                        break;
                    case 2:
                        Sound.Play(SoundEntry.SFX_MenuSelect);
                        GuessWurdz.gameState = GuessWurdz.GameState.HowToPlayMenu;
                       // LoadingScreen.Load(ScreenManager, false, new HowToPlayScreen());
                        break;
                    case 3 :
                        if (Guide.IsTrialMode) // Purchase Game
                        {
                            if (Gamer.SignedInGamers[GuessWurdz.controllingPlayer].IsSignedInToLive)
                            {
                                Guide.ShowMarketplace(GuessWurdz.controllingPlayer);
                            }
                            else
                            {
                                Guide.ShowSignIn(1, true);
                            }
                        }
                        else
                        {
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            GuessWurdz.gameState = GuessWurdz.GameState.ViewHighScores;
                        }
                        break;
                    case 4 :
                        if (Guide.IsTrialMode) // exit game
                        {
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            Sound.Shutdown();
                            ScreenManager.Game.Exit();
                        }
                        else
                        {
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            GuessWurdz.gameState = GuessWurdz.GameState.ViewCredits;

                        }
                        break;
                    case 5:
                        Sound.Play(SoundEntry.SFX_MenuSelect);
                        Sound.Shutdown();
                        ScreenManager.Game.Exit();
                        break;

                    default:
                        break;


                }

            }


        }

        private void drawTitleScreen()
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Draw Background Screen
            spriteBatch.Begin();
            spriteBatch.Draw(titleBackground, new Rectangle(110, 75, 870, 650), new Color(255, 255, 255));
           
      
            
            spriteBatch.End();

            // Draw Title Logo
            drawTitleBlock(85, 70);
            if (Guide.IsTrialMode)
            {
                drawTrialMenu(600, 385);
            }
            else
            {
                drawMenu(600, 385);
            }

        }


        private void drawTitleBlock(int x, int y)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(titleLogo,new Rectangle(x,y,1033,358),new Color(255,255,255));
            spriteBatch.End();

        }

         private void drawTrialMenu(int x, int y)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Vector2 position = new Vector2(x, y);
            spriteBatch.Begin();

            for (int i = 1; i <= 4; i++)
            {
                if (menuPosition == i)
                {
                    spriteBatch.Draw(tmenu_option_active[i], new Vector2(x, y + ((i - 1) * 60) ), null,
                        Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(tmenu_option[i], new Vector2(x, y + ((i - 1) * 60)), new Color(255, 255, 255));

                }

            }
            spriteBatch.End();




        }

        private void drawMenu(int x, int y)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Vector2 position = new Vector2(x, y);
            spriteBatch.Begin();

            for (int i = 1; i <= 5; i++)
            {
                if (menuPosition == i)
                {
                    spriteBatch.Draw(menu_option_active[i], new Vector2(x, y + ((i - 1) * 60) ), null,
                        Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(menu_option[i], new Vector2(x, y + ((i - 1) * 60)), new Color(255, 255, 255));

                }

            }
           

            spriteBatch.End();
       



        }

        public void displayBbutton(int x, int y)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_breturn, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();

        }

        public void displayAbutton(int x, int y)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_acontinue, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();

        }

      

        private void drawHighScore()
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            int count = 0;
            int position;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                              Color.White, 0, 0);
            // Draw Backgrond
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_highscore_bg, fullscreen,
                             new Color(255, 255, 255));

            spriteBatch.Draw(gfx_highscore_text,
                new Rectangle(375, 140, 533, 62), new Color(255, 255, 255));

            // Draw Scores
            for (int i = 0; i <= (GuessWurdz.theHighScore.Count - 1); i++)
            {
                position = (count * 70);
                spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(295, 250 + position), Color.Black);
                spriteBatch.DrawString(gameFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(810, 250 + position), Color.Black);

                count++;
            }

            spriteBatch.End();
            displayBbutton(100, 650);

        }

        #endregion
    }

}
