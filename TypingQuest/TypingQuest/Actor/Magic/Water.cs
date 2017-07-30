using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Scene;
using TypingQuest.Actor.Status;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Magic
{
    class Water : MagicAbstract
    {
        private Direct direct;
        private Motion motion;
        private Timer timer;

        private float effectAlpha;
        private bool effectBool;
        private Step step;
        public Water(Character castChara, GameDevice gameDevice, CharacterManager characterManager):
            base(castChara, "WATER", 64, 0, characterManager, gameDevice)
        {
            if (castChara.GetDirection() == Direct.Left)
            {
                position = new Vector2(castChara.GetCenterWidth() - (castChara.GetSize().Y / 2), castChara.GetCenterHeight() - 31);
                direct = Direct.Left;
            }
            else
            {
                position = new Vector2(castChara.GetCenterWidth() + (castChara.GetSize().Y / 2), castChara.GetCenterHeight() - 31);
                direct = Direct.Right;
            }
            this.characterManager = characterManager;
            motion = new Motion();
            timer = new Timer(1.0f);

            step = Step.Launch;
            effectAlpha = 1.0f;
            effectBool = true;
        }

        public override void Draw(Renderer renderer)
        {
            Rectangle drawRange = collision.DrawRange();
            drawRange.Y = motion.DrawRange().Y;
            if (direct == Direct.Left)
            {
                renderer.DrawTexture("water", position + gameDevice.GetDisplayModify(), drawRange, 0, Vector2.Zero, new Vector2(1, 1), effectAlpha);
            }
            else
            {
                renderer.DrawTexture("water", position + gameDevice.GetDisplayModify(), drawRange, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, effectAlpha);
            }
        }
        
        public override void Initialize()
        {
            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 0, 64);

            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(0, i * 64, 512, 64));
            }
            motion.Initialize(new Range(0, 2), new Timer(0.05f));
            gameDevice.GetSound().PlaySE("water");
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
                if (effectAlpha <= 0.6)
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

            IsCollision();
            if (collision.GetCollision().Width < 512)
            {
                if (direct == Direct.Left)
                {
                    collision.Transform(TransformDirect.LEFT, 20);
                    position.X = collision.GetCollision().X;
                    width = collision.GetCollision().Width;
                }
                else
                {
                    collision.Transform(TransformDirect.RIGHT, 20);
                    width = collision.GetCollision().Width;
                }
            }
            else
            {
                step = Step.End;
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

        protected override void HitEnemy(Character cx)
        {
            cx.AddState(new Freeze(2)); //足止め freeze(継続時間)
            //cx.AddState(new Poison(5, 1, 1)); //毒を付属する　poison(second, damage, 時間間隔)
        }

        protected override void HitPlayer(Player player)
        {
            player.AddState(new Freeze(1.5f));
        }


        public override void Hit(GameObject gameObject)
        {
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
