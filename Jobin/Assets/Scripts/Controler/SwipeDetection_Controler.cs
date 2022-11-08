using Abed.Utils;

using UnityEngine;


namespace Abed.Controler
{
    public class SwipeDetection_Controler : MonoBehaviour
    {
        Vector2 startTouchPos;
        Vector2 currentTouchPos;
        Vector2 endTouchPos;

        ScreenLog_Utils Slog;
        [SerializeField] float timeRang = 2;
        [SerializeField] float swipeRange = 50;
        [SerializeField] float tapRange = 10;

        bool stopTouch = false;
        public bool DebugView;
        public bool SwipeUp = false, SwipeDown = false, SwipeLeft = false, SwipeRight = false, tap = false;
        private void Awake()
        {
          if(DebugView)  Slog = FindObjectOfType<ScreenLog_Utils>();
        }
        private void Update()
        {
            swipe();
        }
        private void swipe()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startTouchPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                        currentTouchPos = touch.position;
                        Vector2 Distance = currentTouchPos - startTouchPos;
                        if (stopTouch == false)
                        {
                            CalculatDirction(Distance, touch);
                        }
                        break;
                    case TouchPhase.Ended:
                        endTouchPos = touch.position;
                        stopTouch = false;

                        SwipeUp = false;
                        // SwipeDown =false;
                        // SwipeRight = false;
                        //SwipeLeft = false;
                        tap = false;
                        CalculateTap();
                        break;
                }
            }
        }
        private void CalculatDirction(Vector2 Distance, Touch touch)
        {
            if (Distance.x > swipeRange)
            {
                SwipeRight = true;
                SwipeLeft = false;
                SwipeDown = false;
                stopTouch = true;
                if (DebugView) Slog.Log(7, "swipe right", 30, Color.black);
            }
            else if (Distance.x < -swipeRange)
            {
                SwipeLeft = true;
                SwipeRight = false;
                SwipeDown = false;
                stopTouch = true;
                if (DebugView) Slog.Log(7, "swipe left");
            }
            else if (Distance.y > swipeRange)
            {
                SwipeUp = true;
                SwipeDown = false;
                stopTouch = true;
                if (DebugView) Slog.Log(7, "swipe up");
            }
            else if (Distance.y < -swipeRange)
            {
                SwipeDown = true;
                stopTouch = true;
                if (DebugView) Slog.Log(7, "swipe down");
            }
        }
        private void CalculateTap()
        {
            Vector2 lastDistance = endTouchPos - startTouchPos;
            if (Mathf.Abs(lastDistance.x) < tapRange && Mathf.Abs(lastDistance.y) < tapRange)
            {
                tap = true;
                SwipeLeft = false;
                SwipeRight = false;
                if (DebugView) Slog.Log(7, "tap");
            }
        }

    }
}
