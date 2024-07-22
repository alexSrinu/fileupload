using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            try
            {
                // Specify the folder path where you want to save the file
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                // If the folder doesn't exist, create it
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Generate a unique file name to prevent overwriting
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Combine the folder path and the file name
                string filePath = Path.Combine(folderPath, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Optionally, you can do something with the file here, like saving its metadata to a database

                ViewBag.Message = "File uploaded successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }
        }
        else
        {
            ViewBag.Message = "Please select a file.";
        }

        return View("Index");
    }
}
