using System;
using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL {
    public sealed class AudioSource : AudioObject {

        public AudioSource([NotNull] AudioContext context) {
            Context = context;
            Alc.MakeContextCurrent(context.NativeContext);
            _source = AL.GenSource();
        }

        public AudioContext Context { get; }

        public bool IsLooped {
            get {
                AL.GetSource(NativeSource, ALSourceb.Looping, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourceb.Looping, value);
        }

        public TimeSpan CurrentTime {
            get {
                AL.GetSource(NativeSource, ALSourcef.SecOffset, out var value);
                return TimeSpan.FromSeconds(value);
            }
            set {
                if (value < TimeSpan.Zero) {
                    value = TimeSpan.Zero;
                }

                AL.Source(NativeSource, ALSourcef.SecOffset, (float)value.TotalSeconds);
            }
        }

        public float Volume {
            get {
                AL.GetSource(NativeSource, ALSourcef.Gain, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourcef.Gain, value);
        }

        public float Pitch {
            get {
                AL.GetSource(NativeSource, ALSourcef.Pitch, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourcef.Pitch, value);
        }

        public void Play() {
            IsLooped = false;
            PlayDirect();
        }

        public void PlayDirect() {
            AL.SourcePlay(NativeSource);
        }

        public void PlayLooped() {
            IsLooped = true;
            PlayDirect();
        }

        public void Pause() {
            AL.SourcePause(NativeSource);
        }

        public void Stop() {
            AL.SourceStop(NativeSource);
        }

        public void QueueBuffer([NotNull] AudioBuffer buffer) {
            var s = NativeSource;
            var b = buffer.NativeBuffer;
            AL.SourceQueueBuffer(s, b);
        }

        public int UnqueueBuffer() {
            var bid = AL.SourceUnqueueBuffer(NativeSource);

            return bid;
        }

        public int GetCurrentBuffer() {
            AL.GetSource(NativeSource, ALGetSourcei.Buffer, out var bid);

            return bid;
        }

        public int ByteOffset {
            get {
                AL.GetSource(NativeSource, ALGetSourcei.ByteOffset, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourcei.ByteOffset, value);
        }

        public int SampleOffset {
            get {
                AL.GetSource(NativeSource, ALGetSourcei.SampleOffset, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourcei.SampleOffset, value);
        }

        public int BuffersProcessed {
            get {
                AL.GetSource(NativeSource, ALGetSourcei.BuffersProcessed, out var value);
                return value;
            }
        }

        public int BuffersQueued {
            get {
                AL.GetSource(NativeSource, ALGetSourcei.BuffersQueued, out var value);
                return value;
            }
        }

        public (float X, float Y, float Z) GetPosition() {
            AL.GetSource(NativeSource, ALSource3f.Position, out var x, out var y, out var z);
            return (x, y, z);
        }

        public void SetPosition(float x, float y, float z) {
            AL.Source(NativeSource, ALSource3f.Position, x, y, z);
        }

        public (float X, float Y, float Z) GetVelocity() {
            AL.GetSource(NativeSource, ALSource3f.Velocity, out var x, out var y, out var z);
            return (x, y, z);
        }

        public void SetVelocity(float x, float y, float z) {
            AL.Source(NativeSource, ALSource3f.Velocity, x, y, z);
        }

        public (float X, float Y, float Z) GetDirection() {
            AL.GetSource(NativeSource, ALSource3f.Direction, out var x, out var y, out var z);
            return (x, y, z);
        }

        public void SetDirection(float x, float y, float z) {
            AL.Source(NativeSource, ALSource3f.Direction, x, y, z);
        }

        public bool IsSourceRelative {
            get {
                AL.GetSource(NativeSource, ALSourceb.SourceRelative, out var value);
                return value;
            }
            set => AL.Source(NativeSource, ALSourceb.SourceRelative, value);
        }

        public void Rewind() {
            AL.SourceRewind(NativeSource);
        }

        public ALSourceState State {
            get {
                if (NativeSource == 0) {
                    throw new InvalidOperationException();
                }

                AL.GetSource(_source, ALGetSourcei.SourceState, out var sourceState);
                return (ALSourceState)sourceState;
            }
        }

        public ALSourceType SourceType {
            get {
                if (NativeSource == 0) {
                    throw new InvalidOperationException();
                }

                AL.GetSource(NativeSource, ALGetSourcei.SourceType, out var sourceType);
                return (ALSourceType)sourceType;
            }
        }

        internal int NativeSource => _source;

        protected override void Dispose(bool disposing) {
            AL.DeleteSource(_source);
            _source = 0;
        }

        private int _source;

    }
}
