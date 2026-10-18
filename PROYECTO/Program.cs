using System;

using System.IO;

using System.Collections.Generic;



namespace MesaDePartesDigital

{

    public class Solicitud

    {

        public string Id { get; set; }

        public string Dni { get; set; }

        public string NombreCompleto { get; set; }

        public string Asunto { get; set; }

        public string FechaRegistro { get; set; }

        public string Estado { get; set; }



        public override string ToString()

        {

            return $"{Id}|{Dni}|{NombreCompleto}|{Asunto}|{FechaRegistro}|{Estado}";

        }



        public static Solicitud DesdeLinea(string linea)

        {

            var partes = linea.Split('|');

            if (partes.Length < 6) return null;

            return new Solicitud

            {

                Id = partes[0],

                Dni = partes[1],

                NombreCompleto = partes[2],

                Asunto = partes[3],

                FechaRegistro = partes[4],

                Estado = partes[5]

            };

        }

    }



    class Program

    {

        private static string rutaArchivo = "solicitudes.txt";



        static void Main(string[] args)

        {

            int opcion = -1;

            do

            {

                Console.Clear();

                Console.WriteLine("========================================");

                Console.WriteLine("  MESA DE PARTES DIGITAL - SISTEMA  ");

                Console.WriteLine("========================================");

                Console.WriteLine("1. Registrar nueva solicitud");

                Console.WriteLine("2. Listar todas las solicitudes");

                Console.WriteLine("3. Buscar solicitud por DNI");

                Console.WriteLine("4. Actualizar estado de solicitud");

                Console.WriteLine("5. Eliminar solicitud");

                Console.WriteLine("6. Ordenar solicitudes");

                Console.WriteLine("0. Salir");

                Console.WriteLine("----------------------------------------");

                Console.Write("Seleccione una opción: ");



                if (!int.TryParse(Console.ReadLine(), out opcion))

                {

                    Console.WriteLine("\n[ERROR] Ingrese un número válido. Presione cualquier tecla...");

                    Console.ReadKey();

                    continue;

                }



                switch (opcion)

                {

                    case 1:

                        RegistrarSolicitud();

                        break;

                    case 2:

                        ListarSolicitudes();

                        break;

                    case 3:

                        BuscarSolicitud();

                        break;

                    case 4:

                        ActualizarSolicitud();

                        break;

                    case 5:

                        EliminarSolicitud();

                        break;

                    case 6:

                        OrdenarSolicitudes();

                        break;

                    case 0:

                        Console.WriteLine("\n¡Gracias por utilizar la Mesa de Partes Digital!");

                        break;

                    default:

                        Console.WriteLine("\n[ERROR] Opción no válida. Presione cualquier tecla...");

                        Console.ReadKey();

                        break;

                }



            } while (opcion != 0);

        }



        private static void RegistrarSolicitud()

        {

            Console.Clear();

            Console.WriteLine("--- REGISTRO DE NUEVA SOLICITUD ---");



            string dni;

            do

            {

                Console.Write("Ingrese DNI (8 dígitos): ");

                dni = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(dni) || dni.Length != 8 || !long.TryParse(dni, out _))

                {

                    Console.WriteLine("[ERROR] El DNI debe contener exactamente 8 números.");

                }

                else

                {

                    break;

                }

            } while (true);



            string nombre;

            do

            {

                Console.Write("Ingrese Nombre Completo: ");

                nombre = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(nombre))

                {

                    Console.WriteLine("[ERROR] El nombre no puede estar vacío.");

                }

                else

                {

                    break;

                }

            } while (true);



            string asunto;

            do

            {

                Console.Write("Ingrese el Asunto / Documento: ");

                asunto = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(asunto))

                {

                    Console.WriteLine("[ERROR] El asunto no puede estar vacío.");

                }

                else

                {

                    break;

                }

            } while (true);



            string id = "SOL-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string estado = "Pendiente";



            Solicitud nuevaSolicitud = new Solicitud

            {

                Id = id,

                Dni = dni,

                NombreCompleto = nombre,

                Asunto = asunto,

                FechaRegistro = fecha,

                Estado = estado

            };



            using (StreamWriter sw = File.AppendText(rutaArchivo))

            {

                sw.WriteLine(nuevaSolicitud.ToString());

            }



            Console.WriteLine("\n----------------------------------------");

            Console.WriteLine("[ÉXITO] Solicitud registrada correctamente.");

            Console.WriteLine($"Código asignado: {id}");

            Console.WriteLine("Presione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }



        private static void ListarSolicitudes()

        {

            Console.Clear();

            Console.WriteLine("--- LISTADO GENERAL DE SOLICITUDES ---");



            if (!File.Exists(rutaArchivo))

            {

                Console.WriteLine("\nNo hay solicitudes registradas aún.");

            }

            else

            {

                string[] lineas = File.ReadAllLines(rutaArchivo);

                if (lineas.Length == 0)

                {

                    Console.WriteLine("\nEl registro está vacío.");

                }

                else

                {

                    Console.WriteLine($"\n{"ID",-20} | {"DNI",-10} | {"NOMBRE",-25} | {"ESTADO",-10}");

                    Console.WriteLine(new string('-', 72));



                    foreach (var linea in lineas)

                    {

                        var sol = Solicitud.DesdeLinea(linea);

                        if (sol != null)

                        {

                            Console.WriteLine($"{sol.Id,-20} | {sol.Dni,-10} | {sol.NombreCompleto,-25} | {sol.Estado,-10}");

                        }

                    }

                }

            }



            Console.WriteLine("\n----------------------------------------");

            Console.WriteLine("Presione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }



        private static void BuscarSolicitud()

        {

            Console.Clear();

            Console.WriteLine("--- BUSCAR SOLICITUD POR DNI ---");

            Console.Write("Ingrese el DNI a buscar: ");

            string dniBuscar = Console.ReadLine()?.Trim();



            if (!File.Exists(rutaArchivo))

            {

                Console.WriteLine("\nNo hay registros guardados.");

            }

            else

            {

                string[] lineas = File.ReadAllLines(rutaArchivo);

                bool encontrado = false;



                foreach (var linea in lineas)

                {

                    var sol = Solicitud.DesdeLinea(linea);

                    if (sol != null && sol.Dni == dniBuscar)

                    {

                        Console.WriteLine("\n----------------------------------------");

                        Console.WriteLine($"Código ID  : {sol.Id}");

                        Console.WriteLine($"DNI     : {sol.Dni}");

                        Console.WriteLine($"Solicitante : {sol.NombreCompleto}");

                        Console.WriteLine($"Asunto   : {sol.Asunto}");

                        Console.WriteLine($"Fecha    : {sol.FechaRegistro}");

                        Console.WriteLine($"Estado   : {sol.Estado}");

                        Console.WriteLine("----------------------------------------");

                        encontrado = true;

                    }

                }



                if (!encontrado)

                {

                    Console.WriteLine($"\n[INFO] No se encontraron solicitudes registradas con el DNI: {dniBuscar}");

                }

            }



            Console.WriteLine("\nPresione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }



        private static void ActualizarSolicitud()

        {

            Console.Clear();

            Console.WriteLine("--- ACTUALIZAR ESTADO DE SOLICITUD ---");

            Console.Write("Ingrese el ID de la solicitud (Ej. SOL-...): ");

            string idBuscar = Console.ReadLine()?.Trim();



            if (!File.Exists(rutaArchivo))

            {

                Console.WriteLine("\nNo hay registros guardados.");

                Console.ReadKey();

                return;

            }



            string[] lineas = File.ReadAllLines(rutaArchivo);

            List<Solicitud> lista = new List<Solicitud>();

            bool encontrado = false;



            foreach (var linea in lineas)

            {

                var sol = Solicitud.DesdeLinea(linea);

                if (sol != null)

                {

                    if (sol.Id.Equals(idBuscar, StringComparison.OrdinalIgnoreCase))

                    {

                        Console.WriteLine($"\nEncontrado: {sol.NombreCompleto} | Estado actual: {sol.Estado}");

                        Console.Write("Ingrese el nuevo estado (Ej. En trámite, Aprobado, Rechazado): ");

                        sol.Estado = Console.ReadLine()?.Trim();

                        encontrado = true;

                    }

                    lista.Add(sol);

                }

            }



            if (encontrado)

            {

                // Sobrescribir archivo con la lista actualizada

                using (StreamWriter sw = new StreamWriter(rutaArchivo, false))

                {

                    foreach (var s in lista)

                    {

                        sw.WriteLine(s.ToString());

                    }

                }

                Console.WriteLine("\n[ÉXITO] Solicitud actualizada correctamente.");

            }

            else

            {

                Console.WriteLine("\n[INFO] No se encontró ninguna solicitud con ese ID.");

            }



            Console.WriteLine("\nPresione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }



        private static void EliminarSolicitud()

        {

            Console.Clear();

            Console.WriteLine("--- ELIMINAR SOLICITUD ---");

            Console.Write("Ingrese el ID de la solicitud a eliminar: ");

            string idBuscar = Console.ReadLine()?.Trim();



            if (!File.Exists(rutaArchivo))

            {

                Console.WriteLine("\nNo hay registros guardados.");

                Console.ReadKey();

                return;

            }



            string[] lineas = File.ReadAllLines(rutaArchivo);

            List<Solicitud> lista = new List<Solicitud>();

            bool encontrado = false;



            foreach (var linea in lineas)

            {

                var sol = Solicitud.DesdeLinea(linea);

                if (sol != null)

                {

                    if (sol.Id.Equals(idBuscar, StringComparison.OrdinalIgnoreCase))

                    {

                        Console.WriteLine($"\nSe eliminará la solicitud de: {sol.NombreCompleto} (Asunto: {sol.Asunto})");

                        encontrado = true;

                        continue; // Omitir para que no se guarde de nuevo

                    }

                    lista.Add(sol);

                }

            }



            if (encontrado)

            {

                using (StreamWriter sw = new StreamWriter(rutaArchivo, false))

                {

                    foreach (var s in lista)

                    {

                        sw.WriteLine(s.ToString());

                    }

                }

                Console.WriteLine("\n[ÉXITO] Solicitud eliminada correctamente.");

            }

            else

            {

                Console.WriteLine("\n[INFO] No se encontró ninguna solicitud con ese ID.");

            }



            Console.WriteLine("\nPresione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }



        private static void OrdenarSolicitudes()

        {

            Console.Clear();

            Console.WriteLine("--- ORDENAR SOLICITUDES ---");



            if (!File.Exists(rutaArchivo))

            {

                Console.WriteLine("\nNo hay registros guardados.");

                Console.ReadKey();

                return;

            }



            string[] lineas = File.ReadAllLines(rutaArchivo);

            List<Solicitud> lista = new List<Solicitud>();



            foreach (var linea in lineas)

            {

                var sol = Solicitud.DesdeLinea(linea);

                if (sol != null) lista.Add(sol);

            }



            if (lista.Count <= 1)

            {

                Console.WriteLine("\nNo hay suficientes registros para ordenar.");

                Console.ReadKey();

                return;

            }



            Console.Write("Ordenar por Nombre (x) | Ordenar por ID (y): ");

            char o = char.Parse(Console.ReadLine() ?? "x");



            // Método Burbuja adaptado para Listas de objetos

            for (int j = 0; j < lista.Count - 1; j++)

            {

                for (int i = 0; i < lista.Count - 1; i++)

                {

                    bool condicion = false;

                    if (o == 'x' || o == 'X')

                    {

                        condicion = string.Compare(lista[i].NombreCompleto, lista[i + 1].NombreCompleto, StringComparison.OrdinalIgnoreCase) > 0;

                    }

                    else

                    {

                        condicion = string.Compare(lista[i].Id, lista[i + 1].Id, StringComparison.OrdinalIgnoreCase) > 0;

                    }



                    if (condicion)

                    {

                        var aux = lista[i];

                        lista[i] = lista[i + 1];

                        lista[i + 1] = aux;

                    }

                }

            }



            // Guardar la lista ordenada de vuelta al archivo

            using (StreamWriter sw = new StreamWriter(rutaArchivo, false))

            {

                foreach (var s in lista)

                {

                    sw.WriteLine(s.ToString());

                }

            }



            Console.WriteLine("\n[ÉXITO] ¡Registros ordenados correctamente!");

            Console.WriteLine("\nPresione cualquier tecla para volver al menú...");

            Console.ReadKey();

        }

    }

}