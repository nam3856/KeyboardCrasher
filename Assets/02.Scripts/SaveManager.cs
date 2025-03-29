using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private bool _notSaved;
    [SerializeField] private PunchingBag _punchingBag;

    private void Awake()
    {
        _notSaved = true;
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        UI_Game.Instance.OnBestScoreChanged += SaveStart;
    }

    public void SaveStart(float score)
    {
        if (!_notSaved) return;
        _notSaved = false;

        RankingManager.Instance.UploadScore(PlayerManager.Instance.playerID, UI_Game.Instance.Name, score);
        PlayerPrefs.SetString("BestScore", score.ToString());

    }
}
