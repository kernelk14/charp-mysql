all: Program.exe
	dotnet ./bin/Debug/net8.0/dottie.dll

Program.exe: Program.cs
	dotnet build
