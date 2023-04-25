using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Character;

[FragmentPacket(OpCodes.Data, OpCodes.DataRegisterChar)]
public class RegisterCharacterRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new RegisterCharacterResponse()
                .Build()
        });
    }

    //TODO: Implement this
    /*
    private uint ExtractCharacterData(GameClientAsync client, byte[] data)
    {
        client.save_slot = data[0];
        client.char_id = ReadByteString(data, 1);
        int pos = 1 + client.char_id.Length;
        client.char_name = ReadByteString(data, pos);
        pos += client.char_name.Length;
        client.char_class = data[pos++];
        client.char_level = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.greeting = ReadByteString(data, pos);
        pos += client.greeting.Length;
        client.char_model = swap32(BitConverter.ToUInt32(data, pos));
        pos += 5;
        client.char_HP = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.char_SP = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.char_GP = swap32(BitConverter.ToUInt32(data, pos));
        pos += 4;
        client.online_god_counter = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.offline_godcounter = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.goldCoinCount = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.silverCoinCount = swap16(BitConverter.ToUInt16(data, pos));
        pos += 2;
        client.bronzeCoinCount = swap16(BitConverter.ToUInt16(data, pos));

        client.classLetter = GetCharacterModelClass(client.char_model);
        client.modelNumber = GetCharacterModelNumber(client.char_model);
        client.modelType = GetCharacterModelType(client.char_model);
        client.colorCode = GetCharacterModelColorCode(client.char_model);

        client.charModelFile = "xf" + client.classLetter + client.modelNumber + client.modelType + "_" + client.colorCode;
        return DBAccess.getInstance().PlayerLogin(client);
    }
    */

    private static char GetCharacterModelClass(uint modelNumber)
    {
        //TODO: convert to constants
        char[] classLetters = { 't', 'b', 'h', 'a', 'l', 'w' };
        int index = (int)(modelNumber & 0x0F);
        return classLetters[index];
    }

    private static int GetCharacterModelNumber(uint modelNumber)
    {
        return (int)(modelNumber >> 4 & 0x0F) + 1;
    }

    private static char GetCharacterModelType(uint modelNumber)
    {
        //TODO: convert to constants
        char[] typeLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' };
        int index = (int)(modelNumber >> 12) & 0x0F;
        return typeLetters[index];
    }

    private static string GetCharacterModelColorCode(uint modelNumber)
    {
        //TODO: convert to constants
        string[] colorCodes = { "rd", "bl", "yl", "gr", "br", "pp" };
        int index = (int)(modelNumber >> 8) & 0x0F;
        return colorCodes[index];
    }
}
