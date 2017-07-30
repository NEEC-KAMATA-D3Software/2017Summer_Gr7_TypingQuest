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


    class Lightning : MagicAbstract
    {
        enum Mode
        {
            Move,
            LastMove,
            Wait,
            MidLightning
        }

        private Mode mode;

        private int count;
        private float timeInterval;
        private Timer timer;

        //中心点からの距離
        private int radius;

        private LightningSingle leftLightning;
        private LightningSingle rightLightning;
        private LightningSingle lastLightning;

        public Lightning(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            : base(castChara, "Lightning", 256, 512, characterManager, gameDevice)
        {
            power = 0;

            //X座標：魔法の中心点、人物の中心点までの距離は256
            //Y座標：魔法の一番上の座標
            if (castChara.GetDirection() == Direct.Left)
            {
                position = new Vector2(castChara.GetCenterWidth() - 288, castChara.GetCenterHeight() + (castChara.GetSize().Y / 2) - 256);
            }
            else
            {
                position = new Vector2(castChara.GetCenterWidth() + 224, castChara.GetCenterHeight() + (castChara.GetSize().Y / 2) - 256);
            }

            //繰り返し
            count = 5;
            timeInterval = 0.4f;
            timer = new Timer(timeInterval);
            timer.Initialize();
            radius = 96;

            leftLightning = new LightningSingle(castChara, gameDevice, characterManager,
                new Path_VerticalLine(new Vector2(position.X - radius, position.Y), 2 * radius + 32, timeInterval, Path.Direct.RIGHT),
                "lightningSingle");
            rightLightning = new LightningSingle(castChara, gameDevice, characterManager,
                new Path_VerticalLine(new Vector2(position.X + radius, position.Y), 2 * radius + 32, timeInterval, Path.Direct.LEFT),
                "lightningSingle");

            collision = new MagicCollisionRange(0, 0, 0, 0);
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            timer.Update();
            switch (mode)
            {
                case Mode.Move:
                    leftLightning.Update(gameTime);
                    rightLightning.Update(gameTime);

                    if (timer.IsTime())
                    {
                        if (--count != 0)
                        {
                            leftLightning = new LightningSingle(castChara, gameDevice, characterManager,
                                new Path_VerticalLine(new Vector2(position.X - radius, position.Y), 2 * radius + 32, timeInterval, Path.Direct.RIGHT),
                                "lightningSingle");
                            rightLightning = new LightningSingle(castChara, gameDevice, characterManager,
                                new Path_VerticalLine(new Vector2(position.X + radius, position.Y), 2 * radius + 32, timeInterval, Path.Direct.LEFT),
                                "lightningSingle");
                            radius += 32;
                            timer.Initialize();
                        }

                        //LastMoveの雷は中心まで移動する
                        else
                        {
                            mode = Mode.LastMove;
                            timer = new Timer(timeInterval / 2);
                            timer.Initialize();
                            leftLightning = new LightningSingle(castChara, gameDevice, characterManager,
                                new Path_VerticalLine(new Vector2(position.X - radius, position.Y), radius, timeInterval / 2, Path.Direct.RIGHT),
                                "lightningSingle");
                            rightLightning = new LightningSingle(castChara, gameDevice, characterManager,
                                new Path_VerticalLine(new Vector2(position.X + radius, position.Y), radius, timeInterval / 2, Path.Direct.LEFT),
                                "lightningSingle");
                        }
                    }
                    break;

                case Mode.LastMove:
                    leftLightning.Update(gameTime);
                    rightLightning.Update(gameTime);

                    if (timer.IsTime())
                    {
                        mode = Mode.Wait;
                        timer = new Timer(timeInterval);
                        timer.Initialize();
                    }
                    break;

                case Mode.Wait:
                    if (timer.IsTime())
                    {
                        mode = Mode.MidLightning;
                        timer = new Timer(timeInterval);
                        timer.Initialize();
                        lastLightning = new LightningSingle(castChara, gameDevice, characterManager,
                                    new Path_VerticalLine(position, 1, 100, Path.Direct.RIGHT),
                                    "lightningLast", 30);
                    }
                    break;

                case Mode.MidLightning:
                    lastLightning.Update(gameTime);

                    if (timer.IsTime())
                    {
                        isEnd = true;
                    }
                    break;

                default:
                    break;
            }
        }


        public override void Draw(Renderer renderer)
        {
            if (mode == Mode.Move || mode == Mode.LastMove)
            {
                leftLightning.Draw(renderer);
                rightLightning.Draw(renderer);
            }
            else if (mode == Mode.MidLightning)
            {
                lastLightning.Draw(renderer);
            }
        }

        protected override void HitEnemy(Character cx)
        {
        }

        protected override void HitPlayer(Player player)
        {
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
