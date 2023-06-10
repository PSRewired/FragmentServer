namespace Fragment.NetSlum.Networking.Constants;

public enum OpCodes : ushort
{
    None = 0x00,
    PingRequest = 0x02,

    Data = 0x30,
    KeyExchangeRequest = 0x34,
    KeyExchangeResponse = 0x35,
    KeyExchangeAcknowledgmentRequest = 0x36,


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


    DataLogonRepeatRequest = 0x7000,
    DataLogonResponse = 0x7001,

    //Check and see if there's new Posts on the BBS?
    DataBBSGetUpdatesRequestRequest = 0x786A,
    DataBBSGetUpdatesSuccess = 0x786b,

    DataLobbyEnterRoomRequest = 0x7006,
    DataLobbyEnterRoomSuccess = 0x7007,

    DataLobbyStatusUpdate = 0x7009,

    DataGetLobbyChatroomListRequest = 0x7406,
    DataLobbyChatroomCategory = 0x7407,

    DataLobbyChatroomListError = 0x7408,

    //not seen?
    DataLobbyChatroomEntryCategory = 0x7409,
    DataLobbyChatroomChatroom = 0x740a,
    DataLobbyChatroomEntryChatroom = 0x740b,


    DataLobbyChatroomCreate = 0x7415,
    DataLobbyChatroomCreateOk = 0x7416,
    DataLobbyChatroomCreateError = 0x7417,

    AreaServerShutdownRequest = 0x7019,
    AreaServerShutdownResponse = 0x701C,
    //Doesn't work
    DataLogonAs2Response = 0x701d,

    DataDiskAuthorizationRequest = 0x7423,

    DataDiskAuthorizationSuccess = 0x7424,
    DataDiskAuthorizationFailed = 0x7425,

    DataSaveIdRequest = 0x7426,
    DataSaveIdSuccess = 0x7427,

    DataLobbyExitRoom = 0x7444,
    DataLobbyExitRoomOk = 0x7445,

    DataPlayerAccountInfoSuccess = 0x742A,
    DataRegisterCharRequest = 0x742B,

    DataRegisterCharSuccess = 0x742C,

    DataUnregisterCharRequest = 0x7432,
    DataUnregisterCharSuccess = 0x7433,

    DataReturnDesktop = 0x744a,
    DataReturnDesktopOk = 0x744b,

    //main lobby...
    DataLobbyGetMenuRequest = 0x7500,
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

    DataNewsGetMenuRequest = 0x784E,
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

    DataNewsPostSizeInfoResponse = 0x7855,
    DataNewsPostDetailsResponse = 0x7856,
    DataNewsPostErrorResponse = 0x7857,


    DataMailGet = 0x7803,
    DataMailGetOk = 0x7804,
    DataMailGetNewMailHeader = 0x788a,
    DataMailGetMailBody = 0x7806,
    DataMailGetMailBodyResponse = 0x7807,

    //BBS	POSTING	STUFF
    DataBbsCheckThreadCreate = 0x780f,
    DataBbsCheckThreadCreateResponse = 0x7810,
    DataBbsCreatePostResponse = 0x7813,
    DataBbsPostContentResponse = 0x781d,
    DataBbsGetMenu = 0x7848,
    DataBbsCategoryCountResponse = 0x7849,
    DataBbsGetMenuFailed = 0x784a,
    DataBbsEntryCategory = 0x784b,
    DataBbsThreadCountResponse = 0x784c,

    DataBbsEntryThread = 0x784d,
    //7849 threadCat
    //784a error
    //784b catEnrty
    //784c threadList
    //784d threadEnrty

    DataBbsGetThreadDetails = 0x7818,
    DataBbsThreadPostCountResponse = 0x7819,
    DataBbsThreadPostEntryDetailResponse = 0x781a,
    DataBbsThreadEntryPost = 0x781b,
    DataBbsGetPostContent = 0x781c,
    //7819
    //781a
    //781b

    RankingViewAll = 0x7832,
    RankingViewPlayer = 0x7838,

    //These happen upon entering ALTIMIT DESKTOP
    DataMailCheckRequest = 0x7867,
    DataMailCheckSuccess = 0x7868,

    DataMailCheckFail = 0x7869,

    //
    DataNewCheckRequest = 0x786D,

    DataNewCheckSuccess = 0x786E,
    //

    DataComRequest = 0x7876,
    DataComSuccess = 0x7877,

    DataSelectCharRequest = 0x789f,

    DataSelectCharSuccess = 0x78A0,

    // probably something else but this works in our favor
    DataSelectCharDenied = 0x78a1,


    DataSelect2CharRequest = 0x78a2,

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
    Data_AreaServerPublishSuccess = 0x78AF,


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
    DataCreateGuildSuccessResponse = 0x7601,
    ArgumentInviteToGuild = 0x7606,
    DataInviteToGuild = 0x7603,
    DataAcceptGuildInvite = 0x7607,
    PrivateBroadcast = 0x788c,
    DataGuildView = 0x772c,
    DataGetGuildInfoResponse = 0x7740,
    DataAreaServerSuccess = 0x78AC,


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
    Data_GuildCategoryEntryCountResponse = 0x7734,
    Data_GuildCategoryListItemResponse = 0x7736,
    Data_GuildListEntryCountResponse = 0x7737,
    Data_GuildListItemResponse = 0x7738,
    DataGuildGetInfo = 0x7739,


    DataGuildLoggedInMembers = 0x789c,
    DataGuildLoggedInMembersResponse = 0x789d,
    DataGuildMemberListRequest = 0x7610,
    DataGuildMemberListCategoryCountResponse = 0x7611,
    DataGuildMemberListCategoryEntryResponse = 0x7613,
    DataGuildMemberListEntryCountResponse = 0x7614,
    DataGuildMemberListMemberEntryResponse = 0x7615,
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
