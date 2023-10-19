namespace ReverseProxy;

public class RequestBody
{
    public string id { get; set; }
    
    public string method { get; set; }
    
    public string jsonrpc { get; set; }
    
    public object @params { get; set; }
}