using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypingQuest.Utility
{
    class Timer
    {
        private float currentTime;
        private float limitTime;
        public Timer()
        {
            limitTime = 60.0f;
        }
        public Timer(float second)
        {
            limitTime = 60.0f * second;
        }
        public void Initialize()
        {
            currentTime = limitTime;
        }
        public void Update()
        {
            currentTime -= 1;
            if (currentTime < 0.0f)
            {
                currentTime = 0.0f;
            }
        }
        public float Now()
        {
            return currentTime;
        }
        public bool IsTime()
        {
            return currentTime <= 0;
        }
        public void Change(float limitTime)
        {
            this.limitTime = limitTime;
            Initialize();
        }
        public float Rate()
        {
            return currentTime / limitTime;
        }

        public void PlusTime(float second)
        {
            if (currentTime + second < limitTime)
            {
                currentTime += (second * 60);
            }
        }
    }
}
