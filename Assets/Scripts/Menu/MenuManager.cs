using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class Stage
{
    public Sprite levelImg;
    public string title;
    public string lowestTimePref;
    public string lowestRetryPref;
    public GameObject LevelObj;
}


public class MenuManager : MonoBehaviour {

    [Header("Level Stuff")]
	public Button[] LevelButtons;
	public Animator camAnimator;
    public GameObject levelParent;

    [Header("References")]
    public GameObject Menu;
    public GameObject Title;
    public GameObject LevelsParent;
    public GameObject levelNames, levelrewards, levelretries, leveltimes;

    public List<Sprite> rankSprites;

    public Color LockedColor;
    private int unlockPosition=0;

    private void Awake()
    {
        //Tracker.LevelChosen = 0;
        //Tracker.SetLevelRetries(1);

        //Tracker.LevelChosen = 1;
        //Tracker.SetLevelRetries(6);
        //PlayerPrefs.Save();
        //PlayerPrefs.DeleteAll();
    }
    // Use this for initialization
    void Start () {

        // save playerprefs pls
        for (int i = 0; i < LevelButtons.Length; i++) {
			int k = i;
			LevelButtons [i].onClick.AddListener (() => LevelPressed (k));
		}

        InitializeLevels();
	}

	//void Update(){

	//	//if (Input.GetKey (KeyCode.Escape)) 
	//}

    void InitializeLevels()
    {
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            int retries = Tracker.GetLevelRetries(i);
            if (retries != 9999)
            {
                unlockPosition = i+1;
                levelretries.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "" + retries;

                float time = Tracker.GetLevelTime(i);
                leveltimes.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "" + time;

                if(retries > 5)
                {
                    SetRankSprite(i, 1);
                }
                else if (retries > 0)
                {
                    SetRankSprite(i, 2);

                }
                else
                {
                    SetRankSprite(i, 3);
                }
            }
        }
        for (int i = unlockPosition + 1; i < LevelButtons.Length; i++)
        {
            levelNames.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = LockedColor;
            SetRankSprite(i, 0);
        }

    }

	public void playPressed(){
		StartCoroutine (playPress ());
	}

	private IEnumerator playPress(){

        Menu.SetActive (false);
        Title.SetActive(false);
        camAnimator.SetTrigger("camup");
        
        yield return new WaitForSeconds(0.5f);

        LevelsParent.SetActive(true);
	}

	public void LevelPressed(int i){
        Debug.Log(unlockPosition + " up");
        if ( i <= unlockPosition)
        {
            Tracker.LevelChosen = i;

            SceneManager.LoadScene("MainGame");
        }


	}


    public void SetRankSprite(int i, int rank)
    {
        levelrewards.transform.GetChild(i).GetComponent<Image>().sprite = rankSprites[rank];
    }
}
