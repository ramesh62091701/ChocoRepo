First attempt at a Virtual Actor Pattern application 

# Server
cd src/NewActor
dapr init
dapr run --dapr-http-port 3500 --app-id new_actor --app-port 5010 dotnet run 

# Client
cd src/Client2
dotnet run