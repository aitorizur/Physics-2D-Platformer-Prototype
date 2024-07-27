using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Debugging")]
    public byte state;

    private float accelRatePerSecGround;
    private float decelRatePerSecGround;
    private float decelRatePerSecOverMaxSpeedGround;

    private float accelRatePerSecAir;
    private float decelRatePerSecAir;
    private float decelRatePerSecOverMaxSpeedAir;

    [Header("Stats")]
    public int jumpsLeft;
    public float jumpForce;
    public float wallHangGravity;
    public float fastFallGravity;
    public float jumpFromWallForce;
    public float maxSpeedX;
    public float fallSpeedLimit;
    public float fastFallSpeedLimit;


    public float timeZeroToMaxGround = 1;
    public float timeMaxToZeroGround = 1;
    public float timeMaxToZeroMaxSpeedGround = 10;

    public float timeZeroToMaxAir = 2;
    public float timeMaxToZeroAir = 2;
    public float timeMaxToZeroMaxSpeedAir = 0;

    public float timerDash;
    public Color dashColor;



    private Color initialColor;
    private int initialJumpsLeft;
    private float initialGravity;
    private float initialTimerDash;

    [Header("Materials for Physics")]
    public PhysicsMaterial2D noBounciness;
    public PhysicsMaterial2D bounciness;

    //Componentes del Jugador para incializar
    Collision coll;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializacion de Componentes
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collision>();
        sr = GetComponent<SpriteRenderer>();


        accelRatePerSecGround = maxSpeedX / timeZeroToMaxGround;
        decelRatePerSecGround = maxSpeedX / timeMaxToZeroGround;
        decelRatePerSecOverMaxSpeedGround = maxSpeedX / timeMaxToZeroMaxSpeedGround;

        accelRatePerSecAir = maxSpeedX / timeZeroToMaxAir;
        decelRatePerSecAir = maxSpeedX / timeMaxToZeroAir;
        decelRatePerSecOverMaxSpeedAir = maxSpeedX / timeMaxToZeroMaxSpeedAir;



        initialGravity = rb.gravityScale;
        initialTimerDash = timerDash;
        initialJumpsLeft = jumpsLeft;
        initialColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {

        EnterDash();

        if (state == 0) //Iddle
        {
            //
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            sr.color = initialColor;
            //
            HorizontalMovementVariant(accelRatePerSecGround, decelRatePerSecGround, decelRatePerSecOverMaxSpeedGround); //Horizontal movement ground
            //LimitSpeedXResidual();
            RunningWalkingControl();
            Fall();
            InputJump();
            EnterAttackGrab();
        }
        else if (state == 1) //Moving
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            //

            HorizontalMovementVariant(accelRatePerSecGround, decelRatePerSecGround, decelRatePerSecOverMaxSpeedGround); //Horizontal movement ground
            //LimitSpeedXResidual();
            RunningWalkingControl();
            Fall();
            InputJump();
            EnterAttackGrab();
        }
        else if (state == 2) //Jumping
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            //

            HorizontalMovementVariant(accelRatePerSecAir, decelRatePerSecAir, decelRatePerSecOverMaxSpeedAir); //Horizontal movement air
            Fall();
            InputJumpAir();
            FastFallEnter();
            EnterAttackGrab();
            //LimitSpeedXResidual();
        }
        else if (state == 3) //Falling
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            //

            HorizontalMovementVariant(accelRatePerSecAir, decelRatePerSecAir, decelRatePerSecOverMaxSpeedAir); //Horizontal movement air
            InputJumpAir();
            FastFallEnter();
            //LimitSpeedXResidual();
            ContactWallHangTest();
            ContactFloorTest();
            LimitFallSpeed(fallSpeedLimit);
            EnterAttackGrab();
        }
        else if (state == 4) //WallHanging
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = wallHangGravity;
            //
            HorizontalOutOfWall();
            InputJumpFromWall();
            ContactFloorTest();
            ExitWaLLHang();
        }
        else if (state == 5) //FastFalling
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = fastFallGravity;
            //

            HorizontalMovementVariant(accelRatePerSecAir, decelRatePerSecAir, decelRatePerSecOverMaxSpeedAir); //Horizontal movement air
            FastFallExit();
            //LimitSpeedXResidual();
            ContactWallHangTest();
            ContactFloorTest();
            EnterAttack();
            LimitFallSpeed(fastFallSpeedLimit);
            EnterAttackGrab();
        }
        else if (state == 6) //ATTACK PROVISIONAL
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            //
        }
        else if (state == 7) //DASH
        {
            //
            if (rb.gravityScale != 0)
            {
                sr.color = dashColor;
                rb.gravityScale = 0;
                rb.sharedMaterial = bounciness;
            }
            //

            ExitDash();

        }
        else if (state == 8) //GRAB PRIVISIONAL
        {
            //
            sr.color = initialColor;
            rb.sharedMaterial = noBounciness;
            rb.gravityScale = initialGravity;
            //
        }

        anim.SetInteger("state",state);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == 7)
        {
            if (collision.gameObject.tag == "Floor")
            {
                rb.velocity = rb.velocity * 1.1f;
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (state == 1)
        {
            //ContactStopAgainstWall(collision);
        }
        else if (state == 2)
        {
            //ContactStopAgainstWall(collision);
        }
        if (state == 3)
        {
            //ContactWallHang(collision);
        }
        else if (state == 5)
        {
            //ContactWallHang(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (state == 1)
        {

        }
        else if (state == 0)
        {

        }
        else if (state == 2)
        {

        }
        else if (state == 3)
        {

        }
        else if (state == 4)
        {
            if (collision.gameObject.tag == "Floor")
            {
                state = 3;
            }
        }

    }



    private void HorizontalMovementVariant(float accelRate, float decelRate, float decelRateOverMaxSpeed)
    {
        float axis = Input.GetAxisRaw("Horizontal");
        float velocityX;
        velocityX = rb.velocity.x;

        if (axis != 0)
        {
            if (Mathf.Abs(velocityX) <= maxSpeedX)
            {
                if (transform.right.x != axis)
                {
                    velocityX /= 2;
                }
                transform.right = new Vector2(axis, 0);

                velocityX += accelRate * axis * Time.deltaTime;
                if (Mathf.Abs(velocityX) > maxSpeedX)
                {
                    velocityX = maxSpeedX * axis;
                }
            }
            else
            {
                if (transform.right.x != axis)
                {
                    velocityX /= 2;
                }
                transform.right = new Vector2(axis, 0);

                if (Mathf.Sign(velocityX) == Mathf.Sign(axis))
                {
                    velocityX -= decelRateOverMaxSpeed * axis * Time.deltaTime;
                }
                else
                {
                    velocityX += accelRate * axis * Time.deltaTime;
                }
            }
        }
        else
        {

            if (velocityX > 0)
            {
                velocityX -= decelRate * Time.deltaTime;
                velocityX = Mathf.Max(velocityX, 0);
            }
            else if (velocityX < 0)
            {
                velocityX += decelRate * Time.deltaTime;
                velocityX = Mathf.Min(velocityX, 0);
            }



        }

        rb.velocity = new Vector2(velocityX, rb.velocity.y);


    }

    private void HorizontalOutOfWall()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(rb.velocity.x + accelRatePerSecAir * Time.deltaTime * axis, rb.velocity.y);

    }

    private void RunningWalkingControl()
    {
        float axis = Input.GetAxisRaw("Horizontal");

        if (axis != 0)
        {
            state = 1;
        }
        else
        {
            state = 0;
        }
    }

    private void LimitSpeedXResidual()
    {

        if (rb.velocity.x > maxSpeedX)
        {
            rb.velocity = new Vector2(maxSpeedX, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeedX)
        {

            rb.velocity = new Vector2(-maxSpeedX, rb.velocity.y);
        }

    }

    private void InputJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                state = 2;
        }
    }

    private void InputJumpAir()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                state = 2;
                jumpsLeft--;
            }
        }
    }

    private void InputJumpFromWall()
    {
        if (Input.GetButtonDown("Jump"))
        {
                state = 2;
                rb.velocity = new Vector2(jumpFromWallForce * transform.right.x, jumpForce * 0.75f);
        }
    }

    public void JumpOnEnemies()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpsLeft = initialJumpsLeft;
    }

    private void Fall()
    {

        if (rb.velocity.y < -0.5)
        {
            state = 3;
        }

    }

    private void ContactFloorTest()
    {
        if (coll.onGround)
        {
            state = 0;
            jumpsLeft = initialJumpsLeft;
        }
    }


    private void ContactWallHangTest()
    {
        if (coll.onLeftWall)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            state = 4;
            jumpsLeft = initialJumpsLeft;
            rb.velocity = Vector2.zero;
        }
        else if (coll.onRightWall)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            state = 4;
            jumpsLeft = initialJumpsLeft;
            rb.velocity = Vector2.zero;
        }
    }

    private void FastFallEnter()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            state = 5;

        }
    }

    private void FastFallExit()
    {

        if (Input.GetKey(KeyCode.S) == false)
        {

            state = 3;

        }

    }

    private void ExitWaLLHang()
    {
        if (transform.eulerAngles == new Vector3(0, 0, 0))
        {
            if (coll.onLeftWall == false)
            {
                state = 3;
            }
        }
        else if (transform.eulerAngles == new Vector3(0, 180, 0))
        {
            if (coll.onRightWall == false)
            {
                state = 3;
            }
        }

    }

    private void EnterAttack()
    {
        if (Input.GetButtonDown("Attack1"))
        {
            state = 6;
        }
    }


    private void LimitFallSpeed(float limitSpeed)
    {
        if (rb.velocity.y < -limitSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -limitSpeed);
        }
    }

    private void EnterDash()
    {
        float xAxis;
        float yAxis;
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(xAxis, yAxis);
        //direction = direction.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.velocity = rb.velocity.magnitude * direction;
            state = 7;
        }
    }

    private void ExitDash()
    {
        timerDash -= Time.deltaTime;
        if (timerDash <= 0)
        {
            if (rb.velocity.y <= 0)
            {
                state = 3;
            }
            else
            {
                state = 2;
            }
            timerDash = initialTimerDash;
        }
    }

    private void EnterAttackGrab()
    {
        if (Input.GetButtonDown("Attack2"))
        {
            state = 8;
            anim.Play("GrabAttck");

        }
    }

}
