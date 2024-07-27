using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public LayerMask WallFloorLayer;

    public bool onRightWall;
    public bool onLeftWall;
    public bool onGround;

    public Color debuggingColor = Color.red;

    [Space]
    [Header("Gizmos")]

    public float collisionRadius;
    public Vector3 boxSize;


    public Vector2 rightOffset;
    public Vector2 leftOffset;
    public Vector2 bottomOffset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, WallFloorLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, WallFloorLayer);
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, boxSize, 0, WallFloorLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debuggingColor;

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, boxSize);
    }
}
