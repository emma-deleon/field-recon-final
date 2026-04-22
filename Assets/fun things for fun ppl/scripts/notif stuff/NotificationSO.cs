using UnityEngine;

[CreateAssetMenu(fileName = "NotificationSO", menuName = "Scriptable Objects/NotificationSO")]
public class NotificationSO : ScriptableObject
{
    [SerializeField, TextArea] private string _message; //main text
	[SerializeField] private float _displayDuration = 2f; //onscreen timer
	[SerializeField] private float _fadeInDuration = 0.5f;
	[SerializeField] private float _fadeOutDuration = 1f;
	[SerializeField] private AudioClip _notificationSound;
	
	public string Message => _message;
	public float DisplayDuration => _displayDuration;
	public float FadeInDuration => _fadeInDuration;
	public float FadeOutDuration => _fadeOutDuration;
	public AudioClip NotificationSound => _notificationSound;
}
