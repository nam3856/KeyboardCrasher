using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string playerID;
    public static PlayerManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            playerID = PlayerPrefs.GetString("PlayerID");
        }
        else
        {
            playerID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayerID", playerID);
        }
        Debug.Log("Player ID: " + playerID);
    }
}
