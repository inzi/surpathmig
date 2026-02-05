using System.Threading.Tasks;
using inzibackend.Views;
using Xamarin.Forms;

namespace inzibackend.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
