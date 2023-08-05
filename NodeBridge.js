const osc = require('osc');
const WebSocket = require('ws');

//Author: Mudskippy

//NOTE: Make sure to run this BEFORE the HTML, otherwise the socket connection will fail.

const wss = new WebSocket.Server({ port: 2588 });

//These variables are in place to prevent spamming with the levers.
let Next_Gate = false;
let Pause_Gate = false;
let Back_Gate = false;

let sockets = [];
wss.on('connection', function(socket){  
  sockets.push(socket);

  socket.on('message', function(msg){ //This function forwards and interprets messages from HTML to VRChat.
    switch(msg.toString()){
      //This sends the parameters to VRChat in order to show/hide appropriate levers.
      case "Automatic":
        clientPort.send({
          address: "/avatar/parameters/isManual",
          args: [
            {
              type: "i",
              value: 0
            }
          ]
        }, "127.0.0.1", 9000)
        break;
      case "Manual":
        clientPort.send({
          address: "/avatar/parameters/isManual",
          args: [
            {
              type: "i",
              value: 1
            }
          ]
        }, "127.0.0.1", 9000)
        break;
      case "Playlist":
        clientPort.send({
          address: "/avatar/parameters/isSingle",
          args: [
            {
              type: "i",
              value: 0
            }
          ]
        }, "127.0.0.1", 9000)
        break;
      case "Single":
        clientPort.send({
          address: "/avatar/parameters/isSingle",
          args: [
            {
              type: "i",
              value: 1
            }
          ]
        }, "127.0.0.1", 9000)
        break;
      default: //This jargon forwards chatbox messages from the HTML to VRChat.
        if(msg.toString().includes("Chat:")){
          clientPort.send({
            address: "/chatbox/input",
            args: [
              {
                type: "s",
                value: msg.toString().replace("Chat:", "")
              }, {
                type: "i",
                value: 1
              }
            ]
          }, "127.0.0.1", 9000);
        }
        break;
    }
    
  });

  socket.on('close', function() {
    sockets = sockets.filter(s => s !== socket);
  });
});

const clientPort = new osc.UDPPort({  //Creates the port that receives information from VRChat.
  localAddress: '0.0.0.0',
  localPort: 9001,
});

clientPort.on('ready', function () {  //Just prints a log to let you know clientPort is ready to be used.
  console.log('Client Port is ready.');
});

clientPort.on('message', function (msg) { //A bit of a mess here. This interprets and forwards OSC messages from VRChat to the HTML.
  switch(msg.address){
    //The _Angle variables send when the lever is pulled all the way down. Afterward, they won't send again until the lever goes all the way up, which "reloads" the lever. This helps prevent spamming.
    case "/avatar/parameters/Next_Angle":
      if(Next_Gate == true && msg.args[0] <= 0.01) Next_Gate = false;
      else if(Next_Gate == false && msg.args[0] >= 0.49){
        Next_Gate = true;
        wss.clients.forEach((client) =>{
          client.send("Next");
        });
        console.log("Next Sent from VRChat!");
      }
      break;
    case "/avatar/parameters/Pause_Angle":
      if(Pause_Gate == true && msg.args[0] <= 0.01) Pause_Gate = false;
      else if(Pause_Gate == false && msg.args[0] >= 0.49){
        Pause_Gate = true;
        wss.clients.forEach((client) =>{
          client.send("Pause");
        });
        console.log("Pause Sent from VRChat!");
      }
      break;
    case "/avatar/parameters/Back_Angle":
      if(Back_Gate == true && msg.args[0] <= 0.01) Back_Gate = false;
      else if(Back_Gate == false && msg.args[0] >= 0.49){
        Back_Gate = true;
        wss.clients.forEach((client) =>{
          client.send("Back");
        });
        console.log("Back Sent from VRChat!");
      }
    //This section is for the VRChat Remote Control, or the VRCRC.
    case "/VRCRC/NextTrack":
      wss.clients.forEach((client) =>{
        client.send("Next");
      });
      console.log("Next Sent from VRCRC!");
      break;
    case "/VRCRC/PlayPauseTrack":
      wss.clients.forEach((client) =>{
        client.send("Pause");
      });
      console.log("Pause Sent from VRCRC!");
      break;
    case "/VRCRC/PreviousTrack":
      wss.clients.forEach((client) =>{
        client.send("Back");
      });
      console.log("Back Sent from VRCRC!");
      break;
    case "/VRCRC/RestartTrack":
      wss.clients.forEach((client) =>{
        client.send("Restart");
      });
      console.log("Restart Sent from VRCRC!");
      break;
    case "/VRCRC/YouTubeURL":
      wss.clients.forEach((client) =>{
        client.send("URL:" + msg.args[0].toString());
      });
      console.log(msg.args[0].toString() +" sent from VRCRC!")
      break;
    default:
      //The discs each have a number in their name. This number is used to identify what disc has been inserted so the HTML can tell what track to play.
      if(msg.address.includes("music_disc") && msg.args[0] == true){
        const tempAddress = msg.address.replace("/avatar/parameters/", "");
        wss.clients.forEach((client) =>{
          client.send(tempAddress);
        });
        console.log("Disc Sent!");
      }
      break;
  }
});

//This is executed
clientPort.open();