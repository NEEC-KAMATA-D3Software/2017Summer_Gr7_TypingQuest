using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;
using TypingQuest.Def;
using TypingQuest.Actor.Status;

namespace TypingQuest.Actor
{
    class CharacterManager
    {
        private List<Character> enemyList;
        private Player player;

        public CharacterManager()
        {
            enemyList = new List<Character>();
        }
        public void AddPlayer(Player player)
        {
            this.player = player;
        }
        public void AddCharacter(Character chara)
        {
            enemyList.Add(chara);
        }
        public Player TargetPlayer()
        {
            if (player != null)
                return player;
            return null;
        }
        public Character NearestEnemy()
        {
            if (enemyList.Count <= 0)
                return null;
            int index = 0, cnt = 0;
            float lenth = 5000;
            foreach (Character cx in enemyList)
            {
                if ((player.GetPosition() - cx.GetPosition()).Length() < lenth)
                {
                    index = cnt;
                    lenth = (player.GetPosition() - cx.GetPosition()).Length();
                }
                cnt++;
            }
            return enemyList[index];
        }
        public List<Character> Enemies()
        {
            return enemyList;
        }
        public void Initialize()
        {
            enemyList.Clear();
        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            enemyList.ForEach((Character cx) => cx.Update(gameTime));
            PlayerDamage();
            enemyList.RemoveAll((Character cx) => cx.IsDead());
        }
        public void Draw(Renderer renderer)
        {
            enemyList.ForEach((Character cx) => cx.Draw(renderer));
            player.Draw(renderer);
        }
        public void DrawPlayerUI(Renderer renderer)
        {
            player.DrawUI(renderer);
        }

        public void DrawPlayerHP(Renderer renderer)
        {
            player.DrawHP(renderer);
        }
        private void PlayerDamage()
        {
            if (player.IsInvicible())
            {
                return;
            }
            foreach (Character enemy in enemyList)
            {
                if (enemy.Collision(player))
                {
                    Vector2 direct = enemy.GetPosition() - player.GetPosition();
                    Direct direction;
                    if (direct.X > 0)
                    {
                        direction = Direct.Left;
                    }
                    else
                    {
                        direction = Direct.Right;
                    }
                    player.Damage(15, direction);
                    player.AddState(new Invincible(1));
                    return;
                }
            }
        }
    }
}
