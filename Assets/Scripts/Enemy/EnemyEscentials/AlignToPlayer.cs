/*****************************************************************************
// File Name : AlignToPlayer.cs
// Author : Logan Dagenais
// Creation Date : March 4th, 2025
//
// Brief Description : This code gets the angle of the player to the enemy
and changes the sprite to whichever direction
*****************************************************************************/
using UnityEngine;

public class AlignToPlayer : MonoBehaviour
{
    private Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;
    private float angle;
    public int lastIndex;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    /// <summary>
    /// gets Sprite renderer and transform
    /// </summary>
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    /// <summary>
    /// Gets the direction the player is from the enemy and changes the sprite
    /// </summary>
    void Update()
    {
        // get Target position and direction
        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        targetDir = targetPos - transform.position;

        // Get angle
        angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        //flips sprites if needed
        Vector3 tempscale = Vector3.one;
        if (angle > 0)
        {
            tempscale.x *= -1f;
        }
        //changes sprite to what is chosen
        spriteRenderer.transform.localScale = tempscale;
        lastIndex = GetIndex(angle);


    }
    /// <summary>
    /// Gets the angle the player is from the enemy perspective and returns value to index to change sprite
    /// </summary>
    /// <param name="angle">angle player is from enemy perspective</param>
    /// <returns></returns>
    private int GetIndex(float angle)
    {
        //front
        if(angle > -22.5f && angle < 22.6f)
        {
            return 0;
        }
        if (angle >= 22.5f && angle < 67.5f)
        {
            return 7;
        }
        if (angle >= 67.5f && angle < 112.5f)
        {
            return 6;
        }
        if (angle >= 112.5f && angle < 157.5f)
        {
            return 5;
        }

        //back
        if (angle <= -157.5 ||  angle >= 157.5f)
        {
            return 4;
        }
        if (angle >= -157.4f && angle < -112.5f)
        {
            return 3;
        }
        if (angle >= -112.5f && angle < -67.5f)
        {
            return 2;
        }
        if (angle >= -67.5f && angle <= -22.5f)
        {
            return 1;
        }
        return lastIndex;
    }
    /// <summary>
    /// draws gizmo for angle
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPos);
    }
}
