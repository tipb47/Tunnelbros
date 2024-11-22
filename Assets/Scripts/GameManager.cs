using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    private GameObject gameOverUI;
    private bool isGameOver = false;

    public GameObject doubleJumpDoor;
    public GameObject punchDoor;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    private void HideAbilityDoors()
    {
        if (doubleJumpDoor != null) { doubleJumpDoor.SetActive(false); }
        if (punchDoor != null) { punchDoor.SetActive(false); }
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAbilityDoors();

        // Reset game state variables
        isGameOver = false;
        Time.timeScale = 1;

        // Dynamically find the GameOverUI in the scene
        gameOverUI = GameObject.Find("GameOverUI");
        if (gameOverUI != null)
        {
            // Ensure the Game Over UI is hidden at the start
            CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0; // Make UI invisible
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

            // Pause the game
            Time.timeScale = 0;

            if (gameOverUI != null)
            {
                // Show the Game Over UI
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
        // Check for left-click input to restart the game
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        // Resume the game
        Time.timeScale = 1;
        isGameOver = false;

        // Hide the Game Over UI
        if (gameOverUI != null)
        {
            CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0; // Make UI invisible
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

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
