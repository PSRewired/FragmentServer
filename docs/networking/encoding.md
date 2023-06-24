# Fragment Binary Message Framing
___

## Endianness
- Primitive types (int,short, etc) are represented in big-endian notation.

## Text Encoding
- Initially it was thought that the game was encoding using Shift-JIS strings, however during the development of this project,
it was determined to be partially incorrect. The game, since it supports control characters, uses [MS/CP932](https://en.wikipedia.org/wiki/Code_page_932)
for its encoding and expect all strings to be null-terminated.

## Image Encoding
Images in Fragment are encoded in [TGA/TARGA](http://www.paulbourke.net/dataformats/tga/) format using 32-bit color depth, RGB colorspace, and Top-Left orientation.

### Known Image Usage
- News posts require an image size of 128x128px

