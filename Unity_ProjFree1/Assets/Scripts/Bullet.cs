using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float hitDmg;
	public float bulletLife = 0.5f;
	public float bulletSpeed = 5;
	
	private void Start() {
		Destroy(gameObject, bulletLife);
	}

	void Update() {
		transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
    }

	private void OnCollisionEnter(Collision other) {
		if(other.transform.root.tag == "Player") {
			Health hp = other.transform.root.GetComponent<Health>();

			if(hp.oneHitKill) {
				hp.Death();
			} else {
				hp.GetDmg(hitDmg);
			}
		}

		Destroy(gameObject);
	}
}
