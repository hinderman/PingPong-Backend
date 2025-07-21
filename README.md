# PingPong_Backend

## Descripción

Este proyecto corresponde a una prueba técnica para la empresa Ofima, cuyo objetivo es emular un videojuego de Ping Pong multijugador en tiempo real utilizando WebSockets.
La solución está desarrollada con .NET 9 y sigue una arquitectura limpia con enfoque hexagonal orientada a microservicios, que promueve una separación clara de responsabilidades y facilita la escalabilidad y el mantenimiento del código.
El proyecto está orientado a la modularidad, reutilización de componentes y cumplimiento de principios SOLID, asegurando un backend robusto y flexible para el manejo de la lógica del juego y la comunicación en tiempo real.
Se integran tanto bases de datos relacionales (PostgreSQL) como no relacionales (MongoDB), y se aplican diversos patrones de diseño y buenas prácticas, entre ellos:
- **Repositorio**
- **CQRS (Command Query Responsibility Segregation)**
- **Mediador (MediatR)**
- **DTOs (Data Transfer Objects)**
- **Fábrica (Factory Pattern)**

- **La estructura de carpetas que se manejan por servicios es la siguiente:**
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

## Bases de datos

Este proyecto integra una base de datos relacional (PostgreSQL) y una base de datos no relacional (MongoDB)

### Base de datos relacional (PostgreSQL)

La base de datos relacion esta diseñada en PostgreSQL, es llamada ping_pong, implementa 2 tablas, Scores y Users

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

### Base de datos NO relacional (MongoDB)

La base de datos NO relacional esta diseñada en MongoDB llamada ping_pong la cual implementa 2 colecciones de datos rooms, sessions