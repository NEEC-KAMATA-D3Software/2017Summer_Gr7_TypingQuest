using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class Block:GameObject
    {
        public Block(Vector2 position, GameDevice gameDevice)
           : base("block1", position, 64, 64, gameDevice)
        {
        }
        public Block(Block other)
            : this(other.position, other.gameDevice)
        {
        }
        public override object Clone()
        {
            return new Block(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
