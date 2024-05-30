using Hawi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnidecodeSharpFork;
using static System.Net.Mime.MediaTypeNames;

namespace Hawi.Extensions
{
    public class ImageFunctions
    {
        HawiContext _context = new HawiContext();

        public string ChangeImageName(IFormFile file)
        {
            try
            {
                var originalFileName = file.FileName;
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + originalFileName.Replace(" ", "").Unidecode();
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CheckCountOfImage(byte imageCount)
        {
            if (imageCount != 0)
                if (imageCount > 5) return "الحد الأقصى للصور 5 صور فقط!!";
            return null;
        }

        public string GetInvalidImageMessage(IFormFile image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var maxFileSize = 50000 * 10000;

            if (image == null || image.Length == 0)
            {
                return "الملف غير موجود أو فارغ.";
            }

            var fileExtension = Path.GetExtension(image.FileName)?.ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return $"يتم قبول صيغ الصور التالية فقط: JPG، JPEG، PNG. الملف: {image.FileName}";
            }

            if (image.Length > maxFileSize)
            {
                return $"حجم الملف لا يجب أن يتجاوز 50000 كيلو بايت. الملف: {image.FileName}";
            }

            return null;
        }

        public async Task<string> UploadImageToServerAsync(IFormFile image, string Path)
        {
            try
            {
                using (var stream = new FileStream(Path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> SaveImageInDBAsync(string imageUrlfullPath, string imageFileName, byte imageTypeId, bool isActive)
        {
            try
            {
                var img = new Models.Image
                {
                    ImageUrlfullPath = imageUrlfullPath,
                    ImageFileName = imageFileName,
                    ImageTypeId = imageTypeId,
                    IsActive = isActive
                };

                _context.Images.Add(img);
                await _context.SaveChangesAsync();

                return img.ImageId.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string?> AddImageAlbumImageAsync(long albumId, long imageId)
        {
            try
            {
                var imageAlbumImage = new ImageAlbumImage
                {
                    AlbumId = albumId,
                    ImageId = imageId,
                };

                _context.ImageAlbumImages.Add(imageAlbumImage);
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ValidateVideo(IFormFile videoFile)
        {
            //TimeSpan videoDuration = GetVideoDuration(videoFile);

            string[] allowedExtensions = { ".mp4", ".avi", ".mov" };
            string fileExtension = System.IO.Path.GetExtension(videoFile.FileName);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
                return "لا يسمح إلا بملفات الفيديو بأمتدادات مثل .mp4, .avi, .mov.";


            //if (videoDuration > TimeSpan.FromSeconds(61))
            //    return "!مدة الفديو لا يجب ان تزيد عن دقيقة واحدة";


            if (videoFile.Length > 500 * 1024 * 1024)
                return "!حجم الفيديو يجب ألا يتجاوز 500 ميجابايت.";


            return null;
        }

        public string ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "الملف غير موجود.";


            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

            if (fileExtension != ".pdf" && fileExtension != ".doc" && fileExtension != ".docx")
                return "امتداد الملف غير صالح. الامتدادات المدعومة هي .pdf، .doc، و .docx.";

            long fileSizeInBytes = file.Length;
            long fileSizeInKB = fileSizeInBytes / 1024; // Convert to KB

            if (fileSizeInKB > 500)
                return "حجم الملف يتجاوز 500 كيلوبايت.";

            return null;
        }

        public async Task<Models.Image> AddImageInDB(string imagepathinDB, string NewFileName, byte ImageTypeId)
        {
            var newimag = new Models.Image
            {
                ImageUrlfullPath = imagepathinDB,
                ImageFileName = NewFileName,
                ImageTypeId = ImageTypeId,
                IsActive = true,
            };
            await _context.Images.AddAsync(newimag);
            await _context.SaveChangesAsync();
            return newimag;
        }

        public async Task<UserProfileImage> AddUserProfileImage(long UserProfileId,long ImageId , byte ImageTypeId)
        {
            var UserProfileImage = new UserProfileImage
            {
                UserProfileId = UserProfileId,
                ImageId = ImageId,
                ImageTypeId = ImageTypeId,
            };
           await _context.UserProfileImages.AddAsync(UserProfileImage);
           await _context.SaveChangesAsync();
           return UserProfileImage;
        }
     
        public async Task<string> AddFolderInServer(string FolderPath, string FolderName)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(FolderPath, FolderName));
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
       
        public async Task CheckDirectoryExist(string directoryPath)
         {
                // Check if the directory exists, and create it if it doesn't
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }    
        }

    }
}
