using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atouNode : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] [Range(0,1)]float T;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        T+=0.2f*Time.deltaTime;
        if(T>=  1) T=0;
       var point =  Vector3.LerpUnclamped(start.position,end.position,T);
       var c=  Color.Lerp(Color.red,Color.clear,T);
     transform.GetComponent<SpriteRenderer>().color=c;
     transform.position =point;
     }
     }


