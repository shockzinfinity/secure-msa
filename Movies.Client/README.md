# CRUD operations on Movies.Client

## operations

| Method | Request URI      | Operation | Description                     | Request body | Response body          |
| :----: | :--------------- | :-------- | :------------------------------ | :----------- | :--------------------- |
|  GET   | /api/movies      | Read      | Get all movie records           | None         | Array of movie records |
|  GET   | /api/movies/{id} | Read      | Get a movie record by ID        | None         | Movie record           |
|  POST  | /api/movies      | Create    | Add a new movie record          | Movie record | Movie record           |
|  PUT   | /api/movies/{id} | Update    | Update an existing movie record | Movie record | None                   |
| DELETE | /api/movies/{id} | Delete    | Delete a movie record           | None         | Movie record           |
