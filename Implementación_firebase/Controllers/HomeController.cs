using Firebase.Auth;
using Firebase.Storage;
using Implementación_firebase.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Implementación_firebase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public bool True { get; private set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            Stream archivoASubir = archivo.OpenReadStream();

            string email = "natalycortez1b@outlook.es";
            string clave = "catolica10";
            string ruta = "gs://subir-archivos-de-mvc.appspot.com\r\n";
            string api_key = "AIzaSyDr61fmZOHdkcsvsie-N9yiMcHtDOULLP8\r\n";
            

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));

            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                ThrowOnCancel = True
            }).Child("Archivos")
            .Child(archivo.FileName)
            .PutAsync(archivoASubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("VerImagen");
        }

        
    }
}
