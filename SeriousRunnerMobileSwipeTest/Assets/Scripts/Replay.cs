using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Replay
    {
        private List<ReplayInfo> replayInfoCollection;

        public Replay()
        {
            replayInfoCollection = new List<ReplayInfo>();
        }

        public Replay(string data)
        {
            replayInfoCollection = data.Split('_').Select(x => new ReplayInfo(x)).OrderBy(x => x.index).ToList();
        }

        public void Add(ReplayInfo info)
        {
            replayInfoCollection.Add(info);
        }

        public Queue<Direction> GetDirectionQueue()
        {
            return new Queue<Direction>(replayInfoCollection.Select(x => x.d));
        }

        public Vector3[] Path
        {
            get
            {
                return replayInfoCollection.OrderBy(x => x.index).Select(x => x.pos).ToArray();
            }
        }

        public int Count
        {
            get
            {
                return replayInfoCollection.Count;
            }
        }

        public override string ToString()
        {
            return string.Join("_", replayInfoCollection.OrderBy(x => x.index).Select(x => x.ToString()).ToArray());
        }
    }
}
