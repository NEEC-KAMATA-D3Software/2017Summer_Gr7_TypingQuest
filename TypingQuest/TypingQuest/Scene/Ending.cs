using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Scene
{
    class Ending : IScene
    {
        private InputState input;
        private Sound sound;
        private bool isEnd;
        private IScene gamePlay;
        private int creditNumber;
        private int currentCreditNumber;
        private Timer timer;
        private int playeTime;

        public Ending(GameDevice gameDevice, IScene gamePlay)
        {
            sound = gameDevice.GetSound();
            this.input = gameDevice.GetInputState();
            this.gamePlay = gamePlay;
            isEnd = false;
            creditNumber = 5;
            currentCreditNumber = 1;
            playeTime = 0;
            timer = new Timer(8);
        }

        public void Initialize()
        {
            isEnd = false;
            playeTime = 0;
            creditNumber = 5;
            currentCreditNumber = 1;
            timer.Initialize();
            sound.ChangeBGMLoopFlag(false);
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("Eos");

            if (input.IsKeyDown(Keys.F1) || playeTime >= 4)
            {
                isEnd = true;
            }
            gamePlay.Update(gameTime);
            timer.Update();
            if (timer.IsTime())
            {
                timer.Initialize();
                currentCreditNumber++;

                if (currentCreditNumber > creditNumber)
                {
                    currentCreditNumber = 1;
                    playeTime++;
                }
            }
        }

        public void Draw(Renderer renderer)
        {
            gamePlay.Draw(renderer);

            renderer.Begin();
            renderer.DrawTexture(("credit" + currentCreditNumber), Vector2.Zero, 1 - (Math.Abs(timer.Rate() * 2 - 1)));
            renderer.End();
        }

        public void Shutdown()
        {
            if (gamePlay is Boss)
            {
                ((Boss)gamePlay).ShutdownEnding();
            }
            sound.ChangeBGMLoopFlag(true);
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            return Scene.Title;
        }
    }
}
