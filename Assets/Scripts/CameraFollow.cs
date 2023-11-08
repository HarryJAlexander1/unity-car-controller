using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variables for camera movement and rotation smoothness.
    public float MoveSmoothness;
    public float RotSmoothness;

    // Variables for camera movement and rotation offset.
    public Vector3 MoveOffset;
    public Vector3 RotOffset;

    // Variable for player to follow.
    public Transform Player;

    public GameObject GameManager;

    public void FixedUpdate()
    {
        // Follow the player every frame.
        FollowPlayer();
    }

    private void Awake()
    {
        SetPlayer();
    }

    // Set the player to follow
    public void SetPlayer()
    {
        GameManager = GameObject.Find("GameManager");
        GameManager gameManagerScript = GameManager.GetComponent<GameManager>();
        Player = gameManagerScript.Player.transform;
    }

    // Follow player function calls HandleMovement and HandleRotation.
    public void FollowPlayer()
    {
        HandleMovement();
        HandleRotation();
    }

    // HandleMovement is responsible for the movement of the camera when following the player.
    void HandleMovement()
    {
        Vector3 targetPos;
        targetPos = Player.transform.TransformPoint(MoveOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, MoveSmoothness * Time.deltaTime);

    }

    // HandleRotation is responsible for the rotation of the camera when following the player.
    void HandleRotation()
    {

        var direction = Player.transform.position - transform.position;
        var rotation = new Quaternion();
        rotation = Quaternion.LookRotation(direction + RotOffset, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, RotSmoothness * Time.deltaTime);
    }

}
