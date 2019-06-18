using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float hitDmg;
	public float bulletLife = 0.5f;
	public float bulletSpeed = 5;

	public Transform player;
	private Vector3 direction;
	private void Start() {
		Destroy(gameObject, bulletLife);
		direction = Vector3.Normalize(player.position - transform.position);
	}

	void Update() {
		transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.Self);
	}

	private void OnTriggerEnter(Collider other) {
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
