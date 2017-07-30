using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Status
{
    class Freeze : State
    {
        public Freeze(float duration)
        {
            this.duration.Change(duration * 60);
        }
        public override void Draw(Renderer renderer)
        {
        }

        public override void Initialize()
        {
            duration.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            duration.Update();
            if (duration.IsTime())
            {
                isEnd = true;
            }
            target.SetVelocity(new Vector2(0, target.GetVelocity().Y));
            target.SetCanMove(false);
        }
    }
}
