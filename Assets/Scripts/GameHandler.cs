using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public bool gameOver = false;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scoreBoardText;
    public ParticleSystem sweatParticle;
    public ParticleSystem explosionParticle;
    public GameObject player;
    public GameObject taptostartUI;
    public GameObject upgradesUI;
    public GameObject tryagainUI;

    private int level;
    private float money;
    private float moneyAddition;
    private float maxStamina;
    private float currStamina;
    private float scoreBoardValue = 400f;
    private SpawnManager spawnManagerScript;
    private PlayerController playerControllerScript;
    private CameraFollow cameraFollowScript;


    void Start()
    {
        spawnManagerScript = GetComponent<SpawnManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraFollowScript = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

        LoadGame();
        currStamina = maxStamina;
        moneyText.text = money.ToString();

        tryagainUI.SetActive(false);
        upgradesUI.SetActive(true);
        taptostartUI.SetActive(true);
    }

    void Update()
    {
        if (gameOver)
        {
            player.SetActive(false);
            this.gameObject.SetActive(false);
            tryagainUI.SetActive(true);
            SaveData();
        }
        else
        {
            explosionParticle.transform.position = player.transform.position + new Vector3(-0.5f, 0.36f, 0);
        }
    }

    public void updateMoney() // gets called from SpawnManager
    {
        money += moneyAddition;
        moneyText.text = money.ToString("0.0");
    }

    public void updateScoreBoard() // gets called from SpawnManager
    {
        scoreBoardValue -= 0.07f;
        scoreBoardText.text = scoreBoardValue.ToString("0.0") + "m";
    }

    public void updateStamina() // gets called from SpawnManager
    {
        currStamina -= 0.5f;
        if (currStamina <= 0)
        {
            explosionParticle.Play();
            gameOver = true;
        }
        else if (currStamina <= maxStamina / 2)
        {
            if (!sweatParticle.isPlaying)
                sweatParticle.Play();
        }
    }

    // functions for buttons
    public void UpgradeStamina()
    {
        maxStamina += 10f;
    }

    public void UpgradeIncome()
    {
        moneyAddition += 0.1f;
    }

    public void UpgradeSpeed()
    {
        playerControllerScript.speed += 0.1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        taptostartUI.SetActive(false);
        upgradesUI.SetActive(false);

        playerControllerScript.enabled = true;
        cameraFollowScript.enabled = true;
        spawnManagerScript.enabled = true;
    }

    // save/load game data
    private Data CreateDataObject()
    {
        Data data = new Data();
        data.level = level;
        data.speed = playerControllerScript.speed;
        data.money = money;
        data.moneyAddition = moneyAddition;
        data.maxStamina = maxStamina;

        return data;
    }

    private void SaveData()
    {
        Data data = CreateDataObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamedata.data");
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");
    }

    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamedata.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamedata.data", FileMode.Open);
            Data data = (Data)bf.Deserialize(file);
            file.Close();

            level = data.level;
            playerControllerScript.speed = data.speed;
            money = data.money;
            moneyAddition = data.moneyAddition;
            maxStamina = data.maxStamina;
        }
        else // no saved data, initialize the variables
        {
            level = 1;
            playerControllerScript.speed = 1f;
            money = 0;
            moneyAddition = 0.5f;
            maxStamina = 20f;
        }
    }

}
