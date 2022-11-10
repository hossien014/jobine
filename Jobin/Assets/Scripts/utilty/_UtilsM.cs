using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _UtilsM : MonoBehaviour
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

    public float TimeAcceleration(float number, float time)
    {
        if (number == 0) accTimer = 0;
        float divied = number / time;
        accTimer += Time.deltaTime;
        float Acceleration = Mathf.Clamp(divied * accTimer, -1, 1);
        return Acceleration;
    }

    public float Timer(float resetTime, Action TimerCallback)
    {
        this.TimerCallback = TimerCallback;
        timer = resetTime;
        return timer;
    }

    public bool IsTimerComplet()
    {
        return timer <= 0;
    }
    #endregion
}
