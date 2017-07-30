using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Scene;
using TypingQuest.Device;
using TypingQuest.Actor.Magic.Path;
using TypingQuest.Utility;
using TypingQuest.Def;
using TypingQuest.Actor.Status;

namespace TypingQuest.Actor.Magic
{
    enum Mode
    {
        Start,
        Stop,
        End
    }
    class MagicGravity : MagicAbstract
    {
        private PathAbstract pathLeft;
        private PathAbstract pathRight;
        private Vector2 positionRight;
        private Timer timer;
        private Mode mode;
        private float time;
        private Motion motion;
        private float effectAlpha;
        private bool effectBool;

        public MagicGravity(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            : base(castChara, "gravity", 200, 64, characterManager, gameDevice)
        {
            time = 1.0f;
            timer = new Timer(1);
            power = 40;
            mode = Mode.Start;
            motion = new Motion();
            effectAlpha = 1.0f;
            effectBool = true;
        }

        public override void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                motion.Add(i, new Rectangle(0, (64 * i), 200, 64));
            }
            motion.Initialize(new Range(0, 1), new Timer(0.2f));

            position = new Vector2(castChara.GetCenterWidth(), castChara.GetCenterHeight() + (castChara.GetSize().Y / 2) - 64);
            positionRight = position;

            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 40, 64);

            pathLeft = new Path_VerticalLine(position, 200, time, Path.Direct.LEFT);
            pathRight = new Path_VerticalLine(positionRight, 200, time, Path.Direct.RIGHT);

            pathLeft.Initialize();
            pathRight.Initialize();

        }

        public override void Update(GameTime gameTime)
        {
            if (effectBool)
            {
                effectAlpha -= 0.03f;
                if (effectAlpha <= 0.4)
                {
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha += 0.03f;
                if (effectAlpha >= 0.65)
                {
                    effectAlpha = 0.65f;
                    effectBool = true;
                }
            }
            motion.Update(gameTime);
            timer.Update();

            if (timer.IsTime())
            {
                switch (mode)
                {
                    case Mode.Start:
                        if (!pathLeft.IsEnd())
                        {
                            pathLeft.Update(gameTime);
                            int amount = (int)(position.X - pathLeft.GetCurrentPosition().X);
                            position = pathLeft.GetCurrentPosition();
                            pathRight.Update(gameTime);
                            positionRight = pathRight.GetCurrentPosition();
                            collision.Transform(TransformDirect.LEFT, amount);
                            collision.Transform(TransformDirect.RIGHT, amount);
                        }
                        else
                        {
                            mode = Mode.Stop;
                        }
                        break;

                    case Mode.Stop:
                        timer.Initialize();
                        pathLeft = new Path_VerticalLine(position, 200, time, Path.Direct.RIGHT);
                        pathRight = new Path_VerticalLine(positionRight, 200, time, Path.Direct.LEFT);
                        pathLeft.Initialize();
                        pathRight.Initialize();
                        mode = Mode.End;
                        break;

                    case Mode.End:
                        if (!pathLeft.IsEnd())
                        {
                            pathLeft.Update(gameTime);
                            int amount = (int)(position.X - pathLeft.GetCurrentPosition().X);
                            position = pathLeft.GetCurrentPosition();
                            pathRight.Update(gameTime);
                            positionRight = pathRight.GetCurrentPosition();
                            collision.Transform(TransformDirect.LEFT, amount);
                            collision.Transform(TransformDirect.RIGHT, amount);
                        }
                        else
                        {
                            isEnd = true;
                        }
                        break;
                }
            }

            if (mode == Mode.End && timer.Now() != 0)
            {
                if (timer.Now() % 15.0f == 0)
                {
                    IsCollision();
                    motion.Initialize(new Range(0, 2), new Timer(0.2f));
                }
            }
            else if(mode == Mode.End)
            {
                motion.Initialize(new Range(0, 1), new Timer(0.2f));
            }

        }
        public override void Draw(Renderer renderer)
        {
            Rectangle DrawRange = collision.DrawRange();
            DrawRange.Width = DrawRange.Width / 2;
            DrawRange.X = 200 - DrawRange.Width;
            DrawRange.Y = motion.DrawRange().Y;
            Vector2 right = new Vector2(positionRight.X - DrawRange.Width, positionRight.Y);
            renderer.DrawTexture("gravity", position + gameDevice.GetDisplayModify(), DrawRange, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, effectAlpha);
            renderer.DrawTexture("gravity", right + gameDevice.GetDisplayModify(), DrawRange, 0, Vector2.Zero, new Vector2(1, 1), effectAlpha);
        }

        protected override void HitEnemy(Character cx)
        {
            Vector2 direct = position - cx.GetPosition();
            Direct direction;
            if (direct.X > 0)
            {
                direction = Direct.Left;
            }
            else
            {
                direction = Direct.Right;
            }
            cx.Damage(power, direction);
            gameDevice.GetSound().PlaySE("rock");
        }

        protected override void HitPlayer(Player player)
        {
            Vector2 direct = position - player.GetPosition();
            Direct direction;
            if (direct.X > 0)
            {
                direction = Direct.Left;
            }
            else
            {
                direction = Direct.Right;
            }
            player.Damage(power, direction);
            gameDevice.GetSound().PlaySE("rock");
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
