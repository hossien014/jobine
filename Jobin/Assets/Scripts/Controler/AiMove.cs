using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abed.Utils;
using Unity.VisualScripting;
using Abed.Controler;

namespace Abed.Controler
{
    [RequireComponent(typeof(Walk))]
    public class AiMove : MonoBehaviour
    {
        ScreenLog sLog;
        MovementBhaviour player;
        [SerializeField] Transform target;
        Transform center;
        Vector2 position;
        Vector2 targetPos;
        [SerializeField] float StopDistance = 9f;
        #region walk
        Walk walk;
        [SerializeField] float acceleration = 1;
        [SerializeField] bool lockVelocity;
        [SerializeField] float maxSpeed = 25;
        [SerializeField] float speed = 500;
        bool IswalkCanceld = false;
        float dir;
        float inputValue;
        #endregion
        void Start()
        {
            walk = GetComponent<Walk>();
            sLog = FindObjectOfType<ScreenLog>();
            player = FindObjectOfType<MovementBhaviour>();
            target = player.transform.Find("jobinCenter").transform;
        }

        void Update()
        {
            walk.Setting(lockVelocity, maxSpeed, speed);    
            center = gameObject.transform.Find("enmeyCenter");
            position = center.transform.position;
            detectTargtDirction();

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, targetPos);
            Gizmos.DrawSphere(position, 0.5f);
            Gizmos.DrawSphere(targetPos, 0.5f);

        }

        private void detectTargtDirction()
        {
            Distance();
            sLog.Log(0, Distance());
            if (Distance().x > 0)
            {
                sLog.Log(1, "right");
                walk.Move(1, false);

            }
            if (Distance().x < 0)
            {
                sLog.Log(1, "left");
                walk.Move(-1, false);
            }
            if (Mathf.Abs(Distance().x) < 7)
            {
                walk.Move(0, true);
                sLog.Log(1, "reach");
            }

        }

        private Vector2 Distance()
        {
            targetPos = target.position;
            Vector2 distance = targetPos - position;
            return distance;
        }
    }
}
