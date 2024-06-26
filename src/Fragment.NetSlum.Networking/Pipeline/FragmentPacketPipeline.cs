using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.Networking.Messaging;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Pipeline.Decoders;
using Fragment.NetSlum.Networking.Pipeline.Encoders;

namespace Fragment.NetSlum.Networking.Pipeline;

/// <summary>
/// I'm sure this could use some additional optimizations, but this pipeline handles decoding, handling, and encoding
/// fragment requests
/// </summary>
/// <typeparam name="TSession"></typeparam>
public class FragmentPacketPipeline<TSession> : IDisposable where TSession : IScopeable
{
    private readonly IEnumerable<IPacketDecoder> _decoders;
    private readonly IPacketHandler<FragmentMessage> _fragmentPacketHandler;
    private readonly IEnumerable<IMessageEncoder> _encoders;

    private readonly IMemoryOwner<byte> _inBufOwner;
    private readonly Memory<byte> _inBuf;
    private int _inBufLength;

    private readonly MemoryStream _outBuf = new();
    private readonly List<FragmentMessage> _decodedObjects = new();
    private readonly List<FragmentMessage> _responseObjects = new();

    public FragmentPacketPipeline(IEnumerable<IPacketDecoder> decoders,
        IPacketHandler<FragmentMessage> fragmentPacketHandler, IEnumerable<IMessageEncoder> encoders)
    {
        _decoders = decoders;
        _fragmentPacketHandler = fragmentPacketHandler;
        _encoders = encoders;
        _inBufOwner = MemoryPool<byte>.Shared.Rent(8192);
        _inBuf = _inBufOwner.Memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public async Task<ReadOnlyMemory<byte>> Handle(TSession session, Memory<byte> data, CancellationToken cancellationToken)
    {
        if (_inBufLength + data.Length > _inBuf.Length)
        {
            throw new OverflowException(
                $"Receive buffer exceeded maximum size. Length: {_inBufLength} Max: {_inBuf.Length}");
        }

        data.CopyTo(_inBuf[_inBufLength..]);
        _inBufLength += data.Length;

        try
        {
            // Continuously run the decoders until we've exhausted the buffer content, or no other data can be read
            int bytesRead = 0;
            do
            {
                // Check that cancellation token has not been cancelled before and after processing
                cancellationToken.ThrowIfCancellationRequested();

                // Run decoders
                foreach (var decoder in _decoders)
                {
                    // Each decoder is able to read from the buffer so we need to make sure that we are always
                    // trimming the stream on each decoder call to account for the changes the previous one made
                    bytesRead = decoder.Decode(_inBuf[.._inBufLength], _decodedObjects);
                    _inBufLength -= bytesRead;

                    if (bytesRead <= 0)
                    {
                        break;
                    }

                    _inBuf[bytesRead..].CopyTo(_inBuf);
                }
            } while (bytesRead > 0 && _inBufLength > 0);

            // Run response handler for each decoded message
            for (var i = 0; i < _decodedObjects.Count; i++)
            {
                var request = _decodedObjects[i];
                var resp = await _fragmentPacketHandler.CreateResponse(session, request);

                _responseObjects.AddRange(resp);
            }

            return Encode(_responseObjects, cancellationToken);
        }
        catch (ObjectDisposedException)
        {
            // If the session was disposed while this is still running its course, the services may have been destroyed
            return Array.Empty<byte>();
        }
        finally
        {
            // Ensure that no matter what happens, we clear out the buffers in case the caller allows this to
            // try running again
            _decodedObjects.Clear();
            _responseObjects.Clear();
        }
    }

    public ReadOnlyMemory<byte> Encode(List<FragmentMessage> messages, CancellationToken cancellationToken)
    {
        try
        {
            // Run message encoders
            foreach (var encoder in _encoders)
            {
                encoder.Encode(messages, _outBuf);
            }

            // Flush encoded messages to the outgoing buffer
            foreach (var rsp in messages)
            {
                _outBuf.Write(rsp.ToArray());
            }

            cancellationToken.ThrowIfCancellationRequested();
            return _outBuf.ToArray();
        }
        finally
        {
            // SetLength is a fairly expensive call, so only run it if data was sent to the output buffer
            if (_outBuf.Length > 0)
            {
                _outBuf.SetLength(0);
            }
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _inBufOwner.Dispose();
    }
}
