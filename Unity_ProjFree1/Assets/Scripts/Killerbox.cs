using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killerbox : MonoBehaviour{
	public float startDelay = 2;
	public float destroyDelay = 0.2f;
	private float startHeight;

	//get the start height after a short wait
	private IEnumerator Start() {
		yield return new WaitForSeconds(startDelay);
		startHeight = transform.position.y;
	}

	//destroy the box when it hit something after it fell
	private void OnCollisionEnter(Collision collision) {
		if (transform.position.y < startHeight) {
			Destroy(gameObject, destroyDelay);
		}
	}
}
