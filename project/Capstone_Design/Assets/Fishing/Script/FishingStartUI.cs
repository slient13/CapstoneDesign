using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStartUI : MonoBehaviour
{
	public Vector3 uiPosAdjuster;
	public Vector3 scale;
	GameObject player;
	GameObject goTemp;
	GameObject goBillboard;
	public GameObject mainCamera;

	void Start()
	{
		uiPosAdjuster = new Vector3(0, 2, 0);
		player = GameObject.FindGameObjectWithTag("Player");
		goTemp = this.gameObject;
		goBillboard = transform.Find("Billboard").gameObject;

		if(mainCamera == null)
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		this.transform.localScale = scale;
	}

	void Update()
	{
		goTemp.transform.LookAt(mainCamera.transform.position);

		//UI위치 설정
		this.transform.position = player.transform.position + uiPosAdjuster;
	}

	public void GetUIScale(Vector3 scaleAdj)
    {
		scale = scaleAdj;
    }
}
