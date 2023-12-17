dotnet build -c Release Lox/Lox.csproj
cd craftinginterpreters || exit
dart tool/bin/test.dart chap13_inheritance --interpreter ../Lox/bin/Release/net8.0/Lox