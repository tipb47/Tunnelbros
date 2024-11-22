using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager Instance { get; private set; }

    private GameObject gameOverUI;
    private bool isGameOver = false;

    public GameObject doubleJumpDoor;
    public GameObject punchDoor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void HideAbilityDoors()
    {
        if (doubleJumpDoor != null) { doubleJumpDoor.SetActive(false); }
        if (punchDoor != null) { punchDoor.SetActive(false); }
    }

	// attempting to combat some of the issues with UI not hiding after restart with these
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAbilityDoors();

        isGameOver = false;
        Time.timeScale = 1;

		//dynamically find new UI
        gameOverUI = GameObject.Find("GameOverUI");
        if (gameOverUI != null)
        {
            CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0; // hide via alpha
            }
            else
            {
                Debug.LogWarning("CanvasGroup component not found on GameOverUI.");
            }
        }
        else
        {
            Debug.LogWarning("GameOverUI not found in the scene.");
        }

        NotificationManager.Instance.ShowNotification("There are 3 hearts. can you find them all?", 3f);
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            Time.timeScale = 0;

            if (gameOverUI != null)
            {

                CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1; // Make UI visible
                }
                else
                {
                    Debug.LogWarning("CanvasGroup component not found on GameOverUI.");
                }
            }
            else
            {
                Debug.LogWarning("GameOverUI not found when GameOver was called.");
            }
        }
    }

    private void Update()
    {

        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {

        Time.timeScale = 1;
        isGameOver = false;


        if (gameOverUI != null)
        {
            CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                Debug.LogWarning("CanvasGroup component not found on GameOverUI.");
            }
        }
        else
        {
            Debug.LogWarning("GameOverUI not found when RestartGame was called.");
        }


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
