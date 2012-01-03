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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using OSKB;

namespace GuessWurdz
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GuessWurdz : Microsoft.Xna.Framework.Game
    {
        //****** DEFINE CONSTANTS
        public const int BOARD_ROW_LENGTH = 14;
        public const int BOARD_NUM_OF_ROWS = 4;
        public const int LETTER_BLOCK_SIZEX = 45;
        public const int LETTER_BLOCK_SIZEY = 45;

        public const int GAMETYPE_NORMAL = 1;
        public const int GAMETYPE_TIMEMODE = 2;
        public const int GAMETYPE_FREESTYLE = 3;




        //***********************
        
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static OnscreenKeyboard oskb;
        TestUnits testUnits = new TestUnits();
        public static ScreenManager screenManager;
        public static ContentManager content;
        public static GameState gameState;
        public static GameMode gameMode;
        public static GameAction gameAction;
        public static int gameType;
        public static int gscore;
        public static int glevel;
        public static HighScores.HighScoreData theHighScore;
        public static bool[] gamePhrase = new bool[2000];
        public static int AUTO = 99;
        public static bool newLevelStart = false;
        public static bool startOfGame = true;
        public static Cue cue;
        public static PlayerIndex controllingPlayer = PlayerIndex.One;
        public static bool controllerConfirmed = false;
        public static bool gameDebug = false;



        /// <summary>
        /// The storage device that the game is saving high-scores to.
        /// </summary>
        public static StorageDevice StorageDevice = null;
        

        

        public GuessWurdz()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;


            Content.RootDirectory = "Content";

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            this.Components.Add(new GamerServicesComponent(this));


            Components.Add(screenManager);

            // Activate Screens
            screenManager.AddScreen(new SplashScreen());
            //screenManager.AddScreen(new MainMenuScreen());

            //Guide.SimulateTrialMode = true;
            
           

      
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // ON Screen Keyboard App
            oskb = new OnscreenKeyboard(this);
            oskb.Visible = false;
            oskb.Enabled = false;
            oskb.DrawOrder = 200;
            oskb.UpdateOrder = 200;
            this.Components.Add(oskb);

            //HighScore Data
            setDefaultHighScore();
            setPhrase();

           



            Sound.Initialize();

            base.Initialize();
        }

      

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            content = new ContentManager(GuessWurdz.screenManager.Game.Services, "Content");



            // TODO: use this.Content to load your game content here
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
            graphics.GraphicsDevice.Clear(Color.Black);



            // TODO: Add your drawing code here
 

            base.Draw(gameTime);
        }

        private void setPhrase()
        {

            for (int i = 0; i < 2000; i++)
            {
                gamePhrase[i] = false;
            }
        }

        public static void setActiveControler()
        {

            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).Buttons.A == ButtonState.Pressed)
                {
                    controllingPlayer = index;
                    break;
                }
            }


        }
        public static void setDefaultHighScore()
        {
            GuessWurdz.theHighScore.PlayerName = new string[5];
            GuessWurdz.theHighScore.Score = new int[5];
            GuessWurdz.theHighScore.Level = new int[5];
            GuessWurdz.theHighScore.Count = 5;
            GuessWurdz.theHighScore.PlayerName[0] = "Carmen";
            GuessWurdz.theHighScore.Score[0] = 50000;
            GuessWurdz.theHighScore.Level[0] = 1;
            GuessWurdz.theHighScore.PlayerName[1] = "Chanel";
            GuessWurdz.theHighScore.Score[1] = 10000;
            GuessWurdz.theHighScore.Level[1] = 1;
            GuessWurdz.theHighScore.PlayerName[2] = "Owen";
            GuessWurdz.theHighScore.Score[2] = 5000;
            GuessWurdz.theHighScore.Level[2] = 1;
            GuessWurdz.theHighScore.PlayerName[3] = "Duane";
            GuessWurdz.theHighScore.Score[3] = 1000;
            GuessWurdz.theHighScore.Level[3] = 1;
            GuessWurdz.theHighScore.PlayerName[4] = "Stephen";
            GuessWurdz.theHighScore.Score[4] = 10;
            GuessWurdz.theHighScore.Level[4] = 1;

        }

       

        public enum GameMode
        {
            None,

            PlayingGame,

            ViewingHighScore,

            ViewingHowToPlay,
        }
        public enum GameAction
        {
            None,

            ShowThinkBlock,

        }
        /// <summary>
        /// This enum is for the state transitions.
        /// </summary>
        public enum GameState
        {
            /// <summary>
            /// Default value - means no state is set
            /// </summary>
            None,

            /// <summary>
            /// Nothing visible, game has just been run and nothing is initialized
            /// </summary>
            Started,

            /// <summary>
            /// Logo Screen is being displayed
            /// </summary>
            LogoSplash,

            StartMenu,

            HowToPlayMenu,

            /// <summary>
            /// Currently playing the 2d version
            /// </summary>
            DrawGameMenu,

            GameStarted,

            GameInit,

            GameWon,

            RevealBoard,

            GameLost,

            GameLostBomb,

            HandelHighScore,

            GameEnd,

            ViewHighScores,

            GamePause,

            ViewCredits,

        }
    }
}
