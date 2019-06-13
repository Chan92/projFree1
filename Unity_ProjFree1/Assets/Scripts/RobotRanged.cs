using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRanged : EnemyControl{
	[Header("Attacks")]
	public GameObject bullet;
	public Transform bulletSpawn;

    void Update(){
		if(!death && !Invisibility()) {
			Movement();
			Offense();
		}
	}

	//attacks the player
	protected override IEnumerator Attack() {
		yield return new WaitForSeconds(hitDelay);

		Transform newBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity).transform;
		newBullet.GetComponent<Bullet>().hitDmg = hitDamage;

		if (soundObj && attackSound)
			soundObj.PlayOneShot(attackSound);
		if (anim)
			anim.SetTrigger("Attack");

		offenseCoroutine = null;
	}

	//check if a box is thrown on the enemy
	private void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Box" && !death) {
			Death();
		}
	}
}
