# Fragment Binary Message Framing
___

## Endianness and Encoding
- Primitive types (int,short, etc) are represented in big-endian notation.
- Text is serialized using Shift-JIS character encoding

## Packet Frame
When fragment sends data to the server, the message payload is presented in the following format:

`[MessageSize][PacketType][Data]`


- **MessageSize**: A unsigned 16 bit integer that represents the size of the message plus the `PacketType` length
- **PacketType**: A unsigned 16 bit integer that represents the type of message being received (Ping, Data, etc)
- **Data**: An arbitrary array of bytes with the length of (MessageSize - 2) that is encrypted using the [Blowfish](https://en.wikipedia.org/wiki/Blowfish_(cipher)) cipher

#### Additional Notes
- On initial connection, the encryption keys are a static value of `hackOnline`. A key exchange is then performed, however
the new keys are **not** used until after the exchange acknowledgement.
