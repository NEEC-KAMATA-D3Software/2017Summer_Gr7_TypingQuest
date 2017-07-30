using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TypingQuest.Device
{
    class Renderer
    {
        GraphicsDevice graphicsDevice;
        ContentManager contentManager;

        SpriteBatch spriteBatch;
        SpriteFont font;

        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Renderer(GraphicsDevice graphics, ContentManager content)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }
        public void LoadFontContent()
        {
            font = contentManager.Load<SpriteFont>("TestFont");
        }
        public void LoadTexture(string name, string filepath = "./")
        {
            //Dictionaryへ二重登録を回避
            if (textures.ContainsKey(name))
            {
#if DEBUG // DEBUGモードの時のみ有効
                System.Console.WriteLine("この" + name + "はKeyで、すでに登録しています");
#endif
                return;
            }
            textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        public void LoadTexture(string name, Texture2D texture)
        {
            if (textures.ContainsKey(name))
            {
#if DEBUG
                System.Console.WriteLine("この" + name + "はkeyで、すでに登録されています");
#endif
                return;
            }
            textures.Add(name, texture);
        }

        public void DrawTexture(string name, Vector2 position, Vector2 scale, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                null,
                Color.White * alpha,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f
             );
        }

        public void Unload()
        {
            textures.Clear();
        }
        public void Begin()
        {
            spriteBatch.Begin();
        }
        public void End()
        {
            spriteBatch.End();
        }
        public void DrawString(string myString, Vector2 position)
        {
            spriteBatch.DrawString(font, myString, position, Color.Red);
        }
        public void DrawString(string myString, Vector2 position, Color color, Vector2 origin, Vector2 scale)
        {
            spriteBatch.DrawString(font, myString, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        #region Effect
        public void Begin(Effect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, effect);
        }
        public void DrawTexture(Texture2D texture, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);

        }

        #endregion

        #region Texture
        public void DrawTexture(string name, Vector2 position, float Alpha = 1.0f)
        {
             spriteBatch.Draw(textures[name], position, Color.White * Alpha);
        }
        public void DrawTexture(string name, Vector2 position, float rotation, Vector2 origin, Vector2 scale, float Alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name], 
                position,
                new Rectangle(0, 0, textures[name].Width, textures[name].Height),
                Color.White * Alpha,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0);
        }
        public void DrawTexture(string name, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin, Vector2 scale, float Alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                rectangle,
                Color.White * Alpha,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0);
        }
        public void DrawTexture(string name, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect , float Alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                rectangle,
                Color.White * Alpha,
                rotation,
                origin,
                scale,
                effect,
                0);
        }
        public void DrawTexture(string name, Vector2 position, Rectangle rectangle, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, Color color, float Alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                rectangle,
                color * Alpha,
                rotation,
                origin,
                scale,
                effect,
                0);
        }
        public void DrawTexture(string name, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                rect,
                Color.White * alpha);
        }
        #endregion
    }
}
