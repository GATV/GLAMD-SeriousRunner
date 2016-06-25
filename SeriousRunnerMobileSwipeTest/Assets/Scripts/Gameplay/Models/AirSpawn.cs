using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gameplay.Models
{
    public class AirSpawn
    {
        public int P, C, N;

        public void Fill(int p, int c, int n)
        {
            P = p;
            C = c;
            N = n;
        }
    }
}
