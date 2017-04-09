using System;
using System.Threading;
using Leap;
using Leap.Vector;

class HandData {
    
    // Data
    public int x;
    public int y;
    public int z;
    public float roll;
    public float pitch;
    public float grip;
    
    public HandData(int x, int y, int z, float roll, float pitch, float grip){
        this.x = x;
        this.y = y;
        this.z = z;
        this.roll = roll;
        this.pitch = pitch;
        this.grip = grip;
    }

    public override String ToString()
    {
        return sprintf("x: " + x + "y: " + y + "z: " + "roll: " + roll + "pitch: " + "grip: " + grip);
    }
}