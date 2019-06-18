using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour{
	[Header("Movement")]
	public bool circularMovement;
    public Transform movePointList;
    public float moveSpeed = 5;
    private List<Transform> movePoints = new List<Transform>();
    private Transform curPoint;
    private int pid = 0;
	private bool idUp = true;
	private bool moveForwards = true;
	private float minDistanceToPoint = 1.2f;
	protected bool death = false;	
	
	[Header("PlayerDetection")]
    public float detectDistance = 5;
    public float detectAngle = 45;
	public float invisibleOffset = 3;
	public MeshRenderer cone;
	public Material coneNormal;
	public Material coneAlert;
	protected RaycastHit hit;
	protected Transform player;

	[Header("Offense")]
	public float hitRange;
	public float hitDamage = 10;
	public float hitDelay = 3;
	protected Coroutine offenseCoroutine;
	protected int attackLayer;

	[Header("Effects")]
	public Animator anim;
	public AudioSource soundObj;
	public AudioClip alertSound;
	public AudioClip attackSound;
	public AudioClip deathSound;


	void Awake(){
		attackLayer = (1 << LayerMask.NameToLayer("EnemyAttack"));
		attackLayer = ~attackLayer;
		MovePoints();
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

	//called when an enemy dies
	protected void Death() {
		death = true;
		transform.localEulerAngles = new Vector3 (180, transform.localEulerAngles.y, transform.localEulerAngles.z);
		if (soundObj && deathSound)
			soundObj.PlayOneShot(deathSound);
	}

    //moves the enemy forwards to given location point
    protected void Movement(){
		if(movePoints != null) {
			//if player is detected, go after the player else return to path
			if(!DetectionCheck()) {
				if(Vector3.Distance(transform.position, curPoint.position) <= minDistanceToPoint) {
					if (circularMovement) {
						PatrolDirection(PointIdCirular());
					} else {
						PatrolDirection(PointIdLinar());
					}
				}
			}

			if(moveForwards) {
				transform.position += transform.forward * (moveSpeed * Time.deltaTime);
			}
		}
    }
    
    //get all patrol points and put them in a list for easy access
    void MovePoints(){
        foreach (Transform point in movePointList){
            movePoints.Add(point);
        }
        PatrolDirection(0);
    }

    //get a direction point the enemy has to move to
    void PatrolDirection(int id){
        curPoint = movePoints[id];
		Vector3 lookdir = curPoint.position;
		lookdir.y = transform.position.y;
        transform.LookAt(lookdir);
    }

    //update the point id up and back to 0, for a cirular movement
    int PointIdCirular() {
        pid = (pid + 1) % movePoints.Count;
        return (pid);
    }

	//update the point id up and down, for a linar movement
	int PointIdLinar() {
		if (idUp) {
			pid++;

			if (pid >= movePoints.Count) {
				idUp = false;
				pid--;
			}

			return pid;
		} else {
			pid--;

			if (pid < 0) {
				idUp = true;
				pid++;
			}

			return pid;
		}
	}

	//detect if the player is sight
	protected bool DetectionCheck(){
		Vector3 rayDir = player.position - transform.position;

		Debug.DrawRay(transform.position, rayDir.normalized * detectDistance, Color.red);
		if(Physics.Raycast(transform.position, rayDir.normalized, out hit, detectDistance, attackLayer)) {
			if(hit.transform.tag == "Player") {
				float angle = Mathf.Abs(Vector3.Angle(transform.forward, player.position - transform.position));
				if(angle < detectAngle) {					
					Vector3 playerdir = player.position;
					playerdir.y = transform.position.y;
					transform.LookAt(playerdir);
					cone.material = coneAlert;
					return true;
				}
			}
		}

		if(offenseCoroutine != null) {
			StopCoroutine(offenseCoroutine);
			offenseCoroutine = null;
		}

		Vector3 lookdir = curPoint.position;
		lookdir.y = transform.position.y;
		transform.LookAt(lookdir);
		cone.material = coneNormal;
		return false;
	}

	//Basic attack
	protected virtual IEnumerator Attack() {
		yield return new WaitForSeconds(hitDelay);
		print(this.name + "_ attack");
		offenseCoroutine = null;
	}

	//attack the player when the player is within hit range
	protected void Offense() {
		if (DetectionCheck()) {
			//when the player is within range, stop the enemy from moving and start attacking
			if (Vector3.Distance(transform.position, player.position) <= hitRange) {
				moveForwards = false;

				if (offenseCoroutine == null) {
					offenseCoroutine = StartCoroutine(Attack());
				}
			} else {
				if (offenseCoroutine == null) {
					moveForwards = true;
					DetectionCheck();
				}
			}
		} else {
			if (offenseCoroutine == null) {
				moveForwards = true;
			}
		}
	}

	//disables the enemy when its aboven the player 
	protected bool Invisibility() {
		if (player.localPosition.y < transform.localPosition.y - invisibleOffset) {
			SetRender(false);
			return true;
		}

		SetRender(true);
		return false;
	}

	//sets the render of each object in the enemy
	protected void SetRender(bool enable) {
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).GetComponent<MeshRenderer>()) {
				transform.GetChild(i).GetComponent<MeshRenderer>().enabled = enable;
			}
		}
	}
}
