namespace AqaBotClient;



public class SelfPlayer : Player , IWorldEntity
{
    public SelfPlayer(string nickname, string userid) : base(nickname, userid)
    {
        
    }

    // Sends a jump command to the server for the client player
    public void Jump()
    {
        throw new NotImplementedException();
    }

    // Sends a eat command to the server for the client player (item is in the client's player's hand)
    public void Eat()
    {
        throw new NotImplementedException();
    }

    // Sends a go command to the server for the client player 
    public void GoTo()
    {
        throw new NotImplementedException();
    }

    public void StopMove()
    {
        throw new NotImplementedException();
    }
    
    public void SendChat(string message)
    {
        // NOT WORKING FUNCTION
        Chat.MessageSend(this,message);
    }
    
    //TODO: Add more functions

}