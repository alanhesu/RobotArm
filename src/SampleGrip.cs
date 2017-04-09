/******************************************************************************\
* Copyright (C) 2012-2016 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
\******************************************************************************/
using System;
using System.Threading;
using Leap;

class SampleListener
{
    public HandData data;
    public SampleListener()
    {
        data = new HandData(0, 0, 0, 0, 0, 0);
    }
    public void OnInit(Controller controller)
    {
        Console.WriteLine("Initialized");
    }

    public void OnConnect(object sender, DeviceEventArgs args)
    {
        Console.WriteLine("Connected");
    }

    public void OnDisconnect(object sender, DeviceEventArgs args)
    {
        Console.WriteLine("Disconnected");
    }

    public void OnFrame(object sender, FrameEventArgs args)
    {
        // Get the most recent frame and report some basic information
        Frame frame = args.frame;
        if (frame.Hands.Count == 2)
        {
            Console.WriteLine("Two Hands in frame, move one out of the way.");
        }
        else if (frame.Hands.Count == 0)
        {
            Console.WriteLine("No Hands in Frame, move one into the frame.");
        }
        else {
            float[] WristPos = frame.Hands[0].WristPosition.ToFloatArray();
            float pitch = frame.Hands[0].Direction.Pitch * 180.0f / (float)Math.PI;
            if (pitch <= -90)
            {
                pitch = -90;
            }
            else if (pitch >= 90)
            {
                pitch = 90;
            }
            float roll = frame.Hands[0].PalmNormal.Roll * 180.0f / (float)Math.PI;
            if (roll >= 90)
            {
                roll = 90;
            }
            else if (roll <= -90)
            {
                roll = -90;
            }
            float gripdist = frame.Hands[0].PinchDistance;
            data.setData((int)WristPos[0], (int)WristPos[1] - 254, (int)WristPos[2] - 350, roll + 90, pitch + 90, gripdist);            
        }
        /**
            Console.WriteLine(
              "Frame id: {0}, timestamp: {1}, hands: {2}",
              frame.Id, frame.Timestamp, frame.Hands.Count
            );

            foreach (Hand hand in frame.Hands)
            {
              Console.WriteLine("  Hand id: {0}, palm position: {1}, fingers: {2}",
                hand.Id, hand.PalmPosition, hand.Fingers.Count);
              // Get the hand's normal vector and direction
              Vector normal = hand.PalmNormal;
              Vector direction = hand.Direction;

              // Calculate the hand's pitch, roll, and yaw angles
              Console.WriteLine(
                "  Hand pitch: {0} degrees, roll: {1} degrees, yaw: {2} degrees",
                direction.Pitch * 180.0f / (float)Math.PI,
                normal.Roll * 180.0f / (float)Math.PI,
                direction.Yaw * 180.0f / (float)Math.PI
              );

              // Get the Arm bone
              Arm arm = hand.Arm;
              Console.WriteLine(
                "  Arm direction: {0}, wrist position: {1}, elbow position: {2}",
                arm.Direction, arm.WristPosition, arm.ElbowPosition
              );

              // Get fingers
              foreach (Finger finger in 
              
              .Fingers)
              {
                Console.WriteLine(
                  "    Finger id: {0}, {1}, length: {2}mm, width: {3}mm",
                  finger.Id,
                  finger.Type.ToString(),
                  finger.Length,
                  finger.Width
                );

                // Get finger bones
                Bone bone;
                for (int b = 0; b < 4; b++)
                {
                  bone = finger.Bone ((Bone.BoneType)b);
                  Console.WriteLine (
                    "      Bone: {0}, start: {1}, end: {2}, direction: {3}",
                    bone.Type, bone.PrevJoint, bone.NextJoint, bone.Direction
                  );
                }
              }
            }

            if (frame.Hands.Count != 0)
            {
              Console.WriteLine("");
            }
            */
    }

    public void OnServiceConnect(object sender, ConnectionEventArgs args)
    {
        Console.WriteLine("Service Connected");
    }

    public void OnServiceDisconnect(object sender, ConnectionLostEventArgs args)
    {
        Console.WriteLine("Service Disconnected");
    }

    public void OnServiceChange(Controller controller)
    {
        Console.WriteLine("Service Changed");
    }

    public void OnDeviceFailure(object sender, DeviceFailureEventArgs args)
    {
        Console.WriteLine("Device Error");
        Console.WriteLine("  PNP ID:" + args.DeviceSerialNumber);
        Console.WriteLine("  Failure message:" + args.ErrorMessage);
    }

    public void OnLogMessage(object sender, LogEventArgs args)
    {
        switch (args.severity)
        {
            case Leap.MessageSeverity.MESSAGE_CRITICAL:
                Console.WriteLine("[Critical]");
                break;
            case Leap.MessageSeverity.MESSAGE_WARNING:
                Console.WriteLine("[Warning]");
                break;
            case Leap.MessageSeverity.MESSAGE_INFORMATION:
                Console.WriteLine("[Info]");
                break;
            case Leap.MessageSeverity.MESSAGE_UNKNOWN:
                Console.WriteLine("[Unknown]");
                break;
        }
        Console.WriteLine("[{0}] {1}", args.timestamp, args.message);
    }

    public HandData GetHandData()
    {
        return data;
    }
}

class SampleGrip
{
    public static void Main()
    {
        using (Leap.IController controller = new Leap.Controller())
        {
            controller.SetPolicy(Leap.Controller.PolicyFlag.POLICY_ALLOW_PAUSE_RESUME);

            // Set up our listener:
            SampleListener listener = new SampleListener();
            controller.Connect += listener.OnServiceConnect;
            controller.Disconnect += listener.OnServiceDisconnect;
            controller.FrameReady += listener.OnFrame;
            controller.Device += listener.OnConnect;
            controller.DeviceLost += listener.OnDisconnect;
            controller.DeviceFailure += listener.OnDeviceFailure;
            controller.LogMessage += listener.OnLogMessage;

            ArduinoControllerMain ArduinoController = new ArduinoControllerMain();
            ArduinoController.SetComPort();
            while (true)
            {
                // Console.WriteLine(listener.GetHandData());
                // ArduinoController.SendArduino(16, 129, 6, Convert.ToByte(listener.GetHandData().GetX()), 4, 30);                                
                // ArduinoController.SendArduino(16, 127, 13, 255, 4, 30);
                // ArduinoController.SendArduino(16, 127, 13, 0, 4, 30);
                byte[] toArduino = listener.GetHandData().GetDataBytes();                
                ArduinoController.SendArduino(toArduino[0], toArduino[1], toArduino[2], toArduino[3], toArduino[4], toArduino[5], 50);
            }
            ArduinoController.ClosePort();

            // Keep this process running until Enter is pressed
            Console.WriteLine("Press any key to quit...");
            Console.ReadLine();
        }
    }
}
