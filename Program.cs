using System.Net;

using Rug.Osc;

using WebSocketSharp;
using WebSocketSharp.Server;


public class OSCSocket : WebSocketBehavior  //Not the best name, but it gets the job done.
{
    const int OUT_PORT = 9000;
    private OscSender toVRChat;


    public OSCSocket()
    {
        toVRChat = new OscSender(IPAddress.Parse("127.0.0.1"),OUT_PORT);
        toVRChat.Connect();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        //This sections of code is for getting messages sent from the web page to VRChat.
        base.OnMessage(e);
        switch (e.Data.ToString())
        {
            case "Automatic":
                toVRChat.Send(new OscMessage("/avatar/parameters/isManual", 0));
                break;
            case "Manual":
                toVRChat.Send(new OscMessage("/avatar/parameters/isManual", 1));
                break;
            case "Playlist":
                toVRChat.Send(new OscMessage("/avatar/parameters/isSingle", 0));
                break;
            case "Single":
                toVRChat.Send(new OscMessage("/avatar/parameters/isSingle", 1));
                break;
            default:
                if (e.Data.ToString().StartsWith("Chat:"))
                {
                    String tempChat = e.Data.ToString().Replace("Chat:", "");
                    toVRChat.Send(new OscMessage("/chatbox/input", tempChat, 1));
                }
                break;
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        WebSocketServer serverSocket = new("ws://localhost:2588");

        serverSocket.AddWebSocketService<OSCSocket>("/Bridge");

        serverSocket.Start();
        Console.WriteLine("WebSocket server started. Web page should be able to connect now.");

        //OscReveiver is here to get signals from VRChat.
        const int IN_PORT = 9001;
        OscReceiver fromVRChat = new(IN_PORT);
        try
        {
            fromVRChat.Connect();
            Console.WriteLine("OSC server started. Will begin listening for VRChat\'s signals.");
        }
        catch (Exception e)
        {
            Console.WriteLine("There was a problem starting the OSC server. Most likely caused by the program not being allowed through the Firewall in order receive from VRChat\'s port. Sometimes, a restart is also required to apply changes to the Firewall.");
            Console.WriteLine(e.ToString());

            serverSocket.Stop();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        bool nextGate = false;
        bool pauseGate = false;
        bool backGate = false;
        while (fromVRChat.State != OscSocketState.Closed)
        {
            //No need to recieve anything if it's not connected.
            if (fromVRChat.State == OscSocketState.Connected)
            {
                //Obviously, OSC needs to be received.
                OscPacket packetVRC = fromVRChat.Receive();

                //Packet format is useless. An OscMessage object is necessary to check the address, which is important.
                OscMessage messageVRC = (OscMessage)packetVRC;

                //This is what does most of the work. The address and arguments are used to determine what WebSocket message to send to the web page.
                //The purpose of this switch is to get information from VRChat to the web page.
                switch (messageVRC.Address)
                {
                    //The _Angle addresses should only send when the lever is pulled all the way down.
                    //To prevent spamming, the lever has to go back up again before it can be pulled again.
                    case "/avatar/parameters/Next_Angle":
                        float nextAngle = Convert.ToSingle(messageVRC[0]);
                        if (nextGate == true && nextAngle <= 0.01) nextGate = false;
                        else if (nextGate == false && nextAngle >= 0.49)
                        {
                            nextGate = true;
                            serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Next");
                            Console.WriteLine("Next sent from VRChat!");
                        }
                        break;
                    case "/avatar/parameters/Pause_Angle":
                        float pauseAngle = Convert.ToSingle(messageVRC[0]);
                        if (pauseGate == true && pauseAngle <= 0.01) pauseGate = false;
                        else if (pauseGate == false && pauseAngle >= 0.49)
                        {
                            pauseGate = true;
                            serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Pause");
                            Console.WriteLine("Pause sent from VRChat!");
                        }
                        break;
                    case "/avatar/parameters/Back_Angle":
                        float backAngle = Convert.ToSingle(messageVRC[0]);
                        if (backGate == true && backAngle <= 0.01) backGate = false;
                        else if (backGate == false && backAngle >= 0.49)
                        {
                            backGate = true;
                            serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Back");
                            Console.WriteLine("Back sent from VRChat!");
                        }
                        break;
                    //This section is for the VRChat Remote Control, or the VRCRC.
                    case "/VRCRC/NextTrack":
                        serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Next");
                        Console.WriteLine("Next sent from VRCRC!");
                        break;
                    case "/VRCRC/PlayPauseTrack":
                        serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Pause");
                        Console.WriteLine("Pause sent from VRCRC!");
                        break;
                    case "/VRCRC/PreviousTrack":
                        serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Back");
                        Console.WriteLine("Back sent from VRCRC!");
                        break;
                    case "/VRCRC/RestartTrack":
                        serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("Restart");
                        Console.WriteLine("Restart sent from VRCRC!");
                        break;
                    case "/VRCRC/YouTubeURL":
                        serverSocket.WebSocketServices["/Bridge"].Sessions.Broadcast("URL:" + messageVRC[0].ToString());
                        Console.WriteLine("URL sent from VRCRC!");
                        break;
                    //This section is a bit messy due to needing to check for prefixes and extracting information from the address.
                    default:
                        if (messageVRC.Address.StartsWith("/avatar/parameters/music_disc") || messageVRC.Address.StartsWith("/avatar/parameters/album_") && (bool)messageVRC[0])
                        {
                            string tempAddress = messageVRC.Address.ToString().Replace("/avatar/parameters/", "");
                            Console.WriteLine("Disc or Album sent from VRChat!");
                        }
                        break;
                }
            }
        }
    }
}