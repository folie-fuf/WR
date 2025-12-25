dotnet build
dotnet run


curl -X 'GET' \
  'http://localhost:5291/api/Valera' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzY0ODM4MzUyLCJleHAiOjE3NjU0NDMxNTIsImlhdCI6MTc2NDgzODM1MiwiaXNzIjoiVmFsZXJhUHJvamVjdCIsImF1ZCI6IlZhbGVyYUNsaWVudCJ9.wKypv3ctcASt1aJ6XvDfWMBJHXQFEn5AUNmQ5ZV2D80' \
  -v  # Флаг -v покажет заголовки ответа

  curl -X 'GET' \
  'http://localhost:5291/api/Valera/1' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzY0ODM4MzUyLCJleHAiOjE3NjU0NDMxNTIsImlhdCI6MTc2NDgzODM1MiwiaXNzIjoiVmFsZXJhUHJvamVjdCIsImF1ZCI6IlZhbGVyYUNsaWVudCJ9.wKypv3ctcASt1aJ6XvDfWMBJHXQFEn5AUNmQ5ZV2D80'


  curl -X 'POST' \
  'http://localhost:5291/api/Valera/1/work' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzY0ODM4MzUyLCJleHAiOjE3NjU0NDMxNTIsImlhdCI6MTc2NDgzODM1MiwiaXNzIjoiVmFsZXJhUHJvamVjdCIsImF1ZCI6IlZhbGVyYUNsaWVudCJ9.wKypv3ctcASt1aJ6XvDfWMBJHXQFEn5AUNmQ5ZV2D80' \
  -d '' | jq .  # jq для красивого вывода JSON


  sqlite3 valera.db
UPDATE Users SET Role = 'Admin' WHERE Email = 'test@example.com';
.exit


curl -X 'GET' \
  'http://localhost:5291/api/Valera' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzY0ODM4MzUyLCJleHAiOjE3NjU0NDMxNTIsImlhdCI6MTc2NDgzODM1MiwiaXNzIjoiVmFsZXJhUHJvamVjdCIsImF1ZCI6IlZhbGVyYUNsaWVudCJ9.wKypv3ctcASt1aJ6XvDfWMBJHXQFEn5AUNmQ5ZV2D80'

  