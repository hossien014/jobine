
/* Unmerged change from project '_Utils.Player'
Before:
using System.Collections;
After:
using System;
using System.Collections;
*/
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
/* Unmerged change from project '_Utils.Player'
Before:
using System;
using UnityEngine.TextCore.Text;
After:
using UnityEngine.TextCore.Text;
*/


namespace Abed.Utils
{
    public class _Utils : MonoBehaviour
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
        public static TextMesh createTextInWorld(string name, Transform parent, Vector3 localPosition, string text, int fontSize, Color TextColor, TextAnchor textanchor)
        {
            GameObject gameObject = new GameObject(name, typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.tag = name;


            TextMesh textmesh = gameObject.GetComponent<TextMesh>();
            textmesh.text = text;
            textmesh.fontSize = fontSize;
            textmesh.color = TextColor;
            textmesh.anchor = textanchor;
            //textmesh.alignment = textAlignment;
            return textmesh;
        }
        public static TextMesh createTextInWorld(string name, Vector3 localPosition, string text, int fontSize, Color TextColor, TextAnchor textanchor)
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
            KeyCode[] Alphabet = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M };
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
        #region colider
        //public static Vector3 GetColider_Lower_Left(BoxCollider2D targetColider) // obsolete 
        //{
        //    Vector3 extend = targetColider.bounds.extents;
        //    Vector3 center = targetColider.bounds.center;
        //    return -extend + center;
        //}  
        public static void GetColiderWidthAndHight(BoxCollider2D targetColider, out float x, out float y)
        {
            Vector3 scale = targetColider.transform.localScale;
            x = Mathf.Abs(targetColider.size.x * scale.x);
            y = Mathf.Abs(targetColider.size.y * scale.y);
        }

        public static Vector3[] GetColiderCorners(GameObject obj)
        {
            if (obj.transform.GetComponent<BoxCollider>() != null)
            {
                Vector3[] vertices = new Vector3[8];
                BoxCollider b = obj.GetComponent<BoxCollider>();

                vertices[0] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
                vertices[1] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);
                vertices[2] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);
                vertices[3] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
                vertices[4] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
                vertices[5] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);
                vertices[6] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
                vertices[7] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);
                return vertices;
            }
            Debug.LogError("game objcet should have boxcolider");
            return null;
        }
        public static Vector3[] GetColiderCorners2D(GameObject obj)
        {
            Vector3[] corners = new Vector3[4];
            if (obj.transform.GetComponent<BoxCollider2D>() != null)
            {
                BoxCollider2D b = obj.transform.GetComponent<BoxCollider2D>();
                corners[0] = obj.transform.TransformPoint(b.offset + new Vector2(b.size.x, b.size.y) * 0.5f); // right up
                corners[1] = obj.transform.TransformPoint(b.offset + new Vector2(-b.size.x, b.size.y) * 0.5f); // left up
                corners[2] = obj.transform.TransformPoint(b.offset + new Vector2(b.size.x, -b.size.y) * 0.5f); // right down
                corners[3] = obj.transform.TransformPoint(b.offset + new Vector2(-b.size.x, -b.size.y) * 0.5f); // left down
                return corners;
            }
            Debug.LogError("game object should have box colider 2d");
            return null;
        }
        public static Vector3[] GetColiderTopLine(GameObject obj)
        {
            Vector3[] topLine = new Vector3[2];
            if (obj.transform.GetComponent<BoxCollider2D>() != null)
            {
                BoxCollider2D b = obj.transform.GetComponent<BoxCollider2D>();
                topLine[0] = obj.transform.TransformPoint(b.offset + new Vector2(-b.size.x, b.size.y) * 0.5f); // left up
                topLine[1] = obj.transform.TransformPoint(b.offset + new Vector2(b.size.x, b.size.y) * 0.5f); // right up
                return topLine;
            }
            Debug.LogError("game object should have box colider 2d");
            return null;
        }
        #endregion
        #region Debug
        public static void DrawDebugSquer(Vector3 P, float size)
        {
            Debug.DrawLine(P, new Vector3(P.x + size, P.y), Color.black, 1000);
            Debug.DrawLine(new Vector3(P.x + size, P.y), new Vector3(P.x + size, P.y + size), Color.black, 1000);
            Debug.DrawLine(new Vector3(P.x + size, P.y + size), new Vector3(P.x, P.y + size), Color.black, 1000);
            Debug.DrawLine(new Vector3(P.x, P.y + size), P, Color.black, 1000);
        }
        public static void DrawOrgineLine(Vector3 orgine) // draw line up and fornt of orgine 
        {
            float lineLeanth = 1000;
            Vector3 fornt = orgine + Vector3.right * lineLeanth;
            Vector3 up = orgine + Vector3.up * lineLeanth;
            Debug.DrawLine(orgine, fornt);
            Debug.DrawLine(orgine, up);
        }
        #endregion
        #region Dirctions
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
        public static Vector3[] DirctionArray4()
        {
            Vector3[] dirctions = new Vector3[4];
            dirctions[0] = new Vector3(1, 0, 0);  //right
            dirctions[1] = new Vector3(0, 1, 0);  //up
            dirctions[2] = new Vector3(-1, 0, 0); //left
            dirctions[3] = new Vector3(0, -1, 0); //down

            return dirctions;
        }
        public static Vector3[] DirctionArray8()
        {
            Vector3[] dirctions = new Vector3[8];
            dirctions[0] = new Vector3(1, 0, 0);  //right
            dirctions[1] = new Vector3(0, 1, 0);  //up
            dirctions[2] = new Vector3(-1, 0, 0); //left
            dirctions[3] = new Vector3(0, -1, 0); //down

            dirctions[4] = new Vector3(1, 1, 0); //up right
            dirctions[5] = new Vector3(1, -1, 0); //down right
            dirctions[6] = new Vector3(-1, 1, 0); //up left
            dirctions[6] = new Vector3(-1, -1, 0); //down left
            return dirctions;
        }

        #endregion
    }
}
