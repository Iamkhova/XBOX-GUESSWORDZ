#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
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
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class EndGameMenuScreen : MenuScreen
    {
        #region Initialization

        public static Texture2D gfxGameLost;
        public static Texture2D gfxPurchase;
        public const string GFX_GAMELOST = "Textures/gameover";
        public const string GFX_PURCHASEGAME = "Textures/full_verison";
        ContentManager content;
        bool guideShown = false;


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public EndGameMenuScreen()
            : base("")
        {
            IsPopup = true;
            // Create our menu entries.

            MenuEntry playAgainMenuEntry = new MenuEntry("Play Again");
            MenuEntry purchaseGameMenuEntry = new MenuEntry("Purchase Full Game");
            MenuEntry returnMenuEntry = new MenuEntry("Return To Menu");
            MenuEntry exitMenuEntry = new MenuEntry("Exit to XBOX Live");

            

  

            // Hook up menu event handlers.
            purchaseGameMenuEntry.Selected += PurchaseGameMenuEntrySelected;
            playAgainMenuEntry.Selected += PlayAgainMenuEntrySelected;
            returnMenuEntry.Selected += ReturnMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            if (Guide.IsTrialMode)
            {
                MenuEntries.Add(purchaseGameMenuEntry);
            }
            else
            {
                MenuEntries.Add(playAgainMenuEntry);
            }
            MenuEntries.Add(returnMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayAgainMenuEntrySelected(object sender, EventArgs e)
        {
            GuessWurdz.gameType = GuessWurdz.GAMETYPE_NORMAL;
            GuessWurdz.newLevelStart = false;
            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);  
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());

        }

        void PurchaseGameMenuEntrySelected(object sender, EventArgs e)
        {
            GuessWurdz.gameType = GuessWurdz.GAMETYPE_NORMAL;
            GuessWurdz.newLevelStart = false;
            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
            if (!guideShown)
            {
                if (Gamer.SignedInGamers[GuessWurdz.controllingPlayer].IsSignedInToLive)
                {
                    guideShown = true;
                    Guide.ShowMarketplace(GuessWurdz.controllingPlayer);
                    

                }
                if (!Gamer.SignedInGamers[GuessWurdz.controllingPlayer].IsSignedInToLive)
                {
                    Guide.ShowSignIn(1, true);
                }
            }

            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
            LoadingScreen.Load(ScreenManager, true, new BackgroundScreen());


        }

        void ReturnMenuEntrySelected(object sender, EventArgs e)
        {
            GuessWurdz.newLevelStart = false;
            GuessWurdz.cue.Stop(AudioStopOptions.Immediate);
            GuessWurdz.gameState = GuessWurdz.GameState.StartMenu;
            LoadingScreen.Load(ScreenManager, false, new BackgroundScreen());

        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit GuessWurdz?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            Sound.Shutdown();
            ScreenManager.Game.Exit();
        }

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");
            gfxGameLost = content.Load<Texture2D>(GFX_GAMELOST);
            gfxPurchase = content.Load<Texture2D>(GFX_PURCHASEGAME);

        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                 Color.White, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            if (!Guide.IsTrialMode)
            {
              //  GameplayScreen.game.display_stats(250, 100);
               
            }

            spriteBatch.Begin();
            spriteBatch.Draw(gfxGameLost, new Vector2(220, 300), new Color(255, 255, 255));
            if (Guide.IsTrialMode)
            {
                spriteBatch.Draw(gfxPurchase, new Vector2(40, 40), new Color(255, 255, 255));
            }
            //  spriteBatch.DrawString(GameplayScreen.gameFont, "GOOD JOB!", new Vector2(200, 200), Color.Blue);
            spriteBatch.End();


            base.Draw(gameTime);
        }

       


        #endregion


    }
}
