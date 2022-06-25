using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
	 [SerializeField] private Animator playerAnimator;

	 [SerializeField] private Transform arrowSlot;

	 [SerializeField] private GameObject arrowPrefab;

	 [SerializeField] private float bulletSpeed=20;

	 [SerializeField] private LineRenderer bowLine;

	 [SerializeField] private GameObject bowLineCenter;

	 private GameObject currentSpawnedArrowInstance;

	 private Vector3 lineInitialPosition;

	 private bool isDrawn;


	 private void Start()
	 {
			lineInitialPosition = bowLine.GetPosition(1);
	 }

	 private void Update()
	 {
			if (Input.GetMouseButtonDown(0))
			{
				 playerAnimator.SetLayerWeight(1, 1);

				 playerAnimator.Play("Draw",1,0);
			}
			if (Input.GetMouseButtonUp(0))
			{
				 Invoke("ReleaseArrowSlot", 0.05f);

				 playerAnimator.Play("Release", 1, 0);
			}
	 }

	 public void SpawnArrow()
	 {
			isDrawn = true;

			currentSpawnedArrowInstance = Instantiate(arrowPrefab, arrowSlot);
	 }

	 public void ReleaseArrow()
	 {
			if (!isDrawn)
			{
				 ResetShootingLayer();
				 return;
			}

			currentSpawnedArrowInstance.transform.parent = null;

			currentSpawnedArrowInstance.GetComponent<Rigidbody>().isKinematic = false;


			currentSpawnedArrowInstance.GetComponent<Rigidbody>().
				 AddForce(-currentSpawnedArrowInstance.transform.forward*bulletSpeed, ForceMode.Impulse);

	 }

	 public void ResetShootingLayer()
	 {
			isDrawn = false;

			playerAnimator.SetLayerWeight(1, 0);

	 }

	 public void SetLinePointToArrowSlot()
	 {
			InvokeRepeating("FollowArrowSlot", 0, Time.deltaTime);
	 }


	 private void FollowArrowSlot()
	 {
			bowLineCenter.transform.position = arrowSlot.transform.position;

			bowLine.SetPosition(1, bowLineCenter.transform.localPosition);

			Debug.Log("arrow Slot position:"+ arrowSlot.position);

			Debug.Log("arrow slot local position:"+arrowSlot.localPosition);

			Debug.Log("line center position:" + bowLineCenter.transform.position);

			Debug.Log("line center local position:" + bowLineCenter.transform.localPosition);
	 }

	 private void ReleaseArrowSlot()
	 {
			CancelInvoke("FollowArrowSlot");

			bowLine.SetPosition(1, lineInitialPosition);
	 }
}
