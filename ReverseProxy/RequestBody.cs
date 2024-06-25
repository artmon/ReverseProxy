namespace ReverseProxy;

public class RequestBody
{
    public string id { get; set; }
    
    public string method { get; set; }
    
    public string jsonrpc { get; set; }
    
    public Params @params { get; set; }
}

public class Params
{
    public string address { get; set; }

    public long lt { get; set; }
}