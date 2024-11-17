#!/bin/bash

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

# API Base URL
API_URL="http://localhost:5149/api/movies"

# Array of movie payloads
movies=(
  '{"title":"The Shawshank Redemption","yearOfRelease":1994,"genres":["Drama","Crime"]}'
  # '{"title":"The Godfather","yearOfRelease":1972,"genres":["Drama","Crime"]}'
  '{"title":"The Dark Knight","yearOfRelease":2008,"genres":["Action","Drama","Crime"]}'
  '{"title":"Pulp Fiction","yearOfRelease":1994,"genres":["Drama","Crime"]}'
  '{"title":"Forrest Gump","yearOfRelease":1994,"genres":["Drama","Romance"]}'
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