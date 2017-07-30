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
    class Fire : MagicAbstract
    {
        private Vector2 targetPosition;
        private Timer motionTimer;

        private PathAbstract path;
        private Motion motion;
        private Step step;
        private float effectAlpha;
        private bool effectBool;

        public Fire(Character castChara, GameDevice gameDevice, CharacterManager characterManager)
            :base(castChara, "fire", 55, 55, characterManager, gameDevice)
        {
            //魔法を放つまでの時間
            motionTimer = new Timer(0.5f);
            //威力
            power = 50;
            step = Step.Launch;
            motion = new Motion();
            effectAlpha = 1.0f;
            effectBool = true;
        }
        public override void Initialize()
        {
            //初期位置
            position = new Vector2(castChara.GetCenterWidth(), castChara.GetCenterHeight() - height - 50);

            motionTimer.Initialize();

            //あたり判定用（中心座標、長さ、高さ）
            collision = new MagicCollisionRange((int)position.X, (int)position.Y, 55, 55);

            //ターゲット位置設定
            SetTargetPosition();

            //経路設定（自分の中心座標、相手の中心座標、射程、射程までかかる時間）
            path = new Path_Line(position, targetPosition, 1000, 1.5f);
            path.Initialize();

            for (int i = 0; i < 8; i++)
            {
                motion.Add(i, new Rectangle((i % 4) * 64, (i / 4) * 64, 64, 64));
            }
            motion.Initialize(new Range(4, 7), new Timer(0.125f));
        }

        public override void Update(GameTime gameTime)
        {
            motionTimer.Update();

            switch (step)
            {
                case Step.Launch:
                    UpdateLaunch(gameTime);
                    IsCollision();
                    break;
                case Step.Path:
                    UpdatePath(gameTime);
                    IsCollision();
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
                effectAlpha -= 0.02f;
                if (effectAlpha < 0.7f)
                {
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha += 0.02f;
                if (effectAlpha >= 1)
                {
                    effectAlpha = 1;
                    effectBool = true;
                }
            }

            if (motionTimer.IsTime())
            {
                motion.Initialize(new Range(0, 3), new Timer(0.125f));
                step = Step.Path;
            }
        }
        private void UpdatePath(GameTime gameTime)
        {
            if (effectBool)
            {
                effectAlpha -= 0.02f;
                if (effectAlpha < 0.7f)
                {
                    effectBool = false;
                }
            }
            else
            {
                effectAlpha += 0.02f;
                if (effectAlpha >= 1)
                {
                    effectAlpha = 1;
                    effectBool = true;
                }
            }
            //経路更新
            path.Update(gameTime);
            //経路取得
            position = path.GetCurrentPosition();
            collision.Move(position, 32, 32);
            if (path.IsEnd())
            {
                step = Step.End;
                motionTimer.Change(60 * 0.5f);
                motionTimer.Initialize();
            }
        }
        private void UpdateEnd(GameTime gameTime)
        {
            effectAlpha = motionTimer.Rate();
            if (motionTimer.IsTime())
            {
                isEnd = true;
            }
        }

        public void SetTargetPosition()
        {
            if (castChara is Player)
            {
                try
                {
                    Character temp = characterManager.NearestEnemy();
                    targetPosition = new Vector2(
                        temp.GetCenterWidth(),
                        temp.GetCenterHeight());
                }
                catch (Exception e)
                {
                    targetPosition = position;
                    Console.WriteLine(e);
                }
            }
            else
            {
                Character temp = characterManager.TargetPlayer();
                targetPosition = new Vector2(
                        temp.GetCenterWidth(),
                        temp.GetCenterHeight());
            }
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, new Vector2(position.X - (width / 2), position.Y - (height / 2)) + gameDevice.GetDisplayModify(), motion.DrawRange(), effectAlpha);
        }


        //エネミーが当たった時の処理
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
            //cx.AddState(new Poison(5, 1, 1, gameDevice)); //毒を付属する　poison(second, damage, 時間間隔)
            motionTimer.Change(60 * 0.2f);
            motionTimer.Initialize();
            gameDevice.GetSound().PlaySE("fire");
            step = Step.End;
        }
        //プレヤーが当たった時の処理
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
            motionTimer.Change(60 * 0.2f);
            motionTimer.Initialize();
            gameDevice.GetSound().PlaySE("fire");
            step = Step.End;
        }


        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Block || gameObject is SlidingBlock || (gameObject is Door && !((Door)gameObject).GetStatus()) || (gameObject is SwitchBlock && ((SwitchBlock)gameObject).GetStatus()))
            {
                isEnd = true;
            }
        }

        public override void IsCollision()
        {
            if (castChara is Player)
            {
                List<Character> temp = characterManager.Enemies();
                foreach (Character cx in temp)
                {
                    if (collision.GetCollision().Intersects(cx.GetRectangle()))
                    {
                        HitEnemy(cx);
                        if (step == Step.End)
                        {
                            return;
                        }
                        //cx.AddState(new Freeze(20));      //足止め freeze(継続時間)
                        //cx.AddState(new Poison(5, 1, 1)); //毒を付属する例　poison(second, damage, 時間間隔)
                    }
                }
            }
            else
            {
                if (collision.GetCollision().Intersects(characterManager.TargetPlayer().GetRectangle()))
                {
                    HitPlayer(characterManager.TargetPlayer());
                }
            }
        }
    }
}
