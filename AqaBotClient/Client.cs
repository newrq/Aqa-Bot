// MINECRAFT VER 1.20.2
// Protocol version currently 764 in Minecraft 1.20.2

using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading.Channels;

namespace AqaBotClient;

public class Client : IDisposable
{
    //TODO: make events 

    #region EventsAndEtc

    public delegate void MessageEventHandler(object sender, Message message); // Message event handler

    public event MessageEventHandler OnMessageRecived = (sender, msg) =>
    {
        Console.WriteLine("Message received by " + msg.GetAuthor() + " ---> " + msg.GetContent());
    }; // When message recived


    public event MessageEventHandler OnMessageError; // When message send error


    public delegate void ConnectionEventHandler(object sender);

    public event ConnectionEventHandler
        OnConnectionSucces = (sender) => { Console.WriteLine("Connection success!"); }; // When connection success

    public event ConnectionEventHandler
        OnConnectionError = (sender) => throw new Exception("Connection error..."); // throw when connection error

    #endregion

    #region server




    public string Name = "AqaBot";
    public string Ip;
    public int Port;

    #endregion

    public TcpClient _client = new TcpClient();
    private NetworkStream _networkStream;
    private bool _isDisposed = false;

    // TODO: make player Entity creating 
    public SelfPlayer PlayerEntity;
    public WorldInfo WorldInfo;


    // Initializing the Client class
    public Client(string ip, int port)
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
            await _client.ConnectAsync(Ip, Port); // Connecting to server 1 attempt
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
                await Task.Delay(5000);
                Console.WriteLine("Switch package send");   
                await LoginAcknowledgedPackage();
                Console.WriteLine("Sending config package");
                await ConfigurationPackageSend();
                await Task.Delay(10000);
                Console.WriteLine("Sending configurationConfirm package");
                await FinishConfigurationPackageSend();
                Console.WriteLine("Spawning player");
                await SpawnPlayer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
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
            await Task.Delay(1000);
            //Package id
            WriteVarInt(memoryStream, 0x00);

            await Task.Delay(1000);
            //Protocol 
            WriteVarInt(memoryStream, 764);

            await Task.Delay(1000);
            // Server ip
            byte[] serverAddressBytes = Encoding.UTF8.GetBytes(Ip);
            WriteVarInt(memoryStream, serverAddressBytes.Length);
            await Task.Delay(1000);
            memoryStream.Write(serverAddressBytes, 0, serverAddressBytes.Length);
            await Task.Delay(1000);
            //Server Port
            byte[] portBytes = BitConverter.GetBytes((ushort)Port);
            Array.Reverse(portBytes);
            await memoryStream.WriteAsync(portBytes, 0, portBytes.Length);

            // Status (2 - login)
            WriteVarInt(memoryStream, 2);
            await Task.Delay(1000);
            
            // Sending data on server
            byte[] packetData = memoryStream.ToArray();
            await Task.Delay(1000);
            await _networkStream.WriteAsync(packetData, 0, packetData.Length);
        }
        await ReadRecivedInfo(_networkStream,_client);

    }

    private async Task AsyncLoginStart()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteVarInt(memoryStream, 0x00); // Login package ID
            await Task.Delay(1000);
            byte[] nicknameBytes = Encoding.UTF8.GetBytes(Name); // nickname bytes
            WriteVarInt(memoryStream, nicknameBytes.Length);
            await Task.Delay(1000);
            Guid playerUIID = Guid.NewGuid();
            byte[] playerUIIDbytes = playerUIID.ToByteArray(); // uiid bytes
            WriteVarInt(memoryStream, playerUIIDbytes.Length);
            await Task.Delay(1000);
            byte[] packetData = memoryStream.ToArray();
            await _networkStream.WriteAsync(packetData, 0, packetData.Length);
            
        }
        await ReadRecivedInfo(_networkStream,_client);
    }


    //TODO: switch PACKAGE SEND ERROR
    private async Task LoginAcknowledgedPackage() // This packet switches the connection state to configuration.
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteVarInt(memoryStream, 0x03); // Switch package ID
            await Task.Delay(1000);

            byte[] playPackage = memoryStream.ToArray();
            await _networkStream.WriteAsync(playPackage, 0, playPackage.Length);
        }
        await ReadRecivedInfo(_networkStream,_client);
    }
    
    private async Task ConfigurationPackageSend()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteVarInt(memoryStream, 0x00); // Switch package ID
            await Task.Delay(1000);

            WriteVarInt(memoryStream, Encoding.UTF8.GetBytes("en_GB").Length); // Locale
            await Task.Delay(500);
            WriteVarInt(memoryStream,16); // View distance
            await Task.Delay(500);
            WriteVarInt(memoryStream,0); // 	0: enabled, 1: commands only, 2: hidden
            await Task.Delay(500);
            WriteVarInt(memoryStream,0x01); // “Colors” multiplayer setting. Can the chat be colored? 0x01 - true 0x00 - false
            await Task.Delay(500);
            WriteVarInt(memoryStream,0x40); // Displayed Skin Parts		Bit mask, see below.
            
            /*Bit 0 (0x01): Cape enabled
             *Bit 1 (0x02): Jacket enabled
             *Bit 2 (0x04): Left Sleeve enabled
             *Bit 3 (0x08): Right Sleeve enabled
             *Bit 4 (0x10): Left Pants Leg enabled
             *Bit 5 (0x20): Right Pants Leg enabled
             *Bit 6 (0x40): Hat enabled   
             */
            
            await Task.Delay(500);
            WriteVarInt(memoryStream,1); // Main Hand .... 0: Left, 1: Right.
            await Task.Delay(500);
            WriteVarInt(memoryStream,0x00); // Enable text filtering (BOOL)
            await Task.Delay(500);
            WriteVarInt(memoryStream,0x01); // Allow server listings... Servers usually list online players, this option should let you not show up in that list.
            await Task.Delay(500);
            byte[] playPackage = memoryStream.ToArray();
            await _networkStream.WriteAsync(playPackage, 0, playPackage.Length);
        }
        await ReadRecivedInfo(_networkStream,_client);
    }

    private async Task FinishConfigurationPackageSend()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteVarInt(memoryStream, 0x02); // Switch package ID
            await Task.Delay(1000);
            byte[] playPackage = memoryStream.ToArray();
            await _networkStream.WriteAsync(playPackage, 0, playPackage.Length);
        }
        await ReadRecivedInfo(_networkStream,_client);
    }


    private async Task SpawnPlayer()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteVarInt(memoryStream,0x00);
            
            byte[] packetData = memoryStream.ToArray();
            await _networkStream.WriteAsync(packetData, 0, packetData.Length);
        }

        ReadRecivedInfo(_networkStream, _client);
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

    private static async Task ReadRecivedInfo(NetworkStream _networkStream,TcpClient _client)
    {
        if (_client.Connected)
        {
            await Task.Delay(3000);
            if (_networkStream.DataAvailable)
            {
                byte[] buffer = new byte[10000];
                int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            else
            {
                Console.WriteLine("No data");
            }
        }
        else
        {
            Console.WriteLine("Not connected");
        }
        
    }


}