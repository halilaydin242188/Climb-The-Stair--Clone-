using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scoreBoardText;
    public ParticleSystem sweatParticle;
    public ParticleSystem explosionParticle;
    public bool gameOver = false;
    public float levelFinishPosY;
    public int level;
    public float speed;
    public float income;
    public float maxStamina;
    public float currStamina;
    public float money;
    public int staminaLevel;
    public int incomeLevel;
    public int speedLevel;

    private SpawnHandler spawnHandlerScript;
    private CameraFollow cameraFollowScript;
    private UIHandler uiHandlerScript;
    private GameObject player;
    private float scoreBoardValue = 400f;


    void Start()
    {
        spawnHandlerScript = GetComponent<SpawnHandler>();
        uiHandlerScript = GetComponent<UIHandler>();
        cameraFollowScript = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        player = GameObject.Find("Player");

        LoadData();
        currStamina = maxStamina;
    }

    void Update()
    {
        if (gameOver)
        {
            player.SetActive(false);
            this.gameObject.SetActive(false);
            SaveData();
            uiHandlerScript.GameOver();
        }
        else
        {
            explosionParticle.transform.position = player.transform.position + new Vector3(-0.5f, 0.36f, 0);

            if (player.transform.position.y >= levelFinishPosY) // is level passed
            {
                Debug.Log("Game Won");
            }
        }
    }

    public void updateMoney() // gets called from SpawnManager
    {
        money += income;
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

    // save/load/get game data
    public Data GetData()
    {
        if (File.Exists(Application.persistentDataPath + "/gamedata.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamedata.data", FileMode.Open);
            Data data = (Data)bf.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            Data data = new Data();
            data.level = 1;
            data.staminaLevel = 1;
            data.incomeLevel = 1;
            data.speedLevel = 1;
            data.speed = 0.5f;
            data.money = 0;
            data.income = 0.5f;
            data.maxStamina = 20f;

            return data;
        }
    }    
    private Data CreateDataObject()
    {
        Data data = new Data();
        data.level = level;
        data.staminaLevel = staminaLevel;
        data.incomeLevel = incomeLevel;
        data.speedLevel = speedLevel;
        data.speed = speed;
        data.money = money;
        data.income = income;
        data.maxStamina = maxStamina;

        return data;
    }
    public void SaveData()
    {
        Data data = CreateDataObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamedata.data");
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");
        Debug.Log(Application.persistentDataPath);
    }

    private void LoadData()
    {
        Data data = GetData();

        level = data.level;
        staminaLevel = data.staminaLevel;
        incomeLevel = data.incomeLevel;
        speedLevel = data.speedLevel;
        speed = data.speed;
        money = data.money;
        income = data.income;
        maxStamina = data.maxStamina;
    }

}
