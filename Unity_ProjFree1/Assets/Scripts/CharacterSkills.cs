using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {
	public Transform holdingSpot;
	public KeyCode pickupKey;
	public KeyCode openDoorKey;
	private bool doorlock1 = false;
	private bool doorlock2 = false;

	private void OnTriggerStay(Collider other) {
		//picksup the plank
		if (Input.GetKeyDown(pickupKey) && other.tag == "Plank") {
			other.transform.parent = holdingSpot;
			other.transform.localPosition = Vector3.zero;
			other.transform.GetComponent<SphereCollider>().enabled = false;
		}

		//places the plank
		if(Input.GetKeyDown(pickupKey) && other.tag == "PlankSpot") {
			if(holdingSpot.childCount > 0) {
				Transform plank = holdingSpot.GetChild(0);
				plank.parent = other.transform;
				plank.localPosition = Vector3.zero;
				plank.localEulerAngles = new Vector3(0, 90, 0);
			}
		}

		//pickup cardkey
		if(Input.GetKeyDown(pickupKey) && other.tag == "Cardkey") {
			doorlock1 = true;
		}

		//press button to open exit door
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Doorbutton") {
			doorlock2 = true;
		}

		//door interaction 1
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Door1") {
			DoorInteraction(doorlock1);
		}

		//door interaction 2
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Door2") {
			DoorInteraction(doorlock2);
		}
	}

	//checks if the door can be opened
	void DoorInteraction(bool doorlock) {
		if(doorlock) {
			print("Open door");
		}
	}
}
