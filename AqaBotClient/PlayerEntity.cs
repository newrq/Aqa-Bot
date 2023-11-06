namespace AqaBotClient;



public class PlayerEntity : Player , IWorldEntity
{
    public PlayerEntity(string nickname, string userid) : base(nickname, userid)
    {
        
    }

    public void Jump()
    {
        throw new NotImplementedException();
    }

    public void Eat()
    {
        throw new NotImplementedException();
    }

    void IWorldEntity.GoTo()
    {
        throw new NotImplementedException();
    }

    public void StopMove()
    {
        throw new NotImplementedException();
    }
    
    //TODO: Add more functions

}