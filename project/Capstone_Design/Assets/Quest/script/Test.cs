using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Test : MonoBehaviour

    {

        // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때

        private void OnCollisionEnter(Collision collision)

        {

            Debug.Log("충돌 시작!");

        }



        // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌중일 때

       /* private void OnCollisionStay(Collision collision)

        {

            Debug.Log("충돌 중!");

        }
        */


        // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌이 끝났을 때

        private void OnCollisionExit(Collision collision)

        {

            Debug.Log("충돌 끝!");

        }

    }





