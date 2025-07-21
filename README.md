# PingPong_Backend

## Descripción

Este proyecto corresponde a una prueba técnica para la empresa Ofima, cuyo objetivo fue desarrollar el backend de un videojuego multijugador en tiempo real tipo Ping Pong utilizando WebSockets.

La solución está construida sobre .NET 9, siguiendo los principios de la Arquitectura Limpia con enfoque Hexagonal y orientada a microservicios, promoviendo una separación clara de responsabilidades, modularidad y mantenibilidad.
El backend maneja la lógica del juego, la gestión de usuarios, el registro de puntajes y la sincronización en tiempo real entre jugadores.

## Principales Tecnologías y Enfoques
- **.NET 9**
- **Arquitectura Limpia + Hexagonal**
- **Microservicios**
- **WebSockets (SignalR)**
- **PostgreSQL (base de datos relacional)**
- **MongoDB (base de datos NoSQL)**
- **MediatR (patrón Mediador)**
- **CQRS (Command Query Responsibility Segregation)**
- **Entity Framework Core y MongoDB Driver**
- **SOLID y buenas prácticas de diseño**
- **JWT Authentication**
- **Repositorio**
- **DTOs (Data Transfer Objects)**
- **Fábrica (Factory Pattern)**

## Estructura del Proyecto
El backend está dividido en carpetas por servicio, manteniendo una clara separación de responsabilidades. La convención es PingPong_{NombreServicio}_{Capa}:
```
{Nombre_servicio}/
├── PingPong_{Nombre_servicio}_Api/
│   ├── Common/            # Utilidades, helpers y constantes comunes del API.
│   ├── Controller/        # Endpoints HTTP expuestos, actúan como puntos de entrada para los casos de uso.
│   ├── Middleware/        # Middlewares personalizados para manejo de errores, autenticación, etc.
│   ├── Settings/          # Configuración propia del entorno (CORS, Swagger, JWT, etc).
│   └── appsettings.json/  # Archivo principal de configuración del API (.NET).
│
├── PingPong_{Nombre_servicio}_Application/
│   ├── Commands/          # Casos de uso que modifican estado (write operations) usando CQRS.
│   ├── Dtos/              # Objetos de transferencia de datos entre capas.
│   ├── Queries/           # Casos de uso para consultas (read operations) usando CQRS.
│   └── Settings/          # Configuraciones específicas de la capa Application (por ejemplo, inyección de dependencias).
│
├── PingPong_{Nombre_servicio}_Domain/
│   ├── Entities/          # Entidades del dominio, modelan el núcleo del negocio.
│   ├── Interfaces/        # Contratos o puertos que definen comportamientos esperados (se implementan en Infraestructura).
│   ├── Repositories/      # Interfaces de acceso a datos que abstraen la lógica de persistencia.
│   ├── Services/          # Servicios de dominio con lógica de negocio que no pertenece a una entidad específica.
│   └── ValueObjects/      # Objetos de valor que encapsulan reglas y comportamientos inmutables del dominio.
│
└── PingPong_{Nombre_servicio}_Infrastructure/
    ├── Persistence/       # Configuración y contexto de acceso a las bases de datos (EF Core, MongoDB, etc).
    ├── Repositories/      # Implementaciones concretas de los repositorios definidos en la capa de dominio.
    ├── Services/          # Implementaciones de servicios externos (por ejemplo, correo, WebSocket, etc).
    └── Settings/          # Configuraciones específicas de infraestructura (conexiones, inyecciones de dependencias, opciones de almacenamiento, etc).
```

## Funcionalidades Clave Implementadas
- **Gestión de usuarios: registro seguro con contraseña hasheada (sal + hash).**
- **Sistema de autenticación y autorización (JWT, si aplica).**
- **Gestión de salas de juego en MongoDB.**
- **Gestión de sesiones y estado de conexión.**
- **Registro de partidas y puntajes por usuario (PostgreSQL).**
- **Comunicación en tiempo real con WebSockets (SignalR).**
- **Manejador de errores global y middleware personalizado.**
- **Separación CQRS para mejorar escalabilidad y mantenimiento.**
- **Pruebas unitarias básicas en la capa de dominio (si se incluyeron).**

## Bases de datos

Este proyecto integra una base de datos relacional (PostgreSQL) y una base de datos no relacional (MongoDB)

### Base de datos relacional (PostgreSQL)

La base de datos relacion esta diseñada en PostgreSQL, es llamada ping_pong, implementa 2 tablas, Scores y Users

- **scores:** La tabla scores almacena los puntos adquiridos por cada jugador, teniendo un historial de todos los puntos adquiridos en las partidas.
- **users** La tabla users almacena la informacion del usuario, como el email, el nickname e informacion de la contraseña la cual se almasena en un hash creado por el aplicativo backend.

para la creacion de esta base de datos y sus tablas se debe tener en cuenta el siguiente script
```
-- Crear la base de datos Postgre
CREATE DATABASE ping_pong;

-- Habilitar extensión para UUID automático
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Crear tabla de usuarios
CREATE TABLE users (
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email       VARCHAR(50) NOT NULL UNIQUE,
    nickname    VARCHAR(50) NOT NULL UNIQUE,
    hash        VARCHAR(80) NOT NULL,
    salt        VARCHAR(80) NOT NULL,
    state       BOOLEAN DEFAULT TRUE,
);

-- Crear tabla de puntajes
CREATE TABLE scores (
    id 			UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id		UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    score 		INTEGER NOT NULL
);

-- Crear índices
CREATE INDEX idx_scores_id ON scores(id);
CREATE INDEX idx_scores_user_id ON scores(user_id);
CREATE INDEX idx_scores_score ON scores(score);
CREATE INDEX idx_users_id ON users(id);
```

### Base de datos NO relacional (MongoDB)

La base de datos NO relacional esta diseñada en MongoDB llamada ping_pong la cual implementa 2 colecciones de datos rooms, sessions.
Se utiliza para gestionar datos dinámicos y en tiempo real relacionados con las salas y sesiones de los jugadores.

- **rooms:** La colección rooms almacena los datos de la sala, id de la sala, los jugadores que se encuentran en la sala y en estado que se encuentra la sala (en espera, jugando o finalizado).
- **sessions** La colección sessions almacena la informacion de la session del usuario.
```
{
  "roomId": "abc123",
  "players": [
    { "playerId": "uuid1", "nickname": "Jugador1" },
    { "playerId": "uuid2", "nickname": "Jugador2" }
  ],
  "state": "playing"
}
```

## Instrucciones de Ejecución

### Clonar el repositorio:
```
{
    git clone https://github.com/hinderman/PingPong-Backend.git
    cd PingPong-Backend
}
```

### Configurar las variables de entorno:
En appsettings.json:
- **Cadena de conexión PostgreSQL.**
- **Cadena de conexión MongoDB.**
- **Configuración JWT.**

### Levantar la API:
```
dotnet run --project PingPong_{Nombre_servicio}_Api
```
se deben agregar todas las capas API de los diferentes servicios por funcionamiento del api gateway

### Pruebas
Se incluyen pruebas unitarias en la capa de dominio (si aplica).
Para ejecutarlas:
```
dotnet test
```