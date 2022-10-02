
using UnityEngine;

public class shosColider : MonoBehaviour
{
    bool grounded;
    public int grounded_C, perper_C, jump_C, inFlight_C, landed_C;
    public float lastTimeJump, lastTimeGrounded; 
    public bool getGround()
    {
        return grounded;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            grounded = true;
            grounded_C += 1;
            lastTimeGrounded = Time.time;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        int contactNum;
        contactNum = collision.contactCount; //chage name
        ContactPoint2D[] ContactPoints = new ContactPoint2D[contactNum];
        collision.GetContacts(ContactPoints);

        foreach (ContactPoint2D contact in ContactPoints)
        {
            if (contact.collider != null && contact.collider.tag == "ground")
            {
                    grounded = true;
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.tag == "ground")
            {
                grounded = false;
                inFlight_C += 1;
                lastTimeJump = Time.time;
            }
        }    
}
