using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class SpaceWall : GameObject
    {
        public SpaceWall(Vector2 position, GameDevice gameDevice)
            : base("space0", position, 64, 64, gameDevice)
        {
        }
        public SpaceWall(SpaceWall other)
            : this(other.position, other.gameDevice)
        {

        }
        public override object Clone()
        {
            return new SpaceWall(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
