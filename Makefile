#Project Variables:
PROJECT_NAME?= SolarCoffee
ORG_NAME?= SolarCoffee
REPO_NAME?= SolarCoffee

.PHONY: migrations db

migrations: 
	cd ./SolarCoffee.data && dotnet-ef --startup-project ../SolarCoffee.Web/ migrations add $(mname) && cd ..

db:
	cd ./SolarCoffee.data && dotnet-ef --startup-project ../SolarCoffee.Web/ database update && cd ..

removeAll:
	cd ./SolarCoffee.data && dotnet-ef --startup-project ../SolarCoffee.Web/ database update 0 && cd ..
	cd ./SolarCoffee.data && dotnet-ef --startup-project ../SolarCoffee.Web/ migrations remove && cd ..