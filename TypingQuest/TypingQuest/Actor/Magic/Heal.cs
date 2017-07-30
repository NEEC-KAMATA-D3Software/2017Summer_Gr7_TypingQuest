using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Scene;
using TypingQuest.Utility;
using TypingQuest.Def;
using TypingQuest.Actor.Status;
using TypingQuest.Actor.Magic.Path;

namespace TypingQuest.Actor.Magic
{
    class Heal : MagicAbstract
    {
        private Timer timer;

        private float effectAlpha;
        private Motion motion;
        private Step step;

        public Heal(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            :base(castChara,"heal",0,0,characterManager,gameDevice)
        {
            timer = new Timer(1.0f);
            power = 100;

            step = Step.Launch;
            effectAlpha = 0;
            motion = new Motion();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Initialize()
        {
            position = new Vector2(castChara.GetCenterWidth(), castChara.GetCenterHeight());
            timer.Initialize();

            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.1f));
            gameDevice.GetSound().PlaySE("heal");
        }

        public override void Update(GameTime gameTime)
        {
            motion.Update(gameTime);
            position = castChara.GetPosition();

            switch (step)
            {
                case Step.Launch:
                    UpdateLaunch();
                    break;
                case Step.End:
                    UpdateEnd();
                    break;
            }
            
        }

        private void UpdateLaunch()
        {
            timer.Update();
            effectAlpha = 1 - timer.Rate();
            if (timer.IsTime())
            {
                castChara.Damage(-power, Direct.Left);
                step = Step.End;
                timer = new Timer(0.5f);
                timer.Initialize();
            }
        }
        private void UpdateEnd()
        {
            timer.Update();
            effectAlpha = timer.Rate();
            if (timer.IsTime())
            {
                isEnd = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
        }

        protected override void HitEnemy(Character cx)
        {
        }

        protected override void HitPlayer(Player player)
        {
        }
    }
}
