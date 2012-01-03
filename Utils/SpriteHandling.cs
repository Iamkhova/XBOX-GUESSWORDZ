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
    #region Sprite handling
    public class SpriteHandling
    {

        /// <summary>
        /// Sprite to render
        /// </summary>
        class SpriteToRender
        {

            /// <summary>
            /// Texture
            /// </summary>
            public Texture2D texture;
            /// <summary>
            /// Rectangle
            /// </summary>
            public Rectangle rect;
            /// <summary>
            /// Source rectangle
            /// </summary>
            public Rectangle? sourceRect;
            /// <summary>
            /// Color
            /// </summary>
            public Color color;

            /// <summary>
            /// Create sprite to render
            /// </summary>
            /// <param name="setTexture">Set texture</param>
            /// <param name="setRect">Set rectangle</param>
            /// <param name="setSourceRect">Set source rectangle</param>
            /// <param name="setColor">Set color</param>
            public SpriteToRender(Texture2D setTexture, Rectangle setRect,
                Rectangle? setSourceRect, Color setColor)
            {
                texture = setTexture;
                rect = setRect;
                sourceRect = setSourceRect;
                color = setColor;
            } // SpriteToRender(setTexture, setRect, setSourceRect)
        } // class SpriteToRender

        /// <summary>
        /// Sprites
        /// </summary>
        List<SpriteToRender> sprites = new List<SpriteToRender>();
        /// <summary>
        /// Sprite batch
        /// </summary>
       // SpriteBatch spriteBatch = null;

        /// <summary>
        /// Render sprite
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="sourceRect">Source rectangle</param>
        /// <param name="color">Color</param>
        public void RenderSprite(Texture2D texture, Rectangle rect, Rectangle? sourceRect,
            Color color)
        {
            sprites.Add(new SpriteToRender(texture, rect, sourceRect, color));
            /// <summary>
            /// Render sprite
            /// </summary>
            /// <param name="texture">Texture</param>
            /// <param name="rect">Rectangle</param>
            /// <param name="sourceRect">Source rectangle</param>
        } // RenderSprite(texture, rect, sourceRect)

        /// <summary>
        /// Render sprite
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="sourceRect">Source rectangle</param>
        /// <param name="color">Color</param>
        public void RenderSprite(Texture2D texture, Rectangle rect, Rectangle? sourceRect)
        {
            RenderSprite(texture, rect, sourceRect, Color.White);
        } // RenderSprite(texture, rect, sourceRect)

        public void RenderSprite(Texture2D texture, int x, int y, Rectangle? sourceRect,
            Color color)
        {
            /// <summary>
            /// Render sprite
            /// </summary>
            /// <param name="texture">Texture</param>
            /// <param name="x">X</param>
            /// <param name="y">Y</param>
            /// <param name="sourceRect">Source rectangle</param>
            RenderSprite(texture,
                new Rectangle(x, y, sourceRect.Value.Width, sourceRect.Value.Height),
                sourceRect, color);
        } // RenderSprite(texture, x, y)

        public void RenderSprite(Texture2D texture, int x, int y, Rectangle? sourceRect)
        {
            /// <summary>
            /// Render sprite
            /// </summary>
            /// <param name="texture">Texture</param>
            /// <param name="rect">Rectangle</param>
            /// <param name="color">Color</param>
            RenderSprite(texture,
                new Rectangle(x, y, sourceRect.Value.Width, sourceRect.Value.Height),
                sourceRect, Color.White);
        } // RenderSprite(texture, x, y)

        /// <summary>
        /// Render sprite
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <param name="rect">Rectangle</param>
        public void RenderSprite(Texture2D texture, Rectangle rect, Color color)
        {
            RenderSprite(texture, rect, null, color);
        } // RenderSprite(texture, rect, color)

        /// <summary>
        /// Render sprite
        /// </summary>
        /// <param name="texture">Texture</param>
        public void RenderSprite(Texture2D texture, Rectangle rect)
        {
            RenderSprite(texture, rect, null, Color.White);
        } // RenderSprite(texture, rect)

        public void RenderSprite(Texture2D texture)
        {
            RenderSprite(texture, new Rectangle(0, 0, 1024, 768), null, Color.White);
        } // RenderSprite(texture)

        /// <summary>
        /// Draw sprites
        /// </summary>
        public void DrawSprites()
        {
            int width, height;

            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            width = GuessWurdz.graphics.PreferredBackBufferWidth;
            height = GuessWurdz.graphics.PreferredBackBufferHeight;
            // No need to render if we got no sprites this frame
            if (sprites.Count == 0)
                return;

            // Start rendering sprites
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.BackToFront, SaveStateMode.None);


            // Render all sprites
            foreach (SpriteToRender sprite in sprites)
                spriteBatch.Draw(sprite.texture,
                    // Rescale to fit resolution
                    new Rectangle(
                    sprite.rect.X * width / 1024,
                    sprite.rect.Y * height / 768,
                    sprite.rect.Width * width / 1024,
                    sprite.rect.Height * height / 768),
                    sprite.sourceRect, sprite.color);

            // We are done, draw everything on screen with help of the end method.
            spriteBatch.End();

            // Kill list of remembered sprites
            sprites.Clear();
        } // DrawSprites()
     #endregion
    }
}
