using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLayers : MonoBehaviour{
	private Transform player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

	void Update() {
		FindChildren();
	}

	void EnableSwap(Transform child) {
		if(player.localPosition.y < child.localPosition.y) {
			child.GetComponent<MeshRenderer>().enabled = false;
		} else {
			child.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	void FindChildren() {
		foreach(Transform child in transform) {
			if(child.GetComponent<MeshRenderer>() != null) {
				EnableSwap(child);
			} else {
				EnableSwap(child.GetChild(0));
			}
		}
	}
}
