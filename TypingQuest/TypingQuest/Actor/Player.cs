using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;
using TypingQuest.Def;
using TypingQuest.Scene;
using TypingQuest.Utility;
using TypingQuest.EffectFolder;

namespace TypingQuest.Actor
{
    class Player : Character
    {
        private InputState input;
        private PlayerMagicSpell magicSpell;
        private bool nextStage;
        private bool isTitle;

        private Vector2 slideModify;

        private HpEffect hpEffect;
        private float effectAlpha;
        private bool effectSwitch;

        #region Motion
        private Motion motion;
        private enum Direction
        {
            RIGHT, LEFT, STOP_R, STOP_L
        };
        private Direction direction;
        #endregion

        public Player(Vector2 position, GameDevice gameDevice, MagicManager magicManager):
            base("player_motion2", position, 64, 64, gameDevice, magicManager)
        {
            this.input = gameDevice.GetInputState();
            magicSpell = new PlayerMagicSpell(input);
            isGround = false;
            gravity = true;
            nextStage = false;
            maxHp = 700;
            hp = maxHp;
            isTitle = false;
            direction = Direction.STOP_L;

            hpEffect = new HpEffect(gameDevice);
            effectAlpha = 0.5f;
            effectSwitch = true;
        }
        public Player(Player player) :
            this(player.position, player.gameDevice, player.magicManager)
        {
        }

        public override void Initialize()
        {
            nextStage = false;
            magicSpell.Initialize();

            #region Motion
            motion = new Motion();
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 64 * 0, 64, 64));
            }
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i + 4, new Rectangle(64 * i, 64 * 1, 64, 64));
            }
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i + 8, new Rectangle(64 * i, 64 * 2, 64, 64));
            }
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i + 12, new Rectangle(64 * i, 64 * 3, 64, 64));
            }
            motion.Initialize(new Range(8, 11), new Timer(0.2f));
            #endregion

            direction = Direction.STOP_L;
            direct = Direct.Left;

            SetDisplayModify();
        }
        public override void Update(GameTime gameTime)
        {
            if (hp <= 0)
            {
                isDead = true;
            }

            if (effectSwitch)
            {
                effectAlpha += 0.01f;
                if (effectAlpha > 0.8f)
                {
                    effectSwitch = false;
                }
            }
            else
            {
                effectAlpha -= 0.01f;
                if (effectAlpha < 0.5f)
                {
                    effectSwitch = true;
                }
            }

            if (input.GetKeyTrigger(Keys.Enter))
            {
                LaunchMagic(magicSpell.SpellToString());
                magicSpell.Initialize();
            }

            if (!isTitle)
            {
                magicSpell.Update();
            }



            if (canMove)
            {
                Move(gameTime);
            }
            canMove = true;

            UpdateState(gameTime);

            float speedX = 5;
            float speedY = 9;

            if (canMove)
            {
                position.X = position.X + (velocity.X * speedX) + slideModify.X;
            }
            else
            {
                position.X = position.X + velocity.X + slideModify.X;
            }

            if (gravity)
            {
                velocity.Y += Gravity.gravaity / 60;
                velocity.Y = (velocity.Y > 16.0f) ? (16.0f) : (velocity.Y);
                isGround = false;
            }
            else
            {
                velocity.Y = (velocity.Y > 0.0f) ? (0.0f) : velocity.Y;
            }

            gravity = true;

            position.Y = position.Y + velocity.Y * speedY + slideModify.Y;

            //if (position.Y >= Screen.Ground - height)
            //{
            //    isGround = true;
            //}

            //行動範囲\\\
            //var min = new Vector2(-Screen.Width / 2, 0);
            //var max = new Vector2(gameDevice.GetStageLength() - Screen.Width / 2 - width, Screen.Ground - height);
            //position = Vector2.Clamp(position, min, max);

            if (!isTitle)
            {
                ScrollDisplay();
            }

            hpEffect.Update();

            #region Motion
            UpdateMotion(velocity);
            motion.Update(gameTime);
            #endregion
        }
        private void Move(GameTime gameTime)
        {
            velocity.X = input.Velocity();
            if (input.GetKeyTrigger(Keys.Left))
            {
                direct = Direct.Left;
            }
            if (input.GetKeyTrigger(Keys.Right))
            {
                direct = Direct.Right;
            }
            if (isGround)
            {
                if (input.GetKeyTrigger(Keys.Space))
                {
                    velocity.Y = -2.5f;//ジャンプの高さ（最初のスピード）
                    isGround = false;
                }
            }

        }
        private void UpdateMotion(Vector2 velocity)
        {
            Timer timer = new Timer(0.2f);

            if (velocity.X == 0.0f && direct == Direct.Right)
            {
                direction = Direction.STOP_R;
                motion.Initialize(new Range(12, 15), timer);
                return;
            }
            else if (velocity.X == 0.0f && direct == Direct.Left)
            {
                direction = Direction.STOP_L;
                motion.Initialize(new Range(8, 11), timer);
                return;
            }

            if ((velocity.X > 0.0f) && (direction != Direction.RIGHT))
            {
                direction = Direction.RIGHT;
                motion.Initialize(new Range(4, 7), timer);
            }
            else if ((velocity.X < 0.0f) && (direction != Direction.LEFT))
            {
                direction = Direction.LEFT;
                motion.Initialize(new Range(0, 3), timer);
            }

        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange());

            DrawState(renderer);
        }
        public void DrawHP(Renderer renderer)
        {
            float hpRate = hp / (maxHp / 1.0f);

            renderer.DrawTexture(
               "hp_out_2",
               new Vector2(50, 43));

            //renderer.DrawTexture(
            //    "hp_in",
            //    new Vector2(50, 43),
            //    new Rectangle(0, 0, 400, 64),
            //    0,
            //    Vector2.Zero,
            //    new Vector2(hpRate, 1));

            renderer.End();

            renderer.Begin(hpEffect.GetEffect());

            renderer.DrawTexture(
                "magicHP_in",
                new Vector2(54, 43),
                new Rectangle(0, 0, 99, 64),
                0,
                Vector2.Zero,
                new Vector2(hpRate * 4, 1),effectAlpha);
            renderer.End();

            renderer.Begin();

            renderer.DrawTexture(
                "hp_out",
                new Vector2(0, 35));

            if (hp < maxHp * 0.6f)
            {
                renderer.DrawTexture("warning", Vector2.Zero, 0.6f - hpRate);
            }
        }
        public void DrawUI(Renderer renderer)
        {
            renderer.DrawString(
                magicSpell.SpellToString(), 
                new Vector2(
                    Screen.Width / 2 - (magicSpell.SpellCount() * 12), 
                    Screen.Height - Screen.UI + 16),
                new Color(0.3f, 0.3f, 0.3f),
                Vector2.Zero,
                new Vector2(1, 1));
        }
        private void SetDisplayModify()
        {
            gameDevice.SetDisplayModify(position);
        }
        private void ScrollDisplay()
        {
            Vector2 cameraPosition = gameDevice.GetCameraPosition();
            if ((position - cameraPosition).Length() > 50)
            {
                gameDevice.ScrollCamera(position - cameraPosition);
            }
        }
        public void StagePosition(Vector2 startPosition)
        {
            position = startPosition;
            nextStage = false;
        }
        public override object Clone()
        {
            return new Player(this);
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
            if (gameObject is Gate)
            {
                if (input.GetKeyState(Keys.Up) && position.Y < 500)
                {
                    nextStage = true;
                }
                else
                {
                    nextStage = false;
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
        public bool IsNext()
        {
            return nextStage;
        }
        public void SetNext(bool isNext)
        {
            nextStage = isNext;
        }

        public void TitlePosition()
        {
            var min = new Vector2(0, 0);
            var max = new Vector2(Screen.Width - 64, gameDevice.GetStageHeight() - 64);
            position = Vector2.Clamp(position, min, max);
            isTitle = true;
        }
    }
}
