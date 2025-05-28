﻿using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;

public class DataManager
{
    public static DatabaseReference dbRef;
    public static FirebaseAuth Auth;
    public static FirebaseUser User;
    static bool isInitialized = false;
    static bool isSignIn = false;
    async void SignInAnonymously(FirebaseApp app)
    {
        Debug.Log("로그인 중");
        try
        {
            Auth = FirebaseAuth.GetAuth(app);
            AuthResult res = await Auth.SignInAnonymouslyAsync();
            User = res.User;
            Debug.Log("�α��� ����! UID: " + User.UserId);
            isSignIn = true;
        }

        catch (Exception e)
        {
            Debug.LogError("�͸� �α��� ���� (await): " + e.Message);
        }
    }
    public async void SaveRanking(RankingData rankingData)
    {
        Debug.Log("랭킹 세이브(데이터 매니저 직접 접근 단계)");
        while (!isSignIn)
        {
            await UniTask.Yield();
        }
        Debug.Log("랭킹 세이브(데이터 매니저 직접 접근 단계)");
        var app = Auth.App;
        var option = app.Options;

        Debug.Log("Project ID: " + option.ProjectId);
        Debug.Log("App ID: " + option.AppId);
        Debug.Log("Database URL: " + option.DatabaseUrl);

        string jsonData = JsonUtility.ToJson(rankingData);

        try
        {
            await dbRef.Child("rankings").Child(User.UserId).SetRawJsonValueAsync(jsonData);
            Debug.Log("랭킹 저장 성공");
        }
        catch (Exception e)
        {
            Debug.LogError($"랭킹 저장 실패{e.StackTrace}");
        }

    }
    public async UniTask<List<RankingData>> LoadRanking()
    {
        while (!isSignIn)
        {
            await UniTask.Yield();
        }
        DataSnapshot snapshot = null;
        List<RankingData> rankings = new List<RankingData>();

        try
        {
            snapshot = await dbRef.Child("rankings").GetValueAsync();
            Debug.Log("�ε� �Ϸ�");
        }

        catch (Exception e)
        {
            Debug.LogError($"�ε� ���� {e.StackTrace}");
        }
        foreach (DataSnapshot d in snapshot.Children)
        {
            Dictionary<string, object> dict = d.Value as Dictionary<string, object>;
            string name = dict["name"].ToString();
            int score = int.Parse(dict["score"].ToString());
            RankingData data = new RankingData(name, score);
            rankings.Add(data);
        }
        return rankings;

    }
    public DataManager()
    {
        if (isInitialized)
            return;

        isInitialized = true;
        var options = new Firebase.AppOptions()
        {
            DatabaseUrl = new System.Uri("https://unity-ranking-default-rtdb.asia-southeast1.firebasedatabase.app"),
            ApiKey = "AIzaSyBNaMnGyzboF5zzRVVHRowqNQGyzaoZb5w",
            AppId = "1:118243326673:android:bd06138048ca3a27db23f0",
            ProjectId = "unity-ranking"
        };
        FirebaseApp app = FirebaseApp.Create(options, "ManualApp");
        FirebaseDatabase db = FirebaseDatabase.GetInstance(FirebaseApp.GetInstance("ManualApp"));

        dbRef = db.RootReference;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously(app);
            }
            else
            {
                Debug.LogError("Firebase Error: " + task.Result);
            }
        });
    }
}

[Serializable]
public class RankingData
{
    public string name;
    public int score;
    public RankingData()
    {

    }
    public RankingData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}