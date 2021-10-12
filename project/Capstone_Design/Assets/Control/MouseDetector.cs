using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 마우스 위치에 따른 충돌 감지등을 구현하기 위한 클래스
[System.Serializable]
public class MouseDetector {
    public Vector2 mousePos = new Vector2();    // 마우스의 위치
    public Transform targetTransform;           // 비교 대상의 Transform 정보.

    public MouseDetector() {
        this.mousePos.x = 0.0f;
        this.mousePos.y = 0.0f;
        this.targetTransform = null;
    }

    public MouseDetector(Transform targetTransform) {
        this.mousePos = this.GetMousePos();
        this.targetTransform = targetTransform;
    }

    public MouseDetector(Vector2 mousePos, Transform targetTransform) {
        this.mousePos = mousePos;
        this.targetTransform = targetTransform;
    }

    public void TargetChange(Transform newTarget) {
        this.targetTransform = newTarget;
    }

    public Vector2 GetMousePos() {
        Message msg = new Message("ControlManager/GetMousePos : ").FunctionCall();
        this.mousePos = (Vector2)msg.returnValue[0];
        return new Vector2(this.mousePos.x, this.mousePos.y);
    }

    public bool Trigger(int pinMode = 1) {
        // pinMode
        /*
            1 = 좌상단.
            5 = 중앙.            
        */

        Vector3 targetPos;                  // 타겟 오브젝트의 위치
        Vector2 targetSize = new Vector2(); // 타겟 오브젝트의 크기.
        Vector2 minPos = new Vector2();     // 트리거 인식 최소 위치
        Vector2 maxPos = new Vector2();     // 트리거 인식 최대 위치

        // 타겟이 지정되지 않은 상태로 트리거 시도시 false 반환 및 로그 출력
        if (targetTransform == null) {
            Debug.Log("MouseDetector/trigger.error : targetTransfrom is null");
            return false;
        }

        // 타겟의 현재 위치와 크기를 받아옴.
        targetPos = targetTransform.position;
        targetSize.x = targetTransform.GetComponent<RectTransform>().rect.width;
        targetSize.y = targetTransform.GetComponent<RectTransform>().rect.height;

        // 마우스 좌표를 최신화 함.
        GetMousePos();

        // 타겟의 위치와 크기를 통해 마우스가 감지되어야 할 범위를 체크함.
        minPos.x = targetPos.x;
        minPos.y = targetPos.y - targetSize.y;
        maxPos.x = targetPos.x + targetSize.x;
        maxPos.y = targetPos.y;

        // pinMode 에 따라 판정 범위를 조정함.
        if (pinMode == 5)
        {
            minPos.x -= targetSize.x / 2;
            maxPos.x -= targetSize.x / 2;
            minPos.y += targetSize.y / 2;
            maxPos.y += targetSize.y / 2;
        }

        // 디버그용 값 표시기
        // Debug.LogFormat("MouseDetector/trigger : targetPos = ({6}, {7}) / targetSize = ({8}, {9}) / " + 
        //     "minPos = ({0}, {1})/ maxPos = ({2}, {3}) / mousePos = ({4}, {5})", 
        //     minPos.x, minPos.y, 
        //     maxPos.x, maxPos.y, 
        //     mousePos.x, mousePos.y,
        //     targetPos.x, targetPos.y,
        //     targetSize.x, targetSize.y);

        // 실제 비교 및 논리값 반환.
        if (mousePos.x > minPos.x && 
            mousePos.x < maxPos.x &&
            mousePos.y > minPos.y && 
            mousePos.y < maxPos.y) return true;
        else return false; 
    }
}