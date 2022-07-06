using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayerMoving = false;
    public float speed;
    public float moveValue; 

    private Animator playerAnim;
    private SpawnHandler spawnHandlerScript;
    private GameHandler gameHandlerScript;
    public float currRotation = 30; // to animate climbing, changes value between 0 to 30f for every climb

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        spawnHandlerScript = GameObject.Find("GameManager").GetComponent<SpawnHandler>();
        gameHandlerScript = GameObject.Find("GameManager").GetComponent<GameHandler>();

        speed = gameHandlerScript.speed;

        SetAnimSpeed();

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
        //float moveValue;
        if (currRotation < 30f)
        {
            moveValue = (currRotation + speed > 30f) ? (30f - currRotation) : speed;

            transform.RotateAround(Vector3.zero, Vector3.up, moveValue);
            transform.Translate(0, 0.14f / (30f / moveValue), 0);

            currRotation += moveValue;
        }
        else
            isPlayerMoving = false;

    }

    public void SetAnimSpeed()
    {
        playerAnim.SetFloat("climbSpeed", 1.8f + ((gameHandlerScript.speedLevel - 1) * 0.2f));
    }
}
