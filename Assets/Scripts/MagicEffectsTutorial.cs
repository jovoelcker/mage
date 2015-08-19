using UnityEngine;
using System.Collections;

public class MagicEffectsTutorial : MagicEffects {
	
	public GameObject vase;
	public GameObject vaseSpawnEffect;
	public AudioClip vaseCastSound;
	public GameObject vaseSpawnPoint;
	private Vector3 vaseSpawnPointPosition;
	private Quaternion vaseSpawnPointRotation;
	
	public override void Attack() {
		if (attackable) {
			attackable = false;

			vaseSpawnPointPosition = vaseSpawnPoint.transform.position;
			vaseSpawnPointRotation = vaseSpawnPoint.transform.rotation;
			Invoke("SpawnVase", 1);

			if (vaseCastSound)
				gameLogic.playSoundEffect(vaseCastSound, 0.4f);
			
			Instantiate(vaseSpawnEffect, vaseSpawnPointPosition, vaseSpawnPointRotation);
		}
	}

	private void SpawnVase() {
		Instantiate(vase, vaseSpawnPointPosition , vaseSpawnPointRotation);
		Invoke("ReleaseAttack", 3);
	}

	public AudioClip growFlowers;
	
	public override void Defend() {
		if (defendable) {
			defendable = false;
		}
	}

}
