using System;
namespace ritchell.library.infrastructure.Hardware
{
    public interface IRFIDReader : IDisposable
    {
        event EventHandler<string> TagRead;
        void StartReader();
        void StopReader();

        void RegisterListener(ITagListener listener);

        void SetListener(ITagListener listener);

        void ClearListeners();
    }
}
