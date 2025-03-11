namespace RareNFTs.Web.Models;

public class ErrorMiddlewareViewModel
{

    public string Path { set; get; } = default!;
    public List<string> ListMessages { set; get; } = default!;

    public string IdEvent { set; get; } = default!;
}
