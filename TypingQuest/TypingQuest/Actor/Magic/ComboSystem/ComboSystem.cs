using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Utility;
using TypingQuest.Def;
using TypingQuest.Device;

namespace TypingQuest.Actor.Magic.ComboSystem
{
    class ComboSystem
    {
        private static int combo;
        private Timer timer;
        private string rank;
        private Vector2 rankPosition;
        private Vector2 comboPosition;
        private Vector2 timerPosition;

        private bool enemyDamage;
        public ComboSystem()
        {
            combo = 0;
            timer = new Timer(7);

            rankPosition = new Vector2(Screen.Width - 300, 40);
            comboPosition = new Vector2(Screen.Width - 300, 90);
            timerPosition = new Vector2(Screen.Width - 300, 160);
            enemyDamage = false;
        }
        public void Initialize()
        {
            combo = 0;
            rank = "F";
        }
        public void Update(GameTime gameTime)
        {
            timer.Update();
            if (timer.IsTime())
            {
                Initialize();
            }
            JudgeRank();
        }
        public void AddCombo()
        {
            combo++;
            enemyDamage = true;
            if (timer.IsTime())
            {
                timer.Initialize();
            }
            else
            {
                switch (rank)
                {
                    case "F":
                        timer.PlusTime(2f);
                        break;
                    case "E":
                        timer.PlusTime(1.8f);
                        break;
                    case "D":
                        timer.PlusTime(1.5f);
                        break;
                    case "C":
                        timer.PlusTime(1.2f);
                        break;
                    case "B":
                        timer.PlusTime(0.8f);
                        break;
                    case "A":
                        timer.PlusTime(0.5f);
                        break;
                    case "S":
                        timer.PlusTime(0.3f);
                        break;
                }
                
            }
        }
        public void Draw(Renderer renderer)
        {
            if (combo == 0)
            {
                return;
            }
            string comboString = combo.ToString() + " Combo";
            renderer.DrawString(rank, rankPosition);
            renderer.DrawString(comboString, comboPosition);
            renderer.DrawTexture(
                "timer", 
                timerPosition, 
                new Rectangle(0, 0, 100, 10),
                0,
                Vector2.Zero,
                new Vector2(timer.Rate() * 2 , 1));
        }

        private void JudgeRank()
        {
            if (combo < 5)
            {
                rank = "F";
            }
            else if (combo < 10)
            {
                rank = "E";
            }
            else if (combo < 15)
            {
                rank = "D";
            }
            else if (combo < 20)
            {
                rank = "C";
            }
            else if (combo < 25)
            {
                rank = "B";
            }
            else if (combo < 30)
            {
                rank = "A";
            }
            else if (combo >= 35)
            {
                rank = "S";
            }
        }
        public bool IsDamageEffect()
        {
            return enemyDamage;
        }
        public void OffDamageEffect()
        {
            enemyDamage = false;
        }
    }
}
