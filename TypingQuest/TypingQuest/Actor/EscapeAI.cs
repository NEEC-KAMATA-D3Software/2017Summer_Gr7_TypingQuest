using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor
{
    class EscapeAI:AI
    {
        private Vector2 velocity = Vector2.Zero;
        private Player target;
        private Vector2 targetPosition;
        private int saveDistance;

        public EscapeAI(Player player, int saveDistance)
        {
            target = player;
            this.saveDistance = saveDistance;
        }

        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            targetPosition = target.GetPosition();


            if ((targetPosition - position).Length() < saveDistance)
            {
                velocity = position - targetPosition;
            }
            else
            {
                velocity = new Vector2(1, 1);
            }

            return velocity;
        }
    }
}
