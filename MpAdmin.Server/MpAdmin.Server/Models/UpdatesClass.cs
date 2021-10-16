
public class Rootobject
{
    public int update_id { get; set; }
    public Message message { get; set; }
}

public class Message
{
    public int message_id { get; set; }
    public From from { get; set; }
    public Chat chat { get; set; }
    public int date { get; set; }
    public string text { get; set; }
    public Entity[] entities { get; set; }
}

public class From
{
    public long id { get; set; }
    public bool is_bot { get; set; }
    public string first_name { get; set; }
    public string language_code { get; set; }
}

public class Chat
{
    public long id { get; set; }
    public string first_name { get; set; }
    public string type { get; set; }
}

public class Entity
{
    public int offset { get; set; }
    public int length { get; set; }
    public string type { get; set; }
}
