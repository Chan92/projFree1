using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkills : MonoBehaviour {
	public bool autoPickup = true;
	public bool autoDoor = true;
	public bool showHint = true;
	public Transform holdingSpot;
	public KeyCode pickupKey;
	public KeyCode openDoorKey;
	public GameObject plankHint;
	public GameObject plankPlaceHint;
	public GameObject pressbuttonHint;
	public GameObject doorHint;
	public GameObject doorpower;
	public GameObject keyCard;
	private bool doorlock1 = false;
	private bool doorlock2 = false;

	private void Start() {
		keyCard.SetActive(false);
		plankHint.SetActive(false);
		plankPlaceHint.SetActive(false);
		pressbuttonHint.SetActive(false);
		doorHint.SetActive(false);
		doorpower.SetActive(false);
	}

	private void OnTriggerStay(Collider other) {
		if(other.tag == "Plank") {
			//show hint how to pickup the plank
			plankHint.SetActive(showHint);
			if(plankHint)
				plankHint.GetComponentInChildren<Text>().text = "Press '" + pickupKey + "' to pickup the plank.";

			//picksup the plank
			if(Input.GetKeyDown(pickupKey) && holdingSpot.childCount <= 0) {
				plankHint.SetActive(false);
				other.transform.parent = holdingSpot;
				other.transform.localPosition = Vector3.zero;
				other.transform.GetComponent<SphereCollider>().enabled = false;
			}
		}

		if(other.tag == "PlankSpot") {
			//shows hint how to place the plank
			plankPlaceHint.SetActive(showHint);
			if(plankPlaceHint)
				plankPlaceHint.GetComponentInChildren<Text>().text = "Press '" + pickupKey + "' to place the plank.";

			//places the plank
			if(Input.GetKeyDown(pickupKey)) {
				if(holdingSpot.childCount > 0) {
					plankPlaceHint.SetActive(false);
					Transform plank = holdingSpot.GetChild(0);
					plank.parent = other.transform;
					plank.localPosition = Vector3.zero;
					plank.localEulerAngles = new Vector3(0, 90, 0);
					other.transform.GetComponent<BoxCollider>().enabled = false;
				}
			}
		}

		//pickup cardkey //autoPickup
		if(other.tag == "Key") {
			if(autoPickup || (!autoPickup && Input.GetKeyDown(pickupKey))) {
				keyCard.SetActive(true);
				doorlock1 = true;
				Destroy(other.gameObject);
                PlaySound("GotKey");
			}
		}

		//press button to open exit door
		if(other.tag == "Button") {
			if(!doorlock2) {
				pressbuttonHint.SetActive(true);
				pressbuttonHint.GetComponentInChildren<Text>().text = "Press '" + openDoorKey + "' to switch the power on.";
			}

			if(Input.GetKeyDown(openDoorKey)) {	
				doorlock2 = true;
				doorpower.SetActive(true);
			}
		}

		//door interaction 1 (need cardkey)
		if(other.tag == "Door") {
			if(!doorlock1) {
				doorHint.SetActive(true);
			}

			if(autoDoor || (!autoDoor && Input.GetKeyDown(openDoorKey))) {
				DoorInteraction(doorlock1, other);
			}
		}

		//door interaction 2 (need to enable door)
		if(other.tag == "Door2") {
			if(!doorlock2) {
				doorHint.SetActive(true);
			}

			if(autoDoor || (!autoDoor && Input.GetKeyDown(openDoorKey))) {
				//DoorInteraction(doorlock2, other);
				if(doorlock2) {
					GameObject.FindObjectOfType<MenuManager>().EndGame();
				}
			}
		}
	}

	//checks if the door can be opened
	void DoorInteraction(bool doorlock, Collider door) {
		if(doorlock) {
			door.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
		}
	}

    void PlaySound(string sound)
    {
        GetComponent<AudioManager>().playSound(sound);
    }

	//closes the door 
	private void OnTriggerExit(Collider other) {
		if(other.tag == "Button") {
			pressbuttonHint.SetActive(false);
			doorpower.SetActive(false);
		}

		if(other.tag == "Plank") {
			plankHint.SetActive(false);
		}

		if(other.tag == "PlankSpot") {
			plankPlaceHint.SetActive(false);
		}

		if(other.tag == "Door" || other.tag == "Door2") {
			doorHint.SetActive(false);
		}

		if (other.tag == "Door" || other.tag == "Door2") {
			if(other.transform.childCount >= 1 && other.transform.GetChild(0).GetComponent<BoxCollider>()) {
				other.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
			}
		}
	}
}
