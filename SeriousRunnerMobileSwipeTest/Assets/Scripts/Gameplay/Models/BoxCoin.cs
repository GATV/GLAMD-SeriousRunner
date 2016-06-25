using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gameplay.Models
{
    public class BoxCoin
    {
        public int C, P, N;
        public void Fill(int c, int p, int n)
        {
            C = c;
            P = p;
            N = n;
        }
    }
}
