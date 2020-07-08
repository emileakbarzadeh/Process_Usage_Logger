# Process_Usage_Logger
DOWNLOAD BINARIES HERE: https://github.com/emileakbarzadeh/Process_Usage_Logger/releases/tag/1.0



Built with .NET Core so should be compilable on linux if you build for it

## Usage ##
This application needs admin privileges, but it should launch with them and ask for your permission.
To use, launch it and enter the number of ms interval between logs OR use the launch option
ex.
`ProcessUsageLogger.exe 1000`

Logs are saved in the same directory the program launched from.

## Background ##
My server kept going to 100% CPU utilisaiton for some unknown reason. With such an issue I can't log in to see what the problem is.
I could not find any applications that did this well, so I quickly made it myself.
