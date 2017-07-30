using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Utility;

namespace TypingQuest.Actor
{
    class BoundAI :AI
    {
        private float velocityX;
        private Timer timer;

        public BoundAI()
        {
            velocityX = -1.2f;
            timer = new Timer(2);
            timer.Initialize();
        }

        //移動不可に応じて改変
        public override Vector2 Think(Character character)
        {
            Vector2 velocity = character.GetVelocity();
            timer.Update();
            velocity.X = velocityX;

            if (timer.IsTime())
            {
                velocityX = -velocityX;
                timer.Initialize();
            }

            return velocity;
        }
    }
}
