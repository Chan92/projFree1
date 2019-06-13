using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour{
	public bool showDebugging = false;
	public bool startOnStart = true;
	public Transform startPos;

	[Header("Basic")]
	public float moveSpeedSneak = 5f;
	public float moveSpeedWalk = 15f;
	public float moveSpeedRun = 25f;
	public float rotateSpeed = 0.2f;
	private Rigidbody rootRB;

	[Header("Effects")]
	public Animator animations;

	[Header("Raycast checks")]
	public Vector3 moveCheckOffset;
	public float moveCheckDis = 0.8f;
	private RaycastHit moveHit;
	private int wallLayer;
	private int fwallLayer;
	private int enemyLayer;

	private void Awake() {
		wallLayer = LayerMask.NameToLayer("Wall");
		fwallLayer = LayerMask.NameToLayer("Frontwall");
		enemyLayer = LayerMask.NameToLayer("Enemy");
		rootRB = transform.root.GetComponent<Rigidbody>();

		if (startOnStart) {
			PlayerToStart();
		}
	}
	
    void Update(){
		Walk();
	}

	public void PlayerToStart() {
		transform.root.localPosition = startPos.localPosition;
	}

	//moves the character and rotates it to the move direction, the camera moves with the player
	protected void Walk() {
		float hor = Input.GetAxisRaw("Horizontal");
		float ver = Input.GetAxisRaw("Vertical");

		Vector3 dirMove = new Vector3(hor, 0, ver);
		Vector3 dirRot = new Vector3(-ver, 0, hor);

		if(dirMove != Vector3.zero) {
			if(animations)
				animations.SetBool("Moving", true);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirRot.normalized), rotateSpeed);
			transform.root.Translate(ModifiedMoveDir(dirMove).normalized * MoveSpeed() * Time.deltaTime, Space.World);
		} else {
			if(animations)
				animations.SetBool("Moving", false);
		}
	}

	//toggles sneaking, walking and running move speed
	private float MoveSpeed() {
		if(Input.GetButton("Sneak")) {
			return moveSpeedSneak;
		} else if(Input.GetButton("Run")) {
			return moveSpeedRun;
		} else {
			return moveSpeedWalk;
		}
	}

	//checks if the character can move in the given direction
	//if there are obstacles in given direction, modify the direction
	private Vector3 ModifiedMoveDir(Vector3 dirMove) {
		if(wallLayer > -1) {
			int layerMask = (1 << wallLayer) | (1 << fwallLayer) | (1 << enemyLayer);
			Vector3 tempDir = dirMove;

			//check horizontal
			Physics.Raycast(transform.position - moveCheckOffset, new Vector3(dirMove.x, 0, 0), out moveHit, moveCheckDis, layerMask);
			if(showDebugging)
				Debug.DrawRay(transform.position - moveCheckOffset, new Vector3(dirMove.x, 0, 0) * moveCheckDis, Color.red);

			if(moveHit.transform) {
				tempDir.x = 0;
			}

			//check vertical
			Physics.Raycast(transform.position - moveCheckOffset, new Vector3(0, 0, dirMove.z), out moveHit, moveCheckDis, layerMask);
			if(showDebugging)
				Debug.DrawRay(transform.position - moveCheckOffset, new Vector3(0, 0, dirMove.z) * moveCheckDis, Color.red);

			if(moveHit.transform) {
				tempDir.z = 0;
			}

			return tempDir;
		} else {
			Debug.LogError("Missing Wall layer.");
			return Vector3.zero;
		}
	}
}
