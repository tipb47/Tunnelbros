using UnityEngine;
using System.Collections;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    // text noti
    [SerializeField] private TextMeshProUGUI notificationText;

    private Coroutine notificationCoroutine;

    private void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (notificationText == null)
        {
            Debug.LogError("NotificationManager: Notification Text is not assigned in the Inspector.");
        }
    }
    public void ShowNotification(string message, float duration)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = StartCoroutine(DisplayNotification(message, duration));
    }

    private IEnumerator DisplayNotification(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        notificationText.gameObject.SetActive(false);
        notificationCoroutine = null;
    }
}
