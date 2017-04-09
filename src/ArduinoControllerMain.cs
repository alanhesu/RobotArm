using System;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Collections.Generic;

public class ArduinoControllerMain
{

    private SerialPort currentPort;
    private bool portFound;
    List<byte> buffer = new List<byte>();

    public bool GetPortFound()
    {
        return portFound;
    }

    public void SetComPort()
    {       
        currentPort = new SerialPort("COM4", 57600);             
        if (!currentPort.IsOpen)
        {           
            currentPort.DataReceived += currentPort_DataReceived;
            currentPort.Open();
            portFound = true;                 
        }
        else
        {
            portFound = false;
            throw new InvalidOperationException("The Serial Port is already open!");
        }
        /*
        try
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {               
                currentPort = new SerialPort(port, 9600);
                byte[] ArduinoCheck = new byte[1];
                currentPort.Read(ArduinoCheck, 0, 1);
                for (int i = 0; i < ArduinoCheck.Length; i++)
                {
                    if (ArduinoCheck[i] != 255)
                    {
                        portFound = false;
                        Console.WriteLine("Not Found");
                    }
                }
                portFound = true;
                Console.WriteLine("Found");
                byte[] buffer = new byte[5];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(127);
                buffer[2] = Convert.ToByte(13);
                buffer[3] = Convert.ToByte(255);
                buffer[4] = Convert.ToByte(4);
                currentPort.Write(buffer, 0, 5);
            }
        }
        catch (Exception e)
        {
        }
        */
    }

    public void ClosePort()
    {
        currentPort.Close();
    }

    void currentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        string data = currentPort.ReadTo("\n");//Read until the newline code
        Console.WriteLine(data);                                                
        string[] dataArray = data.Split(new string[]
        {"\x02", "$" }, StringSplitOptions.RemoveEmptyEntries);
        //Iterate through the split data and parse it into weather data items
        //and add them to the list of received weather data.
        /*foreach (string dataItem in dataArray.ToList())
        {
            Console.WriteLine(dataItem);
        }*/
        Thread.Sleep(5);        
    }

    public void SendArduino(byte a, byte b, byte c, byte d, byte e, byte f, int time)
    {
        if (currentPort.IsOpen)
        {            
            byte[] buffer = new byte[6];
            buffer[0] = Convert.ToByte(a);
            buffer[1] = Convert.ToByte(b);
            buffer[2] = Convert.ToByte(c);
            buffer[3] = Convert.ToByte(d);
            buffer[4] = Convert.ToByte(e);
            buffer[5] = Convert.ToByte(f);
            currentPort.Write(buffer, 0, 6);
            Thread.Sleep(time);
            currentPort.DiscardOutBuffer();
            // currentPort.DiscardInBuffer();
        }
        else
        {
            throw new InvalidOperationException("Can't get data if the serial Port is closed!");
        }
    }

    public bool DetectArduino()
    {
        try
        {
            //The below setting are for the Hello handshake
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(13);
            buffer[3] = Convert.ToByte(255);
            buffer[4] = Convert.ToByte(4);
            int intReturnASCII = 0;
            char charReturnValue = (Char)intReturnASCII;
            currentPort.Open();
            currentPort.Write(buffer, 0, 5);
            Thread.Sleep(1000);
            int count = currentPort.BytesToRead;
            string returnMessage = "";
            while (count > 0)
            {
                intReturnASCII = currentPort.ReadByte();
                returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                count--;
            }
            // ComPort.name = returnMessage;
            currentPort.Close();
            if (returnMessage.Contains("HELLO FROM ARDUINO"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }

}