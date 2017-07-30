using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Utility;
using TypingQuest.Device;

namespace TypingQuest.Actor.Status
{
    abstract class State
    {
        protected Timer duration;
        protected int amount;
        protected bool isEnd;
        protected Character target;

        public State()
        {
            duration = new Timer();
            amount = 0;
            isEnd = false;
        }
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(Renderer renderer);
        public bool IsEnd() { return isEnd; }
        public void SetTarget(Character target)
        {
            this.target = target;
        }
    }
}
