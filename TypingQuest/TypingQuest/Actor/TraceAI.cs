using Microsoft.Xna.Framework;

namespace TypingQuest.Actor
{
    class TraceAI : AI
    {
        private Vector2 velocity = Vector2.Zero;
        private Player traceTarget;
        private Vector2 traceTargetPosition;

        public TraceAI(Player player)
        {
            traceTarget = player;
        }

        //マップ判定追加したら書き直し必要があります
        public override Vector2 Think(Character character)
        {
            character.SetPosition(ref position);

            traceTargetPosition = traceTarget.GetPosition();

            if (position.X - traceTargetPosition.X < 32) velocity.X = 1.0f;
            else velocity.X = -1.0f;

            return velocity;
        }
    }
}
