using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayerMoving = false;

    private Animator playerAnim;
    private SpawnHandler spawnHandlerScript;
    private GameHandler gameHandlerScript;
    private float currRotation = 30f; // to animate climbing, changes value between 0 to 30f for every climb
    private float speed;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        spawnHandlerScript = GameObject.Find("GameManager").GetComponent<SpawnHandler>();
        gameHandlerScript = GameObject.Find("GameManager").GetComponent<GameHandler>();

        speed = gameHandlerScript.speed;

        enabled = false;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !isPlayerMoving)
        {
            isPlayerMoving = true;
            currRotation = 0;

            spawnHandlerScript.SpawnNext();

            playerAnim.SetTrigger("climbStair");
        }

        MovePlayer();
    }

    private void MovePlayer()
    {

        if (currRotation < 30f)
        {
            transform.RotateAround(Vector3.zero, Vector3.up, speed);
            transform.Translate(0, 0.14f / (30f / speed), 0);

            currRotation += speed;
        }
        else
            isPlayerMoving = false;

    }
}
