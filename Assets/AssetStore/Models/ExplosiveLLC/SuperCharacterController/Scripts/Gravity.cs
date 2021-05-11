using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates a this transform to align it towards the target transform's position
/// </summary>
#pragma warning disable 0649
public class Gravity : MonoBehaviour {
    
    [SerializeField]
    Transform planet;

	void Update () {
        Vector3 dir = (transform.position - planet.position).normalized;

        GetComponent<PlayerMachine>().RotateGravity(dir);

        transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
	}
}
#pragma warning restore 0649
