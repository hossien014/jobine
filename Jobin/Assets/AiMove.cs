using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abed.Utils;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class AiMove : MonoBehaviour
{
    ScreenLog sLog;
    [SerializeField] Transform target;
    Transform center;
    Vector2 position;
    Vector2 targetPos;
    [SerializeField] float StopDistance =9f;
    void Start()
    {
        sLog = FindObjectOfType<ScreenLog>();
        center = gameObject.transform.Find("enmeyCenter");
        position = center.transform.position;
    }

    void Update()
    {
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
        sLog.Log(0,Distance());
       if(Distance().x > 0)
        {
            sLog.Log(1, "right");

        }  
        if(Distance().x < 0)
        {
            sLog.Log(1, "left");
        }
        if (Mathf.Abs(Distance().x) < 7)
        {
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
