using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Scene;

namespace TypingQuest.Actor.Magic
{
    enum Step
    {
        Launch,
        Path,
        End
    }
    abstract class MagicAbstract:GameObject
    {
        protected Character castChara;

        protected int power;
        protected bool isEnd;
        protected MagicCollisionRange collision;

        protected CharacterManager characterManager;

        public MagicAbstract(Character castChara, string name, int height, int width, CharacterManager characterManager, GameDevice gameDevice)
            :base(name, Vector2.Zero, height, width, gameDevice)
        {
            this.castChara = castChara;
            this.characterManager = characterManager;
            isEnd = false;
        }
        public abstract void Initialize();
        public bool IsEnd(){ return isEnd; }

        //対敵用判定
        public virtual void IsCollision()
        {
            if (castChara is Player)
            {
                List<Character> temp = characterManager.Enemies();
                foreach (Character cx in temp)
                {
                    if (collision.GetCollision().Intersects(cx.GetRectangle()))
                    {
                        HitEnemy(cx);
                        //cx.AddState(new Freeze(20));      //足止め freeze(継続時間)
                        //cx.AddState(new Poison(5, 1, 1)); //毒を付属する例　poison(second, damage, 時間間隔)
                    }
                }
            }
            else
            {
                if (collision.GetCollision().Intersects(characterManager.TargetPlayer().GetRectangle()))
                {
                    HitPlayer(characterManager.TargetPlayer());
                }
            }
        }

        protected abstract void HitEnemy(Character cx);
        protected abstract void HitPlayer(Player player);

        public Type GetMagicType()
        {
            if (this is Water)
            {
                return typeof(Water);
            }
            if (this is Wind)
            {
                return typeof(Wind);
            }
            if (this is Rock)
            {
                return typeof(Rock);
            }
            return null;
        }
    }
}
