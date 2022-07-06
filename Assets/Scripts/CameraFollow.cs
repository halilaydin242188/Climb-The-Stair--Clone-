using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerControllerScript;
    private GameHandler gameHandlerScript;

    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = player.GetComponent<PlayerController>();
        gameHandlerScript = GameObject.Find("GameManager").GetComponent<GameHandler>();

        enabled = false;
    }
    private void LateUpdate() {
        
    }
    void Update()
    {
        /*
        Vector3 destPos = new Vector3(transform.position.x, player.transform.position.y + 0.75f, transform.position.z);
        float t = Time.deltaTime * playerControllerScript.speed;
        transform.position = Vector3.Lerp(transform.position, destPos, t);
        */

        if (!gameHandlerScript.gameOver)
        {
            Vector3 destPos = new Vector3(transform.position.x, player.transform.position.y + 0.75f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destPos, 0.5f);
            if (playerControllerScript.isPlayerMoving)
            {
                if (transform.rotation.eulerAngles.x >= 7.5f)
                    transform.Rotate(Vector3.left * 0.01f);
            }
            else
            {
                if (transform.rotation.eulerAngles.x < 15f)
                    transform.Rotate(Vector3.right * 0.02f);
            }
        }
    }
}
