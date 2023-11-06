namespace AqaBotClient;

public abstract class Player
{
    private string Name { get; set; }
    private string Userid { get; set; }



    // base player class
    protected Player(string nickname, string userid)
    {
        this.Name = nickname;
        this.Userid = userid;
    }
    
    
    // Get current user nickname
    public string GetName()
    {
        return Name;
    }

    
    // Get  current user id
    public string GetUserId()
    {
        return Userid;
    }

    
    //TODO: Sending message function
    public void SendChat(string message)
    {
        // NOT WORKING FUNCTION
        Chat.MessageSend(this,message);
    }

    public WorldInfo GetWorldInfo()
    {
        return new WorldInfo();
    }
    
    
}