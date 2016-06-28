using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Gameplay;

namespace Assets.Scripts.OnTriggerScripts
{
    class MoveTrigger : MonoBehaviour
    {
        private PlayerCon2 playerCon;
        private GameObject moveableCar;
        private bool hasMoved;
        private float speed = 5;

        void OnTriggerEnter()
        {
            if (!hasMoved && transform.FindChild("FreeCar(Clone)") != null)
            {
                hasMoved = true;
                moveableCar = transform.FindChild("FreeCar(Clone)").gameObject;
                StartMove();
                
            }
            if (!hasMoved && transform.FindChild("jeep(Clone)") != null)
            {
                hasMoved = true;
                moveableCar = transform.FindChild("jeep(Clone)").gameObject;
                StartMove();
            }
        }
        void StartMove()
        {
            float xDelta = transform.position.x - moveableCar.transform.position.x;
            float zDelta = transform.position.z - moveableCar.transform.position.z;
            Vector3 distance = Math.Abs(zDelta) > Math.Abs(xDelta) ? new Vector3(0, 0, zDelta) : new Vector3(xDelta, 0, 0);
            iTween.MoveTo(moveableCar, iTween.Hash("position", moveableCar.transform.position + distance, "easetype", iTween.EaseType.linear, "time", 1.25f));
        }
    }
}
