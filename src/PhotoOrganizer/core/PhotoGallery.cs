using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.core
{
    // Clase para representar los datos de entrada de la imagen
    //public class ImageInputData
    //{
    //    [ImageType(224, 224)]
    //    public Bitmap Image { get; set; }
    //}

    // Clase para representar la predicción de la imagen
    public class ImagePrediction
    {
        [ColumnName("PredictedLabel")]
        public string[] PredictedLabels { get; set; }
    }

    internal class PhotoGallery
    {
        public static void ScanPhotos()
        {
            // Ruta del directorio que contiene las fotos
            string directorioPrincipal = @"E:\Media\Videos_Ant";

            // Verificar si el directorio principal existe
            if (Directory.Exists(directorioPrincipal))
            {
                // Obtener la lista de todos los archivos de imagen en el directorio principal y sus subdirectorios
                string[] archivos = Directory.GetFiles(directorioPrincipal, "*.*", SearchOption.AllDirectories);

                // Recorrer cada archivo
                foreach (string archivo in archivos)
                {
                    try
                    {
                        // Verificar si es un archivo de imagen
                        if (EsArchivoImagen(archivo))
                        {
                            // Obtener la fecha de creación o modificación del archivo
                            DateTime fechaArchivo = File.GetLastWriteTime(archivo);

                            // Crear la ruta de destino para la foto basada en la fecha
                            string destino = Path.Combine(directorioPrincipal, fechaArchivo.Year.ToString(), fechaArchivo.Month.ToString());

                            // Verificar si la carpeta de destino existe, si no, crearla
                            if (!Directory.Exists(destino))
                            {
                                Directory.CreateDirectory(destino);
                            }

                            // Mover el archivo a la carpeta de destino
                            string nuevoArchivo = Path.Combine(destino, Path.GetFileName(archivo));
                            File.Move(archivo, nuevoArchivo);

                            Console.WriteLine($"Foto '{Path.GetFileName(archivo)}' organizada correctamente.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al organizar la foto '{Path.GetFileName(archivo)}': {ex.Message}");
                    }
                }

                Console.WriteLine("Proceso completado.");
            }
            else
            {
                Console.WriteLine("El directorio de fotos especificado no existe.");
            }
        }



        public static void ScanVideos()
        {
            // Ruta del directorio que contiene las fotos
            string directorioPrincipal = @"E:\Media\Videos_Ant";

            // Verificar si el directorio principal existe
            if (Directory.Exists(directorioPrincipal))
            {
                // Obtener la lista de todos los archivos de imagen en el directorio principal y sus subdirectorios
                string[] archivos = Directory.GetFiles(directorioPrincipal, "*.*", SearchOption.AllDirectories);

                // Recorrer cada archivo
                foreach (string archivo in archivos)
                {
                    try
                    {
                        // Verificar si es un archivo de imagen
                        if (EsArchivoVideo(archivo))
                        {
                            // Obtener la fecha de creación o modificación del archivo
                            DateTime fechaArchivo = File.GetLastWriteTime(archivo);

                            // Crear la ruta de destino para la foto basada en la fecha
                            string destino = Path.Combine(directorioPrincipal, fechaArchivo.Year.ToString(), fechaArchivo.Month.ToString());

                            // Verificar si la carpeta de destino existe, si no, crearla
                            if (!Directory.Exists(destino))
                            {
                                Directory.CreateDirectory(destino);
                            }

                            // Mover el archivo a la carpeta de destino
                            string nuevoArchivo = Path.Combine(destino, Path.GetFileName(archivo));
                            File.Move(archivo, nuevoArchivo);

                            Console.WriteLine($"Video '{Path.GetFileName(archivo)}' organizada correctamente.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al organizar el video '{Path.GetFileName(archivo)}': {ex.Message}");
                    }
                }

                Console.WriteLine("Proceso completado.");
            }
            else
            {
                Console.WriteLine("El directorio de videos especificado no existe.");
            }
        }

        public static void DeleteEmptyDirectories()
        {
            // Ruta del directorio que contiene las fotos
            string directorioPrincipal = @"E:\Media";

            // Verificar si el directorio principal existe
            if (Directory.Exists(directorioPrincipal))
            {
                // Eliminar directorios vacíos
                EliminarDirectoriosVacios(directorioPrincipal);

                Console.WriteLine("Proceso completado.");
            }
            else
            {
                Console.WriteLine("El directorio de fotos especificado no existe.");
            }
        }
        static void EliminarDirectoriosVacios(string directorio)
        {
            try
            {
                // Obtener la lista de subdirectorios
                string[] subdirectorios = Directory.GetDirectories(directorio);

                foreach (string subdir in subdirectorios)
                {
                    // Recursivamente eliminar subdirectorios vacíos
                    EliminarDirectoriosVacios(subdir);

                    // Verificar si el directorio está vacío
                    if (Directory.GetFiles(subdir).Length == 0 &&
                        Directory.GetDirectories(subdir).Length == 0)
                    {
                        // Eliminar directorio vacío
                        Directory.Delete(subdir);
                        Console.WriteLine($"Directorio vacío eliminado: {subdir}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar directorio: {ex.Message}");
            }
        }


        static void OrganizarVideosPorFecha(string directorio)
        {
            // Obtener la lista de archivos de video en el directorio
            string[] archivosVideos = Directory.GetFiles(directorio, "*.mp4");

            foreach (string video in archivosVideos)
            {
                try
                {
                    // Obtener la fecha de creación del archivo
                    DateTime fechaCreacion = File.GetCreationTime(video);

                    // Crear una carpeta con el año y el mes si no existe
                    string carpetaDestino = Path.Combine(directorio, fechaCreacion.ToString("yyyy-MM"));
                    Directory.CreateDirectory(carpetaDestino);

                    // Mover el archivo de video a la carpeta correspondiente
                    string nombreArchivo = Path.GetFileName(video);
                    string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);
                    File.Move(video, rutaDestino);

                    Console.WriteLine($"Movido '{nombreArchivo}' a '{carpetaDestino}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar '{video}': {ex.Message}");
                }
            }

            // Procesar subdirectorios recursivamente
            string[] subdirectorios = Directory.GetDirectories(directorio);
            foreach (string subdir in subdirectorios)
            {
                OrganizarVideosPorFecha(subdir);
            }
        }

        // Método para verificar si un archivo es una imagen
        static bool EsArchivoImagen(string archivo)
        {
            string extension = Path.GetExtension(archivo).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp";
        }

        static bool EsArchivoVideo(string archivo)
        {
            string extension = Path.GetExtension(archivo).ToLower();
            return extension == ".mp4" || extension == ".avi" || extension == ".mov";
        }

        //static void EtiquetarFotosAutomaticamente(string[] archivos)
        //{
        //    try
        //    {
        //        // Configurar el entorno de ML.NET
        //        var mlContext = new MLContext();

        //        // Cargar el modelo preentrenado de etiquetado de imágenes
        //        var model = mlContext.Model.Load("ImageClassifierModel.zip", out var modelInputSchema);

        //        // Crear un motor de inferencia
        //        var predictionEngine = mlContext.Model.CreatePredictionEngine<ImageInputData, ImagePrediction>(model);

        //        // Recorrer cada archivo
        //        foreach (string archivo in archivos)
        //        {
        //            try
        //            {
        //                // Verificar si es un archivo de imagen
        //                if (EsArchivoImagen(archivo))
        //                {
        //                    // Realizar predicciones de etiquetado en la imagen
        //                    var prediction = predictionEngine.Predict(new ImageInputData { ImagePath = archivo });

        //                    // Obtener las etiquetas con mayor probabilidad
        //                    var topPredictions = prediction.PredictedLabels.Take(3);

        //                    // Mostrar las etiquetas predichas
        //                    Console.WriteLine($"Etiquetas predichas para '{Path.GetFileName(archivo)}': {string.Join(", ", topPredictions)}");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error al procesar la foto '{Path.GetFileName(archivo)}': {ex.Message}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al etiquetar automáticamente las fotos: {ex.Message}");
        //    }
        //}
    }
}
