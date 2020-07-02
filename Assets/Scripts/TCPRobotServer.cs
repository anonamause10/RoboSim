
using System;
using System.IO;
using System.Collections; 
using System.Collections.Generic; 
using System.Reflection;
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading; 
using UnityEngine;  

public class TCPRobotServer : MonoBehaviour {  	
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient; 
    /// <summary> 	
	/// Robot 	
	/// </summary> 
    private RoboController roboController;
	#endregion 	
		
	// Use this for initialization
	void Start () { 		
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
        roboController = GetComponent<RoboController>();
	}  	
	
	// Update is called once per frame
	void Update () { 		
		sendDataMessage();
	}  	
	
	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 		
										
						do{ 						
							length = stream.Read(bytes, 0, bytes.Length);
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData); 							
							//Debug.Log("client message received as: " + clientMessage);
							string[] messages = clientMessage.Split('|');
							foreach (string m in messages)
							{
								callCommand(m);
							}
						}while (length > 0);
						
					} 				
				} 			
			} 		
		} 		
		catch (IOException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 	
		}     
	}  	
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	private void sendMessage(string message) { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				string serverMessage = message; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				//Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (Exception socketException) { 
			if(socketException is ObjectDisposedException){
				enabled = false;
			}      
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 

	private void sendDataMessage(){
		string str = "data ";
		str += roboController.getMaxForwardVel() + " ";
		str += roboController.getMaxSideVel() + " ";
		str += roboController.getMaxTurnVel() + " ";
		str += roboController.getForwardVel() + " ";
		str += roboController.getSideVel() + " ";
		str += roboController.getTurnVel() + " ";
		str += roboController.getTrueVelocityStr() + " ";
		str += roboController.getTrueAngularVelocity() + " ";
		str += roboController.getGyroAngle() + " ";
		str += roboController.getForwardDist() + " ";
		str += roboController.getBackDist() + " ";
		str += roboController.getLeftDist() + " ";
		str += roboController.getRightDist() + " ";
		sendMessage(str);

	}

    private void callCommand(string message){
        string[] args = message.Split(' ');

		if(args[0] == "connectPlz"){
			sendMessage("ok connected");
			return;
		}

		if(args[0] == "stop"){
			print("done");
			enabled = false;
			return;
		}

		if(args[0]!="robocommand"){
			return;
		}

		switch (args[1])
		{
			case "setMaxForwardVel":
				roboController.setMaxForwardVel(float.Parse(args[2]));
				break;
			case "setMaxSideVel":
				roboController.setMaxSideVel(float.Parse(args[2]));
				break;
			case "setMaxTurnVel":
				roboController.setMaxTurnVel(float.Parse(args[2]));
				break;
			case "setForwardVel":
				roboController.setForwardVel(float.Parse(args[2]));
				break;
			case "setSideVel":
				roboController.setSideVel(float.Parse(args[2]));
				break;
			case "setTurnVel":
				roboController.setTurnVel(float.Parse(args[2]));
				break;
			default:
				break;
		}
		//sendMessage(" command executed");
        
    }
}