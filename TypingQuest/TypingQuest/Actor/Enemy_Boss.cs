using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Scene;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;
using TypingQuest.Utility;
using TypingQuest.Def;

namespace TypingQuest.Actor
{
    class Enemy_Boss : Character
    {
        private Timer moveTimer;
        //private AI ai;
        private MagicSpell magicSpell;

        private Player player;

        private Timer timer;
        private string magic;
        private List<string> magicList;
        private int nowSpell;

        private float alpha;
        private Motion motion;
        private float effectAlpha;
        private bool effectSwitch;

        public Enemy_Boss(Player player, Vector2 position, GameDevice gameDevice, MagicManager magicManager)
            : base("BOSS_motion", position, 256, 256, gameDevice, magicManager)
        {
            maxHp = 1000;
            hp = maxHp;

            this.player = player;
            moveTimer = new Timer(2);

            magicSpell = new MagicSpell();
            timer = new Timer(0.3f);
            magicList = new List<string>();
            magicList.Add("FIRE ");
            magicList.Add("WATER ");
            magicList.Add("WIND ");
            magicList.Add("ROCK ");
            magicList.Add("GRAVITY ");
            magicList.Add("HEAL ");
            magicList.Add("CYCLONE ");
            magicList.Add("LIGHTNING ");

            magic = magicList[0];

            gravity = true;

            alpha = 0;
            motion = new Motion();
            effectAlpha = 0.0f;
            effectSwitch = true;

            Initialize();
        }


        public override void Initialize()
        {
            moveTimer.Initialize();
            magicSpell.Initialize();
            nowSpell = 0;
            timer.Initialize();

            //ai = new BoundAI();

            direct = Direct.Right;

            for (int i = 0; i < 6; i++)
            {
                motion.Add(i, new Rectangle((i * 256), 0, 256, 256));
            }
            motion.Initialize(new Range(0, 5), new Timer(0.3f));
        }

        public override void Update(GameTime gameTime)
        {
            if (hp <= 0)
            {
                isDead = true;
            }

            moveTimer.Update();
            if (canMove)
            {
                //velocity.X = ai.Think(this).X;
                if (moveTimer.IsTime())
                {
                    if (player.GetPosition().X < position.X - 64)
                    {
                        velocity = new Vector2(64 * 1, 64 * 19) - position;
                        if(velocity.Length() > 0)
                            velocity.Normalize();
                    }
                    else if (player.GetPosition().X > position.X + 256)
                    {
                        velocity = new Vector2(64 * 21, 64 * 19) - position;
                        if (velocity.Length() > 0)
                            velocity.Normalize();
                    }
                    moveTimer.Initialize();
                }


                if (velocity.X >= 0)
                {
                    direct = Direct.Right;
                }
                else
                {
                    direct = Direct.Left;
                }
            }
            canMove = true;

            UpdateState(gameTime);


            //移動系状態異常があるためvelocityとpositionの間に行います

            //重力
            if (gravity)
            {
                velocity.Y += Gravity.gravaity / 60;
                velocity.Y = (velocity.Y > 16.0f) ? (16.0f) : (velocity.Y);
            }
            else
            {
                velocity.Y = (velocity.Y > 0.0f) ? (0.0f) : velocity.Y;
            }

            gravity = true;


            float speedY = 12;


            if (canMove)
            {
                float speedX = 7;
                position.X = position.X + velocity.X * speedX;
            }
            else
            {
                position.X = position.X + velocity.X;
            }
            position.Y = position.Y + velocity.Y * speedY;

            timer.Update();

            motion.Update(gameTime);
            if (effectSwitch)
            {
                effectAlpha += 0.01f;
                if (effectAlpha > 0.9)
                {
                    effectSwitch = false;
                }
            }
            else
            {
                effectAlpha -= 0.01f;
                if (effectAlpha < 0.03)
                {
                    effectSwitch = true;
                }
            }
            var min = new Vector2(64, 0);
            var max = new Vector2(64 * 22 - 256, 64 * 20 - 256);
            position = Vector2.Clamp(position, min, max);

            SpellMagic();
        }
        public void SpellMagic()
        {
            if (timer.IsTime())
            {
                magicSpell.AddSpellString(magic[nowSpell].ToString());
                nowSpell++;
                if (nowSpell == magic.Length - 1)
                {
                    if ((position - player.GetPosition()).X > 0)
                    {
                        direct = Direct.Left;
                    }
                    else
                    {
                        direct = Direct.Right;
                    }
                    LaunchMagic(magicSpell.SpellToString());

                }
                else if (nowSpell >= magic.Length)
                {
                    magicSpell.Initialize();
                    nowSpell = 0;
                    JudgeMagic();
                }
                timer.Initialize();
            }
        }

        private void JudgeMagic()
        {
            if (hp > maxHp * 0.6f)
            {
                magic = magicList[(int)(BossMode.Fire)];
                timer = new Timer(0.3f);
                return;
            }
            if (hp > maxHp * 0.3f)
            {
                if ((player.GetPosition() - position).Length() < 256)
                {
                    magic = magicList[(int)(BossMode.Cyclone)];
                    timer = new Timer(0.25f);
                    return;
                }
                if ((player.GetPosition() - position).Length() > 600)
                {
                    magic = magicList[(int)(BossMode.Fire)];
                    timer = new Timer(0.2f);
                    return;
                }
                magic = magicList[(int)(BossMode.Gravity)];
                timer = new Timer(0.2f);
                return;
            }
            else
            {
                Random rnd = new Random();
                if (rnd.Next(1, 101) > 90)
                {
                    magic = magicList[(int)(BossMode.Heal)];
                    timer = new Timer(1);
                    return;
                }
                if (rnd.Next(1, 101) > 30)
                {
                    magic = magicList[(int)(BossMode.Lightning)];
                    timer = new Timer(0.01f);
                    return;
                }
                if ((player.GetPosition() - position).Length() < 256)
                {
                    magic = magicList[(int)(BossMode.Cyclone)];
                    timer = new Timer(0.15f);
                    return;
                }
                if ((player.GetPosition() - position).Length() > 600)
                {
                    magic = magicList[(int)(BossMode.Fire)];
                    timer = new Timer(0.01f);
                    return;
                }
                magic = magicList[(int)(BossMode.Gravity)];
                timer = new Timer(0.15f);
                return;
            }
        }

        public override void Draw(Renderer renderer)
        {
            float rate = hp / (maxHp * 1.0f);
            if (direct == Direct.Right)
            {
                renderer.DrawTexture(
                name,
                position + gameDevice.GetDisplayModify(),
                motion.DrawRange(),
                alpha);

                renderer.DrawTexture(
                "BOSS_light",
                position + gameDevice.GetDisplayModify(),
                motion.DrawRange(),
                effectAlpha);
            }
            else
            {
                renderer.DrawTexture(
                name,
                position + gameDevice.GetDisplayModify(),
                motion.DrawRange(),
                0,
                Vector2.Zero,
                new Vector2(1, 1),
                SpriteEffects.FlipHorizontally,
                alpha);

                renderer.DrawTexture(
                "BOSS_light",
                position + gameDevice.GetDisplayModify(),
                motion.DrawRange(),
                0,
                Vector2.Zero,
                new Vector2(1, 1),
                SpriteEffects.FlipHorizontally,
                effectAlpha);
            }



            renderer.DrawTexture("enemy_HP_G", new Vector2(
                    GetCenterWidth() - (108),
                    position.Y + height + 5) + gameDevice.GetDisplayModify(),
                    new Rectangle(0, 0, 72, 19),
                    0,
                    Vector2.Zero,
                    new Vector2(3, 1)
                    );
            renderer.DrawTexture(
                    "enemy_HP",
                    new Vector2(
                        GetCenterWidth() - (108),
                        position.Y + height + 5) +
                        gameDevice.GetDisplayModify(),
                    new Rectangle(0, 0, 72, 19),
                    0,
                    Vector2.Zero,
                    new Vector2(rate * 3, 1)
                    );

            DrawSpell(renderer);

            DrawState(renderer);
        }
        public void DrawSpell(Renderer renderer)
        {
            renderer.DrawString(
                magicSpell.SpellToString(),
                new Vector2(GetCenterWidth() - (magicSpell.SpellCount() * 14), position.Y - 50) + gameDevice.GetDisplayModify());
        }
        public void ScrollDisplayToBoss()
        {
            Vector2 modifyPosition = new Vector2(position.X + 128, position.Y - 128);
            Vector2 cameraPosition = gameDevice.GetCameraPosition();
            if ((modifyPosition - cameraPosition).Length() > 60)
            {
                gameDevice.ScrollCamera(modifyPosition - cameraPosition);
            }
        }
        public void SetAlpha(float alpha)
        {
            this.alpha = alpha;
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
