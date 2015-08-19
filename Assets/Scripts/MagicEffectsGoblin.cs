using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MageAnimation))]
public class MagicEffectsGoblin : MagicEffects {

	public GameObject goblinUpScaleEffect;
	public GameObject goblinDownScaleEffect;
	public float goblinScaleResetTimer = 10.0f;
	public AudioClip goblinScaleSound;

	private MageAnimation mageAnimation;

	// Use this for initialization
	void Start () {
		mageAnimation = GetComponent<MageAnimation>();
	}

	public override void Attack() {
		if (attackable) {
			attackable = false;
			
			gameLogic.playSoundEffect(goblinScaleSound);

			if (!mageAnimation.Dead) {
				mageAnimation.ScaleUp();
				Instantiate(goblinUpScaleEffect, transform.position, transform.rotation);
				
				Invoke("ResetGoblinsScale", goblinScaleResetTimer);
			}
		}
	}
	
	private void ResetGoblinsScale()
	{
		if (!mageAnimation.Dead) {
			mageAnimation.ScaleDown();
			Instantiate(goblinDownScaleEffect, transform.position, transform.rotation);
		}

		Invoke("ReleaseAttack", 1);
	}
	
	public GameObject playerHealEffect;
	public AudioClip healSound;
	private GameObject mainCamera;
	
	public override void Defend() {
		if (defendable) {
			defendable = false;

			gameLogic.playSoundEffect(healSound);
			Instantiate(playerHealEffect, (mainCamera.transform.position + (mainCamera.transform.forward * 2.0f) - (mainCamera.transform.up * 2.0f)), mainCamera.transform.rotation);
			gameLogic.ApplyCamCommands(CamCtrl.Healing);

			Invoke("ReleaseDefend", 1);
		}
	}

}
