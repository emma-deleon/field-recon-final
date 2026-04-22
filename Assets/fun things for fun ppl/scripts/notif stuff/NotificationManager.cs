using UnityEngine;
using System.Collections;       // Needed for IEnumerator and Coroutines
using System.Collections.Generic; // Needed for Queue<T>
using TMPro;                    // Needed for TMP_Text

public class NotificationManager : MonoBehaviour
{
	public static NotificationManager Instance;
	
	[Header("Notification UI")]
	[SerializeField] private GameObject notificationParent; //parent gameobject with canvasgroup
	[SerializeField] private TMP_Text notificationTextUI; //text component
	[SerializeField] private AudioSource UIAudioSource;
	
	private CanvasGroup notificationUICanvasGroup;
	private Queue<NotificationSO> notificationQueue = new Queue<NotificationSO>();
	private bool isDisplayingNotification = false;
	
	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
		
		notificationUICanvasGroup = notificationParent.GetComponent<CanvasGroup>();
		notificationUICanvasGroup.alpha = 0;
	}
	
	public void ShowNotification(NotificationSO notificationData)
	{
		notificationQueue.Enqueue(notificationData);
		if (!isDisplayingNotification)
		{
			StartCoroutine(DisplayNotification());
		}
	}
	
	private IEnumerator DisplayNotification()
	{
		isDisplayingNotification = true;
		while (notificationQueue.Count > 0)
		{
			NotificationSO data = notificationQueue.Dequeue();
			
			//Apply SO data
			notificationTextUI.text = data.Message;
			
			if (data.NotificationSound != null && UIAudioSource != null)
            {
                UIAudioSource.PlayOneShot(data.NotificationSound);
            }
			
			//fade in
			yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, true, data.FadeInDuration));
			
			//display duration godbless
			yield return new WaitForSeconds(data.DisplayDuration);
			
			//fade OUT
			yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, false, data.FadeOutDuration));
		}
		isDisplayingNotification = false;
	}
	
	public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn, float duration)
	{
		float targetAlpha = fadeIn ? 1f : 0f;
		float initialAlpha = canvasGroup.alpha;
		float elapsedTime = 0f;
		
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
			yield return null;
		}
		canvasGroup.alpha = targetAlpha;
	}
}
