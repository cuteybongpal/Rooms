using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Base
{
    [Header("버튼")]
    public Button B_Start;
    public Button B_Desc;
    public Button B_Setting;
    [Header("게임 오브젝트")]
    public GameObject Description;
    public GameObject Settings;
    bool isShowd = false;
    bool isShowS = false;
    [Header("텍스트")]
    public Text[] texts;
    
    async void Start()
    {
        B_Start.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });
        B_Desc.onClick.AddListener(() =>
        {
            isShowd = !isShowd;
            Description.gameObject.SetActive(isShowd);
        });
        B_Setting.onClick.AddListener(() =>
        {
            isShowS = !isShowS;
            Settings.gameObject.SetActive(isShowS);
        });
        IDataLoad load = DIContainer.GetInstance<IDataLoad>() as IDataLoad;
        load.Init();
        List<RankingData> ranking =  await load.LoadRanking();
        ranking.Sort((a, b) => a.score.CompareTo(b.score));
        for (int i = 0; i < texts.Length; i++)
        {
            if (i > ranking.Count - 1)
                break;
            texts[i].text = $"{ranking[i].name}:{ranking[i].score}s";
        }
    }
}
