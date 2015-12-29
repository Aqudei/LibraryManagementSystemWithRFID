using System;
namespace ritchell.library.infrastructure.Hardware
{
    public interface IRFIDReader : IDisposable
    {
        event EventHandler<string> TagRead;
    }
}
