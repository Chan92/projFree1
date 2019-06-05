using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {
	public Transform holdingSpot;
	public KeyCode pickupKey;
	public KeyCode openDoorKey;

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

		//door interaction
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Door") {
			DoorInteraction();
		}
	}

	//checks if the door can be opened
	void DoorInteraction() {
		print("Open door");
	}
}
