using UnityEngine;

public class testcoroutien : MonoBehaviour
{
    float a = 357.2f;
    float b = 361.8f;
    bool stop = false;
    WaitForSeconds wait = new WaitForSeconds(3);
    private void Update()
    {
        if (a >= b)
        {
            stop = true;
        }
        if (!stop)
        {
            a += 1;
            print("a+1 =>" + (a));
        }
    }


}
