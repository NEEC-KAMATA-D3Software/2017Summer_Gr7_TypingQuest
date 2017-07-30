using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Scene;
using TypingQuest.Device;
using TypingQuest.Utility;
using TypingQuest.Actor.Magic.Path;

namespace TypingQuest.Actor.Magic
{
    class Cyclone : MagicAbstract
    {
        private Timer timer;
        private Mode mode;

        private Motion motion;
        private float effectAlpha;
        private bool effectBool;

        public Cyclone(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            :base(castChara, "cyclone", 256, 128, characterManager, gameDevice)
        {
            power = 20;

            if (castChara.GetDirection() == Direct.Left)
            {
                position = new Vector2(castChara.GetCenterWidth() - castChara.GetSize().X - 74, castChara.GetCenterHeight() + (castChara.GetSize().Y / 2) - 128);
            }
            else
            {
                position = new Vector2(castChara.GetCenterWidth() + castChara.GetSize().X + 74, castChara.GetCenterHeight() + (castChara.GetSize().Y / 2) - 128);
            }

            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 128, 256);

            mode = Mode.Stop;

            timer = new Timer(1.5f);
            motion = new Motion();
            effectAlpha = 0.5f;
            effectBool = true;
        }
        public override void Initialize()
        {
            timer.Initialize();
            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(128 * i, 0, 128, 256));
            }
            motion.Initialize(new Range(0, 2), new Timer(0.05f));
        }

        public override void Update(GameTime gameTime)
        {
            timer.Update();
            motion.Update(gameTime);

            switch (mode)
            {
                case Mode.Stop:
                    UpdateStop(gameTime);
                    break;
                case Mode.End:
                    UpdateEnd(gameTime);
                    break;
            }
        }
        private void UpdateStop(GameTime gameTime)
        {
            if (timer.Now() % 10 == 0)
            {
                IsCollision();
            }
            if (timer.IsTime())
            {
                mode = Mode.End;
                timer = new Timer(0.5f);
                timer.Initialize();
            }

            if (effectBool)
            {
                effectAlpha += 0.01f;
                if (effectAlpha >= 1)
                {
                    effectAlpha = 1;
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha -= 0.01f;
                if (effectAlpha < 0.5)
                {
                    effectBool = true;
                }
            }
        }
        private void UpdateEnd(GameTime gameTime)
        {
            effectAlpha = timer.Rate();
            if (timer.IsTime())
            {
                isEnd = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            Vector2 tempPosition = position - new Vector2(64, 128);
            //Rectangle tempRect = new Rectangle(motion.DrawRange().X, 256 - collision.DrawRange().Height, 128, collision.DrawRange().Height);
            renderer.DrawTexture(name, tempPosition + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
        }

        protected override void HitEnemy(Character cx)
        {
            Vector2 direct = position - cx.GetPosition();
            Direct direction;
            if (direct.X < 0)
            {
                direction = Direct.Left;
            }
            else
            {
                direction = Direct.Right;
            }
            cx.Damage(power, direction);
            gameDevice.GetSound().PlaySE("wind");
        }

        protected override void HitPlayer(Player player)
        {
            Vector2 direct = position - player.GetPosition();
            Direct direction;
            if (direct.X < 0)
            {
                direction = Direct.Left;
            }
            else
            {
                direction = Direct.Right;
            }
            player.Damage(power, direction);
            gameDevice.GetSound().PlaySE("wind");
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
        }

    }
}
