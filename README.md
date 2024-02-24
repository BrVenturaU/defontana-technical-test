# Prueba Técnica Defontana
Este proyecto da solución a los items solicitados en el documento correspondiente a la prueba técnica para la aplicación a la plaza de Desarrollador Backend .NET.

## Antes de ejecutar
Es importante considerar los siguientes dos aspectos previos a la ejecución del proyecto:

1. El contexto de la base de datos fue generado utilizando el comando `Scaffold-DbContext`.
2. La conexión a la base de datos debe ser agregada a los secretos de usuario (User-Secrets) del proyecto en toda máquina que se ejecute el proyecto.

### Generar Contexto y Entidades
El contexto de base de datos y las entidades ya se encuentran generadas e incluidas en el proyecto junto a algunas ligeras modificaciones para mejorar la lectura del código relacionado a las entidades y sus propiedades, por lo que, no es necesario generar nuevamente estos componentes.
El comando utilizado fue: 
```
Scaffold-DbContext "Server=127.0.0.1;Database=DbName;User=UserName;Password=UserPass" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -ContextDir Contexts -Context AppDbContext -DataAnnotations -Force
```

### Agregar User-Secrets
La cadena de conexión esta agregada a los User-Secrets del proyecto en la maquina local del usuario, no se encuentra en el código fuente directamente, esto es, para evitar el seguimiento por el sistema de control de versiones y no exponerla en el repositorio público. 

Dicho lo anterior, para que el proyecto sea ejecutado con éxito debemos realizar los siguientes pasos:

1. Abrir la consola del administrador de paquetes o una consola en la raíz del proyecto. 
2. Agregar la cadena de conexión a la base de datos en los secretos del usuario.
    2.1 **Agregar los secretos del usuario usando el explorador de soluciones:** Clic derecho sobre el proyecto en el explorador de soluciones > Administrar Secretos de Usuario y agregar el siguiente JSON:
    ```
    {
        "ConnectionStrings": {
            "Default": "Server=127.0.0.1;Database=DbName;User=UserName;Password=UserPass"
        }
    }
    ```
    *Los datos deben ser sustituidos por los valores correspondientes a cada propiedad según la cadena de conexión compartida en la prueba técnica.*
    2.2 **Agregar los secretos del usuario utilizando la consola:** Abrir la Consola del Administrador de Paquetes o una consola en el nivel de la raíz del proyecto y ejecutar el siguiente comando:
    ```
    dotnet user-secrets set "ConnectionStrings:Default" "Server=127.0.0.1;Database=DbName;User=UserName;Password=UserPass"
    ```
    *Los datos deben ser sustituidos por los valores correspondientes a cada propiedad según la cadena de conexión compartida en la prueba técnica.*

## Ejecutar el proyecto
Una vez realizadas las configuraciones previas, el proyecto debería funcionar de manera adecuada.

