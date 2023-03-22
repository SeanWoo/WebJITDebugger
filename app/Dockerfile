FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebJITDebugger/WebJITDebugger.csproj", "WebJITDebugger/"]
RUN dotnet restore "WebJITDebugger/WebJITDebugger.csproj"
COPY . .
WORKDIR "/src/WebJITDebugger"
RUN dotnet build "WebJITDebugger.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet-buildtools/prereqs:ubuntu-20.04-latest AS runtime
RUN apt-get update && apt-get install -y git wget
RUN git clone https://github.com/dotnet/runtime.git
WORKDIR /runtime
RUN git fetch --all --tags
RUN git checkout tags/v7.0.4 -b v7.0.4
RUN ./build.sh Clr -c Release -rc Checked

FROM build AS publish
RUN dotnet publish "WebJITDebugger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
COPY --from=runtime /runtime ./runtime
ENTRYPOINT ["dotnet", "WebJITDebugger.dll"]
