using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRotationController : MonoBehaviour
{

	private RaycastHit hit;
	private CinemachineVirtualCamera virtualCamera;

	private void Start()
	{
		virtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{

		Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
		int layerMask = 0;
		layerMask = 1 << 12;

		if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || (Input.GetAxis("Horizontal") != 0)) &&
		    Physics.Raycast(ray, out hit, 1000, layerMask))
		{

			float angle = Input.touchCount > 0 ? -0.15f * Input.GetTouch(0).deltaPosition.x : 0;
			
			#if UNITY_EDITOR
				angle = Input.GetAxis("Horizontal");
			#endif
			
			
			
			gameObject.transform.RotateAround(virtualCamera.Follow.position, Vector3.up, angle);
		}
	}
}
