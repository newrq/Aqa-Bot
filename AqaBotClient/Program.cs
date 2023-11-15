namespace AqaBotClient;


abstract class Program
{
    private static  Client AqaBot = new Client("26.17.199.154",1111); // Testing IP
    public static async Task Main(string[] args)
    {
        await AqaBot.AsyncConnect();
        while(AqaBot._client.Connected)
        {
            await Task.Delay(5000);
        }

        Console.WriteLine("AQA BOT DISCONNECTED");
        
    }
    
}


