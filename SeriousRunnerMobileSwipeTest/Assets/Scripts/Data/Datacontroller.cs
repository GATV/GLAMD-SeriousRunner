using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

namespace Assets.Scripts
{
    public sealed class Datacontroller : MonoBehaviour
    {
        string server = "tcp:seriousrunner.database.windows.net,1433; Database = Serious Runner; User ID = glamd@seriousrunner; Password = G2theArdus; Trusted_Connection = True; Encrypt = True; Connection Timeout = 30";
        private static Datacontroller instance = null;
        public static Datacontroller SingleInstance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return (instance = new Datacontroller());
                }
            }
        }

        public Datacontroller()
        {

        }
        public void GetCoins()
        {
            WWW get_coins = new WWW(server);

        }
    }
}
