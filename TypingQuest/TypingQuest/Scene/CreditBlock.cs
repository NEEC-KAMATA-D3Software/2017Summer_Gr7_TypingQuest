using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class CreditBlock : GameObject
    {

        public CreditBlock(Vector2 position, GameDevice gameDevice)
            :base("creditBlock", position, 64, 64, gameDevice)
        {
        }
        public CreditBlock(CreditBlock other)
            :this(other.position, other.gameDevice)
        {
        }
        public override object Clone()
        {
            return new CreditBlock(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
