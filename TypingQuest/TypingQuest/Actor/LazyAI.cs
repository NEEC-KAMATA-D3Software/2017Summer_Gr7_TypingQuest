using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor
{
    class LazyAI : AI
    {
        private Vector2 velocity = Vector2.Zero;
        private Player target;

        public LazyAI(Player player)
        {
            target = player;
        }
        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            return velocity;
        }
    }
}
