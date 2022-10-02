using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcoroutien : MonoBehaviour
{
    int num=0;
    WaitForSeconds wait = new WaitForSeconds(3);
    void Start()
    {
        StartCoroutine(test2());
    }
   //IEnumerator test1()
   // {
   //     while (num < 5)
   //     {
   //         num++;
   //     print("aaa"+num);
   //     yield return new WaitForSeconds(3);

   //     }
   //     print("num finshed");
   //     yield return wait;
   //     print("finshed");
   // }
    
    IEnumerator test2()
    {
     while(true)
        {
            num++;
        print("aaa"+num);
            yield return num;

        }


    }
}
