using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Rigidbody2D rig;
    public CapsuleCollider2D coll;
    public Animator ani;

    public float speed, jumpForce,currentSpeed;
    public Transform[] groundCheckPoint;
    public Transform[] headCheckPoint;
    public LayerMask GroundMask;

    public bool isGround, isJump ;

    bool jumpPressed,croushPressed;
    public  bool isHeading,isCroush;
    Vector2 croushColSize, croushColOffset;
    Vector2 standColsize, standColOffset;
    float horizontalMove;
    public int jumpCount;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        ani = GetComponent<Animator>();

        standColOffset = coll.offset;
        standColsize = coll.size;

        croushColOffset = new Vector2(coll.offset.x, coll.offset.y*0.5f);
        croushColSize = new Vector2(coll.size.x, coll.size.y * 0.5f);
    }

    void Update()
    {
        PhysicCheck();
        if (Input.GetButtonDown("Jump")&&jumpCount>0)
        {
            jumpPressed = true;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            croushPressed = true;
        }
        else
        {
            croushPressed = false;
        }
        horizontalMove = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        
        GroundMovement();
        Jump();
        Croush();
    }
    void GroundMovement()
    {
        if (isCroush)
        {
            currentSpeed = 0.5f * speed;
        }
        else
        {
            currentSpeed =  speed;
        }
        rig.velocity = new Vector2(horizontalMove * currentSpeed, rig.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    void PhysicCheck()
    {
        if (Physics2D.OverlapCircle(groundCheckPoint[0].position, 0.1f, GroundMask)|| Physics2D.OverlapCircle(groundCheckPoint[1].position, 0.1f, GroundMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        RaycastHit2D headHit1 = Physics2D.Raycast(headCheckPoint[0].position,Vector2.up, 0.2f,GroundMask);
        RaycastHit2D headHit2 = Physics2D.Raycast(headCheckPoint[1].position, Vector2.up, 0.2f, GroundMask);
        //Color color = headHit ? Color.red : Color.green;
        //Debug.DrawRay(headCheckP
        //oint[0].position, Vector2.up*0.2f, color);
        if (headHit1||headHit2)
        {
            isHeading = true;

        }
        else
        {
            isHeading = false;
        }
    }
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (isGround && jumpPressed)
        {
            isJump = true;
            rig.velocity = new Vector2(rig.velocity.x,jumpForce);
            jumpCount--;
            jumpPressed = false; 
        }
        else if (!isGround && jumpCount > 0&&jumpPressed)
        {
            rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }
    void Croush()
    {
        if (!isHeading&& croushPressed&&!isCroush&&isGround)
        {
            coll.size = croushColSize;
            coll.offset = croushColOffset;
            isCroush = true;
 
        }
        else if(!isHeading&& !croushPressed && isCroush && isGround)
        {
            Stand();
        }
       


    }
    void Stand()
    {

            coll.size = standColsize;
            coll.offset = standColOffset;
            isCroush = false;   
    }
}
