using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor.Magic
{
    public enum TransformDirect
    {
        UP, DOWN, LEFT, RIGHT
    }
    class MagicCollisionRange
    {
        private Rectangle collisionRect;

        public MagicCollisionRange(int centerX, int centerY, int width, int height)
        {
            collisionRect = new Rectangle(centerX - (width / 2), centerY - (height / 2), width, height);
        }
        public Rectangle GetCollision() { return collisionRect; }
        public Rectangle DrawRange()
        {
            return new Rectangle(0, 0, collisionRect.Width, collisionRect.Height);
        }
        public void Transform(TransformDirect direct, int amount)
        {
            switch (direct)
            {
                case TransformDirect.UP:
                    collisionRect.Y -= amount;
                    collisionRect.Height += amount;
                    break;
                case TransformDirect.DOWN:
                    collisionRect.Height += amount;
                    break;
                case TransformDirect.LEFT:
                    collisionRect.X -= amount;
                    collisionRect.Width += amount;
                    break;
                case TransformDirect.RIGHT:
                    collisionRect.Width += amount;
                    break;
            }
        }
        public void Transform(TransformDirect direct, Vector2 startPosition, Vector2 currentPosition)
        {

            switch (direct)
            {
                case TransformDirect.UP:
                    collisionRect.Y = (int)currentPosition.Y;
                    collisionRect.Height = (int)(currentPosition.Y - startPosition.Y);
                    break;
                case TransformDirect.DOWN:
                    collisionRect.Height = (int)(startPosition.Y - currentPosition.Y);
                    break;
                case TransformDirect.LEFT:
                    collisionRect.X = (int)currentPosition.X;
                    collisionRect.Width = (int)(startPosition.X - currentPosition.X);
                    break;
                case TransformDirect.RIGHT:
                    collisionRect.Width = (int)(currentPosition.X - startPosition.X);
                    break;
            }
        }
        public void Move(Vector2 centerPosition, int width, int height)
        {
            collisionRect.X = (int)centerPosition.X - width / 2;
            collisionRect.Y = (int)centerPosition.Y - height / 2;
        }
    }
}
