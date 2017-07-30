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
    class LightningSingle : MagicAbstract
    {
        private Timer timer;
        private PathAbstract path;

        private Motion motion;
        private float effectAlpha;
        private bool effectBool;

        public LightningSingle(Character castChara, GameDevice gameDevice, CharacterManager characterManager, PathAbstract path, string name, int power = 10)
            : base(castChara, name, 256, 64, characterManager, gameDevice)
        {
            this.path = path;
            this.path.Initialize();
            position = this.path.GetCurrentPosition();
            this.power = power;
            timer = new Timer(0.25f);
            collision = new MagicCollisionRange((int)position.X + 32, (int)position.Y + 96, 64, 256);

            motion = new Motion();
            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 256));
            }
            motion.Initialize(new Range(0, 2), new Timer(0.05f));
            effectAlpha = 0.3f;
            effectBool = true;
        }

        public override void Initialize()
        {

        }

        protected override void HitEnemy(Character cx)
        {
            Vector2 direct = (position + new Vector2(50, 50)) - cx.GetPosition();
            Direct direction;
            if (direct.X > 0)
            {
                direction = Direct.Right;
            }
            else
            {
                direction = Direct.Left;
            }
            cx.Damage(power, direction);
            gameDevice.GetSound().PlaySE("lightning");
            timer.Initialize();
        }

        protected override void HitPlayer(Player player)
        {
            Vector2 direct = (position + new Vector2(50, 50)) - player.GetPosition();
            Direct direction;
            if (direct.X > 0)
            {
                direction = Direct.Right;
            }
            else
            {
                direction = Direct.Left;
            }
            player.Damage(power, direction);
            gameDevice.GetSound().PlaySE("lightning");
            timer.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            motion.Update(gameTime);

            timer.Update();
            if (timer.IsTime()) IsCollision();
            path.Update(gameTime);
            position = path.GetCurrentPosition();
            collision.Move(new Vector2(position.X + width / 2, position.Y + height / 2), width, height);

            if (effectBool)
            {
                effectAlpha += 0.1f;
                if (effectAlpha >= 0.8f)
                {
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha -= 0.1f;
                if (effectAlpha < 0.3f)
                {
                    effectBool = true;
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
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
