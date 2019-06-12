using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float hitDmg;
	public Vector3 direction;
	public float bulletLife = 0.5f;
	public float bulletSpeed = 5;

	private void Start() {
		//Destroy(transform.gameObject, bulletLife);
	}

	void Update() {
		transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
		print(transform.position);
    }

	private void OnTriggerEnter(Collider other) {
		if(other.transform.root.tag == "Player") {
			other.GetComponent<Health>().GetDmg(hitDmg);
		}
	}
}
