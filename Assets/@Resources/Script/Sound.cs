using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.Instance.volume;
    }
    private void Update()
    {
        audioSource.volume = GameManager.Instance.volume;
    }
}
