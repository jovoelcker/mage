using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Renderer))]
public class MagicEffectsCube : MagicEffects {

	// The object changes his material, when effected by magic
	public Material standardMaterial;
	public Material attackMaterial;
	public Material defendMaterial;
	
	Renderer renderer;
	
	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
	}

	public override void Attack() {
		CancelInvoke("Restore");

		Debug.Log("Attack");
		renderer.material = attackMaterial;

		Invoke("Restore", 1);
	}

	public override void Defend() {
		CancelInvoke("Restore");

		Debug.Log("Defend");
		renderer.material = defendMaterial;

		Invoke("Restore", 1);
	}

	private void Restore() {
		renderer.material = standardMaterial;
	}

}
