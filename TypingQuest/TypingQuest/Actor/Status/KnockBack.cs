using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Actor;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Status
{
    class KnockBack : State
    {
        private Vector2 knockBackAmont;
        private Direct direct;
        public KnockBack(Vector2 knockBackAmont, Direct direct)
        {
            this.knockBackAmont = knockBackAmont;
            this.direct = direct;
            duration = new Timer(0.3f);
        }
        public override void Draw(Renderer renderer)
        {

        }

        public override void Initialize()
        {
            duration.Initialize();
            target.SetCanMove(false);
            //knockBackAmont.Normalize();

            if (direct == Direct.Right)
            {
                target.SetVelocity(knockBackAmont);
            }
            else
            {
                target.SetVelocity(new Vector2(-knockBackAmont.X, knockBackAmont.Y));
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            target.SetCanMove(false);
            duration.Update();
            if(duration.IsTime())
            {
                isEnd = true;
            }
        }
    }
}
