using src.Entities;

namespace src.Features.Shared;

public class LinkHelper(LinkGenerator? linkGenerator)
{
    public Link CreateLink(
        HttpContext context,
        string endpointName,
        string relation,
        string method,
        object? values = null)
    {
        return new Link
        {
            Href = linkGenerator?.GetPathByName(context, endpointName: endpointName, values: values),
            Rel = relation,
            Method = method
        };
    }
}