using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Abed.Utils
{
    public class Utilis : MonoBehaviour
    {
        Action TimerCallback;

        float timer = 0;
        float accTimer = 0;

        private void Update()
        {
            if (timer > 0)
            {
            timer -= Time.deltaTime;
                if (IsTimerComplet())
                {
                    TimerCallback();
                }
            }
        }
        #region Timers
        /// <summary>
        /// عدد که میگرید را تا زمان تعیین شده می شمارد
        /// </summary>
        /// <param name="number">عدد که شمارده می شود</param>
        /// <param name="time">زمان شمارش</param>
        /// <returns></returns>
        public float TimeAcceleration(float number, float time)
        {
            if (number == 0) accTimer = 0;
            float divied = number / time;
            accTimer += Time.deltaTime;
            float Acceleration = Mathf.Clamp(divied * accTimer, -1, 1);
            return Acceleration;
        }
        /// <summary>
        /// یک تایمر را شروع می کند .و بعد از پایان تایمر دستور یا فانکشن دریافتی را اجرا می کند
        /// </summary>
        /// <param name="resetTime">بعد از اتمام این زمان دستور اجرا می شود </param>
        /// <param name="TimerCallback">فانکشن  یا دستوری که بعد از اتمام تایمر انجام می شود</param>
        /// <returns></returns>
        public float Timer(float resetTime,Action TimerCallback)
        {
          this.TimerCallback =TimerCallback;
            timer = resetTime;
            return timer;
        }
        
        public bool IsTimerComplet()
        {
            return timer <= 0;
        }

        //public float Timer(float resetTime)
        //{
        //    timer += Time.deltaTime;
        //    if (resetTime > 0)
        //    {
        //        if (timer >= resetTime) timer = 0;
        //    }
        //    return timer;
        //}
        
        #endregion
        /// <summary>
        /// بازدن دکمه اف پنج صفحه ریست می شود
        /// </summary>
        public void reloadScene()
        {
            Scene cureentSceen = SceneManager.GetActiveScene();
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(cureentSceen.buildIndex);
            }
        }
    }
}
 