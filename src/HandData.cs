using System;
using System.Threading;
using Leap;

class HandData {
    
    // Data
    public int x;
    public int y;
    public int z;
    public float roll;
    public float pitch;
    public float grip;

    static private int xMin;
    static private int xMax;
    static private int yMin;
    static private int yMax;
    static private int zMin;
    static private int zMax;
    static private int gripMin;
    static private int gripMax;

    public HandData(int x, int y, int z, float roll, float pitch, float grip){
        this.x = x;
        this.y = y;
        this.z = z;
        this.roll = roll;
        this.pitch = pitch;
        this.grip = grip;
        xMin = -300;
        xMax = 350;
        yMin = -200;
        yMax = 490;
        zMin = -400;
        zMax =  0;
        gripMin = 10;
        gripMax = 100;
    }

    public void setData(int x, int y, int z, float roll, float pitch, float grip)
    {        
        this.z = 255 - ((Math.Min(zMax, Math.Max(zMin, z)) - zMin) * 255 / (zMax - zMin));
        this.x = (Math.Min(xMax, Math.Max(xMin, x)) - xMin) * 255 / (xMax - xMin);
        this.y = (Math.Min(yMax, Math.Max(yMin, y)) - yMin) * 255 / (yMax - yMin);
        this.roll = roll;
        this.pitch = pitch;
        this.grip = ((Math.Min(gripMax, Math.Max(gripMin, grip)) - gripMin) / (gripMax - gripMin) * 255);
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetZ()
    {
        return z;
    }

    public float GetRoll()
    {
        return roll;
    }

    public float GetPitch()
    {
        return pitch;
    }

    public float GetGrip()
    {
        return grip;
    }

    public byte[] GetDataBytes()
    {
        byte[] data = new byte[6];
        data[0] = Convert.ToByte(x);
        data[1] = Convert.ToByte(y);
        data[2] = Convert.ToByte(z);
        data[3] = Convert.ToByte(roll);
        data[4] = Convert.ToByte(pitch);
        data[5] = Convert.ToByte(grip);
        return data;          
    }

    public override String ToString()
    {
        return "x: " + x + " y: " + y + " z: " + z + " roll: " + roll + " pitch: " + pitch + " grip: " + grip;
    }
}