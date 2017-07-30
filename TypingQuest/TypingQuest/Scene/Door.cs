using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Utility;

namespace TypingQuest.Scene
{
    enum TimerType
    {
        Block,
        Space
    }
    class Door:GameObject
    {
        private bool status;
        private GameObjectID linkedGameObjectID;
        private IGameObjectMediator mediator;

        private bool timer;
        private Timer switchTimer;
        private TimerType type;
        public Door(string name, Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator, bool status = false, bool timer = false, float second = 15, TimerType type = TimerType.Block)
            : base(name, position, 64, 64, gameDevice)
        {
            this.mediator = mediator;
            this.status = status;
            this.timer = timer;
            switchTimer = new Timer(second);
            switchTimer.Initialize();

            this.type = type;
        }
        public Door(Door other)
            : this(other.name, other.position, other.gameDevice, other.mediator)
        {
        }

        public void Operation(bool status)
        {
            this.status = status;
        }
        public bool GetStatus()
        {
            return status;
        }
        public void Flip()
        {
            status = !status;
            if (timer == false)
            {
                return;
            }
            switchTimer.Initialize();
        }
        public override void Draw(Renderer renderer)
        {
            if (status)
            {
                return;
            }
            base.Draw(renderer);
        }
        public override object Clone()
        {
            return new Door(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (timer == false)
            {
                return;
            }
            switch (type)
            {
                case TimerType.Block:
                    if (status == false)
                    {
                        switchTimer.Update();
                        if (switchTimer.IsTime())
                        {
                            status = !status;
                            switchTimer.Initialize();
                            SwitchOff();
                        }
                    }
                    break;

                case TimerType.Space:
                    if (status == true)
                    {
                        switchTimer.Update();
                        if (switchTimer.IsTime())
                        {
                            status = !status;
                            switchTimer.Initialize();
                            SwitchOff();
                        }
                    }
                    break;
            }
            
        }

        public void SetLinkedGameObjectID(GameObjectID id)
        {
            linkedGameObjectID = id;
        }

        public GameObjectID GetLinkedGameObjectID()
        {
            return linkedGameObjectID;
        }

        private void SwitchOff()
        {
            List<GameObject> buttonList = mediator.GetGameObjectList(GetLinkedGameObjectID());
            foreach (var d in buttonList)
            {
                if (d is Button)
                {
                    ((Button)d).SwitchLigth();
                    //gameDevice.GetSound().PlaySE("switch");
                    continue;
                }
            }
        }
    }
}
