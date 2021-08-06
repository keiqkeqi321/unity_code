using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header ("compoent")]
    public Rigidbody2D rig;
    public CapsuleCollider2D coll;
    public Animator ani;


    [Header("value")]
    [SerializeField] float grabDistance;
    public int jumpCount;
    public float grabJumpForce;
    public float speed, jumpForce,croushJumpForce,currentSpeed;


    [Header ("checkPoint")]
    public Transform[] groundCheckPoint;
    public Transform[] headCheckPoint;
    public Transform[] grabCheckPoint;


    [Header("state")]
    public bool isGround, isJump ,isHang;
    public  bool isHeading,isCroush;
    bool jumpPressed,croushPressed;


    [Header ("other")]
    public LayerMask GroundMask;


    float grabDirection;
    Vector2 croushColSize, croushColOffset;
    Vector2 standColsize, standColOffset;
    float horizontalMove;
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
        horizontalMove = Input.GetAxisRaw("Horizontal");
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
       
    }
    private void FixedUpdate()
    {
       
        Movement();
        //此处选择跳跃的方式
        //Jumps();
        CroushJump();
        Croush();
    }
    void Movement()
    {
        if (isHang)
        {
            if (horizontalMove == grabDirection || jumpPressed)
            {
                rig.bodyType = RigidbodyType2D.Dynamic;
                rig.AddForce(new Vector2(20f, grabJumpForce), ForceMode2D.Impulse);
                isHang = false;
                jumpPressed = false;
            }
        }
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




        /*抓握*/
        grabDirection = transform.localScale.x;
        Vector2 grabDir = new Vector2(grabDirection, 0f);
        RaycastHit2D grabCheak =Physics2D.Raycast(grabCheckPoint[0].position, grabDir, grabDistance, GroundMask);
        RaycastHit2D grabCheak1 = Physics2D.Raycast(grabCheckPoint[1].position, grabDir, grabDistance, GroundMask);
        RaycastHit2D grabCheak2 = Physics2D.Raycast(grabCheckPoint[2].position, Vector2.down, grabDistance, GroundMask);
        if (!isGround && rig.velocity.y < 0 && !grabCheak && grabCheak1 && grabCheak2&&!isCroush)
        {
            Vector3 pos = transform.position;
            pos.x += grabDirection * grabCheak1.distance;
            pos.y -= grabCheak2.distance;
            transform.position = pos;
            rig.bodyType = RigidbodyType2D.Static;
            isHang = true;
        }
    }
    void Hang()
    {
        if (isHang)
        {

        }
    }
    void Jumps()
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
    void CroushJump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if (isGround && jumpPressed&&!isCroush)
        {
            jumpCount--;
            isJump = true;
            rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            jumpPressed = false;
        }
        else if(isGround &&jumpPressed && isCroush)
        {
            jumpCount--;
            isJump = true;
            isCroush = false;
            rig.velocity = new Vector2(rig.velocity.x, croushJumpForce);
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
