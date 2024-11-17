up:
	docker compose up -d --remove-orphans

down:
	docker compose down;

db:
	docker compose up postgresql -d --remove-orphans

run:
	dotnet run --project Movies.Api/Movies.Api.csproj

# Migrations in Local DB

# Define constants
CONN_STR := "Server=localhost;Port=5432;Database=movies;User Id=app;Password=Password123;" 

# Create migration
# usage: make migrate MIGRATION_NAME=CreateMoviesAndGenre
migrate:
	@$(if $(MIGRATION_NAME), , $(error MIGRATION_NAME is not set))
	dotnet ef migrations add $(MIGRATION_NAME) --project Movies.Db -- $(CONN_STR)

# Update Db
# usage: make update
update:
	dotnet ef database update --project Movies.Db -- $(CONN_STR)

# Connect to local Db
# psql -U app -h localhost -p 5432 -d movies

create-movies:
	./scripts/createMovies.sh