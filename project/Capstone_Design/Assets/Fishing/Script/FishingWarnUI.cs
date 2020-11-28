using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingWarnUI : MonoBehaviour
{
	public Vector3 uiPosAdjuster;
	public float scaleAdj;
	public GameObject mainCamera;

	GameObject player;
	GameObject goTemp;
	GameObject goBillboard;
	Vector3 initPos;
	bool isEnabled;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		goTemp = this.gameObject;
		goBillboard = transform.Find("Billboard").gameObject;
		initPos = this.transform.position;

		if (mainCamera == null)
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		this.transform.localScale *= scaleAdj;
	}

	void Update()
	{
		goTemp.transform.LookAt(mainCamera.transform.position);

		//UI위치 설정
		if (isEnabled)
			this.transform.position = player.transform.position + uiPosAdjuster;
		else
			this.transform.position = initPos;
	}

	public void SetEnabled(bool bo)
    {
		isEnabled = bo;
    }

	public bool GetEnabled()
    {
		return isEnabled;
    }
}
