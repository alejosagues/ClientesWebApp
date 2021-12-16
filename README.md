# ClientesWebApp
Este es un proyecto de Alejo Sagues para presentar como ejercicio de desarrollar una APP Web.

WebApi (Carpetas /Clientes.API y /Clientes.DAL) escrita en C#, usando ASP.NET Core 2.2 y SQLite para la base de datos.
Frontend (Carpeta /Clientes-SPA) escrita en Angular 8.2.2, con Bootstrap 4.6.1 y otros paquetes para mejorar la parte visual.

## Setup
 Luego de clonar el repo, teniendo instalado angular y .NET, abra una consola en la carpeta Clientes-SPA y use el comando:
  
             npm install

 Esto descargara todos los paquetes necesarios

 ===================================

 Ahora navegue a la carpeta Clientes.API y use:
  
             dotnet ef database update

 Y debería encontrar el archivo App.db y enlazarlo.

 ===================================

 Ahora se deberían cambiar las configuraciones de los archivos appsettings.Development.json y appsettings.json
 Luego de esto navegue a la carpeta Clientes.API y corra:

         dotnet run

 Y a la carpeta Clientes-SPA y:

         ng serve

 Si no aparece ningun problema, ingrese http://localhost:4200 en su buscador.

## Estructura de la WebApp
 
 Primero armé el backend en .NET y la base de datos, ahí se manejan todas las requests HTTP, tanto para la data de clientes como para el ingreso de los usuarios. Para el registro, login, y manejo general de usuarios usé una API llamada Identity, que facilita mucho los servicios de autorización y su enlace a la base de datos.
 Archivos importantes: En la carpeta Clientes.API, las carpetas ViewModels/, MailServices/ para el manejo de correos y en Controllers/ están los controladores de usuarios, autorización y clientes, además de uno "Values" que usé para probar este backend.
 También el archivo Startup.cs , que maneja las configuraciones iniciales del programa.

 En Clientes.DAL/ (de Data Access Layer), están las migraciones de la base de datos y las clases públicas de cliente y usuario.

 Todo lo de angular se encuentra en la carpeta Clientes-SPA/ (de Single Page Application), en src/app/ se encuentra la mayoría del frontend. En auth/ están los componentes de lo que tiene que ver con autorización. En pages/ está la página principal y las futuras páginas secundarias en caso de que haya. En shared/ se encuentran los componentes presentes en varias partes del frontend, como lo son el header, los servicios (de autorización y una barra de progreso) y un componente de prueba que hice (column-one).

## Uso de la página
 
 Una vez hosteados el server y la página, se deberá registrarse para acceder a la tabla de clientes. Una vez creado el usuario existe la opción de confirmar el mail, y loguearse. Al hacerlo, el buscador guardará un token JWT para mantenerlo logueado. Esto le deja acceder a la tabla de clientes y sus funciones.

