using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Pipeline.Decoders;
using Fragment.NetSlum.Networking.Pipeline.Encoders;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Fragment.NetSlum.Networking.Test.Pipeline;

public class FragmentPacketPipelineTest
{
    [Fact]
    public void PipelineCanDecodeAndEncodeResponseObjects()
    {
        var cryptoProvider = new CryptoHandler();
        //var mockLogger = new Mock<ILogger<EncryptionEncoder>>();

        var encryptedMessage = "00320035E3725B557AA0F61D4385B46E2BEE37AC3ECB518FCD6F577941FF97A52DC489A454BAA7EE43DBE8DAC306BC9BC59ECB1B"
            .StringToByteArray();

        var messagesDecoded = new List<FragmentMessage>();
        var decoder = new FragmentFrameDecoder(cryptoProvider);
        var buffer = new MemoryStream();

        buffer.Write(encryptedMessage);
        decoder.Decode(buffer.ToArray(), messagesDecoded);

        var key = new byte[16];
        RandomNumberGenerator.Fill(key);

        var decryptedMessage = new FragmentMessage
        {
            MessageType = messagesDecoded[0].MessageType,
            Data = messagesDecoded[0].Data.ToArray(),
        };

        var logger = new NullLogger<EncryptionEncoder>();
        var encoder = new EncryptionEncoder(cryptoProvider, logger);

        encoder.Encode([decryptedMessage], new MemoryStream());

        buffer.SetLength(0);
        buffer.Write(decryptedMessage.ToArray());
        decoder.Decode(buffer.ToArray(), messagesDecoded);


        Assert.Equal(2, messagesDecoded.Count);

        Assert.Equal(messagesDecoded[0].Checksum, messagesDecoded[1].Checksum);
        Assert.Equal(messagesDecoded[0].Data.Length, messagesDecoded[1].Data.Length);

        for (var i = 0; i < messagesDecoded[0].Data.Length; i++)
        {
            Assert.Equal(messagesDecoded[0].Data.Span[i], messagesDecoded[1].Data.Span[i]);
        }
    }
}
