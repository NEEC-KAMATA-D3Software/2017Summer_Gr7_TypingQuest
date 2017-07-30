using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Actor
{
    class Boss_TraceAI:AI
    {
        private Vector2 velocity = Vector2.Zero;
        private Player traceTarget;
        private Vector2 traceTargetPosition;
        private int saveDistance;

        public Boss_TraceAI(Player player, int saveDistance)
        {
            traceTarget = player;
            this.saveDistance = saveDistance;
        }

        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            traceTargetPosition = traceTarget.GetPosition();


            if (Math.Abs((traceTargetPosition - position).X) > saveDistance)
            {
                velocity = traceTargetPosition - position;
            }
            else
            {
                velocity.Y = (traceTargetPosition - position).Y;
            }

            return velocity;
        }
    }
}
