// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class MyCharacter : MonoBehaviour
{

    // The Rewired player id of this character
    public int playerId = 0;

    // The movement speed of this character
    public float moveSpeed = 3.0f;

    // The bullet speed
    public float bulletSpeed = 15.0f;

    // Assign a prefab to this in the inspector.
    // The prefab must have a Rigidbody component on it in order to work.
    public GameObject bulletPrefab;

    private Player player; // The Rewired Player
    //private CharacterController cc;
    private Vector3 moveVector;
    private bool interact;
    private Rigidbody _rb;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        _rb = GetComponent<Rigidbody>();
        // Get the character controller
        //cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
        moveVector.z = player.GetAxis("Move Vertical");
        interact = player.GetButtonDown("Interact");
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.z != 0.0f)
        {
            //cc.Move(moveVector * moveSpeed * Time.deltaTime);
            _rb.AddForce(moveVector * moveSpeed);
        }

        // Process fire
        if (interact)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.right, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.right * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}