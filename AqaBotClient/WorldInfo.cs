namespace AqaBotClient;


// TODO: Make WorldInfo class
public class WorldInfo
{
    public delegate void PlayerConnectionHandler();

    public event PlayerConnectionHandler OnPlayerConnect = () =>
    {
        
    };
    
    
    //TODO: Add more properties
    
    public int PlayerCount; // Player count on server
    public int ServerMaxSize; // Max player count on server
    public List<Player> Players = new List<Player>();
    
    //TODO: Init WorldInfo
    
    // Инитиализирует класс WorldInfo 
    // Из него можно получить доступ к игрокам и информации о мире/сервере
    public WorldInfo() 
    {
        
    }
    
    
}