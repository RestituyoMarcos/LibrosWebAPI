# LibrosWebAPI

## Tecnologías Utilizadas

  * **ASP.NET Web API:** Subconjunto de .NET Core para la creación de APIs RESTful.
  * **C#:** Lenguaje de programación principal.
  * **HttpClient:** Clase para realizar solicitudes HTTP a la API externa.
  * **System.Text.Json:** Biblioteca para serialización y deserialización de JSON.
  * **Microsoft.Extensions.DependencyInjection:** Para la gestión de dependencias.
  * **Moq (para pruebas unitarias):** Framework de mocking para crear objetos simulados de dependencias.
  * **MSTest o NUnit (para pruebas unitarias):** Frameworks para escribir y ejecutar pruebas unitarias.
  * **MediatR:** Librería para implementar el patrón CQRS (Command Query Responsibility Segregation).

## Funcionalidades

La API proporciona los siguientes endpoints:

### Libros (`/api/book`)

* **`GET /api/books`:** Obtiene una lista paginada de libros.
* **`GET /api/books/{id}`:** Obtiene los detalles de un libro específico por su ID.
* **`POST /api/books`:** Crea un nuevo libro.
* **`PUT /api/books/{id}`:** Actualiza la información de un libro existente.
* **`DELETE /api/books/{id}`:** Elimina un libro por su ID.

### Autores (`/api/author`)

* **`GET /api/authors`:** Obtiene una lista de todos los autores.
* **`GET /api/authors/{id}`:** Obtiene los detalles de un autor específico por su ID.
* **`GET /api/authors/bybook/{bookId}`:** Obtiene la lista de autores asociados a un ID de libro específico.
* **`POST /api/authors`:** Crea un nuevo autor.
* **`PUT /api/authors/{id}`:** Actualiza la información de un autor existente.
* **`DELETE /api/authors/{id}`:** Elimina un autor por su ID.

Fake REST API: https://fakerestapi.azurewebsites.net/


