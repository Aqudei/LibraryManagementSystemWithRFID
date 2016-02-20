using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ritchell.library.infrastructure.Hardware
{
    public abstract class RFIDReaderBase : IRFIDReader
    {
        public event EventHandler<string> TagRead;

        protected List<ITagListener> TagListeners;
        protected bool IsMonitoring = false;

        public RFIDReaderBase()
        {
            TagListeners = new List<ITagListener>();
        }

        public void ClearListeners()
        {
            TagListeners.Clear();
        }

        public void RegisterListener(ITagListener listener)
        {
            TagListeners.Add(listener);
        }

        public void SetListener(ITagListener listener)
        {
            ClearListeners();
            RegisterListener(listener);
        }

        public void StartReader()
        {
            OnReaderStarted();
            IsMonitoring = true;
        }

        public virtual void OnReaderStarted()
        {
            Debug.WriteLine("Reader Started");
        }

        public void StopReader()
        {
            OnReaderStopped();
            IsMonitoring = false;
        }

        public virtual void OnReaderStopped()
        {
            Debug.WriteLine("Reader Stopped");
        }

        public abstract void Dispose();

        protected void RaiseTagRead(string tag)
        {
            var handler = TagRead;
            if (handler != null)
                handler(this, tag);
        }
    }
}
