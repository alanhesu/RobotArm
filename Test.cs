using System;

public class Test
{
	public static void Main()
	{
        ArduinoControllerMain controller = new ArduinoControllerMain();
        controller.SetComPort();          
        for (int i = 0; i < 50; i++)
        {
            controller.SendArduino(16, 129, 6, 0, 4, 30);
            controller.SendArduino(16, 127, 13, 255, 4, 30);
            controller.SendArduino(16, 127, 13, 0, 4, 30);
        }
        controller.ClosePort();
    }
}
