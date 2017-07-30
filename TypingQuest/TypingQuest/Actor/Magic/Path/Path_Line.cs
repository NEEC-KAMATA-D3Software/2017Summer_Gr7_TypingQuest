using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor.Magic.Path
{
    class Path_Line:PathAbstract
    {
        private float speed;
        private Vector2 velocity;
        public Path_Line(Vector2 startPosition, Vector2 endPosition, int length, float moveTimeSecond)
            :base(startPosition, length, moveTimeSecond)
        {
            this.endPosition = endPosition;
        }

        public override void Initialize()
        {
            moveTime.Initialize();
            speed = length / (moveTime.Now());
            velocity = endPosition - startPosition;
            velocity.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            moveTime.Update();
            currentPosition += (velocity * speed);
        }
    }
}
