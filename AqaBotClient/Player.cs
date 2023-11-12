namespace AqaBotClient;

//TODO: Make this
// Fix the abstract Player class
// Make Player and SelfPlayer (Client Player)
public abstract class Player
{
    private string Name { get; set; }
    private string Userid { get; set; } // Or etc else

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
    
    public WorldInfo GetWorldInfo()
    {
        return new WorldInfo();
    }
    
    
}