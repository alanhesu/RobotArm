#include <Servo.h>
#include <Math.h>
Servo servo3; // controls shoulder (measured from x-axis from the projection to the x-y plane)
Servo servo5; // controls elbow (measured from y-axis from the projection to the x-y plane)
Servo servo6; // controls angle (measured from x-axis from the projection to the x-z plane)
Servo servo9; // controls wrist roll
Servo servo10; // controls wrist pitch
Servo servo11; // controls gripper

byte inputByte_0;
byte inputByte_1;
byte inputByte_2;
byte inputByte_3;
byte inputByte_4;
byte inputByte_5;

// lengths of arm pieces (millimeters -- *** corey's dimensions are in inches)
double shoulderLength = 203.2;
double elbowLength = 203.2;
// effective length of gripper (millimeters -- *** my dimensions are in inches)
double gripperLength = 3;

// angles of arm servos
double th1;
double th2;
double th3;
double thtemp;
// angle of gripper
double gripAngle = 0;

// target positions/angles (x, y, z, roll, pitch, gripDistance) -- units: (mm, mm, mm, degrees, degrees, degrees) 
double x = 0;
double y = 0;
double z = 0;
double roll = 0; // from -pi/2 to + pi/2
double pitch = 0;
double grip = 0; // distance between two fingers

// value bounds
double xMin = -441.6;
double xMax = 441.6;
double yMin = 0;
double yMax = 457.2;
double zMin = 0;
double zMax = 257.2;

// offsets
double xoff = 0;
double yoff = 0;
double zoff = 0;

double maxGrip;
// random shit
double C;
double pi = acos(-1);

void setup() {  
  Serial.begin(57600);
  servo3.attach(3);
  servo5.attach(5);
  servo6.attach(6);
  servo9.attach(9);
  servo10.attach(10);
  servo11.attach(11);
}

void loop() {
  //Read Buffer    
  if (Serial.available() == 6) 
  {    
    //Read buffer
    inputByte_0 = Serial.read();
    delay(2);    
    inputByte_1 = Serial.read();
    delay(2);      
    inputByte_2 = Serial.read();
    delay(2);      
    inputByte_3 = Serial.read();
    delay(2);
    inputByte_4 = Serial.read();   
    delay(2);
    inputByte_5 = Serial.read();   
  }

  // Convert buffer values to target positions/angles
  //z = (double)inputByte_2 / 255 * (zMax - zMin) + zMin;
  //y = (double)inputByte_1 / 255 * (yMax - yMin) + yMin;
  //x = (double)inputByte_0 / 255 * (xMax - xMin) + xMin;
  x = shoulderLength;
  y = shoulderLength; // inversed front to back
  z = 0; // side to side
  roll = (double)inputByte_3;
  pitch = (double)inputByte_4;
  grip = (double)inputByte_5 / 255 * 180;

  /////////// ARM CODE //////////////////////////////////////////////////////
  // add offsets to x, y, z (by default, 0,0,0 is set at the base of the arm)
  x = x + xoff;
  y = y + yoff;
  z = z + zoff;
  
  // angle calculations -- takes in x,y,z and converts to th1, th2, th3 (radians) ///////////////////
  C = sqrt(pow(x,2) + pow(y,2) + pow(z,2));
  th3 = acos(sqrt(pow(x,2) + pow(z,2))/C) - acos((pow(C,2) + pow(shoulderLength,2) - pow(elbowLength,2))/(2*C*shoulderLength));
  th1 = atan(z/x);
  //thtemp = acos((pow(elbowLength,2) - pow(shoulderLength,2) - pow(C,2))/(-2*shoulderLength*C));
  //th2 = acos(y/C) + thtemp;
   th2 = acos(y/C) - acos((pow(C,2) + pow(elbowLength,2) - pow(shoulderLength,2))/(2*C*elbowLength));
  //th3 = acos((C*C-pow(shoulderLength,2) - pow(elbowLength,2))/(-2*shoulderLength*elbowLength));
  // convert to degrees
  th1 = (th1*360)/(2*pi);
  //if (th1 < 0) {
  //  th1 = th1 + 180;
  //}
  th2 = (th2*360)/(2*pi);
  th3 = (th3*360)/(2*pi);
  th3 = -th3 - 90;
  th2 = -th2 + 90;
  th1 = -th1 + 90;

  // add offsets and reversals to th1, th2, th3 
  // (by default, th1 = 0 is set as parallel to the base, th2 = 0 is set as normal to the base, and th3 = 0 is set as the linkage's plane as parallel to the x-axis)
  //

  // DEBUG
  Serial.print("th1: ");
  Serial.print(th1);
  Serial.print(" th2: ");
  Serial.print(th2);
  Serial.print(" th3: ");
  Serial.print(th3);
  Serial.print(" x: ");
  Serial.print(x);
  Serial.print(" y: ");
  Serial.print(y);
  Serial.print(" z: ");
  Serial.println(z);
  // write angle values to motors (degrees) ////////
  if (!(isnan(th1) || isnan(th2) || isnan(th3))) {
    servo3.write(th1);
    servo5.write(th2);
    servo6.write(th3);
  }

  // normalize angle values
  // roll = roll + 90;
  if (roll > 0 && roll < 180) {
    servo9.write(roll);
  }
  // pitch = pitch + 90;
  if (pitch > 0 && pitch < 180) {
    servo10.write(pitch);
  }

  // GRIPPER CODE /////////////
  if (grip > 0 && grip < 180) {
    servo11.write(grip);
  }
  // gripAngle = (asin(gripDistance/gripperLength)*360)/(2*pi); // degrees
  // if (gripAngle <= 180) {
  //   servo11.write(gripAngle); // fix this value later
  // }
}
