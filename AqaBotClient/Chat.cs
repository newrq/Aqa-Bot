namespace AqaBotClient;


// TODO: Make chat class  \\\ maybe delete it \\\
public class Chat
{
    
    // TODO: Not working, do
    public static void MessageSend(Player player, string message)
    {
        
    }
}


public class Message
{
    private string _author;
    private string _content;

    public Message(string author,string content)
    {
        _author = author;
        _content = content;
    }


    public string GetAuthor()
    {
        return _author;
    }
    
    public string GetContent()
    {
        return _content;
    }
}