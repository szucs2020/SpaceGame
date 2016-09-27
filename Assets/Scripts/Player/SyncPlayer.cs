using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncPlayer : NetworkBehaviour {

    //flip syncing
	[SyncVar(hook="FlipHook")]
	private bool facingRight = false;

    ////jetpack syncing
    //[SyncVar(hook = "JetpackHook")]
    //private bool jetpackOn = false;
    //private SpriteRenderer jetpack;

    //void Awake() {
    //    jetpack = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    //}

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

    //[Command]
    //public void CmdSyncJetpack(bool jet) {
    //    jetpackOn = jet;
    //    jetpack.enabled = jetpackOn;
    //}

    //void JetpackHook(bool jet) {
    //    jetpackOn = jet;
    //    jetpack.enabled = jetpackOn;
    //}
}