using src.Entities;

namespace src.Features.Shared;

public class LinkHelper(LinkGenerator? linkGenerator)
{
    public Link CreateLink(
        HttpContext context,
        string actionName,
        string relation,
        string method,
        object? values = null)
    {
        return new Link
        {
            Href = linkGenerator?.GetPathByAction(context, action: actionName, values: values),
            Rel = relation,
            Method = method
        };
    }
}