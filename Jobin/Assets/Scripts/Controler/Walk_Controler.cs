using Abed.Utils;
using UnityEngine;
namespace Abed.Controler
{
    public class Walk_Controler : MonoBehaviour
    {
        public enum MoveState { Stoping, Walking };
        public MoveState movestate = MoveState.Stoping;

        Rigidbody2D rb;
        Animator animator;
        //ShosColider_Controler sColid;

        bool lockVelocity;
        float maxSpeed = 25;
        float speed = 500;
        float dir;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
           // sColid = GetComponentInChildren<ShosColider_Controler>();
        }
        private void FixedUpdate()
        {
            LockSpeed();
            _Utils.reloadScene();
        }
        public void Setting(bool lockVelocity, float maxSpeed, float speed)
        {
            this.lockVelocity = lockVelocity;
            this.maxSpeed = maxSpeed;
            this.speed = speed;
        }
        public void Move(float dir, bool walkIsCanceld)
        {
            this.dir = dir;
            FilpSprit();
            preformWalk();
            if (walkIsCanceld) { movestate = MoveState.Stoping; };
            if (Mathf.Abs(dir) > 0) movestate = MoveState.Walking;
    
        }

        private void FilpSprit()
        {
            if (dir > 0) { this.transform.localScale = new Vector2(1, 1); }
            else if (dir < 0) { this.transform.localScale = new Vector2(-1, 1); }
        }
        private void preformWalk()
        {
            rb.velocity = new Vector2(dir * (speed * Time.fixedDeltaTime), rb.velocity.y);
            if (movestate == MoveState.Stoping) rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("speed_A", Mathf.Abs(dir));

            if (!GetComponent<Jump_Controler>().grounded)
            {
                animator.SetFloat("speed_A", 0);
            }
        }
        public void LockSpeed()
        {
            if (lockVelocity)
            {
                rb.velocity = new Vector2
                (Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
            }
        }

    }
}

