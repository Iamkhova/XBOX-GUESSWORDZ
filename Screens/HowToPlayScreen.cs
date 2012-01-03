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
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class HowToPlayScreen : GameScreen
    {
        #region Fields

        
        public const string GFX_BACKGROUND = "Backgrounds/howtoplay";
        public const string GFX_BRETURN = "Textures/b-returnmenu";

        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D gfx_bReturn;
        

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HowToPlayScreen()
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

            backgroundTexture = content.Load<Texture2D>(GFX_BACKGROUND);
            gfx_bReturn = content.Load<Texture2D>(GFX_BRETURN);
           
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
                                               Color.White, 0, 0);

            spriteBatch.Draw(backgroundTexture, fullscreen,
                 new Color(fade, fade, fade));

            spriteBatch.End();
            displayBbutton(40, 650);

        }

        public override void HandleInput(InputState input)
        {
           

          
            if (input.IsNewKeyPress(Keys.Escape) || input.IsNewButtonPress(Buttons.B))
            {
                returnToMenu();
            }


        }

        private void returnToMenu()
        {
            LoadingScreen.Load(ScreenManager, false, new BackgroundScreen());

            //GuessWurdz.screenManager.AddScreen(new BackgroundScreen());
        }

        public void displayBbutton(int x, int y)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(gfx_bReturn, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();

        }

        #endregion
    }

}
