using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tracker  {
	public static int LevelChosen=1;

    public static int GetLevelRetries(int levelNum)
    {
        return PlayerPrefs.GetInt("levelretries" + levelNum, 9999);
    }

    public static void SetLevelRetries(int retries)
    {
        int current_retries = PlayerPrefs.GetInt("levelrank" + LevelChosen, 9999);

        if (retries < current_retries)
        {
            PlayerPrefs.SetInt("levelretries" + LevelChosen, retries);
        }
    }




    public static float GetLevelTime(int levelNum)
    {
        return PlayerPrefs.GetFloat("leveltime" + levelNum, 9999);
    }


    public static void SetLevelTime(float time)
    {

        float current_time = PlayerPrefs.GetFloat("leveltime" + LevelChosen, 9999);
        if (time < current_time)
            PlayerPrefs.SetFloat("leveltime" + LevelChosen, time);
    }
}
