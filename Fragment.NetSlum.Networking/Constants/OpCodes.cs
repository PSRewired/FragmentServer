namespace Fragment.NetSlum.Networking.Constants;

public enum OpCodes : ushort
{
    Ping = 0x02,

    Data = 0x30,
    KeyExchangeRequest = 0x34,
    KeyExchangeResponse = 0x35,
    KeyExchangeAcknowledgment = 0x36,


    MaxAsNameLen = 0x14,

    LobbyUserEnter = 0x01,
    LobbyUserTell = 0x02,

    //There's several packets that come in when a user enters,
    LobbyUserEnterName = 0x4,
    LobbyUserEnterGreeting = 0x5,

    //MISC_DEFINES
    ClienttypeGame = 0x7430,
    ClienttypeAreaserver = 0x7431,
    ClienttypeWebclient = 0x7432,

    AreaserverStatusOpen = 0x00,
    AreaserverStatusBusy = 0x02,

    //Packet 0x30 subOpcode Defines
    //The area server likes to ping in a DATA packet...
    DataPing = 0x02,

    //Nice to know.
    DataServerkeyChange = 0x31,

    //Not sure that's actually what this does.
    DataPing2 = 0x40,
    DataPong2 = 0x41,


    DataLogonRepeat = 0x7000,
    DataLogonResponse = 0x7001,

    //check and see if there's new posts on the BBS?
    DataBbsGetUpdates = 0x786A,


    DataLobbyEnterroom = 0x7006,
    DataLobbyEnterroomOk = 0x7007,

    DataLobbyStatusUpdate = 0x7009,

    DataLobbyChatroomGetlist = 0x7406,
    DataLobbyChatroomCategory = 0x7407,

    DataLobbyChatroomListerror = 0x7408,

    //not seen?
    DataLobbyChatroomEntryCategory = 0x7409,
    DataLobbyChatroomChatroom = 0x740a,
    DataLobbyChatroomEntryChatroom = 0x740b,


    DataLobbyChatroomCreate = 0x7415,
    DataLobbyChatroomCreateOk = 0x7416,
    DataLobbyChatroomCreateError = 0x7417,

    //Why?
    DataLogonAs2 = 0x7019,

    //Doesn't work
    DataLogonAs2Response = 0x701d,

    DataDiskid = 0x7423,

    DataDiskidOk = 0x7424,
    DataDiskidBad = 0x7425,

    DataSaveid = 0x7426,
    DataSaveidOk = 0x7427,

    DataLobbyExitroom = 0x7444,
    DataLobbyExitroomOk = 0x7445,

    DataRegisterChar = 0x742B,

    DataRegisterCharok = 0x742C,

    DataUnregisterChar = 0x7432,
    DataUnregisterCharok = 0x7433,

    DataReturnDesktop = 0x744a,
    DataReturnDesktopOk = 0x744b,

    //main lobby...
    DataLobbyGetmenu = 0x7500,
    DataLobbyCategorylist = 0x7501,     // uint16_t numberOfCategories
    DataLobbyGetmenuFail = 0x7502,     //Failed to get list
    DataLobbyEntryCategory = 0x7503,     //uint16_t categoryNum, char* categoryName
    DataLobbyLobbylist = 0x7504,     //uint16_t numberOfLobbies

    DataLobbyEntryLobby = 0x7505,     //uint16_t lobbyNum, char* lobbyName, uint32_t numUsers (?)

    //LOBBY_EVENT?
    DataLobbyEvent = 0x7862,


    DataLobbyGetservers = 0x7841,
    DataLobbyGetserversOk = 0x7842,

    //ANOTHER Tree
    DataLobbyGetserversGetlist = 0x7506,
    DataLobbyGetserversCategorylist = 0x7507,     //arg is # items?
    DataLobbyGetserversFail = 0x7508,     //FAILED
    DataLobbyGetserversEntryCategory = 0x7509,     //The DIRS
    DataLobbyGetserversServerlist = 0x750A,     //arg is # items?
    DataLobbyGetserversEntryServer = 0x750B,     //yay...


    DataLobbyGetserversExit = 0x7844,
    DataLobbyGetserversExitOk = 0x7845,

    DataNewsGetmenu = 0x784E,
    DataNewsCategorylist = 0x784F,     //arg is #of items in category list
    DataNewsGetmenuFailed = 0x7850,     //Failed
    DataNewsEntryCategory = 0x7851,     //Category list Entry
    DataNewsArticlelist = 0x7852,     //Article list, Arg is # entries

    DataNewsEntryArticle = 0x7853,     //Article List Entry

    //7853 - ok/no data
    //7852 - ok/wants more data?
    //7851 - ok/no data?
    //7850 - failed
    //784f - ok


    DataNewsGetpost = 0x7854,

    DataNewsSendpost = 0x7855,
    //7856
    //7857
    //7855


    DataMailGet = 0x7803,
    DataMailGetok = 0x7804,
    DataMailGetNewmailHeader = 0x788a,
    DataMailGetMailBody = 0x7806,
    DataMailGetMailBodyResponse = 0x7807,

    //BBS	POSTING	STUFF
    DataBbsGetmenu = 0x7848,
    DataBbsCategorylist = 0x7849,
    DataBbsGetmenuFailed = 0x784a,
    DataBbsEntryCategory = 0x784b,
    DataBbsThreadlist = 0x784c,

    DataBbsEntryThread = 0x784d,
    //7849 threadCat
    //784a error
    //784b catEnrty
    //784c threadList
    //784d threadEnrty			

    DataBbsThreadGetmenu = 0x7818,
    DataBbsThreadList = 0x7819,
    DataBbsEntryPostMeta = 0x781a,
    DataBbsThreadEntryPost = 0x781b,
    DataBbsThreadGetContent = 0x781c,
    //7819
    //781a
    //781b


    RankingViewAll = 0x7832,
    RankingViewPlayer = 0x7838,

    //These happen upon entering ALTIMIT DESKTOP
    DataMailcheck = 0x7867,
    DataMailcheckOk = 0x7868,

    DataMailcheckFail = 0x7869,

    //
    DataNewcheck = 0x786D,

    DataNewcheckOk = 0x786E,
    //

    DataCom = 0x7876,
    DataComOk = 0x7877,

    DataSelectChar = 0x789f,

    DataSelectCharok = 0x78A0,

    // probably something else but this works in our favor
    DataSelectChardenied = 0x78a1,


    DataSelect2Char = 0x78a2,

/*
DATA_SELECT_CHAR2 seems to be a 1:1 clone of the normal OPCODE_DATA_SELECT_CHAR packet.
*/
    DataSelect2Charok = 0x78a3,


    DataLogon = 0x78AB,

    //Area server doesn't like 0x7001
    DataLogonResponseAs = 0x78AD,


    DataMailSend = 0x7800,
    DataGuildMailSend = 0x7809,

    DataBbsPost = 0x7812,

    DataMailSendOk = 0x7801,
    DataGuildMailSendOk = 0x780A,


    DataLobbyFavoritesAsInquiry = 0x7858,

    ///////////////
    //AREA	SERVER	DEFINES:
    ///////////////
    DataAsDiskid = 0x785B,
    DataAsDiskidOk = 0x785C,
    DataAsDiskidFail = 0x785d,

    DataAsIpport = 0x7013,
    DataAsIpportOk = 0x7014,

    DataAsPublish = 0x78AE,
    DataAsPublishOk = 0x78AF,


    DataAsPublishDetails1 = 0x7011,

    DataAsPublishDetails1Ok = 0x7012,

    DataAsPublishDetails2 = 0x7016,

    DataAsPublishDetails2Ok = 0x7017,
    //I'm still not sure what's up with this dude.


    DataAsPublishDetails3 = 0x7881,
    DataAsPublishDetails3Ok = 0x7882,

    DataAsPublishDetails4 = 0x7887,
    DataAsPublishDetails4Ok = 0x7888,

    DataAsUpdateUsernum = 0x741D,     //uint32_t numUsers

    DataAsPublishDetails5Ok = 0x741e,
    //update user num?


    DataAsPublishDetails6 = 0x78a7,
    DataAsPublishDetails6Ok = 0x78a8,

    DataAsUpdateStatus = 0x780C,

    DataAsPublishDetails7Ok = 0x780d,

    DataAsNameid = 0x5778,
    DataAsDiskid2 = 0x78a7,     //again?

    LobbyTypeGuild = 0x7418,
    LobbyTypeMain = 0x7403,
    ClientLeavingLobby = 0x700a,
    ArgumentInviteToGuild = 0x7606,
    DataInviteToGuild = 0x7603,
    DataAcceptGuildInvite = 0x7607,
    PrivateBroadcast = 0x788c,
    DataGuildView = 0x772c,
    DataGetGuildInfoResponse = 0x7740,
    DataAreaserverOk = 0x78AC,


    DataGuildCreate = 0x7600,
    DataGuildGetAllGuilds = 0x7722,
    DataGuildGroupCount = 0x7723,
    DataGuildGroupCategory = 0x7725,
    DataGuildCount = 0x7726,
    DataGuildEntry = 0x7727,
    DataGuildGetListOfItems = 0x772F,
    DataGuildItemsCount = 0x7730,
    DataGuildItemDetails = 0x7731,
    DataGuildGetmenu = 0x7733,
    DataGuildGetInfo = 0x7739,


    DataGuildLoggedinMembers = 0x789c,
    DataGuildMemberlist = 0x7610,
    DataGuildGetitemsTobuy = 0x7708,
    DataGuildGetitems = 0x7728,
    DataGuildBuyItem = 0x770C,
    DataGuildDonateItem = 0x7702,
    DataGuildGetDonationSettings = 0x7879,
    DataGuildUpdateitemPricingAvailability = 0x7703,
    DataGuildUpdateitemPricing = 0x7712,
    DataGuildGmLeaving = 0x788D,
    DataGuildPlayerLeaving = 0x7616,
    DataGuildPlayerKicked = 0x7864,
    DataGuildDissolved = 0x7619,
    DataGuildUpdateDetails = 0x761C,
    DataGuildTakeGp = 0x770E,
    DataGuildTakeItem = 0x7710,
    DataGuildDonateCoins = 0x7700,
}