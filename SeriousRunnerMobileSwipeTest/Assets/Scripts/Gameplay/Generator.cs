using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay.Models;


namespace Assets.Scripts.Gameplay
{
    public class Generator
    {
        public SpawnObstacle SO = new SpawnObstacle();
        public GroundSpawn GS = new GroundSpawn();
        public AirSpawnObstacle ASO = new AirSpawnObstacle();
        public BoxCoin BC = new BoxCoin();
        public AirSpawn AS = new AirSpawn();
        public void Fill (int o, int p, int c, int n)
        {
            SO.Fill(o, p, c, n);
            GS.Fill(p, c, (100 - (p + c)));
            AS.Fill(p, c, (100 - (p + c)));
            ASO.Fill(c, (100 - c));
            BC.Fill(c, p, (100 - (c + p)));
        }
    }
}
