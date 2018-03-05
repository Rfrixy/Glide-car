using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



[System.Serializable]
public class Level{
	public List<CheckPoint> Checkpoints;
	public GameObject Map;

}

public class GameManager : MonoBehaviour {

	public Text timerText;
	public Text retryText;
	private int RetryCount;
	public float timeTaken;
	private float startTime;
	public List<Level> Levels;
	public static GameManager SharedInstance;
	private MessagesManager MM;
	public GameObject Car;
	public int CurrentLevel;
    public LeaderBoard LB;
    [Header("EndSCreen")]
	public GameObject EndScreen;

    public TextMeshProUGUI TM_retry, TM_prev_retry, TM_time, TM_prev_time;
	void Awake(){
		SharedInstance = this;

	}
	// Use this for initialization
	void Start () {
        EndScreen.SetActive(false);

        CurrentLevel = Tracker.LevelChosen;
		Levels [CurrentLevel].Map.SetActive (true);
		Car.transform.position=Levels[CurrentLevel].Checkpoints[0].SpawnPoint.transform.position;
		Car.transform.rotation=Levels[CurrentLevel].Checkpoints[0].SpawnPoint.transform.rotation;
		MM = GetComponent<MessagesManager> ();
		MM.DisplayMessage ("GO!");
		startTime = Time.time;
		StartCoroutine (Timer ());
        LevelMessages();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape))
			GoToMenu ();
	}
	private IEnumerator Timer(){

		while (true) {
			yield return new WaitForSeconds (0.2f);
			timeTaken = Time.time - startTime;
			timerText.text= (timeTaken).ToString("F2");
		}
	}
	public void LevelComplete(){


        LB.pushScore((long)(timeTaken * 100), CurrentLevel);

        int prevRetry = Tracker.GetLevelRetries(CurrentLevel);
        float prevTime = Tracker.GetLevelTime(CurrentLevel);
        Tracker.SetLevelRetries(RetryCount);
        Tracker.SetLevelTime(timeTaken);
        //		MM.DisplayMessage ("Level Complete");        
		//retryText.transform.localPosition = new Vector3 (0f, 150f, 0f);
		//retryText.fontSize = 30;

		//timerText.transform.localPosition = new Vector3 (0f, 0f, 0f);
		//timerText.fontSize = 30;
		StopAllCoroutines ();
		Car.SetActive (false);


       string prevRetryString = prevRetry.ToString();
       string prevTimeString = prevTime.ToString();

        if (prevRetry == 9999)
            prevRetryString = "-";
        if (prevTime == 9999)
            prevTimeString = "-";

        TM_retry.text = RetryCount + "";
        TM_prev_retry.text = prevRetryString + "";
        TM_time.text = timeTaken + "";
        TM_prev_time.text = prevTimeString + "";


        EndScreen.SetActive (true);
	}


	public void Retry(){
		for (int i = Levels [CurrentLevel].Checkpoints.Count - 1; i >= 0; i--) {
			if ((Levels [CurrentLevel].Checkpoints [i].hasBeenReached)|| (i==0)) {
                Car.GetComponent<Rigidbody>().isKinematic = true;
                Car.SetActive (false);
				Car.transform.position = Levels [CurrentLevel].Checkpoints [i].SpawnPoint.transform.position;
				Car.transform.rotation = Levels [CurrentLevel].Checkpoints [i].SpawnPoint.transform.rotation;
                Car.GetComponent<Rigidbody>().isKinematic = false;

                Car.SetActive (true);
				RetryCount++;
				if (i == 0) {
					RetryCount--;
					startTime = Time.time;
				}
				retryText.text = "Retry Count: " + RetryCount;
				return;
			} 

		}


	}
	
    public void LevelMessages()
    {
        switch (CurrentLevel)
        {
            case 0:
                MM.DisplayMessage("Tilt phone to steer");
                break;
            case 1:
                MM.DisplayMessage("Use boost. Please.");
                break;
            case 2:
                MM.DisplayMessage("Use boost to steer in air");
                break;
            case 3:
                MM.DisplayMessage("Handbrake for tight turns");
                break;
        }
    }
	public void GoToMenu(){
		SceneManager.LoadScene ("Menu");
	}

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
