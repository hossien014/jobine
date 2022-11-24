
using System.Collections.Generic;
using System.Threading.Tasks;
using Abed.Utils;
using UnityEngine;

public enum platformLevel {same,notsame,unkown};
namespace Abed.Controler
{
[RequireComponent(typeof(Walk_Controler))]
    public class AIMove_Controler : MonoBehaviour
    {
        ScreenLog_Utils sLog;
        MovementBhaviour_Controler player;
        [SerializeField] Transform target;
        [SerializeField]platformLevel platform = platformLevel.unkown;
        RaycastHit2D ThisRayBoxHit;
        RaycastHit2D TargetRayBoxHit;
        Transform center;
        Vector3 position;
        Vector3 targetPos;
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
        }

        void Update()
        {
            walk.Setting(lockVelocity, maxSpeed, speed);
            ThisRayBoxHit = GetComponent<Jump_Controler>().GetRayBoxHit();
            TargetRayBoxHit = target.transform.GetComponent<Jump_Controler>().GetRayBoxHit();
        }
        private void FixedUpdate()
        {
            checkPlatformsLevel();
            if(platform==platformLevel.same)
            {
                detectTargtDirctionAndGo();
            }
            else
            {
                thePath = FindObjectOfType<Path>().ThePath; 
                FindAndFollowThePath();
            }
        }

        private void checkPlatformsLevel()
        {
            if (ThisRayBoxHit && TargetRayBoxHit)
            {
                int thisPlatformHash = ThisRayBoxHit.transform.GetHashCode();
                int targetPlatformHash = TargetRayBoxHit.transform.GetHashCode();

                if (thisPlatformHash == targetPlatformHash) 
                {
                platform = platformLevel.same;
                }
                else {
                 platform = platformLevel.notsame;
                }
            }
            else{platform=platformLevel.unkown;}
        }

        private async void FindAndFollowThePath()
    {
        print((Distance(true) <= StopDistance+2 )+ "distance = "+ Distance(true).ToString());
        if (thePath == null || thePath.Count == 0 || Distance(true) <= StopDistance+2 ) { walk.Move(0, true)  ;return;}

        if(thePath[0].Type==NodeType.Jump)
        {
            GetComponent<Jump_Controler>().JumpPresed=true;
             walk.Move(thePath[0].Dir.x, false);
            await Task.Delay(800);
                GetComponent<Jump_Controler>().JumpPresed = false;

            }

        walk.Move(thePath[0].Dir.x, false);
        
    }

    private void OnDrawGizmos()
        {
       // Gizmos.color=Color.red;
       // if(thePath.Count==0||thePath==null ) return;
       // Gizmos.DrawSphere(thePath[0].pos,0.6f);
        }
      
        public void detectTargtDirctionAndGo()
        {
            if (Distance(false) > 0)
            {
               // sLog.Log(1, "right");
                walk.Move(1, false);
            }
            if (Distance(false) < 0)
            {
                //sLog.Log(1, "left");
                walk.Move(-1, false);
            }
            if (Mathf.Abs(Distance(false)) < StopDistance)
            {
                walk.Move(0, true);

            }

        }
        private float Distance(bool absolut)
        {
         float distance;

           if(!absolut)
           {
              distance = (target.position - transform.position).x;
           } 
           else
           {
            distance =Vector2.Distance(target.position,transform.position);
           } 
            return distance;
        }
    }
}
