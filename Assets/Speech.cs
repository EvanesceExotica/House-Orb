using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Speech", menuName = "DialogeOptions/Speech", order = 1)]
public class Speech : ScriptableObject {

	public float textSpeed;

	public Color bubbleColor;
	public Color playerColor;
	public Color orbColor;
	public Color friendColor;
	public enum OurSpeaker{
		player,
		orb,
		friend
	}
	public OurSpeaker speaker;

	public List<string> textChoices;
	public string text;
	void Start(){
	}
	 
	void Awake(){
		playerColor = new Color32(182, 102, 210, 255);
		orbColor = new Color32(64, 224, 208, 255);
		friendColor = new Color32(240,230,140, 255);
		SetTextColor();

	}
	public void SetTextColor(){
		if(speaker == OurSpeaker.player){
			bubbleColor = playerColor;
		}
		else if(speaker == OurSpeaker.orb){
			bubbleColor = orbColor;
		}
		else if(speaker == OurSpeaker.friend){
			bubbleColor = friendColor;
		}
	}

	public string GrabRandomTextChoice(){
		string ourTextChoice = null;
		int randomIndex = UnityEngine.Random.Range(0, textChoices.Count);
		ourTextChoice = textChoices[randomIndex];
		return ourTextChoice;
	}
	
}
