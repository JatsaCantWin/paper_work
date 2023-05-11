using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float teleportDistance = 5f; // distance to teleport the player
    private bool canTeleport = true; // flag to indicate if teleporting is allowed

    void Update()
    {
        // check if left or right arrow key is pressed
        if (Input.GetKey(KeyCode.LeftArrow) && canTeleport)
        {
            transform.position -= new Vector3(teleportDistance, 0, 0); // teleport left
            canTeleport = false; // prevent teleporting again until player hits the boundary
        }
        else if (Input.GetKey(KeyCode.RightArrow) && canTeleport)
        {
            transform.position += new Vector3(teleportDistance, 0, 0); // teleport right
            canTeleport = false; // prevent teleporting again until player hits the boundary
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // activation
            Debug.Log("Teleport up (placeholder)");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // going back down
            Debug.Log("Teleport down (placeholder)");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // check if player collides with a boundary
        if (other.CompareTag("Boundary"))
        {
            canTeleport = true; // allow teleporting again
        }
    }
}
