using System.Net.Sockets;

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
    
    private bool _isDisposed=false;

    
    // TODO: make player Entity creating 
    public PlayerEntity PlayerEntity;

    public Client(string ip,int port)
    {
        this.Ip = ip;
        this.Port = port;
    }
    
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
        }
        catch (Exception e) // On connection error
        {
            OnConnectionError.Invoke(this);
            Console.WriteLine(e.Message);
            throw;
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
    public async Task AsyncFirstPackageAsyncSend()
    {
        
    }
}