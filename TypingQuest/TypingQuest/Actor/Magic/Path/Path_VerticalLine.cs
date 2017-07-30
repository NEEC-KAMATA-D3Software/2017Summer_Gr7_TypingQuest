using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor.Magic.Path
{
    class Path_VerticalLine : PathAbstract
    {
        private float speed;
        private Vector2 velocity;
        public Path_VerticalLine(Vector2 startPosition, int length, float moveTimeSecond, Direct direct)
            :base(startPosition, length, moveTimeSecond)
        {
            switch (direct)
            {
                case Direct.UP:
                    endPosition = new Vector2(startPosition.X, startPosition.Y - length);
                    break;
                case Direct.DOWN:
                    endPosition = new Vector2(startPosition.X, startPosition.Y + length);
                    break;
                case Direct.LEFT:
                    endPosition = new Vector2(startPosition.X - length, startPosition.Y);
                    break;
                case Direct.RIGHT:
                    endPosition = new Vector2(startPosition.X + length, startPosition.Y);
                    break;
            }
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
