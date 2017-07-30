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
    class Title : IScene
    {
        private GameDevice gameDevice;
        private Sound sound;
        private InputState input;
        private bool isEnd;

        private Map map;
        private Player player;
        private MagicManager magicManager;

        private float effectAlpha;
        private bool effectSwitch;

        public Title(GameDevice gameDevice, MagicManager magicManager)
        {
            this.gameDevice = gameDevice;
            sound = gameDevice.GetSound();
            this.magicManager = magicManager;
            input = gameDevice.GetInputState();
            isEnd = false;

            map = new Map(gameDevice);

            effectAlpha = 0.0f;
            effectSwitch = true;
        }

        public void Initialize()
        {
            map.Load("./title.csv");
            magicManager.Initialize();
            player = new Player(new Vector2(gameDevice.GetStageLength() - 194, gameDevice.GetStageHeight() - 128), gameDevice, magicManager);
            player.Initialize();
            isEnd = false;

            gameDevice.SetDisplayModify(new Vector2(0, 566));
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("Moon_Grav");

            map.Update(gameTime);
            map.Hit(player);
            player.Update(gameTime);
            player.TitlePosition();

            magicManager.Update(gameTime);

            if (player.GetPosition().X < 32 || player.GetPosition().X > gameDevice.GetStageLength() - 96)
            {
                isEnd = true;
            }

            if (effectSwitch)
            {
                effectAlpha += 0.006f;
                if (effectAlpha > 0.9)
                {
                    effectSwitch = false;
                }
            }
            else
            {
                effectAlpha -= 0.006f;
                if (effectAlpha < 0.2)
                {
                    effectSwitch = true;
                }
            }
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("background", Vector2.Zero);
            map.Draw(renderer);

            magicManager.Draw(renderer);
            player.Draw(renderer);

            renderer.DrawTexture("title",  new Vector2(350, 70), new Rectangle(0, 0, 800, 600), 0, Vector2.Zero, new Vector2(0.8f, 0.8f));
            renderer.DrawTexture("title_light", new Vector2(350, 70), new Rectangle(0, 0, 800, 600), 0, Vector2.Zero, new Vector2(0.8f, 0.8f), effectAlpha);

            renderer.End();
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            if (player.GetPosition().X > gameDevice.GetStageLength() - 96)
            {
                return Scene.Credit;
            }
            return Scene.Stage1;
        }

        public void Shutdown()
        {
            map.Unload();
        }
    }
}
