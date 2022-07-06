using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public GameObject wallMain; // the first wall;
    public GameObject ground;
    public GameObject stickPrefap;
    public GameObject stairPrefap;
    public GameObject wallPrefap;
    public Material[] materials;

    private GameObject scoreBoard;
    private GameObject sticksParent;
    private GameObject stairsParent;
    private PlayerController playerControllerScript;
    private GameHandler gameHandlerScript;
    private float stickSpawnPosY = 0.08f;
    private float stairSpawnPosY = 0.04f;
    private float stairRotation = 15;

    void Start()
    {
        scoreBoard = GameObject.Find("ScoreBoard");
        sticksParent = GameObject.Find("Sticks");
        stairsParent = GameObject.Find("Stairs");
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameHandlerScript = GameObject.Find("GameManager").GetComponent<GameHandler>();

        stairPrefap.transform.rotation.Set(0, 90, 0, stairPrefap.transform.rotation.w);

        PrepareLevel();

        enabled = false;
    }

    public void SpawnNext()
    {
        CreateStick();
        CreateStick();
        scoreBoard.transform.Translate(new Vector3(0, 0.14f, 0));
        CreateStair();
        CreateStair();
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

    private void PrepareLevel(int maxWallCount = 5)
    {
        ground.GetComponent<MeshRenderer>().material = materials[gameHandlerScript.level];

        Material[] wallMaterials = { materials[gameHandlerScript.level], materials[gameHandlerScript.level] };
        wallMain.GetComponent<MeshRenderer>().materials = wallMaterials;

        const float startPosY = 2.7f;

        for (int i = 1; i <= maxWallCount; i++)
        {
            Vector3 instantiatePos = new Vector3(-1f, startPosY + i * 5.3f, 0);
            GameObject wall = Instantiate(wallPrefap, instantiatePos, wallPrefap.transform.rotation);
            wall.GetComponent<MeshRenderer>().materials = wallMaterials;

            wall.transform.SetParent(wallMain.transform);
        }

        // create a wall with next level's material to the end
        Vector3 instPos = new Vector3(-1f, startPosY + (maxWallCount + 1) * 5.3f, 0);
        GameObject lastWall = Instantiate(wallPrefap, instPos, wallPrefap.transform.rotation);
        Material[] nextLevelWallMat = { materials[gameHandlerScript.level + 1], materials[gameHandlerScript.level + 1] };
        lastWall.GetComponent<MeshRenderer>().materials = nextLevelWallMat;

        lastWall.transform.SetParent(wallMain.transform);

        gameHandlerScript.levelFinishPosY = 5f + (maxWallCount * 5.3f);
    }
}
