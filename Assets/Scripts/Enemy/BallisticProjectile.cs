using UnityEngine;
using System.Collections;

// Attach this script to a cannon. Tag it, then use separate script to control.
public class BallisticProjectile : MonoBehaviour
{

	public Rigidbody projectilePrefab;
	public GameObject launchEffectPrefab;
	public GameObject fallPointIndicatorPrefab;


	public void Fire (Vector3 barrelEnd, Vector3 target, float time, float crosshairDelay = 0.5f)
	{
		GameObject projectile = Instantiate (projectilePrefab.gameObject, barrelEnd, Quaternion.identity) as GameObject;
		projectile.GetComponent<Rigidbody>().velocity = CalculateVelocity (barrelEnd, target, time);

		StartCoroutine (DisplayCrosshair (target, crosshairDelay));
	}


	protected virtual Vector3 CalculateVelocity (Vector3 initPos, Vector3 targetPos, float time)
	{
		Vector3 v_0 = (targetPos - initPos) / time  -  0.5f * Physics.gravity * time;
		return v_0;
	}


	protected virtual IEnumerator DisplayCrosshair (Vector3 position, float delay)
	{
		yield return new WaitForSeconds (delay);
		Instantiate (fallPointIndicatorPrefab, position, Quaternion.identity);
	}
}
