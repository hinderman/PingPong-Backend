# PingPong_Backend

## Descripción

Este proyecto corresponde a una prueba técnica para la empresa Ofima, cuyo objetivo es emular un videojuego de Ping Pong multijugador en tiempo real utilizando WebSockets.
La solución está desarrollada con .NET 9 y sigue una arquitectura limpia con enfoque hexagonal orientada a microservicios, que promueve una separación clara de responsabilidades y facilita la escalabilidad y el mantenimiento del código.
Se integran tanto bases de datos relacionales (PostgreSQL) como no relacionales (MongoDB), y se aplican diversos patrones de diseño y buenas prácticas, entre ellos:
- **Repositorio**
- **CQRS (Command Query Responsibility Segregation)**
- **Mediador (MediatR)**
- **DTOs (Data Transfer Objects)**
- **Fábrica (Factory Pattern)**
El proyecto está orientado a la modularidad, reutilización de componentes y cumplimiento de principios SOLID, asegurando un backend robusto y flexible para el manejo de la lógica del juego y la comunicación en tiempo real.

- **La estructura de carpetas que se manejan por servicios es la siguiente:**
```
{Nombre_servicio}/
├── PingPong_{Nombre_servicio}_Api/
│   ├── Common/
│   ├── Controller/
│   ├── Middleware/
│   ├── Settings/
│   └── appsettings.json/
│
├── PingPong_{Nombre_servicio}_Application/
│   ├── Commands/
│   ├── Dtos/
│   ├── Queries/
│   └── Settings/
│
├── PingPong_{Nombre_servicio}_Domain/
│   ├── Entities/
│   ├── Interfaces/
│   ├── Repositories/
│   ├── Services/
│   └── ValueObjects/
│
└── PingPong_{Nombre_servicio}_Infrastructure/
    ├── Persistence/
    ├── Repositories/
    ├── Services/
    └── Settings/
```

## Bases de datos

Este proyecto integra una base de datos relacional (PostgreSQL) y una base de datos no relacional (MongoDB)

### Base de datos relacional (PostgreSQL)

La base de datos relacion implementa 2 tablas, Scores y Users

- **Scores:** La tabla Scores almacena los puntos adquiridos por cada jugador, teniendo un historial de todos los puntos adquiridos en las partidas
- **Users** La tabla Users almacena la informacion del usuario, como el email, el nickname e informacion de la contraseña la cual se almasena en un hash creado por el aplicativo backend

para la creacion de esta base de datos y sus tablas se debe tener en cuenta el siguiente script
```
-- Crear la base de datos Postgre
CREATE DATABASE ping_pong;

-- Habilitar extensión para UUID automático
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Crear tabla de usuarios
CREATE TABLE users (
    id 			UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email 		VARCHAR(50) NOT NULL UNIQUE,
	nickname	VARCHAR(50) NOT NULL UNIQUE,
    hash 		VARCHAR(80) NOT NULL,
	salt 		VARCHAR(80) NOT NULL,
	state		BOOLEAN DEFAULT TRUE,
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

### Base de datos no relacional (MongoDB)