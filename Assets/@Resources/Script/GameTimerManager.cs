// GameTimerManager.cs
using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager instance;

    private float startTime;
    private bool isTiming = false;
    private float finalElapsedTime = 0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // ���� �ٲ� �� ������Ʈ�� �����ǰ� �Ϸ��� �Ʒ� �ּ� ����
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
        finalElapsedTime = 0f; // Ÿ�̸� ����� �� ���� �ð� �ʱ�ȭ
        Debug.Log("���� Ÿ�̸� ����!");
    }

    public void StopTimer()
    {
        if (isTiming)
        {
            finalElapsedTime = Time.time - startTime;
            isTiming = false;
            Debug.Log("���� Ÿ�̸� ����! ��� �ð�: " + finalElapsedTime.ToString("F2") + "��");
        }
    }

    public float GetElapsedTime()
    {
        if (isTiming)
        {
            return Time.time - startTime;
        }
        return finalElapsedTime; // Ÿ�̸Ӱ� �������� ���� ������ �ð��� ��ȯ
    }

    public string GetFormattedElapsedTime()
    {
        float timeToFormat = GetElapsedTime(); // �׻� ���� �Ǵ� ���� ��� �ð��� ������
        int minutes = Mathf.FloorToInt(timeToFormat / 60F);
        int seconds = Mathf.FloorToInt(timeToFormat % 60F);
        // int milliseconds = Mathf.FloorToInt((timeToFormat * 100F) % 100F); // �ʿ�� ���
        // return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool IsTiming()
    {
        return isTiming;
    }
}