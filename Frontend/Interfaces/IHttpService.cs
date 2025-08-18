using Ardalis.Result;
using Microsoft.AspNetCore.Components.Forms;

namespace Frontend.Interfaces
{
    public interface IHttpService
    {
        Task<Result<T>> PostFileAsync<T>(string path, IBrowserFile file);
    }
}
