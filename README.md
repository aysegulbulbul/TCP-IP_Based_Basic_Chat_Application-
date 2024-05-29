# TCP-IP_Based_Basic_Chat_Application 
 It was done using Windows form with C#.


**Purpose of the Project:**

The purpose of this project is to develop a TCP/IP based chat application. The app allows users to message each other and send files. Users perform these operations using the TCP/IP protocol.



**Working Logic of the Application:**

The application can operate as a server and client. The server side listens on a specific port and initiates communication when a client connects. The client side connects to the server and communicates. Messages and files are sent and received over TCP.



**Project:**

I created this Chat program using C# language with Windows Form. In order for the program to work interactively, I opened a second identical form screen and enabled them to connect and message each other.

---

**Server Side:**

• First, I launched the application that will work as the server on the first form screen. When the server is started, it starts listening for incoming connections on the specified port number.

• The client on the second form screen uses the server's IP address and port number to connect to the server.

• The client connecting to the server can perform message sending and receiving operations.



**Client Side:**

• Secondly, I started the application that will run as a client on the second form screen.

• When the client is started, it connects to the server using the server's IP address and the specified port number, and the client can send and receive messages.


---

**Connection and Message Transmission:**

• Communication between the server and the client is provided through TCP/IP sockets.

• The server starts listening on the specified port number using the TcpListener class and accepts incoming clients. The client connects and communicates with the server using the TcpClient class.

• Messages sent by the client or server are transferred over TCP/IP using the StreamWriter and StreamReader classes.

• When messages are received or sent, the Invoke method is used to update the user interface.



## Summary:

When the server starts, it starts listening for connections on the specified port number. The client connects to the server using the server's IP address and the specified port number. Once the connection is successfully established, users can message each other. When messages are received or sent, the user interface updates and displays the messages
