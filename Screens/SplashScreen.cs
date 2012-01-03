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
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class SplashScreen : GameScreen
    {
        #region Fields

        
        public const string GFX_SPLASH = "Backgrounds/splashscreen";
        public const string GFX_ACONTINUE = "Textures/a-continue";

        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D gfxAContinue;
        
        //float interval = 5000.0f; // 60 milliseconds
        float timer = 0.0f;
        bool playEffect = true;

       

      

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public SplashScreen()
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

            backgroundTexture = content.Load<Texture2D>(GFX_SPLASH);
            gfxAContinue = content.Load<Texture2D>(GFX_ACONTINUE);
           
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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            

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

            spriteBatch.Begin(SpriteBlendMode.None);
            // Set Screen To White
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            spriteBatch.Draw(backgroundTexture, fullscreen,
                 new Color(fade, fade, fade));

            if (playEffect)
            {
                Sound.Play(SoundEntry.SFXLogo);
                playEffect = false;
            }

            spriteBatch.Draw(gfxAContinue, new Vector2(70, 650), new Color(255, 255, 255));

            spriteBatch.End();

        }

        public override void HandleInput(InputState input)
        {
           

          
            if (input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
            {
                GuessWurdz.setActiveControler();
                
                startgame();
            }


        }



        private void startgame()
        {
            LoadingScreen.Load(ScreenManager, false, new BackgroundScreen());
            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
            

            //GuessWurdz.screenManager.AddScreen(new BackgroundScreen());
        }

    

        #endregion
    }

}
