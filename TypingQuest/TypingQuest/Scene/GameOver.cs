using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class GameOver : IScene
    {
        private GameDevice gameDevice;
        private InputState input;
        private bool isEnd;

        public GameOver(GameDevice gameDevice)
        {
            input = gameDevice.GetInputState();
            isEnd = false;

            this.gameDevice = gameDevice;
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin();


            renderer.DrawTexture("GameOver", new Vector2(262, 234));

            renderer.End();
        }

        public void Initialize()
        {
            isEnd = false;
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            return Scene.Title;
        }

        public void Shutdown()
        {
        }

        public void Update(GameTime gameTime)
        {
            gameDevice.GetSound().PlayBGM("tomoshibi");
            if (input.IsKeyDown(Keys.Space))
            {
                isEnd = true;
            }
        }
    }
}
