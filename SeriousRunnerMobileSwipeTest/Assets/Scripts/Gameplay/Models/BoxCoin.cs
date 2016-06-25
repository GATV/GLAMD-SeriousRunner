using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gameplay.Models
{
    public class BoxCoin
    {
        public int CP, C, P, N;
        public void Fill(int cp, int c, int p, int n)
        {
            CP = cp;
            C = c;
            P = p;
            N = n;
        }
    }
}
