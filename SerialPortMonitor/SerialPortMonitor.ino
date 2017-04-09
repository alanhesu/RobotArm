#include <Servo.h>

//Setup Output
int ledPin_3 = 13;

Servo servo1;
double convert=0;

//Setup message bytes
byte inputByte_0;
byte inputByte_1;
byte inputByte_2;
byte inputByte_3;
byte inputByte_4;

//Setup
void setup() {
  pinMode(ledPin_3, OUTPUT);
  Serial.begin(57600);
  digitalWrite(ledPin_3, HIGH);//
  delay(1000);//
  digitalWrite(ledPin_3, LOW);//
  delay(1000);//  
  servo1.attach(6);  
}

void loop() {
  //Read Buffer    
  if (Serial.available() == 5) 
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
  }
  //Check for start of Message
  if(inputByte_0 == 16)
  {       
       //Detect Command type
       switch (inputByte_1) 
       {
          case 127:
             //Set PIN and value
             ledPin_3 = inputByte_2;
             if(inputByte_3 == 255)
                {
                  digitalWrite(ledPin_3, HIGH); 
                  break;
                }
                else
                {
                  digitalWrite(ledPin_3, LOW); 
                  break;
                }             
                break;          
          case 128:
            //Say hello
            // Serial.println("HELLO FROM ARDUINO");
            break;
          case 129:
            //Servo motor
            servo1.write(inputByte_3);
            break;
        } 
        //Clear Message bytes
        inputByte_0 = 0;
        inputByte_1 = 0;
        inputByte_2 = 0;
        inputByte_3 = 0;
        inputByte_4 = 0;
        //Let the PC know we are ready for more data
        // Serial.println("-READY TO RECEIVE");
  }
}
