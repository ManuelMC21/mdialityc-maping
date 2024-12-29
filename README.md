# mdialityc-maping
API RESTfull desarrollada utilizando .NET 7, EntityFrameworkCore y PostrgreSQL. Su propósito principal es la gestión de entidades(restaurantes, cafeterias, bares), con soporte para funcionalidades avanzadas como autentificación, autorización basada en roles, gestión de imagenes asociadas a entidades, rutas dinámicas y posicionamiento geográfico.

# Principales Características:
## 1-Gestión de Usuarios:
  - Registro, login y actualización de información de usuarios
  - Autentificación mediante JWT con soporte para roles (Admin y User)
  - Cambio de contraseñas y recuperación segura de contraseñas
## Gestión de Entidades:
  - CRUD completo para entidades y sus tipos
  - Asignación de entidades a usuarios y control de permisos
  - Soporte para asociar imágenes a entidades, con almacenamiento en carpetas específicas por entidad
## Rutas dinámicas:
  - Cálculo y generación de rutas optimizadas entre puntos geográficos, respetando restricciones como carreteras y caminos
## Subida de Imagenes:
  - Subida de imágenes asociadas a cada entidad en rutas específicas
## Formularios Dinámicos:
  - Creación de formularios personalizados desde ek backoffice con campos y tipos definidos por el usuario
## Documentación con Swagger:
  - Implementación de Swagger UI para la documentación interactiva de la API
  - Soporte para autentificación JWT desde la interfaz de Swagger
  - Configuración personalizada para la carga de archivos

# Tecnologías Utilizadas:
  - **.NET 7:** Framework principal para el desarrollo de la API
  - **EntityFrameworkCore:** ORM para interactuar con la base de datos PostrgreSQL
  - **PostgreSQL:** Base de datos relacional para el almacenamiento de datos
  - **JWT:** Mecanismo de autentificación y autorización segura
  - **Swagger/OpenAPI:** Para la documentación interactiva de la API
  - **Geolocalización:** Implementación de cálculos geográficos, rutas y posicionamiento

# Casos de Uso:
  - Gestión de negocios locales como restaurantes, cafeterías y bares
  - Sistemas de recomendación basados en proximidad
  - Aplicaciones con formularios dinámicos creados desde el backend
  - Administración de usuarios y roles en sistemas multiusuario










