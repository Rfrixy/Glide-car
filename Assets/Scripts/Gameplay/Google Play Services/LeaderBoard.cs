using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class LeaderBoard : MonoBehaviour {
    private List<string> leaderboards;
	// Use this for initialization
	void Start () {
        leaderboards = new List<string>();

        leaderboards.Add(GPGSIds.leaderboard_hi_there);
        leaderboards.Add(GPGSIds.leaderboard_get_jumping);
        leaderboards.Add(GPGSIds.leaderboard_air_steer);
        leaderboards.Add(GPGSIds.leaderboard_handbrake_life);
        leaderboards.Add(GPGSIds.leaderboard_final_exam);
    }
	
    
    public void pushScore(long score,int levelNum)
    {

        //if (!Social.localUser.authenticated)
        //    Social.localUser.Authenticate((bool success) =>
        //    {
        //        if (success)
        //        {
        //            Debug.Log("You've successfully logged in");
        //        }
        //        else
        //        {
        //            Debug.Log("Login failed for some reason");
        //        }
        //    });

        if (Social.localUser.authenticated)
            Social.ReportScore(score, leaderboards[levelNum], (bool success) =>
            {

            });
    }
}
