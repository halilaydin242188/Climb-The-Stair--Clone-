using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject stickPrefap;
    public GameObject stairPrefap;
    public GameObject wallPrefap;
    public GameObject scoreBoard;

    private GameObject sticksParent;
    private GameObject stairsParent;
    private PlayerController playerControllerScript;
    private GameHandler gameHandlerScript;
    private float stickSpawnPosY = 0.08f;
    private float stairSpawnPosY = 0.04f;
    private float stairRotation = 15;

    void Start()
    {
        sticksParent = GameObject.Find("Sticks");
        stairsParent = GameObject.Find("Stairs");
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameHandlerScript = GameObject.Find("GameManager").GetComponent<GameHandler>();

        stairPrefap.transform.rotation.Set(0, 90, 0, stairPrefap.transform.rotation.w);

        enabled = false;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !playerControllerScript.isPlayerMoving && !gameHandlerScript.gameOver)
        {
            CreateStick();
            CreateStick();
            scoreBoard.transform.Translate(new Vector3(0, 0.14f, 0));
            CreateStair();
            CreateStair();
        }
    }

    private void CreateStick()
    {
        GameObject stick = Instantiate(stickPrefap, new Vector3(0, stickSpawnPosY, 0), stickPrefap.transform.rotation);
        stick.transform.SetParent(sticksParent.transform);
        stickSpawnPosY += 0.07f;
    }

    private void CreateStair()
    {   
        // Create a stair, rotate, set parent, update the variables
        Vector3 instantiatePos = new Vector3(stairPrefap.transform.position.x, stairSpawnPosY, stairPrefap.transform.position.z);
        GameObject stair = Instantiate(stairPrefap, instantiatePos, stairPrefap.transform.rotation);
        stair.transform.RotateAround(Vector3.zero, Vector3.up, stairRotation);
        stair.transform.SetParent(stairsParent.transform);
        stairSpawnPosY += 0.07f;
        stairRotation += 15f;
        stairRotation %= 360;

        // Increase the money and the score board value
        gameHandlerScript.updateMoney();
        gameHandlerScript.updateScoreBoard();
        gameHandlerScript.updateStamina();
    }
}