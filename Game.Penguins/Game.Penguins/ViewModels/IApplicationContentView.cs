using System.Threading.Tasks;

namespace Game.Penguins.ViewModels
{
    interface IApplicationContentView
    {
        string Name { get; }

        string PreviousButtonContent { get; }
        
        string NextButtonContent { get; }

        bool HasPreviousView { get; }

        bool HasNextView { get; }

        IApplicationContentView GetPreviousView();

        IApplicationContentView GetNextView();
    }
}
