using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Utility;

namespace TypingQuest.Actor.Magic.Path
{
    public enum Direct
    {
        UP, DOWN, RIGHT, LEFT
    }
    abstract class PathAbstract
    {
        protected Vector2 startPosition;
        protected Vector2 endPosition;
        protected Vector2 currentPosition;
        protected int length;
        protected Timer moveTime;

        public PathAbstract(Vector2 startPosition, int length, float moveTimeSecond)
        {
            this.startPosition = startPosition;
            currentPosition = startPosition;
            this.length = length;
            moveTime = new Timer(moveTimeSecond);
            endPosition = Vector2.Zero;
        }
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);

        public Vector2 GetCurrentPosition() { return currentPosition; }

        public bool IsEnd() { return moveTime.IsTime(); }
    }
}
