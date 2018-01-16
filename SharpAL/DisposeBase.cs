using System;
using System.Diagnostics;

namespace SharpAL {
    public abstract class DisposeBase : IDisposable {

        [DebuggerStepThrough]
        ~DisposeBase() {
            if (IsDisposed) {
                return;
            }

            Dispose(false);
            IsDisposed = true;

            Disposed?.Invoke(this, EventArgs.Empty);
        }

        [DebuggerStepThrough]
        public void Dispose() {
            if (IsDisposed) {
                return;
            }

            Dispose(true);
            IsDisposed = true;

            Disposed?.Invoke(this, EventArgs.Empty);

            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }

        public event EventHandler<EventArgs> Disposed;

        protected abstract void Dispose(bool disposing);

    }
}
