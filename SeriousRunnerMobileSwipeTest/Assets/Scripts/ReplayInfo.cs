using System;
using System.Linq;
using UnityEngine;

class ReplayInfo
{
    public int index;
    public Vector3 pos;
    public Direction d;

    public ReplayInfo(string info)
    {
        string[] data = info.Split(',');
        index = Convert.ToInt32(data[0]);
        pos = new Vector3(x: Convert.ToSingle(data[1]), y: Convert.ToSingle(data[2]), z: Convert.ToSingle(data[3]));
        d = (Direction)Convert.ToByte(data[4]);
    }

    public ReplayInfo(int index, Vector3 pos, Direction d)
    {
        this.index = index;
        this.pos = pos;
        this.d = d;
    }

    public override string ToString()
    {
        return String.Format("{0},{1:0.0},{2:0.0},{3:0.0},{4}", index, pos.x, pos.y, pos.z, (int)d);
    }
}