using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class Space : GameObject
    {
        public Space(Vector2 position, GameDevice gameDevice)
            : base("", position, 64, 64, gameDevice)
        {
        }
        public Space(Space other)
            : this(other.position, other.gameDevice)
        {

        }
        public override object Clone()
        {
            return new Space(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
