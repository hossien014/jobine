
using System.Collections.Generic;
using Abed.Controler;
using Abed.Utils;
using UnityEngine;


//namespace Abed.Controler
//{
[RequireComponent(typeof(Walk_Controler))]
    public class AIMove_Controler : MonoBehaviour
    {
        ScreenLog_Utils sLog;
        MovementBhaviour_Controler player;
        [SerializeField] Transform target;
        Transform center;
        Vector2 position;
        Vector2 targetPos;
        [SerializeField] float StopDistance = 9f;
        #region walk
        Walk_Controler walk;
        [SerializeField] float acceleration = 1;
        [SerializeField] bool lockVelocity;
        [SerializeField] float maxSpeed = 25;
        [SerializeField] float speed = 500;
        bool IswalkCanceld = false;
        float dir;
        float inputValue;
        #endregion

       public List<NodeG> thePath;
        void Start()
        {
            walk = GetComponent<Walk_Controler>();
            sLog = FindObjectOfType<ScreenLog_Utils>();
            player = FindObjectOfType<MovementBhaviour_Controler>();
            target = player.transform.Find("jobinCenter").transform;
        }

        void Update()
        {
            walk.Setting(lockVelocity, maxSpeed, speed);
            center = gameObject.transform.Find("enmeyCenter");
            position = center.transform.position;
            thePath= FindObjectOfType<Path>().ThePath;
           // detectTargtDirction();
        }
        private void FixedUpdate() {
            if(thePath==null) return;
            if(thePath.Count < 4) return;
            GetComponent<Walk_Controler>().Move(thePath[1].Dir.x,false);
        }
       /*public void MoveTo(List<testnod> path)
        {
            foreach (testnod testnod in path)
            {
                if (Vector3.Distance(transform.position, testnod.pos) > StopDistance)
                {
                    walk.Move(testnod.Dir.x, testnod.Dir.x == 0);
                }
                else continue;
            }
        }
       */
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, targetPos);
            Gizmos.DrawSphere(position, 0.5f);
            Gizmos.DrawSphere(targetPos, 0.5f);

        }
        //public void detectTargtDirction()
        //{
        //    //Distance();
        //    sLog.Log(0, Distance());
        //    if (Distance().x > 0)
        //    {
        //        sLog.Log(1, "right");
        //        walk.Move(1, false);

        //    }
        //    if (Distance().x < 0)
        //    {
        //        sLog.Log(1, "left");
        //        walk.Move(-1, false);
        //    }
        //    if (Mathf.Abs(Distance().x) < 7)
        //    {
        //        walk.Move(0, true);
        //        sLog.Log(1, "reach");
        //    }

        //}
        public void detectTargtDirction()
        {

            //sLog.Log(0, Distance());
            if (Distance().x > 0)
            {
               // sLog.Log(1, "right");
                walk.Move(1, false);

            }
            if (Distance().x < 0)
            {
                //sLog.Log(1, "left");
                walk.Move(-1, false);
            }
            if (Mathf.Abs(Distance().x) < 7)
            {
                walk.Move(0, true);
                //sLog.Log(1, "reach");
            }

        }
        private Vector2 Distance()
        {
            targetPos = target.position;
            Vector2 distance = targetPos - position;
            return distance;
        }
    }
//}
