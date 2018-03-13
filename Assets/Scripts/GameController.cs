using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using UnityEditor;

public class GameController : MonoBehaviour {
    private Text heartsText;
    private Text coinsText;
    private RectTransform endLevelPanel;
    private RectTransform gameOverPanel;
    private Dude2D player;
    private Transform playerSpawn;
    private Camera mainCamera;
    private GameState stateData;
    private string currentLevel;

    public enum States { running, gameOver, endLevel, endGame, death }
    public States activeState = States.running;
    public SoundController soundController;

    [Header("Player")]
    public Dude2D playerPrefab;

    [Header("Effects")]
    public Transform puffPrefab;

    public void InitGame(string level)
    {
        // Find components
        heartsText = GameObject.Find("HeartsText").GetComponent<Text>();
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        endLevelPanel = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasContainer>().EndLevelPanel;
        gameOverPanel = GameObject.FindObjectOfType<Canvas>().GetComponent<CanvasContainer>().GameOverPanel;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Dude2D>();
        playerSpawn = GameObject.Find("Spawn").transform;
        mainCamera = GameObject.FindObjectOfType<Camera>();
        stateData = GameObject.FindObjectOfType<GameState>();
        soundController = GameObject.FindObjectOfType<SoundController>();

        // Configure camera
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        mainCamera.GetComponent<CameraFollow>().m_Player = player.gameObject.transform;
        endLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        // Set state
        Debug.Log("Initing with level " + level);
        this.currentLevel = level;
        activeState = States.running;
    }

    public void Update()
    {
        if (activeState == States.running && stateData != null)
        {
            UpdateHud();
            if (stateData.Health == 0) // Game Over!
            {
                Debug.Log("Game Over started");
                activeState = States.gameOver;
                StartPlayerDeath();
                gameOverPanel.gameObject.SetActive(true);
            }
        }
    }

    public void GetCoin()
    {
        stateData.Coins++;     
    }

    public void PlayerHit(Transform enemy)
    {
        // Make sound
        soundController.PlayPlayerHurtSound();

        // Add hit
        player.AddHit(enemy);

        // Substract life
        stateData.Health--;
    }

    private IEnumerator GameRestart(int delay, string level, string debug)
    {
        Debug.Log(debug);
        yield return new WaitForSeconds(delay);
        stateData.Reset();
        Debug.Log("Unloading " + currentLevel);
        yield return SceneManager.UnloadSceneAsync(currentLevel);
        Debug.Log("Loading " + level);
        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        activeState = States.running;
        InitGame(level);
    }
    
    public void KillPlayer()
    {
        stateData.Health--;
        UpdateHud();
        StartCoroutine(PlayerDeath());
    }

    private void StartPlayerDeath()
    {
        Debug.Log("Player death started");
        activeState = States.death;
        StartCoroutine(PlayerDeath());
    }

    private IEnumerator PlayerDeath()
    {
        activeState = GameController.States.death;
        mainCamera.GetComponent<CameraFollow>().m_Player = mainCamera.transform;
        Transform t = player.transform;
        Instantiate(puffPrefab, t.position, Quaternion.identity);
        Destroy(player.gameObject);

        if (stateData.Health > 0)
        {
            yield return new WaitForSeconds(3);
            player = Instantiate(playerPrefab);
            player.GetComponent<Transform>().position = playerSpawn.position;          
            mainCamera.GetComponent<CameraFollow>().m_Player = player.transform;
            activeState = GameController.States.running;
        } else
        {
            gameOverPanel.gameObject.SetActive(true);
            Debug.Log("GameRestart after Player Death");
            yield return GameRestart(2, currentLevel, "PlayerDeath");
        }
    }

    private void UpdateHud()
    {
        if (stateData != null)
        {
            heartsText.text = stateData.Health + "";
            coinsText.text = stateData.Coins + "";
        }
    }

    public void NextLevelStart(string name)
    {
        StartCoroutine(NextLevel(name));
        activeState = States.endLevel;
    }

    private IEnumerator NextLevel(string name)
    {
        endLevelPanel.gameObject.GetComponentInChildren<Text>().text = "Level Complete!";
        endLevelPanel.gameObject.SetActive(true);
        mainCamera.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(3);
        Debug.Log("Unloading " + name);
        yield return SceneManager.UnloadSceneAsync(currentLevel);
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        currentLevel = name;
        InitGame(name);
    }

    public void EndGame(string text)
    {
        if (activeState != States.endGame)
        {
            activeState = States.endGame;
            endLevelPanel.gameObject.SetActive(true);
            endLevelPanel.gameObject.GetComponentInChildren<Text>().text = text;
            mainCamera.GetComponent<CameraFollow>().enabled = false;
            StartCoroutine(GameRestart(5, "Level1", "EndGame"));
        }
    }
    
}
