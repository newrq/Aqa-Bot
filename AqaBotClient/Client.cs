namespace AqaBotClient;

public class Client : IDisposable
{
    public string Name = "Aqa Bot";
    public string Ip;
    public int? Port;

    private bool _isDisposed=false;

    public Client(string ip,int port)
    {
        this.Ip = ip;
        this.Port = port;
    }
    
    
    public Client(string ip,int port,string name)
    {
        this.Ip = ip;
        this.Port = port;
        this.Name = name;
    }

    //TODO:Connect
    public void Connect()
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
}