using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Actor;
using TypingQuest.Actor.Status;
using TypingQuest.Utility;

namespace TypingQuest.Scene
{
    class TrapBall : GameObject
    {
        private int angle;
        private float radius;

        private Motion motion;
        private float effectAlpha;
        private bool effectSwitch;
        public TrapBall(Vector2 centerPosition, GameDevice gameDevice, float radius)
            :base("spark", centerPosition, 55, 55, gameDevice)
        {
            angle = 0;
            this.radius = radius;
            position.X += radius;
            position.Y -= radius;

            motion = new Motion();
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.1f));

            effectAlpha = 1.0f;
            effectSwitch = true;
        }
        public TrapBall(TrapBall other)
            :this(other.position, other.gameDevice, other.radius)
        {
        }
        public override object Clone()
        {
            return new TrapBall(this);
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (((Player)gameObject).IsInvicible())
                {
                    return;
                }
                Vector2 direct = position - gameObject.GetPosition();
                Direct direction;
                if (direct.X > 0)
                {
                    direction = Direct.Left;
                }
                else
                {
                    direction = Direct.Right;
                }
                ((Player)gameObject).Damage(7, direction);
                ((Player)gameObject).AddState(new Invincible(1));
            }
        }

        public override void Update(GameTime gameTime)
        {
            angle += 6;
            angle = (angle > 359) ? 0 : angle;
            Vector2 velocity = new Vector2(
                radius * (float)Math.Cos(MathHelper.ToRadians(angle)),
                radius * (float)Math.Sin(MathHelper.ToRadians(angle)));
            velocity.Normalize();
            position += (velocity) * 10;

            motion.Update(gameTime);

            if (effectSwitch)
            {
                effectAlpha -= 0.03f;
                if (effectAlpha < 0.6f)
                {
                    effectSwitch = false;
                }
            }
            else
            {
                effectAlpha += 0.03f;
                if (effectAlpha > 1)
                {
                    effectAlpha = 1.0f;
                    effectSwitch = true;
                }
            }
        }
    }
}
