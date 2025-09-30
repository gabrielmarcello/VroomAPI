using Microsoft.AspNetCore.Mvc;
using VroomAPI.Models;

namespace VroomAPI.Helpers
{
    public static class HateoasHelper
    {
        public static void AddLink(this HateoasResource resource, string href, string rel, string method = "GET")
        {
            resource.Links.Add(new Link
            {
                Href = href,
                Rel = rel,
                Method = method
            });
        }

        public static string GetBaseUrl(HttpContext httpContext)
        {
            var request = httpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }

        public static void AddSelfLink(this HateoasResource resource, string baseUrl, string controller, int id)
        {
            resource.AddLink($"{baseUrl}/{controller}/{id}", "self");
        }

        public static void AddSelfLink(this HateoasResource resource, string fullUrl)
        {
            resource.AddLink(fullUrl, "self");
        }

        public static void AddCollectionLink(this HateoasResource resource, string baseUrl, string controller)
        {
            resource.AddLink($"{baseUrl}/{controller}", "collection");
        }

        public static void AddEditLink(this HateoasResource resource, string baseUrl, string controller, int id)
        {
            resource.AddLink($"{baseUrl}/{controller}/{id}", "edit", "PUT");
        }

        public static void AddDeleteLink(this HateoasResource resource, string baseUrl, string controller, int id)
        {
            resource.AddLink($"{baseUrl}/{controller}/{id}", "delete", "DELETE");
        }

        public static void AddCreateLink(this HateoasResource resource, string baseUrl, string controller)
        {
            resource.AddLink($"{baseUrl}/{controller}", "create", "POST");
        }

        public static void AddRelatedLink(this HateoasResource resource, string baseUrl, string controller, int id, string relation)
        {
            resource.AddLink($"{baseUrl}/{controller}/{id}", relation);
        }
    }
}