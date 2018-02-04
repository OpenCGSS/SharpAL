using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL {
    partial class AudioContext {

        private sealed class WatchThread {

            internal WatchThread([NotNull] AudioContext audioContext) {
                _audioContext = audioContext;
                _synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
            }

            internal void Start() {
                _shouldContinue = true;
                _thread = new Thread(ThreadProc) {
                    IsBackground = true
                };
                _thread.Start();
            }

            internal void Terminate() {
                _shouldContinue = false;
                _thread.Join();
            }

            private void ThreadProc() {
                var sources = _audioContext._createdSources;

                var states = new Dictionary<AudioSource, ALSourceState>();
                var newStates = new Dictionary<AudioSource, ALSourceState>();

                while (_shouldContinue) {
                    if (sources.Count > 0) {
                        foreach (var source in sources) {
                            var newState = source.State;

                            if (states.ContainsKey(source) && newState == ALSourceState.Stopped && newState != states[source]) {
                                RaisePlaybackStoppedFor(source);
                            }

                            newStates.Add(source, newState);
                        }

                        states.Clear();

                        foreach (var entry in newStates) {
                            states[entry.Key] = newStates[entry.Key];
                        }

                        newStates.Clear();
                    }

                    Thread.Sleep(10);
                }
            }

            private void RaisePlaybackStoppedFor([NotNull] AudioSource audioSource) {
                _synchronizationContext.Post(_ => audioSource.RaisePlaybackStopped(_audioContext, EventArgs.Empty), null);
            }

            private readonly AudioContext _audioContext;
            private readonly SynchronizationContext _synchronizationContext;
            private Thread _thread;
            private bool _shouldContinue;

        }

    }
}
