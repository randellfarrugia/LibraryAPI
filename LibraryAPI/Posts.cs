using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace LibraryAPI
{
    public class Posts
    {
        IHttpContextAccessor context;
        public Posts(IHttpContextAccessor _httpContext)
        {
            context = _httpContext;
        }
        public void RedirectWithPost(string url, Dictionary<string, string> data)
        {
            // Create a form element
            var form = new TagBuilder("form");
            form.Attributes["id"] = "postform";
            form.Attributes["method"] = "post";
            form.Attributes["action"] = url;

            // Add form data as hidden input elements
            foreach (var item in data)
            {
                var input = new TagBuilder("input");
                input.Attributes["type"] = "hidden";
                input.Attributes["name"] = item.Key;
                input.Attributes["value"] = item.Value;
                form.InnerHtml.AppendHtml(input);
            }

            // Render the form as a string
            var formHtml = form.ToString();

            // Write the form to the response body
            context?.HttpContext?.Response.WriteAsync(String.IsNullOrEmpty(formHtml) ? "" : formHtml);

            // Submit the form using JavaScript
            context?.HttpContext?.Response.WriteAsync("<script language='javascript'>document.getElementById('postform').submit();</script>");
        }

        public async Task<string> MakePostRequestAsync(string postUrl, string data = "", string contentType = "application/json", HttpMethod? _method = null, string BearerToken="")
        {

            var httpMethod = _method == null ? HttpMethod.Post : _method;

            using (var client = new HttpClient())
            {
                // Set the content and content type for the request
                var requestContent = new StringContent(data, Encoding.UTF8, contentType);
                if (!String.IsNullOrEmpty(BearerToken))
                {
                    requestContent.Headers.Add("Authorization", $"Bearer {BearerToken}");
                }

                // Send the request using the specified HTTP method
                HttpResponseMessage response;
                if (httpMethod == HttpMethod.Post)
                {
                    response = await client.PostAsync(postUrl, requestContent);
                }
                else
                {
                    response = await client.GetAsync(postUrl);
                }

                // Return the response as a string
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

}
