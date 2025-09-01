FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ["TicketBookingApi/TicketBookingApi.csproj", "TicketBookingApi/"]
RUN dotnet restore "TicketBookingApi/TicketBookingApi.csproj"

COPY ./TicketBookingApi ./TicketBookingApi
WORKDIR /app/TicketBookingApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/TicketBookingApi/out .

ENTRYPOINT ["dotnet", "TicketBookingApi.dll"]