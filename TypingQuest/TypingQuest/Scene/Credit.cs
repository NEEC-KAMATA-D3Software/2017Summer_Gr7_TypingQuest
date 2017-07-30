using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;
using TypingQuest.Utility;
using TypingQuest.Actor;
using TypingQuest.Actor.Magic;
using TypingQuest.Def;

namespace TypingQuest.Scene
{
    class Credit : IScene
    {
        private GameDevice gameDevice;
        private Sound sound;
        private InputState input;
        private bool isEnd;

        private Map map;
        private Player player;
        private MagicManager magicManager;

        private int creditNumber;
        private int currentCreditNumber;
        private Timer timer;

        public Credit(GameDevice gameDevice, MagicManager magicManager)
        {
            this.gameDevice = gameDevice;
            sound = gameDevice.GetSound();
            this.magicManager = magicManager;
            input = gameDevice.GetInputState();
            isEnd = false;

            map = new Map(gameDevice);

            creditNumber = 4;
            currentCreditNumber = 1;
            timer = new Timer(8);
        }

        public void Initialize()
        {
            map.Load("./credit.csv");
            magicManager.Initialize();
            player = new Player(new Vector2(64, gameDevice.GetStageHeight() - 128), gameDevice, magicManager);
            player.Initialize();
            isEnd = false;

            creditNumber = 4;
            currentCreditNumber = 1;
            timer.Initialize();

            gameDevice.SetDisplayModify(new Vector2(0, 566));
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("background", Vector2.Zero);
            map.Draw(renderer);

            magicManager.Draw(renderer);
            player.Draw(renderer);

            renderer.DrawTexture(("credit" + currentCreditNumber), Vector2.Zero, 1 - (Math.Abs(timer.Rate() * 2 - 1)));

            renderer.End();
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
            map.Unload();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("Moon_Grav");

            map.Update(gameTime);
            map.Hit(player);
            player.Update(gameTime);
            player.TitlePosition();

            magicManager.Update(gameTime);

            if (player.GetPosition().X < 32)
            {
                isEnd = true;
            }

            timer.Update();
            if (timer.IsTime())
            {
                timer.Initialize();
                currentCreditNumber++;

                if (currentCreditNumber > creditNumber)
                {
                    currentCreditNumber = 1;
                }
            }

        }
    }
}
