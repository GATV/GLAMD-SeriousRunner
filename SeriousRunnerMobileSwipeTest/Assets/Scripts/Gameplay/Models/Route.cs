using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Models;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gameplay.Models
{
    public class Route
    {
        public Generator gen = new Generator();
        public void Fill(int o, int p, int c, int n)
        {
            gen.Fill(o, p, c, n);
        }
    }
}
