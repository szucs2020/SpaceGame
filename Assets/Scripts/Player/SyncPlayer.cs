using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncPlayer : NetworkBehaviour {

    //flip syncing
	[SyncVar(hook="FlipHook")]
	private bool facingRight = false;

    //jetpack syncing
    [SyncVar(hook = "JetpackHook")]
    private bool jetpackOn = false;
    private SpriteRenderer jetpack;

    //position syncing
    [SyncVar]
    private Vector3 syncPos;
    [SerializeField] Transform player;
    [SerializeField] float smoothRate;

    private Vector3 lastPosition;
    public float movementThreshold;

    void Awake() {
        jetpack = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        UpdatePosition();
        if (!isLocalPlayer) {
            player.position = Vector3.Lerp(player.position, syncPos, Time.deltaTime * smoothRate);
            //player.position = syncPos;
        }
    }

    [ClientCallback]
    void UpdatePosition() {
        if (isLocalPlayer && Vector3.Distance(player.position, lastPosition) > movementThreshold) {
            CmdUpdatePosOnServer(player.position);
            lastPosition = player.position;
        }
    }

    [Command]
    void CmdUpdatePosOnServer(Vector3 position) {
        syncPos = position;
    }

    [Command]
	public void CmdSyncFlip(bool direction){

		facingRight = direction;
		Vector3 temp = transform.localScale;
		if (facingRight){
			temp.x = -1;
		} else{
			temp.x = 1;
		}
		transform.localScale = temp;
	}

	void FlipHook(bool direction){
		
		facingRight = direction;
		Vector3 temp = transform.localScale;
		if (facingRight){
			temp.x = -1;
		} else {
			temp.x = 1;
		}
		transform.localScale = temp;
	}

    [Command]
    public void CmdSyncJetpack(bool jet) {
        jetpackOn = jet;
        jetpack.enabled = jetpackOn;
    }

    void JetpackHook(bool jet) {
        jetpackOn = jet;
        jetpack.enabled = jetpackOn;
    }
}