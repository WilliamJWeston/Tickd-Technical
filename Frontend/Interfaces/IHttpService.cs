using Ardalis.Result;
using Microsoft.AspNetCore.Components.Forms;

namespace Frontend.Interfaces
{
    public interface IHttpService
    {
        Task<Result> PostFileAsync(string path, IBrowserFile file);
    }
}
