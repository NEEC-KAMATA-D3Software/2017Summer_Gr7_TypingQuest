using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Status
{
    class Invincible : State
    {

        public Invincible(float second)
        {
            duration = new Timer(second);
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
            target.SetInvicible(true);
            duration.Update();
            if (duration.IsTime())
            {
                target.SetInvicible(false);
                isEnd = true;
            }
        }
    }
}
