namespace AqaBotClient;


abstract class Program
{
    private static  Client AqaBot = new Client("localhost",1111); // Testing IP
    public static async Task Main(string[] args)
    {
        await AqaBot.AsyncConnect();
        await Task.Delay(-1);
        
    }
    
}


