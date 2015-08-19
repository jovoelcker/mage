using UnityEngine;
using System.Collections;

public class MagicEffectsBoss : MagicEffects {
	
	public GameObject vase;
	public GameObject vaseSpawnEffect;
	public AudioClip vaseCastSound;
	public GameObject vaseSpawnPoint;
	private Vector3 vaseSpawnPointPosition;
	private Quaternion vaseSpawnPointRotation;
	
	public override void Attack() {
		if (attackable) {
			attackable = false;
			
			vaseSpawnPointPosition = GoblinBoss.transform.position + new Vector3(0, 6, 0);
			vaseSpawnPointRotation = GoblinBoss.transform.rotation;
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
	
	public override void Defend() { }
}
