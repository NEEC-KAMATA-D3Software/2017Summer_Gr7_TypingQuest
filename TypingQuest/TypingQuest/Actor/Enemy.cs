using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Device;
using TypingQuest.Def;
using TypingQuest.Actor.Magic;
using TypingQuest.Utility;
using TypingQuest.Scene;

namespace TypingQuest.Actor
{
    class Enemy : Character
    {
        private Random rnd;
        private AI ai;
        private MagicSpell magicSpell;

        private Vector2 slideModify;

        private Timer timer;
        private string magic = "FIRE ";
        private int nowSpell;
        private Player player;

        private Motion motion;

        public Enemy(Player player, AI ai, Vector2 position, GameDevice gameDevice, MagicManager magicManager, string magic, Random rnd)  
            :base("fire_enemy_motion", position, 64, 64, gameDevice, magicManager)
        {
            this.rnd = rnd;
            timer = new Timer(rnd.Next(80, 120) / 100.0f);
            this.ai = ai;
            magicSpell = new MagicSpell();

            this.player = player;

            this.magic = magic;

            gravity = true;
            slideModify = Vector2.Zero;

            motion = new Motion();
            Initialize();
        }
        public Enemy(Enemy enemy):
            this(enemy.player, enemy.ai, enemy.position, enemy.gameDevice, enemy.magicManager, enemy.magic, enemy.rnd)
        {
        }

        public  override void Initialize()
        {
            ChangeTexture();

            magicSpell.Initialize();
            nowSpell = 0;
            timer.Initialize();

            for(int i = 0; i < 8; i++)
            {
                motion.Add(i, new Rectangle((i % 4) * 64, (i / 4) * 64, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.4f));
        }

        public override void Update(GameTime gameTime)
        {
            if (hp <= 0)
            {
                isDead = true;
            }

            if (canMove)
            {
                velocity.X = ai.Think(this).X;
                if (velocity.X >= 0)
                {
                    direct = Direct.Right;
                }
                else
                {
                    direct = Direct.Left;
                }
                if (ai is LazyAI)
                {
                    if ((position - player.GetPosition()).X > 0)
                    {
                        direct = Direct.Left;
                    }
                    else
                    {
                        direct = Direct.Right;
                    }
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
                float speedX = 3;
                position.X = position.X + velocity.X * speedX + slideModify.X;
            }
            else
            {
                position.X = position.X + velocity.X + slideModify.X;
            }
            position.Y = position.Y + velocity.Y * speedY + slideModify.Y;


            timer.Update();
            SpellMagic();

            motion.Update(gameTime);
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
                    motion.Initialize(new Range(4, 7), new Timer(0.1f));

                }
                else if (nowSpell >= magic.Length)
                {
                    magicSpell.Initialize();
                    nowSpell = 0;
                    motion.Initialize(new Range(0, 3), new Timer(0.4f));
                }
                timer.Initialize();
            }
        }

        public override void Draw(Renderer renderer)
        {
            float rate = hp / (maxHp * 1.0f);

            if (direct == Direct.Right)
            {
                renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange());
            }
            else
            {
                renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange(), 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally);
            }



            renderer.DrawTexture("enemy_HP_G", new Vector2(
                    GetCenterWidth() - (36),
                    position.Y + height + 5) + gameDevice.GetDisplayModify());
            renderer.DrawTexture(
                    "enemy_HP", 
                    new Vector2(
                        GetCenterWidth() - (36),
                        position.Y + height + 5) + 
                        gameDevice.GetDisplayModify(),
                    new Rectangle(0, 0, 72, 19),
                    0,
                    Vector2.Zero,
                    new Vector2(rate, 1)
                    );

            /*renderer.DrawString(
                hp.ToString(), 
                new Vector2(
                    GetCenterWidth() - (hp.ToString().Length * 15), 
                    position.Y + height + 5) + gameDevice.GetDisplayModify());*/

            DrawSpell(renderer);

            DrawState(renderer);
        }
        public void DrawSpell(Renderer renderer)
        {
            renderer.DrawString(
                magicSpell.SpellToString(), 
                new Vector2(GetCenterWidth() - (magicSpell.SpellCount() * 14), position.Y - 50) + gameDevice.GetDisplayModify());
        }

        public override object Clone()
        {
            return new Enemy(this);
        }

        public override void Hit(GameObject gameObject)
        {
            Scene.Direction dir = this.CheckDirection(gameObject);
            if (gameObject is Block || gameObject is SlidingBlock || (gameObject is Door && !((Door)gameObject).GetStatus()) || (gameObject is SwitchBlock && ((SwitchBlock)gameObject).GetStatus()))
            {
                if (dir == Scene.Direction.Top)
                {
                    if (position.Y > 0.0f)
                    {
                        position.Y = gameObject.GetRectangle().Top - height + 1;
                        gravity = false;
                        isGround = true;
                    }
                    if (gameObject is SwitchBlock)
                    {
                        slideModify = ((SwitchBlock)gameObject).GetVelocity();
                    }
                }
                else if (dir == Scene.Direction.Right)
                {
                    position.X = gameObject.GetRectangle().Right;
                }
                else if (dir == Scene.Direction.Left)
                {
                    position.X = gameObject.GetRectangle().Left - width;
                }
                else if (dir == Scene.Direction.Bottom)
                {
                    position.Y = gameObject.GetRectangle().Bottom;
                    velocity.Y = 0;
                }
            }
            if ((gameObject is SlidingBlock && dir == Scene.Direction.Top))
            {
                position.Y = gameObject.GetRectangle().Top - height + 1;
                gravity = false;
                isGround = true;
                slideModify = ((SlidingBlock)gameObject).GetVelocity();
            }
            else if (!(gameObject is SlidingBlock) && !(gameObject is SwitchBlock) && !(gameObject is Button))
            {
                slideModify = Vector2.Zero;
            }
        }

        private void ChangeTexture()
        {
            switch (magic)
            {
                case "FIRE ":
                    name = "fire_enemy_motion";
                    break;
                case "WIND ":
                    name = "wind_enemy_motion";
                    break;
                case "WATER ":
                    name = "water_enemy_motion";
                    break;
                case "ROCK ":
                    name = "rock_enemy_motion";
                    break;
                case "GRAVITY ":
                    name = "rock_enemy_motion";
                    break;
                default:
                    name = "water_enemy_motion";
                    break;
            }
        }
    }
}
