using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameUp;
using Facebook.Unity;
using System.Linq;
using System.Text;
using Assets.Scripts.Gameplay.Models;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Gameplay
{
    public class Generate : MonoBehaviour
    {
        // Chance is goed
        public Route R = new Route();

        // SpawnPositions
        public SpawnObject[] SpawnObstacles = new SpawnObject[6]; // elke lijst heeft verschillende hoogtes en locaties van spawnen, hebben individueel effect op elkaar in de return
        public SpawnObject[] GroundSpawns = new SpawnObject[30];  // objecten beneden en worden aangemaakt vanaf spawnobject, dit bestaat uit een object locatie die ik vul uit Fill()
        public SpawnObject[] AirSpawnObstacles = new SpawnObject[30]; // en worden later onderin gesspant met een foreach (als ze gebruikt worden) in Spawn(), het gaat dus vooral om
        public SpawnObject[] BoxCoins = new SpawnObject[21]; // deze 2 methodes met het spawnen, wat er tussendoor gebeurt is allerlei formules, switches en dependecies definen.
        public SpawnObject[] AirSpawns = new SpawnObject[9];

        // Obstacles
        public GameObject CartoonCar;
        public GameObject Jeep;
        public GameObject Box;
        public GameObject Scooter;
        public GameObject Hotdogtruck;
        public GameObject Pizzatruck;

        // Powerups
        public GameObject Speed;
        public GameObject Diamond;
        public GameObject DoubleCoins;
        public GameObject Shield;

        // Coins
        public GameObject Coin;

        // 1 = shortpath, 2 = MediumPath, 3 = Long Path
        public int path;

        // coin bows over enkele obstakels
        HashSet<int> Bow1 = new HashSet<int>() { 0, 3, 6, 9, 12 };
        HashSet<int> Bow2 = new HashSet<int>() { 1, 4, 7, 10, 13 };
        HashSet<int> Bow3 = new HashSet<int>() { 2, 5, 8, 11, 14 };
        HashSet<int> Bow4 = new HashSet<int>() { 15, 18, 21, 24, 27 };
        HashSet<int> Bow5 = new HashSet<int>() { 16, 19, 22, 25, 28 };
        HashSet<int> Bow6 = new HashSet<int>() { 17, 20, 23, 26, 29 };

        // rows met autowegen voor groundobstacles
        HashSet<int> Row1 = new HashSet<int>() { 0, 3, 6 };
        HashSet<int> Row2 = new HashSet<int>() { 1, 4, 7 };
        HashSet<int> Row3 = new HashSet<int>() { 2, 5, 8 };

        // Dependencies en chancevalues voor het bijhouden van variaties aan gameplay en wiskunde (uitschakelen en kansberekenen van objecten en gameplay)
        private bool bigobstacle = false;
        private int powerupcount = 0;
        private int coinbowcount = 0;
        private int truckcount = 0;
        private int bigtype;
        private string routeswitch, type;
        private int spawncounter = 0;
        private int listcounter = 0;
        private int o, p, c, n;
        private object[] obstacletype = new object[5];

        // moving car parameters
        private PlayerCon2 playerCon;
        void Start()
        {
            playerCon = GameObject.Find("SeriousRunnerGirl").GetComponent<PlayerCon2>();
        }

        void OnEnable()
        {
            Fill();
            Spawn();
            foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name == "New Game Object")
                {
                    Destroy(go);
                }
            }
        }

        void Fill()
        {
            // assign each spawnobject to corresponding transform
            for (int i = 0; i <= 5; i++)
            {
                SpawnObstacles[i] = new SpawnObject();
                if (i <= 2)
                {
                    SpawnObstacles[i].loc = transform.Find("Spawner/Obstacles/Line1/SpawnObstacle" + (i + 1).ToString());
                }
                else
                {
                    SpawnObstacles[i].loc = transform.Find("Spawner/Obstacles/Line2/SpawnObstacle" + (i + 1).ToString());
                }
            }
            for (int i = 0; i <= 29; i++)
            {
                GroundSpawns[i] = new SpawnObject();
                GroundSpawns[i].loc = transform.Find("Spawner/GroundSpawns/GroundSpawn" + (i + 1).ToString());
            }
            for (int i = 0; i <= 29; i++)
            {
                AirSpawnObstacles[i] = new SpawnObject();
                if (i <= 14)
                {
                    AirSpawnObstacles[i].loc = transform.Find("Spawner/AirSpawnObstacles/Line1/AirObstacleSpawn" + (i + 1).ToString());
                }
                else
                {
                    AirSpawnObstacles[i].loc = transform.Find("Spawner/AirSpawnObstacles/Line2/AirObstacleSpawn" + (i + 1).ToString());
                }
            }
            for (int i = 0; i <= 20; i++)
            {
                BoxCoins[i] = new SpawnObject();
                if (i <= 6)
                {
                    BoxCoins[i].loc = transform.Find("Spawner/BoxCoins/Lane1/BoxCoin" + (i + 1).ToString());
                }
                else if (i > 6 && i <= 13)
                {
                    BoxCoins[i].loc = transform.Find("Spawner/BoxCoins/Lane2/BoxCoin" + (i + 1).ToString());
                }
                else
                {
                    BoxCoins[i].loc = transform.Find("Spawner/BoxCoins/Lane3/BoxCoin" + (i + 1).ToString());
                }
            }
            for (int i = 0; i <= 8; i++)
            {
                AirSpawns[i] = new SpawnObject();
                if (i <= 2)
                {
                    AirSpawns[i].loc = transform.Find("Spawner/Airspawns/Line1/AirSpawn" + (i + 1).ToString());
                }
                else if (i > 2 && i <= 5)
                {
                    AirSpawns[i].loc = transform.Find("Spawner/Airspawns/Line2/AirSpawn" + (i + 1).ToString());
                }
                else
                {
                    AirSpawns[i].loc = transform.Find("Spawner/Airspawns/Line3/AirSpawn" + (i + 1).ToString());
                }
            }

            switch (path) // hier kunnen zoveel routes worden toegevoegd als nodig, elke kan een individuele balancing hebben.
            {
                case 1: // Score = 500 - seconden + muntjesx2 + obstakelsx2
                    {
                        R.Fill(20, 0, 5, 75); // Intro start ----> 1e splitsing
                        break;
                    }
                case 2:
                    {
                        R.Fill(40, 5, 5, 50); // Transition 1e splitsing ----> 2e splitsing
                        break;
                    }
                case 3:
                    {
                        R.Fill(35, 15, 15, 25); // Transition splitsing rechts van het spel waar middel en lang meeten, dit is het gemiddelde van middel en lang.
                        break;
                    }
                case 4:
                    {
                        R.Fill(90, 0, 10, 0); // Short = super klein, geen powerups, ontzettend veel obstakels en een paar muntjes
                        break;
                    }
                case 5:
                    {
                        R.Fill(60, 10, 10, 20); // Middle = middel lang, redelijke kans powerups, weinig kans muntjes, aardig wat obstakels
                        break;
                    }
                case 6:
                    {
                        R.Fill(30, 5, 20, 45); // Long = Lange route, bijna geen obstakels, erg veel powerups, iets meer muntjes
                        break;
                    }
            }
            for (int i = 0; i < 5; i++)
            {
                switch (listcounter)
                {
                    case 0:
                        {
                            bigtype = GlobalRandom.Next(1, 3); //word het een auto of een rij dozen?
                            foreach (SpawnObject S in SpawnObstacles)
                            {
                                GenerateChance(S);
                                if (S.obj == CartoonCar)
                                {
                                    S.move = true;
                                }
                                spawncounter++;
                            }
                            spawncounter = 0;
                            listcounter++;
                            break;
                        }
                    case 1:
                        {
                            foreach (SpawnObject S in GroundSpawns)
                            {
                                GenerateChance(S);
                                spawncounter++;
                            }
                            spawncounter = 0;
                            listcounter++;
                            break;
                        }
                    case 2:
                        {
                            foreach (SpawnObject S in AirSpawnObstacles)
                            {
                                GenerateChance(S);
                                spawncounter++;
                            }
                            spawncounter = 0;
                            listcounter++;
                            break;
                        }
                    case 3:
                        {
                            foreach (SpawnObject S in BoxCoins)
                            {
                                GenerateChance(S);
                                spawncounter++;
                            }
                            spawncounter = 0;
                            listcounter++;
                            break;
                        }
                    case 4:
                        {
                            foreach (SpawnObject S in AirSpawns)
                            {
                                GenerateChance(S);
                                spawncounter++;
                            }
                            spawncounter = 0;
                            listcounter++;
                            break;
                        }
                }
            }
            listcounter = 0;
        }

        void GenerateChance(SpawnObject Spawn)
        {
            switch (listcounter)
            {
                case 0:
                    {
                        o = R.gen.SO.O;
                        p = R.gen.SO.P;
                        c = R.gen.SO.C;
                        n = R.gen.SO.N;
                        break;
                    }
                case 1:
                    {
                        p = R.gen.GS.P;
                        c = R.gen.GS.C;
                        n = R.gen.GS.N;
                        break;
                    }
                case 2:
                    {
                        c = R.gen.ASO.C;
                        n = R.gen.ASO.N;
                        break;
                    }
                case 3:
                    {
                        p = R.gen.BC.P;
                        c = R.gen.BC.C;
                        n = R.gen.BC.N;
                        break;
                    }
                case 4:
                    {
                        p = R.gen.AS.P;
                        c = R.gen.AS.C;
                        n = R.gen.AS.N;
                        break;
                    }
            }

            int chance = GlobalRandom.Next(1, 101);
            if (Spawn.obj == null && Spawn.alive)
            {
                switch (listcounter)
                {
                    case 0:
                        {
                            if (chance <= o)
                            {
                                Spawn.obj = Obstakel(bigtype);
                            }
                            else if (chance <= (o + p))
                            {
                                Spawn.obj = PowerUp(Spawn);
                            }
                            else if (chance <= (o + p + c))
                            {
                                Spawn.obj = Coins(Spawn);
                            }
                            else if (chance <= (o + p + c + n))
                            {
                                Spawn.alive = false;
                            }
                            break;
                        }
                    case 1:
                    case 4:
                        {
                            if (chance <= (p))
                            {
                                Spawn.obj = PowerUp(Spawn);
                            }
                            else if (chance <= (p + c))
                            {
                                Spawn.obj = Coins(Spawn);
                            }
                            else if (chance <= (p + c + n))
                            {
                                Spawn.alive = false;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (chance <= (c))
                            {
                                Spawn.obj = Coins(Spawn);
                            }
                            else if (chance <= (c + n))
                            {
                                Spawn.alive = false;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (chance <= (c))
                            {
                                Spawn.obj = Coins(Spawn);
                            }
                            else if (chance <= (c + p))
                            {
                                Spawn.obj = PowerUp(Spawn);
                            }
                            else if (chance <= (c + p + n))
                            {
                                Spawn.alive = false;
                            }
                            break;
                        }
                }
            }
            o = p = c = n = 0;
        }

        GameObject PowerUp(SpawnObject spawn)
        {
            GameObject powerup = new GameObject();
            if (powerupcount < 1)// hoeveelheid powerups per straight tile
            {
                switch (listcounter)
                {
                    case 0:
                    case 1:
                        {
                            powerup = PowerupChance();
                            powerupcount++;
                            break;
                        }
                    case 3:
                        {
                            if (GroundSpawns[6].obj == Box && spawncounter > 0 && spawncounter < 6) // kijken of er een box rij is
                            {

                                AirSpawns[0].alive = false;
                                if (SpawnObstacles[3].obj == Box)
                                {
                                    AirSpawns[6].alive = false;
                                }
                                powerup = PowerupChance();
                                powerupcount++;

                            }
                            else if (GroundSpawns[7].obj == Box && spawncounter > 7 && spawncounter < 13) // kijken of er een box rij is
                            {
                                AirSpawns[1].alive = false;
                                if (SpawnObstacles[3].obj == Box)
                                {
                                    AirSpawns[6].alive = false;
                                }
                                powerup = PowerupChance();
                                powerupcount++;

                            }
                            else if (GroundSpawns[8].obj == Box && spawncounter > 14 && spawncounter < 20) // kijken of er een box rij is
                            {
                                AirSpawns[2].alive = false;
                                if (SpawnObstacles[3].obj == Box)
                                {
                                    AirSpawns[6].alive = false;
                                }
                                powerup = PowerupChance();
                                powerupcount++;

                            }
                            else
                            {
                                spawn.alive = false;
                            }
                            break;
                        }
                    case 4:
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if (spawncounter <= 2 && GroundSpawns[i + 6].obj != Box) // kijken of voor de airspawn een groundspawn een box heeft of niet ivm boxrijen
                                {
                                    powerup = PowerupChance();
                                }
                                else if (spawncounter >= 3 && spawncounter <= 5 && GroundSpawns[i + 15].obj != Box)
                                {
                                    powerup = PowerupChance();
                                }
                                else if (spawncounter >= 6 && spawncounter <= 8 && GroundSpawns[i + 21].obj != Box)
                                {
                                    powerup = PowerupChance();
                                }
                            }
                            powerupcount++;
                            break;
                        }
                }
            }
            else
            {
                spawn.alive = false;
            }
            return powerup;
        }

        GameObject Coins(SpawnObject spawn)
        {
            GameObject coin = new GameObject();
            switch (listcounter)
            {
                case 0:
                case 1:
                    {
                        if (spawn.obj == null && spawn.alive)
                        {
                            coin = CoinChance();
                        }
                        break;
                    }
                case 2:
                    {

                        if (coinbowcount <= 10) // een boog met coins is 5 muntjes, de max aantal bogen staat nu dus op 2
                        {
                            if (Bow1.Contains(spawncounter) && GroundSpawns[6].obj != Box && SpawnObstacles[0].obj != null && SpawnObstacles[0].obj != coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                            else if (Bow2.Contains(spawncounter) && GroundSpawns[7].obj != Box && SpawnObstacles[1].obj != null && SpawnObstacles[1].obj != Coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                            else if (Bow3.Contains(spawncounter) && GroundSpawns[8].obj != Box && SpawnObstacles[2].obj != null && SpawnObstacles[2].obj != Coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                            else if (Bow4.Contains(spawncounter) && GroundSpawns[18].obj != Box && SpawnObstacles[3].obj != CartoonCar && SpawnObstacles[3].obj != null && SpawnObstacles[3].obj != Coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                            else if (Bow5.Contains(spawncounter) && GroundSpawns[19].obj != Box && SpawnObstacles[4].obj != CartoonCar && SpawnObstacles[4].obj != null && SpawnObstacles[4].obj != Coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                            else if (Bow6.Contains(spawncounter) && GroundSpawns[20].obj != Box && SpawnObstacles[5].obj != CartoonCar && SpawnObstacles[5].obj != null && SpawnObstacles[4].obj != Coin)
                            {
                                coin = Coin;
                                coinbowcount++;
                            }
                        }
                        else
                        {
                            spawn.alive = false;
                        }
                        break;
                    }
                case 3:
                    {
                        if (spawncounter <= 1 && spawncounter >= 5 && GroundSpawns[6 + (spawncounter * 3)].obj == Box)
                        {
                            Coin = CoinChance();
                        }
                        else if (spawncounter <= 8 && spawncounter >= 12 && GroundSpawns[7 + (spawncounter * 3)].obj == Box)
                        {
                            Coin = CoinChance();
                        }
                        else if (spawncounter <= 15 && spawncounter >= 19 && GroundSpawns[8 + (spawncounter * 3)].obj == Box)
                        {
                            Coin = CoinChance();
                        }
                        else
                        {
                            spawn.alive = false;
                        }
                        break;
                    }
                case 4:
                    {
                        if (spawn.obj == null && spawncounter <= 2 && SpawnObstacles[spawncounter].obj != null && SpawnObstacles[spawncounter].alive == true)
                        {
                            coin = CoinChance();
                        }
                        else if (spawn.obj == null && spawncounter <= 5 && spawncounter > 2 && GroundSpawns[spawncounter + 9].obj != null && GroundSpawns[spawncounter + 9].alive == true)
                        {
                            coin = CoinChance();
                        }
                        else if (spawn.obj == null && spawncounter > 5 && SpawnObstacles[spawncounter - 3].obj != null && SpawnObstacles[spawncounter - 3].alive == true)
                        {
                            coin = CoinChance();
                        }
                        else
                        {
                            spawn.alive = false;
                        }
                        break;
                    }
            }
            return coin;
        }

        GameObject Obstakel(int bigversion)
        {
            GameObject obstakel = new GameObject();
            int chance = GlobalRandom.Next(1, 3); // Check to see if we want a big obstacle or not, if O == 50% than 1 == 25%

            if (chance == 1 && bigversion == 1 && spawncounter <= 2 && bigobstacle == false)
            {
                chance = 1;
                bigobstacle = true;
            }
            else if (chance == 1 && bigversion == 2 && spawncounter >= 3 && bigobstacle == false)
            {
                chance = 1;
                bigobstacle = true;
            }
            else
            {
                chance = 2;
            }
            switch (chance)
            {
                case 1:
                    {
                        if (spawncounter <= 2)
                        {
                            chance = GlobalRandom.Next(2, 5);
                            //Debug.Log("rij van dozen is:" + chance.ToString());
                            if (chance != 4)
                            {
                                SpawnObstacles[spawncounter + 3].alive = false;
                            }
                            for (int i = 0; i < (chance - 1); i++)
                            {
                                int x = spawncounter + 6;
                                if ((i + 1) != 3)
                                {
                                    x += ((i + 1) * 3) + (i * 3);
                                    //Debug.Log(x.ToString());
                                    GroundSpawns[x].obj = Box;
                                    GroundSpawns[x - 3].alive = false; //achter een doos in rij dozen
                                    GroundSpawns[x + 3].alive = false; //voor een doos in rij dozen

                                }
                                else
                                {
                                    //Debug.Log(spawncounter + 3);
                                    SpawnObstacles[spawncounter + 3].obj = Box;
                                    GroundSpawns[spawncounter + 18].alive = false; // 2e rij spawnobstacles
                                    GroundSpawns[spawncounter + 21].alive = false;
                                }
                            }
                            GroundSpawns[spawncounter + 3].alive = false; // 1e rij spawnobstacles
                            GroundSpawns[spawncounter + 6].alive = false;
                            obstakel = Box;
                        }
                        else // alles op de autobaan moet weg
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                int x = spawncounter;
                                if (i == 0) // spawnobstakels eerste rij weghalen
                                {
                                    SpawnObstacles[spawncounter - 3].alive = false;
                                }
                                else // andere groundspawns weghalen
                                {
                                    x += (i * 3);
                                    GroundSpawns[x].alive = false;
                                }
                            }
                            AirSpawns[spawncounter].alive = false; // airspawns op de weg van auto disable
                            AirSpawns[spawncounter - 3].alive = false;
                            switch (spawncounter) // bowcoins op de weg van de auto op false zetten, 3 wegen
                            {
                                case 3:
                                    {
                                        foreach (int i in Bow1)
                                        {
                                            AirSpawnObstacles[i].alive = false;
                                        }
                                        foreach (int i in Row1)
                                        {
                                            AirSpawns[i].alive = false;
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        foreach (int i in Bow2)
                                        {
                                            AirSpawnObstacles[i].alive = false;
                                        }
                                        foreach (int i in Row2)
                                        {
                                            AirSpawns[i].alive = false;
                                        }
                                        break;
                                    }
                                case 5:
                                    {
                                        foreach (int i in Bow3)
                                        {
                                            AirSpawnObstacles[i].alive = false;
                                        }
                                        foreach (int i in Row3)
                                        {
                                            AirSpawns[i].alive = false;
                                        }
                                        break;
                                    }
                            }
                            chance = GlobalRandom.Next(1, 101);
                            if (chance <= 50)
                            {
                                obstakel = Jeep;
                            }
                            else
                            {
                                obstakel = CartoonCar;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        chance = GlobalRandom.Next(1, 5);
                        switch (chance)
                        {
                            case 1:
                                {
                                    if (spawncounter <= 2 && truckcount < 2)
                                    {
                                        for (int i = 0; i < 4; i++)
                                        {
                                            int x = spawncounter;
                                            if (i == 0)
                                            {
                                                GroundSpawns[spawncounter].alive = false;
                                            }
                                            else
                                            {
                                                x += (i * 3);
                                                GroundSpawns[x].alive = false;
                                            }
                                        }
                                        switch (spawncounter) // bowcoins weghalen anders in vrachtwagen
                                        {
                                            case 0:
                                                {
                                                    foreach (int i in Bow1)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    foreach (int i in Bow2)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    foreach (int i in Bow3)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                        }
                                        chance = GlobalRandom.Next(1, 101);
                                        if (chance <= 50)
                                        {
                                            obstakel = Hotdogtruck;
                                        }
                                        else
                                        {
                                            obstakel = Pizzatruck;
                                        }
                                        truckcount++;
                                    }
                                    else if (truckcount < 2)
                                    {
                                        for (int i = 0; i < 4; i++)
                                        {
                                            int x = spawncounter;
                                            if (i == 0)
                                            {
                                                GroundSpawns[spawncounter + 12].alive = false;
                                            }
                                            else
                                            {
                                                x += (i * 3);
                                                GroundSpawns[x + 12].alive = false;
                                            }
                                        }
                                        switch (spawncounter) // bowcoins op de weg van de auto op false zetten, 3 wegen
                                        {
                                            case 3:
                                                {
                                                    foreach (int i in Bow4)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                            case 4:
                                                {
                                                    foreach (int i in Bow5)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                            case 5:
                                                {
                                                    foreach (int i in Bow6)
                                                    {
                                                        AirSpawnObstacles[i].alive = false;
                                                    }
                                                    break;
                                                }
                                        }
                                        chance = GlobalRandom.Next(1, 101);
                                        if (chance <= 50)
                                        {
                                            obstakel = Hotdogtruck;
                                        }
                                        else
                                        {
                                            obstakel = Pizzatruck;
                                        }
                                        truckcount++;
                                    }
                                    else
                                    {
                                        obstakel = Box;
                                        if (spawncounter >= 2)
                                        {
                                            GroundSpawns[spawncounter + 3].alive = false;
                                            GroundSpawns[spawncounter + 6].alive = false;
                                        }
                                        else
                                        {
                                            GroundSpawns[spawncounter + 15].alive = false;
                                            GroundSpawns[spawncounter + 18].alive = false;
                                        }
                                    }
                                    break;
                                }
                            case 2:
                            case 3:
                            case 4:
                                {
                                    HashSet<int> scoot = new HashSet<int>() { 0, 2, 3, 5 };
                                    chance = GlobalRandom.Next(1, 3);
                                    if (chance == 1 && scoot.Contains(spawncounter))
                                    {
                                        obstakel = Scooter;
                                    }
                                    else
                                    {
                                        obstakel = Box;
                                        if (spawncounter <= 2)
                                        {
                                            GroundSpawns[spawncounter + 3].alive = false;
                                            GroundSpawns[spawncounter + 6].alive = false;
                                        }
                                        else
                                        {
                                            //Debug.Log((spawncounter + 15).ToString() + " en " + (spawncounter + 18).ToString() + " staat uit");
                                            GroundSpawns[spawncounter + 15].alive = false;
                                            GroundSpawns[spawncounter + 18].alive = false;
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
            return obstakel;
        }

        GameObject PowerupChance()
        {
            GameObject pc = new GameObject();
            int powerupchance = GlobalRandom.Next(1, 4);
            switch (powerupchance)
            {
                case 1:
                    pc = Speed;
                    break;
                case 2:
                    pc = DoubleCoins;
                    break;
                case 3:
                    pc = Shield;
                    break;
            }
            return pc;
        }

        GameObject CoinChance()
        {
            GameObject c = new GameObject();
            int coinchance = GlobalRandom.Next(0, 100);
            if (coinchance < 5) // Het diamandje heeft nu een kans van 5 procent als er een muntje word gespawnt, 
            {
                c = Diamond;
            }
            else
            {
                c = Coin;
            }
            return c;
        }

        void Spawn()
        {
            int counter = 0;
            foreach (SpawnObject spawn in SpawnObstacles)
            {
                if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
                {
                    //Debug.Log(counter.ToString() + " " + spawn.loc.transform.position.ToString());
                    GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
                    spawned.transform.parent = GameObject.Find(transform.name + "/Spawner/EnterTrigger").transform;
                    counter++;
                }
            }
            foreach (SpawnObject spawn in GroundSpawns)
            {
                if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
                {

                    GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
                    spawned.transform.parent = GameObject.Find(transform.name + "/Spawner").transform;
                    counter++;

                }
            }
            foreach (SpawnObject spawn in AirSpawnObstacles)
            {
                if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
                {
                    GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
                    spawned.transform.parent = GameObject.Find(transform.name + "/Spawner").transform;
                }
            }
            foreach (SpawnObject spawn in BoxCoins)
            {
                if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
                {
                    GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
                    spawned.transform.parent = GameObject.Find(transform.name + "/Spawner").transform;
                }
            }
            foreach (SpawnObject spawn in AirSpawns)
            {
                if (spawn != null && spawn.alive && spawn.obj != null && spawn.loc != null && spawn.loc.position != null && spawn.loc.rotation != null)
                {

                    GameObject spawned = Instantiate(spawn.obj, spawn.loc.position, spawn.loc.rotation) as GameObject;
                    spawned.transform.parent = GameObject.Find(transform.name + "/Spawner").transform;

                }
            }
            foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (go.name == "New Game Object")
                {
                    Destroy(go);
                }
            }
        }
    }
}
