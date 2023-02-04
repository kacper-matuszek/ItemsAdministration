[![Actions Status](https://github.com/kacper-matuszek/ItemsAdministration/workflows/master/badge.svg)](https://github.com/kacper-matuszek/ItemsAdministration/actions)

# ItemsAdministration

### How to start the solution?

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

### Testing
* __postgres-compose.yaml__ contains also pg admin container (http://localhost:5050). You can sign in using e-mail: __admin@admin.com__ and password: __root__. After that you should connect to database using user: __admin__ and password: __admin__
* To properly test the role you need to generate a token, which is generated using the __auth__ endpoint
