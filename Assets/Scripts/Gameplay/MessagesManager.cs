using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessagesManager : MonoBehaviour {
	public Text message;
	public Animator messageAnimator;
	private bool busy = false;
	private List<string> WaitingMessages;
	public static MessagesManager SharedInstance;
    //int count = 0;
    //public bool Test;
    void Awake(){
		SharedInstance = this;
        WaitingMessages = new List<string>();

    }
    void Start(){

	}
    //void Update()
    //{
    //    if (Test)
    //    {

    //        DisplayMessage("Test " + ++count);

    //        Test = false;
    //    }

    //}

    public void DisplayMessage(string received_message){

		if (busy) {
			WaitingMessages.Add (received_message);
			StartCoroutine (RetryMessages());
			return;
		}

		busy = true;
		message.text = received_message;
		messageAnimator.SetTrigger ("messageIn");
		StartCoroutine (RemoveMessage ());  

	}


	private IEnumerator RemoveMessage(){
		yield return new WaitForSeconds (2f);
		messageAnimator.SetTrigger ("messageOut");
		yield return new WaitForSeconds (1f);
		busy = false;

	}


	private IEnumerator RetryMessages(){
		while (WaitingMessages.Count > 0) {

			yield return new WaitForSeconds (1f);
			if (!busy) {
				string ReceivedMessage = WaitingMessages [0];
				WaitingMessages.RemoveAt (0);
				DisplayMessage (ReceivedMessage);
			}
		}

	}

}
