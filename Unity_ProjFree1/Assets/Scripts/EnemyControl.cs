using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour{
	[Header("Movement")]
	public bool circularMovement;
    public Transform movePointList;
    public float moveSpeed = 10;
    private List<Transform> movePoints = new List<Transform>();
    private Transform curPoint;
    private int pid = 0;
	private bool idUp = true;
	private bool moveForwards = true;
	protected bool death = false;
	
	[Header("PlayerDetection")]
    public float detectDistance = 5;
    public float detectAngle = 45;
    protected RaycastHit hit;
	protected Transform player;

	[Header("Offense")]
	public float hitRange;
	public float hitDamage = 10;
	public float hitDelay = 3;
	protected Coroutine offenseCoroutine;

	[Header("Effects")]
	public Animator anim;
	public AudioSource soundObj;
	public AudioClip alertSound;
	public AudioClip attackSound;
	public AudioClip deathSound;

	void Awake(){
        MovePoints();
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

	//called when an enemy dies
	protected void Death() {
		print("Enemy died");
	}

    //moves the enemy forwards to given location point
    protected void Movement(){
		if(movePoints != null) {
			//if player is detected, go after the player else return to path
			if(DetectionCheck()) {

			} else {
				if(Vector3.Distance(transform.position, curPoint.position) <= 2) {
					if (circularMovement) {
						PatrolDirection(PointIdCirular());
					} else {
						PatrolDirection(PointIdLinar());
					}
				}
			}

			if (moveForwards) {
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
        pid++;
        
        if (pid >= movePoints.Count){
            pid = 0;
        }
        
        return pid;
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
		if(Physics.Raycast(transform.position, rayDir.normalized, out hit, detectDistance)) {
			if(hit.transform.tag == "Player") {
				float angle = Mathf.Abs(Vector3.Angle(transform.forward, player.position - transform.position));
				if(angle < detectAngle) {					
					Vector3 playerdir = player.position;
					playerdir.y = transform.position.y;
					transform.LookAt(playerdir);
					return true;
				}
			}
		}

		Vector3 lookdir = curPoint.position;
		lookdir.y = transform.position.y;
		transform.LookAt(lookdir);
		return false;
	}

	//Basic attack
	protected virtual IEnumerator Attack() {
		yield return new WaitForSeconds(0);
		print(this.name + "_ attack");
		yield return new WaitForSeconds(hitDelay);
		print(this.name + "_ wait finish");
		offenseCoroutine = null;
	}

	//attack the player when the player is within hit range
	protected void Offense() {
		if (DetectionCheck()) {
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
}
