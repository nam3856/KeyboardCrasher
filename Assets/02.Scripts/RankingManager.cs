using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    const string baseUrl = "https://ranking-api-797224474650.asia-northeast3.run.app";

    [System.Serializable]
    public class ScoreUploadData
    {
        public string playerId;
        public string nickname;
        public float bestScore;
        public int bestCombo;
    }

    [System.Serializable]
    public class RankingEntry
    {
        public string nickname;
        public float bestScore;
        public int bestCombo;
    }

    [System.Serializable]
    public class RankingList
    {
        public List<RankingEntry> rankings;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    // 점수 업로드
    public void UploadScore(string playerId, string nickname, float score)
    {
        ScoreUploadData data = new ScoreUploadData
        {
            playerId = playerId,
            nickname = nickname,
            bestScore = score,
            bestCombo = UI_Game.Instance.ComboCount
        };

        StartCoroutine(UploadScoreCoroutine(data));
    }

    IEnumerator UploadScoreCoroutine(ScoreUploadData data)
    {
        string json = JsonUtility.ToJson(data);
        UnityWebRequest req = UnityWebRequest.Put($"{baseUrl}/score", json);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("점수 업로드 성공!");
        }
        else
        {
            Debug.LogError("업로드 실패: " + req.error);
        }
    }

    // 랭킹 받아오기
    public void GetTopRankings(int limit, Action<RankingList> onCompleted)
    {
        StartCoroutine(GetRankingCoroutine(limit, onCompleted));
    }

    IEnumerator GetRankingCoroutine(int limit, Action<RankingList> onCompleted)
    {
        UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/rankings?limit={limit}");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;

            RankingList list = JsonUtility.FromJson<RankingList>(json);

            onCompleted?.Invoke(list);
        }
        else
        {
            Debug.LogError("랭킹 요청 실패: " + req.error);
        }
    }
}
