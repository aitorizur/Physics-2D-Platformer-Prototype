using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttack : MonoBehaviour
{
    public Color hitboxColor = Color.red;
    public Vector2 bottomOffset;
    public float hitBoxRadius = 1;
    public LayerMask enemiesLayer;
    Player playerScript;
    Rigidbody2D rb;
    private Collider2D[] enemiesHit;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateHitbox()
    {
        enemiesHit = Physics2D.OverlapCircleAll((Vector2)transform.position + bottomOffset, hitBoxRadius, enemiesLayer);

        if (enemiesHit.Length > 0)
        {
            playerScript.JumpOnEnemies();
        }

    }

    public void ExitAttack()
    {
        if (rb.velocity.y > 0)
        {
            playerScript.state = 2;
        }
        else
        {
            playerScript.state = 3;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = hitboxColor;

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, hitBoxRadius);
    }
}
