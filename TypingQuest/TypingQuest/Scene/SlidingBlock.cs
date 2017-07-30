using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class SlidingBlock:GameObject
    {
        private Vector2 leftPosition;
        private Vector2 rightPosition;
        private Vector2 velocity;
        public SlidingBlock(Vector2 leftPosition, Vector2 rightPosition, Vector2 velocity, GameDevice gameDevice)
            : base("block1", leftPosition, 64, 64, gameDevice)
        {
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;
            this.velocity = velocity;
        }
        public SlidingBlock(SlidingBlock other)
            : this(other.leftPosition, other.rightPosition, other.velocity, other.gameDevice)
        {
        }
        public override object Clone()
        {
            return new SlidingBlock(this);
        }
        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (velocity.Y == 0)
            {
                if (position.X > rightPosition.X)
                {
                    position.X = rightPosition.X;
                    velocity.X = -velocity.X;
                }
                if (position.X < leftPosition.X)
                {
                    position.X = leftPosition.X;
                    velocity.X = -velocity.X;
                }
                return;
            }
            else if(velocity.X == 0)
            {
                if (position.Y > rightPosition.Y)
                {
                    position.Y = rightPosition.Y;
                    velocity.Y = -velocity.Y;
                }
                if (position.Y < leftPosition.Y)
                {
                    position.Y = leftPosition.Y;
                    velocity.Y = -velocity.Y;
                }
                return;
            }
            
        }
    }
}
