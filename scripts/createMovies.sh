#!/bin/bash

echo 'Retrieving JWT'

# URL and payload
API_TOKEN_URL="http://localhost:8081/token"
PAYLOAD='{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "cesarl@x.com",
  "customClaims": {
    "admin": false,
    "trusted_member": true
  }
}'

# Execute curl and capture the response
response=$(curl -s POST "$API_TOKEN_URL" \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d "$PAYLOAD")


# Extract the JWT string (assuming it's in the 'token' field)
JWT=$(echo "$response")

# Print the JWT
# echo " "
# echo "JWT: $JWT"

# Check if token retrieval succeeded
if [[ -z "$JWT" || "$JWT" == "null" ]]; then
  echo "Error: Failed to retrieve token. Response: $response"
  exit 1
fi

echo 'Creating Movies'

# API Base URL
API_URL="http://localhost:5149/api/movies"

# Array of movie payloads
movies=(
  '{"title":"The Shawshank Redemption","yearOfRelease":1994,"genres":["Drama","Crime"]}'
  '{"title":"The Godfather","yearOfRelease":1972,"genres":["Drama","Crime"]}'
  '{"title":"The Dark Knight","yearOfRelease":2008,"genres":["Action","Drama","Crime"]}'
  '{"title":"Pulp Fiction","yearOfRelease":1994,"genres":["Drama","Crime"]}'
  '{"title":"Forrest Gump","yearOfRelease":1994,"genres":["Drama","Romance"]}'
  '{"title":"The Lord of the Rings: The Fellowship of the Ring","yearOfRelease":2001,"genres":["Adventure","Drama","Fantasy"]}'
  '{"title":"The Silence of the Lambs","yearOfRelease":1991,"genres":["Crime","Drama","Thriller"]}'
  '{"title":"Saving Private Ryan","yearOfRelease":1998,"genres":["Drama","War"]}'
  '{"title":"The Green Mile","yearOfRelease":1999,"genres":["Crime","Drama","Fantasy"]}'
  '{"title":"Fight Club","yearOfRelease":1999,"genres":["Drama"]}'
  '{"title":"The Avengers","yearOfRelease":2012,"genres":["Action","Adventure","Sci-Fi"]}'
  '{"title":"Braveheart","yearOfRelease":1995,"genres":["Biography","Drama","History"]}'
  '{"title":"Django Unchained","yearOfRelease":2012,"genres":["Drama","Western"]}'
  '{"title":"Titanic","yearOfRelease":1997,"genres":["Drama","Romance"]}'
  '{"title":"Avatar","yearOfRelease":2009,"genres":["Action","Adventure","Sci-Fi"]}'
  '{"title":"Inception","yearOfRelease":2010,"genres":["Action","Sci-Fi","Thriller"]}'
  '{"title":"The Matrix","yearOfRelease":1999,"genres":["Action","Sci-Fi"]}'
  '{"title":"Interstellar","yearOfRelease":2014,"genres":["Adventure","Drama","Sci-Fi"]}'
  '{"title":"Gladiator","yearOfRelease":2000,"genres":["Action","Adventure","Drama"]}'
  '{"title":"Schindlers List","yearOfRelease":1993,"genres":["Biography","Drama","History"]}'
)

# Loop through movies and add each one
for movie in "${movies[@]}"
do

  # Movie payload
  PAYLOAD='{
    "title": "The Godfather",
    "yearOfRelease": 1972,
    "genres": ["Drama", "Crime"]
  }'

  # Execute curl and capture the HTTP status code
  response=$(curl -s -o /dev/null -w "%{http_code}" -X POST "$API_URL" \
    -H "Authorization: Bearer $JWT" \
    -H "Content-Type: application/json" \
    -d "$movie")

  # Check the HTTP status code
  if [ "$response" -eq 201 ]; then
    echo "Movie created successfully!"
  else
    echo "Error: Failed to create movie. HTTP Status: $response"
    exit 1
  fi

done

echo "All movies added successfully!"