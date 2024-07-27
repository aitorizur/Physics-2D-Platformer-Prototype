using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour
{
    public Vector2 offsetAttack;
    public Vector2 offsetGrab;
    private Collider2D enemiesHit;
    private Collider2D enemyGrab;
    public  float hitBoxRadius;
    public LayerMask grabLayer;
    private Transform grabbedTransform;
    public Color hitboxColor;
    public Color GrabColor;
    public float throwSpeed;
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    //Funcion especifica para poder anadir una funcion con los animation events qeu permita argumentos //IMPORTANTE

    public void ChangeOffsetX(float x)
    {
        offsetGrab = new Vector2(x, offsetGrab.y);
    }
    public void ChangeOffsetY(float y)
    {
        offsetGrab = new Vector2(offsetGrab.x, y);
    }
    public void UpdateGrab()
    {
        enemyGrab.transform.position = (Vector2)transform.position + transform.right * offsetGrab + new Vector2(0, offsetGrab.y);
    }


    public void GenerateHitboxGrab()
    {
        enemiesHit = Physics2D.OverlapCircle((Vector2)transform.position + transform.right * offsetAttack + new Vector2(0, offsetAttack.y), hitBoxRadius, grabLayer);

        if (enemiesHit)
        {
            enemyGrab = enemiesHit;
            enemyGrab.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            anim.Play("GrabAttack");
        }

    }

    public void ThrowEnemy()
    {
        enemyGrab.GetComponent<Rigidbody2D>().velocity = new Vector2(throwSpeed * transform.right.x + rb.velocity.x, 0);
    }

    public void CheckTurnAround()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        print(axis);
        if (axis != 0)
        {
            transform.right = new Vector2(axis,0);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = hitboxColor;

        Gizmos.DrawWireSphere((Vector2)transform.position + transform.right * offsetAttack + new Vector2(0, offsetAttack.y), hitBoxRadius);

        Gizmos.color = GrabColor;

        Gizmos.DrawWireSphere((Vector2)transform.position + transform.right * offsetGrab + new Vector2(0, offsetGrab.y), 0.2f);
    }



}
