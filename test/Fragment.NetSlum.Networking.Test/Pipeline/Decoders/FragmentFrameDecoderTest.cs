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
        var clientKey = "F26C30B852E4415E1D8B719B3803ECC3".StringToByteArray();
        var data = "0012003057B9B5EA2691E3E229359DC28AAF0CEB".StringToByteArray();

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
