# Leaderboard API

## Overview
A lightweight ASP.NET Core API for game leaderboards. It handles sign-up/sign-in with JWT, lets you submit and update scores, and provides a few ways to read leaderboards (top lists and “nearest” scores).
Runs on an in-memory database by default.

## What’s inside
- User auth with JWT (register, login, refresh, logout)
- Create/update/delete game scores
- Query top scores and nearest scores around a target with using LINQ
- Simple domain: Users, Players, Games, GameScores
    - Player history stays even if the related User is deleted
- ORM Tool: Entity Framework Core
- Input validation with Fluent Validation
- Object mapping with AutoMapper
- Custom exception middleware to log requests and responses with using generic logger interface.

## Tech Stack
- .NET 9
- Entity Framework Core
- Auto Mapper
- Fluent Validation
- Newtonsoft.Json
- Jwt Bearer

## Quick start
``` bash
git clone <your-repo-url> leaderboard-api
cd leaderboard-api
dotnet restore
dotnet run --project LeaderboardApi/LeaderboardApi
```
## Config
Set JWT settings via appsettings, user-secrets, or environment variables:
``` json
{
  "Token": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "SecurityKey": "a-very-long-random-secret-key"
  }
}
```
Tokens have their lifetimes configured in the appsettings.json file.
## Auth flow
1. Register
2. Login to get an access token (and refresh token)
3. Call protected endpoints with:
    - Authorization: Bearer <access_token>
4. Refresh when the access token expires
5. Logout to invalidate a refresh token

## Endpoints
Most endpoints require a Bearer token, except register/login.
### Users
- POST api/users/register
Create a user.
``` json
  {
    "email": "player@example.com",
    "password": "P@ssw0rd!",
  }
```
- POST api/users/login
Get access/refresh tokens.
``` json
  {
    "email": "player@example.com",
    "password": "P@ssw0rd!"
  }
```
Response:
``` json
  {
    "accessToken": "<jwt>",
    "refreshToken": "<refresh>",
    "expiresIn": 3600
  }
```
- POST api/users/refresh
Exchange a refresh token for a new access token.
``` json
  {
    "refreshToken": "<refresh>"
  }
```
- POST /users/logout
Invalidate a refresh token.
``` json
  {
    "refreshToken": "<refresh>"
  }
```
- DELETE api/users/{id}
Delete a user (player records remain).

### Game scores
- GET api/gamescores
List scores (filter with query params like gameId, playerId).
- GET api/gamescores/{id}
Get one score.
- GET api/gamescores/top?gameId=1&count=10
Top N scores for a game.
- GET api/gamescores/nearest?playerId=3&gameId=2&countAbove=4&countBelow=3)
Scores nearest to a target.
- POST api/gamescores
Create a score.
``` json
  {
    "gameId": 1,
    "playerId": 42,
    "score": 12345,
  }
```
- PUT api/gamescores/{id}
Update a score.
``` json
  {
    "score": 12400,
  }
```
- DELETE api/gamescores/{id}
Remove a score.

## Examples
- Register
``` bash
  curl -X POST http://localhost:5000/users/register \
    -H "Content-Type: application/json" \
    -d '{"email":"player@example.com","password":"P@ssw0rd!","name":"Player One"}'
```
- Login
``` bash
  curl -X POST http://localhost:5000/users/login \
    -H "Content-Type: application/json" \
    -d '{"email":"player@example.com","password":"P@ssw0rd!"}'
```
- Create a score
``` bash
  curl -X POST http://localhost:5000/gamescores \
    -H "Authorization: Bearer <access_token>" \
    -H "Content-Type: application/json" \
    -d '{"gameId":1,"playerId":42,"score":12345}'
```
- Get top scores
``` bash
  curl "http://localhost:5000/gamescores/top?gameId=1&limit=10" \
    -H "Authorization: Bearer <access_token>"
```
## Notes
- Authorization is enabled globally; register/login are the usual exceptions.
- Using in-memory database, data resets on restart. Swap EF Core provider for persistence.
- If you get 401s, check the Authorization header and token expiry. If tokens fail to validate, confirm Issuer/SecurityKey config.
