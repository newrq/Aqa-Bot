namespace AqaBotClient;

public class Client : IDisposable
{
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
    
    public string Name = "Aqa Bot";
    public string Ip;
    public int? Port;

    private bool _isDisposed=false;

    
    // TODO: make player Entity creating 
    public PlayerEntity PlayerEntity;

    public Client(string ip,int port)
    {
        this.Ip = ip;
        this.Port = port;
    }
    
    public Client(string ip)
    {
        this.Ip = ip;
    }
    
    public Client(string ip, string name)
    {
        this.Ip = ip;
        this.Name = name;
    }
    
    
    public Client(string ip,int port,string name)
    {
        this.Ip = ip;
        this.Port = port;
        this.Name = name;
    }

    //TODO:Make Connecting to server
    public async Task AsyncConnect()
    {
        
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
    public void FirstPackageAsyncSend()
    {
        
    }
}