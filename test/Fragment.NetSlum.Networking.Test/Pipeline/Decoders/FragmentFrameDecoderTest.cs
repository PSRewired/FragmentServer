using System.Collections.Generic;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Pipeline.Decoders;
using Xunit;

namespace Fragment.NetSlum.Networking.Test.Pipeline.Decoders;

public class FragmentFrameDecoderTest
{
    [Fact]
    public void DecoderCanSuccessfullyParseDataTypePackets()
    {
        var clientKey = "AFA8BB3A0E88107AC4CAE3B689EEE85D".StringToByteArray();
        var data = "00120030385DE8434EDBD09DA52449CDF47FD6B2".StringToByteArray();

        var crypto = new BlowfishProvider(clientKey);
        crypto.Initialize();

        var cryptoHandler = new CryptoHandler();
        cryptoHandler.ClientCipher = crypto;

        var decoder = new FragmentFrameDecoder(cryptoHandler);

        var decodedMessages = new List<FragmentMessage>();
        decoder.Decode(data, decodedMessages);

        Assert.Single(decodedMessages);
    }
}
