using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public GameObject taptostartUI;
    public GameObject upgradesUI;
    public GameObject tryagainUI;
    public GameObject levelIndicatorUI;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI staminaUpgradeText;
    public TextMeshProUGUI incomeUpgradeText;
    public TextMeshProUGUI speedUpgradeText;
    public TextMeshProUGUI staminaLevelText;
    public TextMeshProUGUI incomeLevelText;
    public TextMeshProUGUI speedLevelText;
    public GameObject staminaUpgradeButtonGameobject;
    public GameObject incomeUpgradeButtonGameobject;
    public GameObject speedUpgradeButtonGameobject;

    private GameHandler gameHandlerScript;
    private PlayerController playerControllerScript;
    private CameraFollow cameraFollowScript;
    private SpawnHandler spawnHandlerScript;
    private Data data;

    private void Start()
    {
        gameHandlerScript = GetComponent<GameHandler>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraFollowScript = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        spawnHandlerScript = GetComponent<SpawnHandler>();

        data = gameHandlerScript.GetData();

        // at start, configure the ui elements
        StartUI();
    }

    public void GameOver()
    {
        tryagainUI.SetActive(true);
    }

    public void LevelPassed()
    {
        gameHandlerScript.level++;
        gameHandlerScript.speed = 0.5f + (gameHandlerScript.level * 0.1f);
        gameHandlerScript.maxStamina /= 2;
        gameHandlerScript.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartUI()
    {
        moneyText.text = data.money.ToString("0.0");
        levelText.text = "Level " + data.level.ToString();
        staminaUpgradeText.text = (Mathf.Pow(1.15f, data.staminaLevel-1) * 20f).ToString("0.0");
        incomeUpgradeText.text = (Mathf.Pow(1.15f, data.incomeLevel-1) * 20f).ToString("0.0");
        speedUpgradeText.text = (Mathf.Pow(1.15f, data.speedLevel-1) * 20f).ToString("0.0");
        staminaLevelText.text = "LVL " + data.staminaLevel;
        incomeLevelText.text = "LVL " + data.incomeLevel;
        speedLevelText.text = "LVL " + data.speedLevel;

        tryagainUI.SetActive(false);
        upgradesUI.SetActive(true);
        taptostartUI.SetActive(true);
        levelIndicatorUI.SetActive(true);

        CheckButtonsInteractable();
    }

    private void CheckButtonsInteractable()
    {
        staminaUpgradeButtonGameobject.GetComponent<Button>().interactable = (gameHandlerScript.money >= float.Parse(staminaUpgradeText.text));
        incomeUpgradeButtonGameobject.GetComponent<Button>().interactable = (gameHandlerScript.money >= float.Parse(incomeUpgradeText.text));
        speedUpgradeButtonGameobject.GetComponent<Button>().interactable = (gameHandlerScript.money >= float.Parse(speedUpgradeText.text));

    }

    // BUTTON FUNCTIONS \\

    public void UpgradeStamina() // upgrade stamina button
    {
        float upgradeValue = float.Parse(staminaUpgradeText.text);
        if (upgradeValue <= gameHandlerScript.money)
        {
            gameHandlerScript.staminaLevel++;
            gameHandlerScript.money -= upgradeValue;
            moneyText.text = gameHandlerScript.money.ToString("0.0");
            staminaLevelText.text = "LVL " + gameHandlerScript.staminaLevel;

            gameHandlerScript.maxStamina += 10f;
            gameHandlerScript.currStamina = gameHandlerScript.maxStamina;
            staminaUpgradeText.text = (Mathf.Pow(1.15f, gameHandlerScript.staminaLevel-1) * 20f).ToString("0.0");

            gameHandlerScript.SaveData();
            data = gameHandlerScript.GetData();
            CheckButtonsInteractable();
        }
    }

    public void UpgradeIncome() // upgrade income button
    {
        float upgradeValue = float.Parse(incomeUpgradeText.text);
        if (upgradeValue <= gameHandlerScript.money)
        {
            gameHandlerScript.incomeLevel++;
            gameHandlerScript.money -= upgradeValue;
            moneyText.text = gameHandlerScript.money.ToString("0.0");
            incomeLevelText.text = "LVL " + gameHandlerScript.incomeLevel;

            gameHandlerScript.income += 0.1f;
            incomeUpgradeText.text = (Mathf.Pow(1.15f, gameHandlerScript.incomeLevel-1) * 20f).ToString("0.0");

            gameHandlerScript.SaveData();
            data = gameHandlerScript.GetData();
            CheckButtonsInteractable();
        }
    }

    public void UpgradeSpeed() // upgrade speed button
    {
        float upgradeValue = float.Parse(speedUpgradeText.text);
        if (upgradeValue <= gameHandlerScript.money)
        {
            gameHandlerScript.speedLevel++;
            gameHandlerScript.money -= upgradeValue;
            moneyText.text = gameHandlerScript.money.ToString("0.0");
            speedLevelText.text = "LVL " + gameHandlerScript.speedLevel;

            gameHandlerScript.speed += 0.05f;
            speedUpgradeText.text = (Mathf.Pow(1.15f, gameHandlerScript.speedLevel-1) * 20f).ToString("0.0");

            gameHandlerScript.SaveData();
            data = gameHandlerScript.GetData();
            CheckButtonsInteractable();

            playerControllerScript.speed = data.speed;
            playerControllerScript.SetAnimSpeed();
        }
    }

    public void Restart() // restart button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame() // tap to play button
    {
        taptostartUI.SetActive(false);
        upgradesUI.SetActive(false);
        levelIndicatorUI.SetActive(false);

        playerControllerScript.enabled = true;
        cameraFollowScript.enabled = true;
        spawnHandlerScript.enabled = true;
    }

}
