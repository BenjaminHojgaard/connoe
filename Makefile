PROJECT_NAME ?= ConnOe
ORG_NAME ?= ConnOe
REPO_NAME ?= ConnOe

.PHONY: migrations db

migrations:
	cd ./ConnOe.Data && dotnet ef --startup-project ../ConnOe.Web/ migrations add $(mname) && cd ..
	
db:
	cd ./ConnOe.Data && dotnet ef --startup-project ../ConnOe.Web/ database update && cd ..