[![Actions Status](https://github.com/kacper-matuszek/ItemsAdministration/workflows/master/badge.svg)](https://github.com/kacper-matuszek/ItemsAdministration/actions)

# ItemsAdministration

**How to start the solution?**

Start the infrastructure using Docker:
```
docker-compose -f "postgres-compose.yaml" -p "postgres" up -d
```

Start API located under WebHost project:
```
cd src/WebHost
dotnet run
```
If you want run app in release mode use __build.ps1__ script:
```
./build.ps1
cd publish
./ItemsAdministration.WebHost.exe
```