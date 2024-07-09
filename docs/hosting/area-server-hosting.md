# Hosting an Area Server

## Patching the Area Server Executable
- Download the latest release of the [Fragment Patcher](https://github.com/Zero1UP/dot-Hack-Fragment-Patcher/releases/latest)
- Open the AreaServer executable in the patcher and update the IP address to the server you wish to connect to
- Click Patch.
- No prompt will be given, but you may now close the patcher tool.

## Assigning your server to a specific category
By design, the lobby server support the use of multiple defined AreaServer categories. By default, area servers will
be assigned to the "Main" category unless the following format is used in the server's name. `<CategoryName>|<ServerName>` (Ex: Test|MyAreaServer)

Using the above format will instruct the lobby server to assign your AreaServer to the specified category within the game.
If the category does not exist, the full name of your AreaServer will be used and assigned to the "Main" category.
