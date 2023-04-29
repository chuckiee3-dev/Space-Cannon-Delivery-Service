using UnityEngine;

public class PlayerPrefsSaveManager : ISaveManager
{
    private const string LEVEL_SAVE_KEY = "Current_Level";
    
    public int CurrentLevel() {
        return PlayerPrefs.GetInt( LEVEL_SAVE_KEY, 0 );
    }

    public void LevelCompleted() {
        int finishedLevel = PlayerPrefs.GetInt( LEVEL_SAVE_KEY, 0 );
        finishedLevel++;
        PlayerPrefs.SetInt( LEVEL_SAVE_KEY, finishedLevel );
    }

}
