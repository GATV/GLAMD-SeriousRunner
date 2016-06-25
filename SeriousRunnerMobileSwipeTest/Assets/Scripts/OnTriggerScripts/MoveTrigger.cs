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
        private GameObject Moveable;
        private bool startmoving = false;
        private float speed = 5;

        void Start()
        {

        }

        void Update()
        {
            if (startmoving)
            {
                Moveable = GameObject.Find(gameObject.name + "/FreeCar(Clone)") as GameObject;
                Moveable.transform.position = Vector3.MoveTowards(Moveable.transform.position, gameObject.transform.position, 5);
            }
        }
        void OnTriggerEnter(Collision colider)
        {
            if (GameObject.Find(transform.name + "/FreeCar(Clone)") != null)
            {
                startmoving = true;
                Debug.Log("Gevonden!" + gameObject.name.ToString());
            }
        }
    }
}
