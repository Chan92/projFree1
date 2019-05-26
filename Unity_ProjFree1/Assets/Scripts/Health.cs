using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour{
	[Header("Basic")]
	public float maxHealth = 100;
	private float curHealth;

	public float invulnerableTime = 1;
	private bool invulnerable;

	[Header("UI")]
	public Image hpbar;

	[Header("Effects")]
	public Animator anim;
	public AudioSource soundObj;
	public AudioClip dmgSound;
	public ParticleSystem dmgEffect;

    void Start(){
		Heal();
	}

	//gets the damage when getting hit
	public void GetDmg(float dmg) {
		//dont do any dmg if the player is invulnerable
		if (invulnerable) {
			return;
		}

		curHealth -= dmg;

		if (curHealth <= 0) {
			curHealth = 0;
			Death();
		}

		if (anim)
			anim.SetTrigger("GetDmg");
		if (soundObj && dmgSound)
			soundObj.PlayOneShot(dmgSound);
		if (dmgEffect)
			Instantiate(dmgEffect, transform.position, Quaternion.identity);

		UpdateUI();
		StartCoroutine(Invulnerable());
	}

	//unable to get dmg for a short time to avoid getting hit many times at once
	IEnumerator Invulnerable() {
		invulnerable = true;
		yield return new WaitForSeconds(invulnerableTime);
		invulnerable = false;
	}

	//called upon death
	void Death() {
		if (anim)
			anim.SetBool("Dead", true);
		print("ÿou are dead");
	}

	//gets the health info in the ui
	void UpdateUI() {
		if (hpbar)
			hpbar.fillAmount = curHealth / maxHealth;
	}

	//heals the player
	public void Heal(float hp = float.MaxValue) {
		curHealth += hp;

		if (curHealth >= maxHealth) {
			curHealth = maxHealth;
		}

		UpdateUI();
	}
}
