using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;
using TypingQuest.Scene;
using TypingQuest.Actor.Status;
using TypingQuest.Actor.Magic.ComboSystem;

namespace TypingQuest.Actor
{
    public enum Direct
    {
        Right, Left
    }
    abstract class Character:GameObject
    {
        protected int maxHp;
        protected int hp;
        protected Direct direct;
        protected MagicManager magicManager;
        protected List<State> state;
        protected Vector2 velocity;
        protected ComboSystem comboSystem;
        protected bool canMove;
        protected bool isGround;
        protected bool isInvicible;
        protected bool gravity;

        public Character(string name, Vector2 position, int height, int width, GameDevice gameDevice, MagicManager magicManager)
            :base(name, position, height, width, gameDevice)
        {
            comboSystem = gameDevice.GetComboSystem();
            this.magicManager = magicManager;
            maxHp = 300;
            hp = maxHp;
            state = new List<State>();
            velocity = Vector2.Zero;
            canMove = true;
            isInvicible = false;
        }
        public abstract void Initialize();

        public void LaunchMagic(string spell)
        {
            magicManager.AddMagic(this, spell, position);
            AddState(new Freeze(0.3f));
        }
        public void Damage(int damage, Direct direct, bool isState = false)
        {
            //無敵状態は回復、状態系有効
            if (isInvicible && damage < 0 || isState)
            {
                hp -= damage;
                hp = (hp > maxHp) ? maxHp : hp;
            }
            else
            {
                hp -= damage;
                hp = (hp > maxHp) ? maxHp : hp;
            }
            if (isState == true || damage < 0)
            {
                return;
            }
            this.AddState(new KnockBack(new Vector2(1.8f, -1.2f), direct)); //撃退値
            if (this is Player )
            {
                return;
            }
            comboSystem.AddCombo();
        }

        public void Damage(int damage, Direct direct, Vector2 knockBack, bool isState = false)
        {
            //無敵状態は回復、状態系有効
            if (isInvicible && damage < 0 || isState)
            {
                hp -= damage;
                hp = (hp > maxHp) ? maxHp : hp;
            }
            else
            {
                hp -= damage;
                hp = (hp > maxHp) ? maxHp : hp;
            }
            if (isState == true || damage < 0)
            {
                return;
            }
            this.AddState(new KnockBack(knockBack, direct)); //撃退値
            if (this is Player)
            {
                return;
            }
            comboSystem.AddCombo();
        }

        #region 移動系 Method
        public void SetPosition(ref Vector2 otherPosition)
        {
            otherPosition = this.position;
        }
        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        public Vector2 GetVelocity()
        {
            return velocity;
        }
        public Direct GetDirection()
        {
            return direct;
        }
        public void SetCanMove(bool canMove)
        {
            this.canMove = canMove;
        }
        #endregion

        #region 描画系 Method
        public int GetCenterHeight()
        {
            return (int)(position.Y + (height / 2));
        }
        public int GetCenterWidth()
        {
            return (int)(position.X + (width / 2));
        }
        #endregion

        #region 状態系 Method
        protected void UpdateState(GameTime gameTime)
        {
            if (state.Count <= 0)
            {
                return;
            }
            state.ForEach((State st) => st.Update(gameTime));
            state.RemoveAll((State st) => st.IsEnd());
        }
        public void AddState(State state)
        {
            state.SetTarget(this);
            state.Initialize();
            this.state.Add(state);
        }
        public void DrawState(Renderer renderer)
        {
            state.ForEach((State st) => st.Draw(renderer));
        }
        public void SetInvicible(bool invicible)
        {
            this.isInvicible = invicible;
        }
        public bool IsInvicible()
        {
            return isInvicible;
        }
        #endregion
    }
}
