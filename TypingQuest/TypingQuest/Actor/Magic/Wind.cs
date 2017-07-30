using System;
using Microsoft.Xna.Framework;
using TypingQuest.Scene;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Magic
{
    class Wind : MagicAbstract
    {
        private int count;
        private Timer timer;

        private Motion motion;
        private float effectAlpha;
        private bool effectBool;
        private Step step;

        public Wind(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            : base(castChara, "wind", 100, 100, characterManager, gameDevice)
        {
            power = 20;

            //X座標：魔法の中心点から人物の中心点までの距離は300
            //Y座標：魔法の中心点は人物の中心点と同じ
            if (castChara.GetDirection() == Direct.Left)
            {
                position = new Vector2(castChara.GetCenterWidth() - (castChara.GetSize().X / 2) - 300, castChara.GetCenterHeight());
            }
            else
            {
                position = new Vector2(castChara.GetCenterWidth() + (castChara.GetSize().X / 2) + 300, castChara.GetCenterHeight());
            }

            //攻撃次数と攻撃間隔
            count = 6;
            timer = new Timer(1.0f / count);
            timer.Initialize();

            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 100, 100);
            motion = new Motion();

            position -= new Vector2(50, 50);
        }

        public override void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(0, i * 100, 100, 100));
            }
            motion.Initialize(new Range(0, 2), new Timer(0.05f));

            step = Step.Launch;
            effectAlpha = 1.0f;
            effectBool = true;
        }

        protected override void HitEnemy(Character cx)
        {
            Vector2 direct = (position + new Vector2(50, 50))- cx.GetPosition();
            Direct direction;
            if (direct.X > 0)
            {
                direction = Direct.Right;
            }
            else
            {
                direction = Direct.Left;
            }
            gameDevice.GetSound().PlaySE("wind");
            cx.Damage(power, direction);
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
            gameDevice.GetSound().PlaySE("wind");
            player.Damage(power, direction);
        }



        public override void Update(GameTime gameTime)
        {
            switch (step)
            {
                case Step.Launch:
                    UpdateLaunch(gameTime);
                    break;
                case Step.End:
                    UpdateEnd(gameTime);
                    break;
            }
            motion.Update(gameTime);
        }
        private void UpdateLaunch(GameTime gameTime)
        {
            if (effectBool)
            {
                effectAlpha -= 0.05f;
                if (effectAlpha <= 0.7)
                {
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha += 0.05f;
                if (effectAlpha >= 1)
                {
                    effectAlpha = 1;
                    effectBool = true;
                }
            }
            timer.Update();
            if (timer.IsTime())
            {
                IsCollision();
                --count;
                timer.Initialize();
            }
            if (count == 0)
            {
                step = Step.End;
                timer.Change(60 * 1.0f);
                timer.Initialize();
            } 
        }

        private void UpdateEnd(GameTime gameTime)
        {
            timer.Update();
            effectAlpha -= 0.05f;
            if (timer.IsTime())
            {
                isEnd = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("wind", position + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
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
