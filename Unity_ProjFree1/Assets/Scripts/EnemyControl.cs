using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour{
    [Header("Movement")]
    public Transform movePointList;
    public float moveSpeed = 10;
    private List<Transform> movePoints = new List<Transform>();
    private Transform curPoint;
    private int pid = 0;

    [Header("PlayerDetection")]
    public float detectDistance = 5;
    public float detectAngle = 45;
    protected RaycastHit hit;
	protected Transform player;

    void Awake(){
        MovePoints();
		player = GameObject.FindGameObjectWithTag("Player").transform.root;
	}

    //moves the enemy forwards to given location point
    protected void Movement(){
		if(movePoints != null) {
			//if player is detected, go after the player else return to path
			if(DetectionCheck()) {

			} else {
				if(Vector3.Distance(transform.position, curPoint.position) <= 2) {
					PatrolDirection(PointId());
				}
			}

			transform.position += transform.forward * (moveSpeed * Time.deltaTime);
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
        transform.LookAt(curPoint);
    }

    //update the point id
    int PointId() {
        pid++;
        
        if (pid >= movePoints.Count){
            pid = 0;
        }
        
        return pid;
    }

	//detect if the player is sight
	protected bool DetectionCheck(){
		Vector3 rayDir = player.position - transform.position;

		Debug.DrawRay(transform.position, rayDir.normalized * detectDistance, Color.red);
		if(Physics.Raycast(transform.position, rayDir.normalized, out hit, detectDistance)) {
			if(hit.transform.tag == "Player") {
				float angle = Mathf.Abs(Vector3.Angle(transform.forward, player.position - transform.position));
				if(angle < detectAngle) {
					return true;
				}
			}
		}

		return false;
	}

	protected virtual void Attack() {
		if(!DetectionCheck()) {
			print(this.name + ": No player detected");
			transform.LookAt(curPoint);
			Time.timeScale = 1f;
			return;
		}
		Time.timeScale = 0.3f;
		transform.LookAt(player);
		print(this.name + ": Player found");
	}
}
