
#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
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
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        public struct ThinkBlock
        {
            public string line1;
            public string line2;
            public string SoundFx;
            public bool playSFX;
        }

        public const string GFX_LETTER_BOARD = "Textures/alphablock";
        public const string GFX_LETTERTILE_ACTIVE = "Textures/letterblock_active";
        public const string GFX_LETTERTILE_NORMAL = "Textures/letterblock_normal";
        public const string GFX_LETTERTILE_INACTIVE = "Textures/letterblock_inactive";
        public const string GFX_GAMETILE = "Textures/gameblocks";
        public const string GFX_GAMELETTER = "Textures/game_letter_blocks";
        public const string GFX_SCORE_BOARD = "Textures/score_board";
        public const string GFX_TITLE_BOARD = "Textures/titleboard";
        public const string GFX_SCORE_TEXT = "Textures/score";
        public const string GFX_CHANCES_TEXT = "Textures/chances";
        public const string GFX_ROUND_TEXT = "Textures/round";
        public const string GFX_NUMBERTILE_001 = "Textures/number09";
        public const string GFX_GAMEWON = "Textures/roundwon";
        public const string GFX_GAMELOST = "Textures/gameover";
        public const string GFX_ACONTINUE = "Textures/a-continue";
        public const string GFX_BOMBPIECE = "Textures/bomb_piece";
        public const string GFX_GOLDENBALL = "Textures/golden_ball";
        public const string GFX_THINKBLOCK = "Textures/think_block";
        public const string GFX_TRIALTEXT = "Textures/trial_mode";

        // High Score Screens
        public const string GFX_HIGHSCORE_BG = "Backgrounds/highscore_bg";
        public const string GFX_NEWHIGHSCORE_TEXT = "Textures/newhighscore";
        public const string GFX_TROPHYIMAGE = "Textures/trophy";
        public const string GFX_BRETURNMENU = "Textures/b-returnmenu";

        public const int LetterA = 0;
        public const int LetterB = 1;
        public const int LetterC = 2;
        public const int LetterD = 3;
        public const int LetterE = 4;
        public const int LetterF = 5;
        public const int LetterG = 6;
        public const int LetterH = 7;
        public const int LetterI = 8;
        public const int LetterJ = 9;
        public const int LetterK = 10;
        public const int LetterL = 11;
        public const int LetterM = 12;
        public const int LetterN = 13;
        public const int LetterO = 14;
        public const int LetterP = 15;
        public const int LetterQ = 16;
        public const int LetterR = 17;
        public const int LetterS = 18;
        public const int LetterT = 19;
        public const int LetterU = 20;
        public const int LetterV = 21;
        public const int LetterW = 22;
        public const int LetterX = 23;
        public const int LetterY = 24;
        public const int LetterZ = 25;

        public const int NUM_GFX_BACKGROUND = 10;
        public const int NUM_MUSIC_BACKGROUND = 4;

        // Game Backgrounds

        ContentManager content;
        public static SpriteFont gameFont;
        public static SpriteFont highScoreFont;
        public static SpriteFont gameFontSmall;
        public static SpriteFont scoreFont;
        public static SpriteFont snap_itc;
        public static SpriteFont thinkblockFont;
        public static Texture2D gfxLetterBoard;
        public static Texture2D gfxLetterTileActive;
        public static Texture2D gfxLetterTileNormal;
        public static Texture2D gfxLetterTileInactive;
        public static Texture2D gfxGameTile;
        public static Texture2D gfxGameLetter;
        public static Texture2D gfxGameBackground;
        public static Texture2D gfxScoreBoard;
        public static Texture2D gfxTitleBoard;
        public static Texture2D gfxScoreText;
        public static Texture2D gfxChancesText;
        public static Texture2D gfxRoundText;
        public static Texture2D gfxNumberTile001;
        public static Texture2D gfxRoundWon;
        public static Texture2D gfxGameLost;
        public static Texture2D gfxAContinue;
        public static Texture2D gfxBombTiles;
        public static Texture2D gfxGoldenBall;
        public static Texture2D gfxThinkBlock;
        public static Texture2D gfxTrialModeText;

        public static Texture2D gfx_highscore_bg;
        public static Texture2D gfx_newhighscore_text;
        public static Texture2D gfx_trophy;
        public static Texture2D gfx_breturnmenu;
        HighScores highScore = new HighScores();
        bool scoreUpdated = false;
        bool musicChecked = false;
        public static bool highScoreFound = false;
       

       

        float timer = 0f;
        float interval = 2000.0f; // 60 milliseconds
        // ThinkBlock timer
        float tb_timer = 0f;
        float tb_interval = 5000.0f;
        public static bool show_thinkblock = false;
        public static ThinkBlock thinkBlocktext;
        string backgroundMusic;
        bool isMusicPlaying = false;
     

        //Initalize Game Variable
        public static GuessWurdzGame game = new GuessWurdzGame();
        
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            GuessWurdz.gameState = GuessWurdz.GameState.GameStarted;
            game.initGame();

          // GuessWurdz.gameState = GuessWurdz.GameState.GameStarted;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("Fonts/gamefont");
            gameFontSmall = content.Load<SpriteFont>("Fonts/gamefont_small");
            scoreFont = content.Load<SpriteFont>("Fonts/scorefont");
            snap_itc = content.Load<SpriteFont>("Fonts/snap_itc");
            thinkblockFont = content.Load<SpriteFont>("Fonts/thinkblockfont");
            highScoreFont = content.Load<SpriteFont>("Fonts/highscore");

            gfxLetterBoard = content.Load<Texture2D>(GFX_LETTER_BOARD);
            gfxLetterTileActive = content.Load<Texture2D>(GFX_LETTERTILE_ACTIVE);
            gfxLetterTileNormal = content.Load<Texture2D>(GFX_LETTERTILE_NORMAL);
            gfxLetterTileInactive = content.Load<Texture2D>(GFX_LETTERTILE_INACTIVE);
          
            gfxGameTile = content.Load<Texture2D>(GFX_GAMETILE);
            gfxGameLetter = content.Load<Texture2D>(GFX_GAMELETTER);
            gfxScoreBoard = content.Load<Texture2D>(GFX_SCORE_BOARD);
            gfxTitleBoard = content.Load<Texture2D>(GFX_TITLE_BOARD);
            gfxScoreText = content.Load<Texture2D>(GFX_SCORE_TEXT);
            gfxChancesText = content.Load<Texture2D>(GFX_CHANCES_TEXT);
            gfxRoundText = content.Load<Texture2D>(GFX_ROUND_TEXT);
            gfxNumberTile001 = content.Load<Texture2D>(GFX_NUMBERTILE_001);
            gfxRoundWon = content.Load<Texture2D>(GFX_GAMEWON);
            gfxGameLost = content.Load<Texture2D>(GFX_GAMELOST);
            gfxAContinue = content.Load<Texture2D>(GFX_ACONTINUE);
            gfxBombTiles = content.Load<Texture2D>(GFX_BOMBPIECE);
            gfxGoldenBall = content.Load<Texture2D>(GFX_GOLDENBALL);
            gfxThinkBlock = content.Load<Texture2D>(GFX_THINKBLOCK);
            gfxTrialModeText = content.Load<Texture2D>(GFX_TRIALTEXT);

            gfx_newhighscore_text = content.Load<Texture2D>(GFX_NEWHIGHSCORE_TEXT);
            gfx_trophy = content.Load<Texture2D>(GFX_TROPHYIMAGE);
            gfx_breturnmenu = content.Load<Texture2D>(GFX_BRETURNMENU);
            gfx_highscore_bg = content.Load<Texture2D>(GFX_HIGHSCORE_BG);
  
            

       
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
            playBGMusic();
            thinkBlocktext.playSFX = false;
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            const float randomization = 1;
            Random random = new Random();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);


            if (IsActive)
            {
               
                // HANDEL GAME LOOP

                switch (GuessWurdz.gameState)
                {
                    case GuessWurdz.GameState.GameStarted:
                        {
                            // Apply some random jitter to make the enemy move around.

                            game.bombJitter.X = (float)(random.NextDouble() - 0.5) * randomization;
                            game.bombJitter.Y = (float)(random.NextDouble() - 0.5) * randomization;

                            //Check Player Life
                      
                            game.CheckLife();

                            if (show_thinkblock)
                            {
                                //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                                tb_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (tb_timer > tb_interval)
                                {
                                    show_thinkblock = false;
                                    tb_timer = 0f;
                                }
                            }

                            if (game.isGameWon())
                            {
                                game.wonLevel();
                            }

                            if (!musicChecked)
                            {
                                if (game.reloadBGMusic())
                                {
                                    playBGMusic();
                                    musicChecked = true;
                                }
                            }

                            if (!isMusicPlaying)
                            {
                                playBGMusic();
                                isMusicPlaying = true;
                            }
     
                        }
                        break;
                    case GuessWurdz.GameState.GameWon:
                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (timer > interval)
                        {
                            game.newLevel();
                            timer = 0f;
                        }

                        break;
                    case GuessWurdz.GameState.GameLost:
                        {
                            // clear Think Block
                            show_thinkblock = false;
                            // Check For High Score
                            highScoreFound = game.isHighScore();
                        }
                        break;
                    case GuessWurdz.GameState.HandelHighScore:
                        {
                            if (GuessWurdz.oskb.ExitState > 0)
                            {
                                GuessWurdz.oskb.Visible = false;
                                GuessWurdz.oskb.Enabled = false;
                                
                                if (GuessWurdz.oskb.Text.Length == 0)
                                {
                                    // Default if user aborts out
                                    GuessWurdz.oskb.Text = "Barack Obama";
                                }
                                // Save High Score

                                game.saveHighScore();
                                
                                GuessWurdz.gameState = GuessWurdz.GameState.ViewHighScores;
                            }
                        }
                        break;
                    case GuessWurdz.GameState.ViewHighScores:
                        {
                            if (!scoreUpdated)
                            {
                                
                                    HighScores.LoadHighScores();
                              
                                scoreUpdated = true;
                            }

                        }
                        break;
                    default:
                        break;

                }

            


     
               
            }
        }



        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            bool keycheck = false;

            if (input == null)
                throw new ArgumentNullException("input");




            switch (GuessWurdz.gameState)
            {
                case GuessWurdz.GameState.GameStarted:
                    {

                        if (input.PauseGame)
                        {
                            // If they pressed pause, bring up the pause menu screen.
                            stopBGMusic();
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            GuessWurdz.gameState = GuessWurdz.GameState.GamePause;
                            isMusicPlaying = false;
                            ScreenManager.AddScreen(new PauseMenuScreen());
                        }

                        // Move Down

                        
                        if (input.IsNewKeyPress(Keys.Down) ||  input.IsNewButtonPress(Buttons.LeftThumbstickDown) || input.IsNewButtonPress(Buttons.DPadDown))
                        {
                            game.moveActiveDown();
                            keycheck = true;

                        }
                        // Move Up
                        else if (input.IsNewKeyPress(Keys.Up) || input.IsNewButtonPress(Buttons.LeftThumbstickUp) || input.IsNewButtonPress(Buttons.DPadUp))
                        {
                            game.moveActiveUp();
                            keycheck = true;

                        }
                        // Move Right
                        else if (input.IsNewKeyPress(Keys.Right) || input.IsNewButtonPress(Buttons.LeftThumbstickRight) || input.IsNewButtonPress(Buttons.DPadRight))
                        {
                            game.moveActiveRight();
                            keycheck = true;

                        }
                        // Move Left
                        else if (input.IsNewKeyPress(Keys.Left) || input.IsNewButtonPress(Buttons.LeftThumbstickLeft) || input.IsNewButtonPress(Buttons.DPadLeft))
                        {
                            game.moveActiveLeft();
                            keycheck = true;

                        }

                        if (keycheck == true)
                        {
                            game.setLetterActivePosition();
                            Sound.Play(SoundEntry.SFX_Click);
                        }

                        // Check for "Enter" Button Pressed
                        if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                        {
                            game.chooseLetter(GuessWurdz.AUTO);
                        }

                     
                        if (input.IsNewKeyPress(Keys.F1) || input.IsNewButtonPress(Buttons.Y))
                        {
                              game.ballActivated(0);
                        }

                        if (GuessWurdz.gameDebug)
                        {
                            if (input.IsNewKeyPress(Keys.F5))
                            {
                                game.ballActivated(10);
                            }

                            if (input.IsNewKeyPress(Keys.F2) || (input.IsNewButtonPress(Buttons.RightShoulder) && input.IsNewButtonPress(Buttons.LeftTrigger)))
                            {
                                game.ballActivated(999);
                            }

                            if (input.IsNewKeyPress(Keys.F3))
                            {
                                game.cheat_takelife();
                            }

                            if (input.IsNewKeyPress(Keys.F4) || (input.IsNewButtonPress(Buttons.X)))
                            {
                                game.cheat_addpoints();
                            }

                            if (input.IsNewKeyPress(Keys.F12))
                            {

                                // game.stop_bgmusic();
                                GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
                                bool test = GuessWurdz.cue.IsStopping;
                                bool test2 = GuessWurdz.cue.IsStopped;

                            }

                            if (input.IsNewKeyPress(Keys.F11))
                            {
                                stopBGMusic();
                                playBGMusic();
                            }
                        }
                        // ChatPad KeyBoard Support

                        if (input.IsNewKeyPress(Keys.A)) { game.chooseLetter(LetterA); }
                        if (input.IsNewKeyPress(Keys.B)) { game.chooseLetter(LetterB); }
                        if (input.IsNewKeyPress(Keys.C)) { game.chooseLetter(LetterC); }
                        if (input.IsNewKeyPress(Keys.D)) { game.chooseLetter(LetterD); }
                        if (input.IsNewKeyPress(Keys.E)) { game.chooseLetter(LetterE); }
                        if (input.IsNewKeyPress(Keys.F)) { game.chooseLetter(LetterF); }
                        if (input.IsNewKeyPress(Keys.G)) { game.chooseLetter(LetterG); }
                        if (input.IsNewKeyPress(Keys.H)) { game.chooseLetter(LetterH); }
                        if (input.IsNewKeyPress(Keys.I)) { game.chooseLetter(LetterI); }
                        if (input.IsNewKeyPress(Keys.J)) { game.chooseLetter(LetterJ); }
                        if (input.IsNewKeyPress(Keys.K)) { game.chooseLetter(LetterK); }
                        if (input.IsNewKeyPress(Keys.L)) { game.chooseLetter(LetterL); }
                        if (input.IsNewKeyPress(Keys.M)) { game.chooseLetter(LetterM); }
                        if (input.IsNewKeyPress(Keys.N)) { game.chooseLetter(LetterN); }
                        if (input.IsNewKeyPress(Keys.O)) { game.chooseLetter(LetterO); }
                        if (input.IsNewKeyPress(Keys.P)) { game.chooseLetter(LetterP); }
                        if (input.IsNewKeyPress(Keys.Q)) { game.chooseLetter(LetterQ); }
                        if (input.IsNewKeyPress(Keys.R)) { game.chooseLetter(LetterR); }
                        if (input.IsNewKeyPress(Keys.S)) { game.chooseLetter(LetterS); }
                        if (input.IsNewKeyPress(Keys.T)) { game.chooseLetter(LetterT); }
                        if (input.IsNewKeyPress(Keys.U)) { game.chooseLetter(LetterU); }
                        if (input.IsNewKeyPress(Keys.V)) { game.chooseLetter(LetterV); }
                        if (input.IsNewKeyPress(Keys.W)) { game.chooseLetter(LetterW); }
                        if (input.IsNewKeyPress(Keys.X)) { game.chooseLetter(LetterX); }
                        if (input.IsNewKeyPress(Keys.Y)) { game.chooseLetter(LetterY); }
                        if (input.IsNewKeyPress(Keys.Z)) { game.chooseLetter(LetterZ); }

                    }
                    break;
                case GuessWurdz.GameState.GameWon:
                    {
                        if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                        {
                            game.newLevel();
                        }

                    }
                    break;
                case GuessWurdz.GameState.GameLost:
                    {
                        if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                        {
                            game.gameLost();
                        }

                    }
                    break;
                case GuessWurdz.GameState.ViewHighScores:
                    {
                        if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
                        {
                           // ScreenManager.AddScreen(new BackgroundScreen());
                            Sound.Play(SoundEntry.SFX_MenuSelect);
                            ScreenManager.AddScreen(new EndGameMenuScreen());

                            
                        }

                    }
                    break;


                default:
                    break;
            }

    
        }

        public string selectBGMusic()
        {
            int randomNumber;
            string filePath, musicNum;
            Functions function = new Functions();

            randomNumber = function.RandomNumber(1, GameplayScreen.NUM_MUSIC_BACKGROUND +1);
            filePath = "bg_music_";
            musicNum = randomNumber.ToString();

            filePath = filePath + musicNum;

            return filePath;

        }

        public void playBGMusic()
        {
            if (!isMusicPlaying)
            {
                backgroundMusic = selectBGMusic();
                GuessWurdz.cue = Sound.Play(backgroundMusic);
                isMusicPlaying = true;
            }
        }

        public void stopBGMusic()
        {
            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
            isMusicPlaying = false;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            
     /*      
            if (GuessWurdz.oskb.Visible == true)
            {
                if (GuessWurdz.oskb.ExitState == 99)
                {
                    GuessWurdz.oskb.Visible = false;
                    GuessWurdz.oskb.Enabled = false;
                }

            }
            else
            {
      */
             


                //Draw Screen

                switch (GuessWurdz.gameState)
                {
                    case GuessWurdz.GameState.GameStarted:
                        {
                            displayScreen(0);
                        }
                        break;
                    case GuessWurdz.GameState.GameLost:
                        {
                            // Draw Backgrond

                            displayScreen(1);
                            
                        }
                        break;

                    case GuessWurdz.GameState.GameWon:
                        {// Draw Backgrond

                            displayScreen(0);
                            ScreenManager.FadeBackBufferToBlack(50);
                            //game.displayAbutton(40, 650);
                        }
                        break;
                    case GuessWurdz.GameState.HandelHighScore:
                        {
                            GuessWurdz.oskb.Visible = true;
                            GuessWurdz.oskb.Enabled = true;
                            //GuessWurdz.oskb.Text = GuessWurdz.gamer.Gamertag
                            /*
                            if (Gamer.SignedInGamers[GuessWurdz.controllingPlayer].IsSignedInToLive)
                            {
                                GuessWurdz.oskb.Text = Gamer.SignedInGamers[GuessWurdz.controllingPlayer].Gamertag;
                            }
                             */
                            GuessWurdz.oskb.Prompt = "Enter Name for New High Score";
                            

                            
                        }
                        break;
                    case GuessWurdz.GameState.ViewHighScores:
                        {
                            if (scoreUpdated)
                            {
                                displayHighScoreScreen();
                            }
                        }
                        break;
                    default:
                        break;
                }


                // If the game is transitioning on or off, fade it out to black.
                if (TransitionPosition > 0)
                    ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

        }

        private void displayHighScoreScreen()
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
                             new Color(255, 255, 255));
            spriteBatch.End();

            game.DrawHighScoreScreen();


        }

        private void displayScreen(int type)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                   Color.Black, 0, 0);

                // Draw Backgrond
                spriteBatch.Begin(SpriteBlendMode.None);
                spriteBatch.Draw(gfxGameBackground, fullscreen,
                                 new Color(fade, fade, fade));
                spriteBatch.End();

                if (type == 1)
                {
                    game.displayGameOverScreen(220, 50);
                    game.displayAbutton(100, 650);
                }
                game.DisplayGameBoard(100, 475);
                game.DisplayTitle(230, 380);
                if (type != 1)
                {
                    game.DisplayLetterBoard(50, 50);
                    game.DisplayScoreBoard(830, 80);
                    game.DisplayGoldenBalls(870, 345);
                }

                if (show_thinkblock)
                {
                    game.display_thinkblock(180, 20, 1, 0);
                }

        }
        #endregion
    }
}
