
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
        bool Updatpath;

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
            checkPlatformsLevel(); //step 1 
            if(platform==platformLevel.same)
            {
                detectTargtDirctionAndGo();
                Updatpath = false;
            }
            else
            {
                Updatpath = true;
                FindAndFollowThePath();
            }
        }

        private void SetPath()
        {
            FindObjectOfType<Path>().FindingPathProcees(transform, target);
            thePath = FindObjectOfType<Path>().ThePath;
        }

        private void LateUpdate()
        {
            if(Updatpath) SetPath(); 

        }
        private void checkPlatformsLevel()
        {
            if (ThisRayBoxHit && TargetRayBoxHit)
            {
                int thisPlatformHash = ThisRayBoxHit.transform.GetHashCode();
                int targetPlatformHash = TargetRayBoxHit.transform.GetHashCode();
                FindObjectOfType<ScreenLog_Utils>().Log(1, "enemy plat " + thisPlatformHash);
                FindObjectOfType<ScreenLog_Utils>().Log(2, "jobin plat " + targetPlatformHash);

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
