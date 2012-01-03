using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace GuessWurdz 
{
    public class TestUnits : SpriteHandling
    {
        /// <summary>
        /// Run Test on Loading Keyboard
        /// </summary>
        public void showKeyboard() 
        {

            GuessWurdz.oskb.Visible = true;
            GuessWurdz.oskb.Enabled = true;
            GuessWurdz.oskb.Prompt = "Enter Name for High Score";
           
        }


        /// <summary>
        /// Screen Testing
        /// </summary>
        public void showBackground()
        {


            GuessWurdz.screenManager.AddScreen(new BackgroundScreen());
            GuessWurdz.screenManager.AddScreen(new MainMenuScreen());


        }

        /// <summary>
        /// XML Testing
        /// </summary>
        public void savexml()
        {
            LoadPhrases phrase = new LoadPhrases();

            phrase.SaveThePhrase();



        }

        /// <summary>
        /// Draw Sprites
        /// </summary>
        public void drawsprite()
        {
            ContentManager content;
            SpriteFont gameFont;
            
                content = new ContentManager(GuessWurdz.screenManager.Game.Services, "Content");

            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("Fonts/gamefont");
            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, "Chances Left: ", new Vector2(10, 10), Color.Blue);
            spriteBatch.End();
        }

        public void gamescript()
        {
            GuessWurdzGame game;
            game = new GuessWurdzGame();

            game.initGame();


        }
    


    }
}
