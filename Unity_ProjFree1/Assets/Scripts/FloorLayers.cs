using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLayers : MonoBehaviour{
	public float offset = 2f;
	private Transform player;
	public bool setTransparent = true;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

	void Update() {
		FindChildren();
	}

	//makes the enviorment above the player invisible only when the player is below it
	void EnableSwap(Transform child) {
		if(player.localPosition.y < child.localPosition.y - offset && setTransparent) {
			child.GetComponent<MeshRenderer>().enabled = false;
		} else {
			child.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	//search for the parts to swap invisibility
	void FindChildren() {
		foreach(Transform child in transform) {
			if(child.GetComponent<MeshRenderer>() != null) {
				EnableSwap(child);
			} else if(child.childCount > 0 && child.GetChild(0).GetComponent<MeshRenderer>() != null) {
				EnableSwap(child.GetChild(0));
			}
		}
	}
}
