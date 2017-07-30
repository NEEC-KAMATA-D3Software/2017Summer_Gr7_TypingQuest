using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Status
{
    class Poison : State
    {
        private Timer interval;

        private float effectAlpha;
        private bool isDamage;
        private Motion motion;

        private GameDevice gameDevice;
        public Poison(float duration, int amount, float interval, GameDevice gameDevice, bool isDamage)
        {
            this.gameDevice = gameDevice;
            this.duration.Change(duration * 60);
            this.amount = amount;
            this.interval = new Timer(interval);
            this.isDamage = isDamage;
            motion = new Motion();

            effectAlpha = 0;
        }
        public override void Initialize()
        {
            duration.Initialize();
            interval.Initialize();

            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.1f));
        }
        public override void Update(GameTime gameTime)
        {
            duration.Update();
            interval.Update();
            motion.Update(gameTime);

            effectAlpha = interval.Rate();

            if (duration.IsTime())
            {
                isEnd = true;
            }
            if (interval.IsTime())
            {
                target.Damage(amount, Direct.Left, true);
                interval.Initialize();
            }
        }
        public override void Draw(Renderer renderer)
        {
            if (isDamage)
            {
            }
            else
            {
                renderer.DrawTexture("recovery" ,target.GetPosition() + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
            }
        }
    }
}
