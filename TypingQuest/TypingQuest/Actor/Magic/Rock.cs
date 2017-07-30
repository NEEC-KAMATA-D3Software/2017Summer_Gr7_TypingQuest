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
    class Rock : MagicAbstract
    {
        class SmallRock
        {
            private PathAbstract path;
            private Timer timer;
            private Vector2 position;
            private GameDevice gameDevice;
            private Vector2 startPosition;
            private Direct direct;

            private MagicCollisionRange collision;

            public SmallRock(GameDevice gameDevice)
            {
                this.gameDevice = gameDevice;
                timer = new Timer(0);
            }
            public void Initialize(Vector2 startPosition, Direct direct)
            {
                if (direct == Direct.Left)
                {
                    this.startPosition = startPosition;
                    position = startPosition;
                    path = new Path_VerticalLine(position, 100, 0.2f, Path.Direct.LEFT);
                    this.direct = direct;
                }
                else
                {
                    this.startPosition = startPosition;
                    position = startPosition;
                    path = new Path_VerticalLine(position, 100, 0.2f, Path.Direct.RIGHT);
                    this.direct = direct;
                }
                collision = new MagicCollisionRange((int)position.X, (int)position.Y, 0, 64);
                path.Initialize();
            }
            public void Update(GameTime gameTime, ref MagicCollisionRange collision, Direct direct)
            {
                if (path.IsEnd())
                {
                    timer.Update();
                }
                else
                {
                    path.Update(gameTime);
                    position = path.GetCurrentPosition();
                    if (direct == Direct.Left)
                    {
                        collision.Transform(TransformDirect.LEFT, startPosition, position);
                    }
                    else
                    {
                        collision.Transform(TransformDirect.RIGHT, startPosition, position);
                    }
                    this.collision = collision;
                }
            }
            public void Draw(Renderer renderer)
            {
                if (direct == Direct.Left)
                {
                    renderer.DrawTexture("rock", position + gameDevice.GetDisplayModify(), collision.DrawRange());
                }
                else
                {
                    renderer.DrawTexture("rock", startPosition + gameDevice.GetDisplayModify(), collision.DrawRange(),
                        0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally);
                }

            }
            public void SetTime(float second)
            {
                timer.Change(second * 60);
            }
            public bool IsEnd()
            {
                return timer.IsTime() && path.IsEnd();
            }
            public Vector2 GetPosition()
            {
                return position;
            }
        }

        private Timer timer;
        private Direct direct;
        private int number;
        private List<SmallRock> childs;
        private bool isHit;

        public Rock(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            : base(castChara, "ROCK", 64, 100, characterManager, gameDevice)
        {
            power = 30;
            number = 3;
            direct = castChara.GetDirection();
            timer = new Timer(0);
            timer.Initialize();
            childs = new List<SmallRock>();
            isHit = false;
        }
        public override void Initialize()
        {
            for (int i = 0; i < number; i++)
            {
                childs.Add(new SmallRock(gameDevice));
            }
            if (direct == Direct.Left)
            {
                childs[0].Initialize(new Vector2(castChara.GetCenterWidth() - (castChara.GetSize().X / 2) - 10, castChara.GetCenterHeight() - 32), direct);
                for (int i = 1; i < childs.Count; i++)
                {
                    Vector2 position = childs[i - 1].GetPosition() - new Vector2(90, 0);
                    childs[i].Initialize(position, Direct.Left);
                }
            }
            else
            {
                childs[0].Initialize(new Vector2(castChara.GetCenterWidth() + (castChara.GetSize().X / 2) + 10, castChara.GetCenterHeight() - 32), direct);
                for (int i = 1; i < childs.Count; i++)
                {
                    Vector2 position = childs[i - 1].GetPosition() + new Vector2(90, 0);
                    childs[i].Initialize(position, Direct.Right);
                }
            }
            position = new Vector2(childs[0].GetPosition().X, childs[0].GetPosition().Y + 32);
            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 0, 64);

            number = 0;
            gameDevice.GetSound().PlaySE("rock");
        }

        public override void Update(GameTime gameTime)
        {
            timer.Update();
            if (childs[childs.Count - 1].IsEnd() && isHit == false)
            {
                IsCollision();
                isHit = true;
            }

            childs[number].Update(gameTime, ref collision, direct);
            position.X = collision.GetCollision().X;
            if (childs[number].IsEnd() && timer.IsTime())
            {

                if (number < childs.Count - 1)
                {
                    gameDevice.GetSound().PlaySE("rock");
                    IsCollision();
                    position = new Vector2(childs[number].GetPosition().X, childs[number].GetPosition().Y);
                    collision = new MagicCollisionRange((int)position.X, (int)position.Y, 0, 64);
                    number++;
                    if (number == childs.Count - 1)
                    {
                        timer.Change(1 * 60.0f);
                        timer.Initialize();
                    }
                }
                else
                {
                    childs.Clear();
                    isEnd = true;
                }
            }
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
            cx.Damage(power, direction, new Vector2(5f, -1.2f));
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

            player.Damage(power, direction, new Vector2(5f, -1.2f));
        }

        public override void Draw(Renderer renderer)
        {
            for (int i = number; i >= 0; i--)
            {
                childs[i].Draw(renderer);
            }
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
