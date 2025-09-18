using src.Entities;

namespace src.Features.Shared;

public class LinkHelper(LinkGenerator? linkGenerator)
{
    public Link CreateLink(
        HttpContext context,
        string endpointName,
        string relation,
        string? method = null,
        object? values = null)
    {
        if (method is null)
        {
            return new Link
            {
                Href = linkGenerator?.GetUriByName(context, endpointName, values: values),
                Rel = relation,
            };
        }
        
        return new Link
        {
            Href = linkGenerator?.GetUriByName(context, endpointName, values: values),
            Rel = relation,
            Method = method
        };
    }
}