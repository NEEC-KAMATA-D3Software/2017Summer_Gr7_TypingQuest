using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class SwitchBlock : GameObject
    {
        private Vector2 leftPosition;
        private Vector2 rightPosition;
        private Vector2 velocity;
        private bool status;
        public SwitchBlock(string name , Vector2 startPostion, Vector2 leftPosition, Vector2 rightPosition, Vector2 velocity, GameDevice gameDevice, bool status = false)
            :base(name, startPostion, 64, 64, gameDevice)
        {
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;
            this.velocity = velocity;
            this.status = status;
        }
        public SwitchBlock(SwitchBlock other)
            :this(other.name, other.position, other.leftPosition, other.rightPosition, other.velocity, other.gameDevice)
        {
        }
        public override object Clone()
        {
            return new SwitchBlock(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }
        public void Flip()
        {
            status = !status;
        }
        public override void Update(GameTime gameTime)
        {
            if (!status)
            {
                return;
            }
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
            else if (velocity.X == 0)
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

        public override void Draw(Renderer renderer)
        {
            if (!status)
            {
                return;
            }
            base.Draw(renderer);
        }
        public Vector2 GetVelocity()
        {
            return velocity;
        }
        public bool GetStatus()
        {
            return status;
        }
    }
}
