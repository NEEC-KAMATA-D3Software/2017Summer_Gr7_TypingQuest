using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;

namespace TypingQuest.Scene
{
    class Button:GameObject
    {
        private bool isTouch;
        private bool isHit;

        private GameObjectID linkedGameObjectID;
        private IGameObjectMediator mediator;

        private bool textureOn;
        private float effect;
        private bool effectBool;

        private Type triggerMagic;
        public Button(string name, Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator, Type triggerMagic)
            : base(name, position, 32, 32, gameDevice)
        {
            this.mediator = mediator;
            isTouch = false;
            isHit = false;
            textureOn = false;
            this.triggerMagic = triggerMagic;

            effect = 50;
            effectBool = true;
        }
        public Button(Button other)
            : this(other.name, other.position, other.gameDevice, other.mediator, other.triggerMagic)
        {
        }
        public void SetLinkedGameObjectID(GameObjectID id)
        {
            linkedGameObjectID = id;
        }
        public GameObjectID GetLinkedGameObjectID()
        {
            return linkedGameObjectID;
        }
        public override object Clone()
        {
            return new Button(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (!(gameObject is MagicAbstract))
            {
                return;
            }
            if (isHit == false && ((MagicAbstract)gameObject).GetMagicType() == (triggerMagic))
            {
                if (isTouch == false)
                {
                    textureOn = !textureOn;
                    List<GameObject> doorList = mediator.GetGameObjectList(GetLinkedGameObjectID());
                    foreach (var d in doorList)
                    {
                        if (d is Door)
                        {
                            ((Door)d).Flip();
                            gameDevice.GetSound().PlaySE("switch");
                            continue;
                        }
                        else if (d is SwitchBlock)
                        {
                            ((SwitchBlock)d).Flip();
                            gameDevice.GetSound().PlaySE("switch");
                            continue;
                        }

                    }
                }
                isHit = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            isTouch = (isHit) ? (true) : (false);

            isHit = false;

            if (textureOn == false)
            {
                return;
            }

            if (effectBool)
            {
                effect += 0.008f;
                if (effect >= 1.0f)
                {
                    effect = 1.0f;
                    effectBool = false;
                }
            }
            else
            {
                effect -= 0.008f;
                if (effect <= 0.3f)
                {
                    effect = 0.3f;
                    effectBool = true;
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify());

            if (textureOn)
            {
                renderer.DrawTexture(
                    "light", 
                    position + gameDevice.GetDisplayModify(),
                    effect);
            }
        }

        public void SwitchLigth()
        {
            textureOn = !textureOn;
            gameDevice.GetSound().PlaySE("switch");
        }
    }
}
