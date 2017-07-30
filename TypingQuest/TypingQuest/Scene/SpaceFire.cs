using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Scene
{
    class SpaceFire:GameObject
    {
        private Motion motion;

        public SpaceFire(Vector2 position, GameDevice gameDevice)
            : base("space2", position, 64, 64, gameDevice)
        {
            motion = new Motion();
            for (int i = 0; i < 4; i++)
            {
                motion.Add(i, new Rectangle(i * 64, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.2f));
        }
        public SpaceFire(SpaceFire other)
            : this(other.position, other.gameDevice)
        {

        }
        public override object Clone()
        {
            return new SpaceFire(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
            motion.Update(gameTime);
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange());
        }
    }
}
