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
            // 씬이 바뀌어도 이 오브젝트가 유지되게 하려면 아래 주석 해제
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
        finalElapsedTime = 0f; // 타이머 재시작 시 최종 시간 초기화
        Debug.Log("게임 타이머 시작!");
    }

    public void StopTimer()
    {
        if (isTiming)
        {
            finalElapsedTime = Time.time - startTime;
            isTiming = false;
            Debug.Log("게임 타이머 중지! 경과 시간: " + finalElapsedTime.ToString("F2") + "초");
        }
    }

    public float GetElapsedTime()
    {
        if (isTiming)
        {
            return Time.time - startTime;
        }
        return finalElapsedTime; // 타이머가 멈췄으면 멈춘 시점의 시간을 반환
    }

    public string GetFormattedElapsedTime()
    {
        float timeToFormat = GetElapsedTime(); // 항상 현재 또는 최종 경과 시간을 가져옴
        int minutes = Mathf.FloorToInt(timeToFormat / 60F);
        int seconds = Mathf.FloorToInt(timeToFormat % 60F);
        // int milliseconds = Mathf.FloorToInt((timeToFormat * 100F) % 100F); // 필요시 사용
        // return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool IsTiming()
    {
        return isTiming;
    }
}