using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Enums
{
    public enum UserRoleEnum
    {
        Lector = 0,//Usuario con acceso de lectura limitada al contenido, visualiza el dashboard y descarga reportes
        Soporte = 10,//Usuarios que únicamente atenderán feedbacks y podrán gestionar usuarios y publicaciones
        Editor = 20,//Usuario con acceso a gestión de contenidos y reportes, no pueden modificar usuarios
        Colaborador = 30,//Usuario que puede manejar todo el contenido pero no cambiar configuraciones o ejecutar tareas/procesos, no puede eliminar al usuario administrador
        Desarrollador = 40,//Usuario con acceso completo al área de administración, no puede eliminar al usuario administrador (es un superuser)
        Administrador = 50//Usuario con acceso completo al área de administración, es el único que puede dar permisos de administrador
    }
}
