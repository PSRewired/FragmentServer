using System;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Crypto;

public class CryptoHandler
{
    public BlowfishProvider ClientCipher { get; set; } = new BlowfishProvider();
    public BlowfishProvider ServerCipher { get; internal set; } = new BlowfishProvider();

    public bool TryEncrypt(byte[] data, out byte[] encrypted)
    {
        if (ServerCipher == null)
        {
            encrypted = data;
            return false;
        }

        encrypted = ServerCipher.Encrypt(data);

        return true;
    }

    public bool TryDecrypt(byte[] encrypted, out byte[] decrypted)
    {
        decrypted = ClientCipher.Decrypt(encrypted);

        var receivedChecksum = BinaryPrimitives.ReadUInt16BigEndian(decrypted.AsSpan()[..2]);
        var decryptedChecksum = BlowfishProvider.Checksum(decrypted[2..]);

        decrypted = decrypted[2..];

        return receivedChecksum == decryptedChecksum;
    }
}
