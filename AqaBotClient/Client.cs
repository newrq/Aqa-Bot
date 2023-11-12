// MINECRAFT VER 1.20.2
// Protocol version currently 764 in Minecraft 1.20.2

using System.Net.Sockets;
using System.Text;

namespace AqaBotClient;

public class Client : IDisposable
{
    //TODO: make events 
    #region EventsAndEtc

    public delegate  void MessageEventHandler(object sender, Message message); // Message event handler
        
    public event MessageEventHandler OnMessageRecived = (sender,msg) =>
    {
        Console.WriteLine("Message received by " + msg.GetAuthor() + " ---> " + msg.GetContent());
    }; // When message recived
    
    
    public event MessageEventHandler OnMessageError; // When message send error


    public delegate void ConnectionEventHandler(object sender);
    public event ConnectionEventHandler OnConnectionSucces = (sender) => {
        Console.WriteLine("Connection success!");
    }; // When connection success
    public event ConnectionEventHandler OnConnectionError = (sender) => throw new Exception("Connection error..."); // throw when connection error

    #endregion

    #region server

    

    
    public string Name = "Aqa Bot";
    public string Ip;
    public int Port;

    #endregion
    
    private TcpClient _client = new TcpClient();
    private NetworkStream _networkStream;
    private bool _isDisposed=false;
    
    // TODO: make player Entity creating 
    public SelfPlayer PlayerEntity;
    public WorldInfo WorldInfo;
    
    
    // Initializing the Client class
    public Client(string ip,int port)
    {
        this.Ip = ip;
        this.Port = port;
    }
    
    // Initializing the Client class
    public Client(string ip, string name)
    {
        this.Ip = ip;
        this.Name = name;
    }
    
    //TODO:Make Connecting to server
    public async Task AsyncConnect() 
    {
        try
        {
            await _client.ConnectAsync(Ip,Port); // Connecting to server 1 attempt
            _networkStream = _client.GetStream();

        }
        catch (Exception e) // On connection error
        {
            OnConnectionError.Invoke(this);
            Console.WriteLine(e.Message);
            throw;
        }

        if (_client.Connected)
        {
            try
            {
                Console.WriteLine("Starting handshake");
                await AsyncHandShakeSend();
                Console.WriteLine("Starting login");
                await AsyncLoginStart();
                Console.WriteLine("Play package send");
                await PlayPackageSend();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
    
    private void Disposing()    
    {
        if (_isDisposed)
        {
            Console.WriteLine("Client disposing...");
            return;
            
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
        _isDisposed = true;
        Disposing();
    }
    
    
    //TODO:Make first package send
    private async Task AsyncHandShakeSend()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            //Package id
            WriteVarInt(memoryStream,0x00);
            
            
            //Protocol 
            WriteVarInt(memoryStream,764);
            
            // Server ip
            byte[] serverAddressBytes = Encoding.UTF8.GetBytes(Ip);
            WriteVarInt(memoryStream, serverAddressBytes.Length);
            memoryStream.Write(serverAddressBytes, 0, serverAddressBytes.Length);

            
            //Server Port
            byte[] portBytes = BitConverter.GetBytes((ushort)Port);
            Array.Reverse(portBytes);
            memoryStream.Write(portBytes, 0, portBytes.Length);
            
            // Status (1 - login)
            WriteVarInt(memoryStream, 2);
            
            // Sending data on server
            byte[] packetData = memoryStream.ToArray();
            await _networkStream.WriteAsync(packetData, 0, packetData.Length);
        }

        try
        {
            byte[] buffer = new byte[9000];
            int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
            Console.WriteLine(Encoding.UTF8.GetString(buffer,0,bytesRead));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n\n CANT READ");
            throw;
        }
        
    }
    
    private async Task AsyncLoginStart()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
           WriteVarInt(memoryStream,0x00); // Login package ID

           byte[] nicknameBytes = Encoding.UTF8.GetBytes(Name); // nickname bytes
           WriteVarInt(memoryStream,nicknameBytes.Length);

           byte[] playerUIID = Encoding.UTF8.GetBytes(Name); // uiid bytes
           WriteVarInt(memoryStream,playerUIID.Length);

           byte[] packetData = memoryStream.ToArray();
           await _networkStream.WriteAsync(packetData, 0, packetData.Length);
        }
        //
        // byte[] buffer = new byte[3000];
        // int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
        // Console.WriteLine(Encoding.UTF8.GetString(buffer,0,bytesRead));
    }

    
    //TODO: PLAYER PACKAGE SEND ERROR
    private async Task PlayPackageSend()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
           WriteVarInt(memoryStream,0x00); // Play package ID
           byte[] playPackage = memoryStream.ToArray();
           await _networkStream.WriteAsync(playPackage, 0, playPackage.Length);
        }
        byte[] buffer = new byte[3000];
        int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
        Console.WriteLine(Encoding.UTF8.GetString(buffer,0,bytesRead));
    }
    
    
    static void WriteVarInt(Stream stream, int value)
    {
        do
        {
            byte temp = (byte)(value & 0b01111111);
            value >>= 7; 
            if (value != 0)
            {
                temp |= 0b10000000;
            }
            stream.WriteByte(temp);
        } while (value != 0); 
    }
}