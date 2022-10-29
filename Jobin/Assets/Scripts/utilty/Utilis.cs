using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.TextCore.Text;

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
        /// <summary>
        /// بازدن دکمه اف پنج صفحه ریست می شود
        /// </summary>
        public static void reloadScene()
        {
            Scene cureentSceen = SceneManager.GetActiveScene();
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(cureentSceen.buildIndex);
            }
        }
        public static TextMesh createTextInWorld(string name,Transform parent, Vector3 localPosition,string text,int fontSize,Color TextColor,TextAnchor textanchor)
        {
            GameObject gameObject = new GameObject(name,typeof( TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition= localPosition;
            transform.tag = name;


            TextMesh textmesh = gameObject.GetComponent<TextMesh>();
            textmesh.text = text;
            textmesh.fontSize =fontSize;
            textmesh.color = TextColor;
            textmesh.anchor = textanchor;
            //textmesh.alignment = textAlignment;
            return textmesh;
        }
        public static TextMesh createTextInWorld(string name,  Vector3 localPosition, string text, int fontSize, Color TextColor, TextAnchor textanchor)
        {
            GameObject gameObject = new GameObject(name, typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.localPosition = localPosition;

            TextMesh textmesh = gameObject.GetComponent<TextMesh>();
            textmesh.text = text;
            textmesh.fontSize = fontSize;
            textmesh.color = TextColor;
            textmesh.anchor = textanchor;
            // textmesh.alignment = textAlignment;
            return textmesh;

        }
        public static Vector3 GetMousePos()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        public static KeyCode[] AlphabetKeycod()
        {
            KeyCode[] Alphabet = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.F,KeyCode.G,KeyCode.H,KeyCode.J,KeyCode.K,KeyCode.L,KeyCode.Z,KeyCode.X,KeyCode.C,KeyCode.V,KeyCode.B,KeyCode.N,KeyCode.M};
            return Alphabet;
        }
        public static string[] AlphabetKeyStrung()
        {
            string[] Alphabet = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
            return Alphabet;
        }
        /// <summary>
        /// get tow posision and return where b object is 
        /// </summary>
        /// <param name="a"> base object</param>
        /// <param name="b"> secndery object</param>
        /// <returns></returns>
        public static Vector3 GetDirction(Vector3 a, Vector3 b)
        {
            if (b.x > a.x)
            {
                if (b.y > a.y) { /*print("up right")*/ return new Vector3(1, 1); }
                if (b.y < a.y) { /*print("down right")*/ return new Vector3(1, -1); }
                if (b.y == a.y) {/* print("right")*/ return new Vector3(1, 0); }

            }
            if (b.x < a.x)
            {
                if (b.y > a.y) { /*print("up left");*/return new Vector3(-1, 1); }
                if (b.y < a.y) {/* print("down left");*/ return new Vector3(-1, -1); }
                if (b.y == a.y) { /*print("left");*/return new Vector3(-1, 0); }
            }
            if (b.y < a.y && b.x == a.x) { /*print("down")*/return new Vector3(0, -1); }
            if (b.y > a.y && b.x == a.x) { /*print("up")*/ return new Vector3(0, 1); }

            return Vector3.zero;
        }
    }
}
 