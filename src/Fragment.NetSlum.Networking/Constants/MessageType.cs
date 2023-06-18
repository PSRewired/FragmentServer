namespace Fragment.NetSlum.Networking.Constants;

public enum MessageType
{
    None = 0x00,
    PingRequest = 0x02,

    Data = 0x30,
    KeyExchangeRequest = 0x34,
    KeyExchangeResponse = 0x35,
    KeyExchangeAcknowledgmentRequest = 0x36,
}
