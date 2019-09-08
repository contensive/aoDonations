
rem all paths are relative to the git scripts folder
rem appName is on the command line
rem if appName not given, defaultAppName usedenvAppName is the default upload app name
rem 
rem
set defaultAppName=app20190821v51
set appName=%1
set collectionName=Donations
set collectionPath=..\collections\Donations\
set binPath=..\source\aoDonations\bin\debug\

rem
rem
rem

IF [%appName%] == [] (
	set appName=%defaultAppName%
)

