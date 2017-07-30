using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TypingQuest.Def;
using TypingQuest.Utility;
using TypingQuest.Device;

namespace TypingQuest.EffectFolder
{
    class DamageFilter : IFilter
    {
        private DamageEffect damageEffect;
        private GraphicsDevice graphics;
        private RenderTarget2D target;

        private Vector2 position;
        private Vector2 velocity;
        private readonly float speed;
        private bool isStart;

        public DamageFilter(GraphicsDevice graphics, DamageEffect damageEffect)
        {
            this.graphics = graphics;
            this.damageEffect = damageEffect;
            speed = 0.01f;
        }
        public void Initialize()
        {
            target = new RenderTarget2D(graphics, Screen.Width, Screen.Height);
            position = Vector2.Zero;
            velocity = new Vector2(-speed, 0);
            isStart = false;
        }

        public void Update()
        {
            if (isStart)
            {
                VelocityUpdate();
                damageEffect.Update(target, position.X, position.Y);
            }
            
        }

        public void VelocityUpdate()
        {
            position += velocity;
            if (position.X < -0.01f && velocity.X < 0)
            {
                velocity = new Vector2(0, -speed);
            }
            else if (position.Y < -0.01f && velocity.Y < 0)
            {
                velocity = new Vector2(speed, 0);
            }
            else if (position.X > 0 && velocity.X > 0)
            {
                velocity = new Vector2(0, speed);
            }
            else if (position.X > 0 && position.Y > 0)
            {
                position = Vector2.Zero;
                velocity = new Vector2(-speed, 0);
                isStart = false;
            }
        }

        public void ReleaseRenderTarget()
        {
            graphics.SetRenderTarget(null);
        }

        public void WriteRenderTarget()
        {
            graphics.SetRenderTarget(target);
            graphics.Clear(Color.White);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin(damageEffect.GetEffect());

            renderer.DrawTexture(target, Vector2.Zero);
            
            renderer.End();
        }

        public void GetStart()
        {
            isStart = true;
        }
        public bool IsStart()
        {
            return isStart;
        }
        public RenderTarget2D GetRenderTarget()
        {
            return target;
        }
    }
}
