del publish\*.* /Q
md publish
dotnet pack -c Release -o publish
nuget.exe push publish\*.* -Source http://nuget.europlan.ru/nuget/Default/ -ApiKey europlanApiKey