using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
	[SerializeField] private NotificationSO notificationData; //put SO asset here
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (NotificationManager.Instance != null)
			{
				NotificationManager.Instance.ShowNotification(notificationData);
			}
			gameObject.SetActive(false); //for one-time triggers
		}
	}
}
