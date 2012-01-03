#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
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
using Microsoft.Xna.Framework.Audio;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization
        public const string GFX_BACKGROUND = "Backgrounds/pause_screen";
        ContentManager content;
        Texture2D gfxPauseScreen;




        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("")
        {
            // Flag that there is no need for the game to transition
            // off when the pause menu is on top of it.
            IsPopup = true;

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Return To Menu");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, EventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            GuessWurdz.newLevelStart = false;
            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
            LoadingScreen.Load(ScreenManager, false, new BackgroundScreen());
                                                   
        }

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");
            gfxPauseScreen = content.Load<Texture2D>(GFX_BACKGROUND);

        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            byte fade = TransitionAlpha;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 3/4);

            spriteBatch.Begin();
            // Set Screen To White
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);

            spriteBatch.Draw(gfxPauseScreen, fullscreen,
                 new Color(255, 255,255));
            
            spriteBatch.End();

            GameplayScreen.game.display_stats(800,330);



            base.Draw(gameTime);
        }


        #endregion
    }
}
