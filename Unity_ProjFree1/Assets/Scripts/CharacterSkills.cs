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
		if(Input.GetKeyDown(pickupKey) && other.tag == "Key") {
			doorlock1 = true;
			Destroy(other.transform.GetComponent<MeshRenderer>());
		}

		//press button to open exit door
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Button") {
			doorlock2 = true;
		}

		//door interaction 1 (need cardkey)
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Door") {
			DoorInteraction(doorlock1, other);
		}

		//door interaction 2 (need to enable door)
		if(Input.GetKeyDown(openDoorKey) && other.tag == "Door2") {
			DoorInteraction(doorlock2, other);
		}
	}

	//checks if the door can be opened
	void DoorInteraction(bool doorlock, Collider door) {
		if(doorlock) {
			door.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
		}
	}

	//closes the door 
	private void OnTriggerExit(Collider other) {
		if (other.tag == "Door" || other.tag == "Door2") {
			other.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
		}
	}
}
