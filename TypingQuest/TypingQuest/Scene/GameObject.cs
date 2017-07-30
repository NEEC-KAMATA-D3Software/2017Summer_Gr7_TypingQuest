using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    public enum Direction
    {
        Top, Bottom, Left, Right
    }
    abstract class GameObject : ICloneable
    {
        protected string name;
        protected Vector2 position;
        protected int height;
        protected int width;
        protected bool isDead;
        protected GameDevice gameDevice;

        protected GameObjectID id;
        public GameObject(string name, Vector2 position, int height, int width, GameDevice gameDevice)
        {
            this.name = name;
            this.position = position;
            this.height = height;
            this.width = width;
            this.gameDevice = gameDevice;
        }
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public abstract object Clone();
        public abstract void Update(GameTime gameTime);
        public abstract void Hit(GameObject gameObject);
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModifyMap());
        }
        public bool IsDead()
        {
            return isDead;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(
                (int)position.X, 
                (int)position.Y,
                width,
                height);
        }
        public Vector2 GetSize()
        {
            return new Vector2(width, height);
        }
        public bool Collision(GameObject otherObj)
        {
            return this.GetRectangle().Intersects(otherObj.GetRectangle());
        }
        public Direction CheckDirection(GameObject otherObj)
        {
            Point thisCenter = this.GetRectangle().Center;
            Point otherCenter = otherObj.GetRectangle().Center;
            Vector2 dir = new Vector2(
                thisCenter.X - otherCenter.X,
                thisCenter.Y - otherCenter.Y);

            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
            else
            {
                if (dir.Y > 0)
                    return Direction.Bottom;
            }
            return Direction.Top;
        }

        public GameObjectID GetID()
        {
            return id;
        }
        public void SetID(GameObjectID id)
        {
            this.id = id;
        }
    }
}
