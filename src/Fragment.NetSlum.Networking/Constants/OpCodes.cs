namespace Fragment.NetSlum.Networking.Constants;

public enum OpCodes : ushort
{
    None = 0x00,
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
    ClientTypeGame = 0x7430,
    ClientTypeAreaServer = 0x7431,
    ClientTypeWebclient = 0x7432,

    AreaServerStatusOpen = 0x00,
    AreaServerStatusBusy = 0x02,

    //Packet 0x30 subOpcode Defines
    //The area server likes to ping in a DATA packet...
    DataPing = 0x02,

    //Nice to know.
    DataServerKeyChange = 0x31,

    //Not sure that's actually what this does.
    DataPing2 = 0x40,
    DataPong2 = 0x41,


    Data_LogonRepeatRequest = 0x7000,
    Data_LogonResponse = 0x7001,

    //Check and see if there's new Posts on the BBS?
    Data_BBSGetUpdatesRequest = 0x786A,
    Data_BBSGetUpdatesSuccess = 0x786b,

    DataLobbyEnterRoom = 0x7006,
    DataLobbyEnterRoomOk = 0x7007,

    DataLobbyStatusUpdate = 0x7009,

    DataLobbyChatroomGetList = 0x7406,
    DataLobbyChatrookCategory = 0x7407,

    DataLobbyChatroomListError = 0x7408,

    //not seen?
    DataLobbyChatroomEntryCategory = 0x7409,
    DataLobbyChatroomChatroom = 0x740a,
    DataLobbyChatroomEntryChatroom = 0x740b,


    DataLobbyChatroomCreate = 0x7415,
    DataLobbyChatroomCreateOk = 0x7416,
    DataLobbyChatroomCreateError = 0x7417,

    //Why?
    Data_AreaServerLogon2Request = 0x7019,
    Data_AreaServerLogon2Response = 0x701C,
    //Doesn't work
    DataLogonAs2Response = 0x701d,

    Data_DiskAuthorizationRequest = 0x7423,

    Data_DiskAuthorizationSuccess = 0x7424,
    Data_DiskAuthorizationFailed = 0x7425,

    DataSaveId = 0x7426,
    DataSaveIdOk = 0x7427,

    DataLobbyExitRoom = 0x7444,
    DataLobbyExitRoomOk = 0x7445,

    Data_PlayerAccountInfoOk = 0x742A,
    DataRegisterChar = 0x742B,

    DataRegisterCharOk = 0x742C,

    DataUnregisterChar = 0x7432,
    DataUnregisterCharOk = 0x7433,

    DataReturnDesktop = 0x744a,
    DataReturnDesktopOk = 0x744b,

    //main lobby...
    DataLobbyGetMenu = 0x7500,
    DataLobbyCategoryList = 0x7501,     // uint16_t numberOfCategories
    DataLobbyGetMenuFail = 0x7502,     //Failed to get list
    DataLobbyEntryCategory = 0x7503,     //uint16_t categoryNum, char* categoryName
    DataLobbyLobbyList = 0x7504,     //uint16_t numberOfLobbies

    DataLobbyEntryLobby = 0x7505,     //uint16_t lobbyNum, char* lobbyName, uint32_t numUsers (?)

    //LOBBY_EVENT?
    DataLobbyEvent = 0x7862,


    DataLobbyGetServers = 0x7841,
    DataLobbyGetServersOk = 0x7842,

    //ANOTHER Tree
    DataLobbyGetServersGetList = 0x7506,
    DataLobbyGetServersCategoryList = 0x7507,     //arg is # items?
    DataLobbyGetServersFail = 0x7508,     //FAILED
    DataLobbyGetServersEntryCategory = 0x7509,     //The DIRS
    DataLobbyGetServersServerList = 0x750A,     //arg is # items?
    DataLobbyGetServersEntryServer = 0x750B,     //yay...


    DataLobbyGetServersExit = 0x7844,
    DataLobbyGetServersExitOk = 0x7845,

    DataNewsGetMenu = 0x784E,
    DataNewsCategoryList = 0x784F,     //arg is #of items in category list
    DataNewsGetMenuFailed = 0x7850,     //Failed
    DataNewsEntryCategory = 0x7851,     //Category list Entry
    DataNewsArticleList = 0x7852,     //Article list, Arg is # entries

    DataNewsEntryArticle = 0x7853,     //Article List Entry

    //7853 - ok/no data
    //7852 - ok/wants more data?
    //7851 - ok/no data?
    //7850 - failed
    //784f - ok


    DataNewsGetPost = 0x7854,

    DataNewsSendPost = 0x7855,
    //7856
    //7857
    //7855


    DataMailGet = 0x7803,
    DataMailGetOk = 0x7804,
    DataMailGetNewMailHeader = 0x788a,
    DataMailGetMailBody = 0x7806,
    DataMailGetMailBodyResponse = 0x7807,

    //BBS	POSTING	STUFF
    DataBbsGetMenu = 0x7848,
    DataBbsCategoryList = 0x7849,
    DataBbsGetMenuFailed = 0x784a,
    DataBbsEntryCategory = 0x784b,
    DataBbsThreadlist = 0x784c,

    DataBbsEntryThread = 0x784d,
    //7849 threadCat
    //784a error
    //784b catEnrty
    //784c threadList
    //784d threadEnrty

    DataBbsThreadGetMenu = 0x7818,
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
    DataMailCheck = 0x7867,
    DataMailCheckOk = 0x7868,

    DataMailCheckFail = 0x7869,

    //
    DataNewCheck = 0x786D,

    DataNewCheckOk = 0x786E,
    //

    DataCom = 0x7876,
    DataComOk = 0x7877,

    DataSelectChar = 0x789f,

    DataSelectCharOk = 0x78A0,

    // probably something else but this works in our favor
    DataSelectCharDenied = 0x78a1,


    DataSelect2Char = 0x78a2,

/*
DATA_SELECT_CHAR2 seems to be a 1:1 clone of the normal OPCODE_DATA_SELECT_CHAR packet.
*/
    Data_Select2_CharacterSuccess = 0x78a3,
    

    Data_LogonRequest = 0x78AB,

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
    Data_AreaServerDiskAuthorizationRequest = 0x785B,
    Data_AreaServerDiskAuthorizationSuccess = 0x785C,
    Data_AreaServerDiskAuthorizationFailed = 0x785d,

    Data_AreaServerIpPortRequest = 0x7013,
    Data_AreaServerIpPortSuccess = 0x7014,

    Data_AreaServerPublishRequest = 0x78AE,
    Data_AreaServerPublishSucess = 0x78AF,


    Data_AreaServerPublishDetails1Request = 0x7011,
    Data_AreaServerPublishDetails2Request = 0x7016,
    Data_AreaServerPublishDetails3Request = 0x7881,
    Data_AreaServerPublishDetails4Request = 0x7887,
    Data_AreaServerPublishDetails6Request = 0x78a7,

    Data_AreaServerPublishDetails1Success = 0x7012,
    Data_AreaServerPublishDetails2Success = 0x7017,
    Data_AreaServerPublishDetails3Success = 0x7882,
    Data_AreaServerPublishDetails4Success = 0x7888,
    Data_AreaServerPublishDetails5Success = 0x741e,
    Data_AreaServerPublishDetails6Success = 0x78a8,
    Data_AreaServerPublishDetails7Success = 0x780d,


    Data_AreaServerUpdateUserCountRequest = 0x741D,     //uint32_t numUsers
    Data_AreaServerUpdateStatusRequest = 0x780C,

   

    DataAsNameId = 0x5778,

    LobbyTypeGuild = 0x7418,
    LobbyTypeMain = 0x7403,
    ClientLeavingLobby = 0x700a,
    ArgumentInviteToGuild = 0x7606,
    DataInviteToGuild = 0x7603,
    DataAcceptGuildInvite = 0x7607,
    PrivateBroadcast = 0x788c,
    DataGuildView = 0x772c,
    DataGetGuildInfoResponse = 0x7740,
    DataAreaServerOk = 0x78AC,


    DataGuildCreate = 0x7600,
    DataGuildGetAllGuilds = 0x7722,
    DataGuildGroupCount = 0x7723,
    DataGuildGroupCategory = 0x7725,
    DataGuildCount = 0x7726,
    DataGuildEntry = 0x7727,
    DataGuildGetListOfItems = 0x772F,
    DataGuildItemsCount = 0x7730,
    DataGuildItemDetails = 0x7731,
    DataGuildGetMenu = 0x7733,
    DataGuildGetInfo = 0x7739,


    DataGuildLoggedInMembers = 0x789c,
    DataGuildMemberList = 0x7610,
    DataGuildGetItemsToBuy = 0x7708,
    DataGuildGetItems = 0x7728,
    DataGuildBuyItem = 0x770C,
    DataGuildDonateItem = 0x7702,
    DataGuildGetDonationSettings = 0x7879,
    DataGuildUpdateItemPricingAvailability = 0x7703,
    DataGuildUpdateItemPricing = 0x7712,
    DataGuildGmLeaving = 0x788D,
    DataGuildPlayerLeaving = 0x7616,
    DataGuildPlayerKicked = 0x7864,
    DataGuildDissolved = 0x7619,
    DataGuildUpdateDetails = 0x761C,
    DataGuildTakeGp = 0x770E,
    DataGuildTakeItem = 0x7710,
    DataGuildDonateCoins = 0x7700,
}
