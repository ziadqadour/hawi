using AutoMapper;
using Contracts;
using Hawi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Hawi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hawi.Extensions;

namespace Hawi.Controllers
{
    [ApiController]
    [Route("hawi/Events/[action]")]
    public class EventsController : ControllerBase
    {
        private readonly HawiContext _context ;
        private readonly ImageFunctions _ImageFunctions ;

        public EventsController(HawiContext context, ImageFunctions imageFunctions)
        {
            _context = context;
            _ImageFunctions = imageFunctions;
        }

        [HttpGet(Name = "GetActiveEvent")]
        public IActionResult GetActiveEvent()
        {
            try
            {
                var events = _context.Events
                 .Where(e => e.IsActive == true)
                 .OrderByDescending(e => e.StratDate)
                 .Select(e => new
                 {
                     e.EventId,
                     e.IsActive,
                     e.EventPlaceLocation,
                     e.FinishDate,
                     e.StratDate,
                     e.EventText,
                     e.EventTitle,
                     EventVideoUrlfullPath = e.EventVideos.FirstOrDefault().Video.VideoUrl,
                     mainImage = e.EventImages.Any() ? e.EventImages.First().Image.ImageUrlfullPath : null,
                     imagescount = e.EventImages.Count(),
                     e.LastUpdate,
                     UserId = e.UserProfile.UserId
                 })
                 .ToList();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while retrieving active events: {ex.Message}");
            }
        }

        [HttpGet(Name = "GetAllEvent")]
        public IActionResult GetAllEvent(string? status)
        {
            try
            {
                var events = _context.Events
                    .OrderByDescending(e => e.StratDate)
                    .Select(e => new
                    {
                        e.EventId,
                        e.IsActive,
                        e.EventPlaceLocation,
                        e.FinishDate,
                        e.StratDate,
                        e.EventText,
                        e.EventTitle,
                        EventVideoUrlfullPath = e.EventVideos.FirstOrDefault().Video.VideoUrl,
                        mainImage = e.EventImages.Any() ? e.EventImages.First().Image.ImageUrlfullPath : null,
                        imagescount = e.EventImages.Count(),
                        e.LastUpdate,
                        UserId = e.UserProfile.UserId
                    })
                    .ToList();

                if (status == "disactive")
                    events = events.Where(x => x.IsActive == false).ToList();
                
                else if (status == "finished")
                    events = events.Where(x => x.FinishDate <= DateTime.Now).ToList();
                
                else if (status == "notfinished")
                    events = events.Where(x => x.FinishDate > DateTime.Now).ToList();
                
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while retrieving active events: {ex.Message}");
            }
        }

        [HttpGet(Name = "GetEventbyid")]
        public IActionResult GetEventbyid(long eventId)
        {
            try
            {
                var events = _context.Events
                    .Where(e => e.EventId == eventId)
                    .Select(e => new
                    {
                        e.EventId,
                        e.IsActive,
                        e.EventPlaceLocation,
                        e.FinishDate,
                        e.StratDate,
                        e.EventText,
                        e.EventTitle,
                        EventVideoUrlfullPath = e.EventVideos.FirstOrDefault().Video.VideoUrl,
                        mainImage = e.EventImages.Any() ? e.EventImages.First().Image.ImageUrlfullPath : null,
                        Images = e.EventImages.Select(i => i.Image.ImageUrlfullPath),
                        imagescount = e.EventImages.Count(),
                        e.LastUpdate,
                        UserId = e.UserProfile.UserId
                    }).FirstOrDefault();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while retrieving active events: {ex.Message}");
            }
        }

        [HttpPost(Name = "Addevents")]
        public async Task<IActionResult> Addevents(long UserProfileId, [FromForm] AddEventDto EventDto)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validate user and get user profile

                    var entereduser = _context.UserProfiles.Where(x => x.UserProfileId==UserProfileId).FirstOrDefault();
                    if (entereduser == null) return BadRequest("!المستخدم الذى ادخلتة غير موجود");

                    var User = _context.Users.Where(x => x.UserId== entereduser.UserId).FirstOrDefault();
                    var UserPhone = User.Mobile ??null;

                    if (EventDto.Images != null && EventDto.Images.Count > 0)
                    {
                        var countOfImage = _ImageFunctions.CheckCountOfImage((byte)EventDto.Images.Count);
                        if (countOfImage != null) return BadRequest(countOfImage);

                        foreach (var image in EventDto.Images)
                        {
                            var checkimage = _ImageFunctions.GetInvalidImageMessage(image);
                            if (checkimage != null) return BadRequest(checkimage);
                        }
                    }

                    #endregion
                 
                    #region add Event

                    var NewEvent = new Event
                    {
                        UserProfileId = UserProfileId,
                        EventTitle = EventDto.EventTitle,
                        EventText = EventDto.EventText,
                        StratDate = EventDto.StratDate,
                        FinishDate = EventDto.FinishDate,
                        EventPlaceLocation = EventDto.EventPlaceLocation,
                        IsActive = EventDto.IsActive,
                        LastUpdate = DateTime.Now,

                    };
                    _context.Events.Add(NewEvent);
                    await _context.SaveChangesAsync();

                    #endregion
                  
                    #region add images

                    if (EventDto.Images != null && EventDto.Images.Count > 0)
                    {
                        foreach (var image in EventDto.Images)
                        {
                            try
                            {
                                var imagename=_ImageFunctions.ChangeImageName(image);
                                var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\events\" + imagename;
                                            // h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await image.CopyToAsync(stream);
                                }

                                var img = new Image
                                {
                                    ImageUrlfullPath = $"http://mobile.hawisports.com/image/events/" + imagename,
                                    ImageFileName = imagename,
                                    ImageTypeId = EventDto.Images.IndexOf(image) == 0 ? (byte)4 : (byte)5,
                                    IsActive = true,
                                };

                                _context.Images.Add(img);
                                await _context.SaveChangesAsync();

                                var ArticleImage = new EventImage
                                {
                                    EventId = NewEvent.EventId,
                                    ImageId = img.ImageId,
                                    ImageTypeId = img.ImageTypeId,
                                };

                                _context.EventImages.Add(ArticleImage);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                transication.Rollback();
                                return BadRequest($"An error occurred while uploading the image {image.FileName}: {ex.Message}");
                            }
                        }
                    }

                    #endregion
                  
                    #region add video
                    if (!string.IsNullOrEmpty(EventDto.VideoUrl))
                    {
                        try
                        {
                            var video = new Video
                            {
                                VideoUrl = EventDto.VideoUrl,
                                VideoTypeId = 2,
                                IsActive = true,

                            };
                            _context.Videos.Add(video);
                            _context.SaveChanges();

                            var eventVideo = new EventVideo
                            {
                                VideoTypeId = 2,
                                VideoId = video.VideoId,
                                EventId = NewEvent.EventId,
                            };
                            _context.EventVideos.Add(eventVideo);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($"An error occurred while uploading the video: {ex.Message}");
                        }
                    }

                    #endregion

                    transication.Commit();
                    return Ok("تمت إضافة الفعالية بنجاح");

                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest($"An error occurred while uploading the Event: {ex.Message}");
                }
            }
        }
        
        [HttpPut(Name = "EditEvent")]
        public async Task<IActionResult> EditEventAsync(long UserProfileId, long eventId, [FromForm] AddEventDto EventDto)
        {
            using (var transication = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validate user and get user profile
                    var existingEvent = await _context.Events.FindAsync(eventId);
                    if (existingEvent == null) return BadRequest($"الفعالية غير موجودة");

                    var entereduser = _context.UserProfiles.Where(x => x.UserProfileId == UserProfileId).FirstOrDefault();
                    if (entereduser == null) return BadRequest("المستخدم الذى ادخلتة غير موجود");
                    
                    var User = _context.Users.Where(x => x.UserId == entereduser.UserId).FirstOrDefault();
                    var UserPhone = User.Mobile;

                    if (EventDto.Images != null && EventDto.Images.Count > 0)
                    {
                        var countOfImage = _ImageFunctions.CheckCountOfImage((byte)EventDto.Images.Count);
                        if (countOfImage!=null) return BadRequest(countOfImage);
                        foreach (var image in EventDto.Images)
                        {
                            var checkimage = _ImageFunctions.GetInvalidImageMessage(image);
                            if (checkimage != null) return BadRequest(checkimage);
                        }
                    }

                    #endregion

                    #region find and update event

                    existingEvent.EventTitle = EventDto.EventTitle;
                    existingEvent.EventText = EventDto.EventText;
                    existingEvent.StratDate = EventDto.StratDate;
                    existingEvent.FinishDate = EventDto.FinishDate;
                    existingEvent.EventPlaceLocation = EventDto.EventPlaceLocation;
                    existingEvent.IsActive = EventDto.IsActive;
                    existingEvent.LastUpdate = DateTime.Now;

                    await _context.SaveChangesAsync();
                    #endregion

                    #region add or update images

                    if (EventDto.Images != null && EventDto.Images.Count > 0)
                    {

                        // delete existing images
                        var existingEventImages = _context.EventImages.Where(x => x.EventId == existingEvent.EventId).ToList();
                        foreach (var existingImage in existingEventImages)
                        {
                            var img = await _context.Images.FindAsync(existingImage.ImageId);
                            if (img != null)
                            {
                                var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\events\" + img.ImageFileName;
                                _context.EventImages.Remove(existingImage);
                                _context.Images.Remove(img);
                                _context.SaveChanges();
                                if (System.IO.File.Exists(path))
                                    System.IO.File.Delete(path);
                            }
                        }

                        // add new images
                        foreach (var image in EventDto.Images)
                        {
                            try
                            {
                                var imagename = _ImageFunctions.ChangeImageName(image);
                                var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\events\" + imagename;
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await image.CopyToAsync(stream);
                                }

                                var img = new Image
                                {
                                    ImageUrlfullPath = $"http://mobile.hawisports.com/image/events/" + imagename,
                                    ImageFileName = imagename,
                                    ImageTypeId = EventDto.Images.IndexOf(image) == 0 ? (byte)4 : (byte)5,
                                    IsActive = true,
                                };

                                _context.Images.Add(img);
                                await _context.SaveChangesAsync();

                                var ArticleImage = new EventImage
                                {
                                    EventId = eventId,
                                    ImageId = img.ImageId,
                                    ImageTypeId = img.ImageTypeId,
                                };

                                _context.EventImages.Add(ArticleImage);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                transication.Rollback();
                                return BadRequest($"An error occurred while uploading the image {image.FileName}: {ex.Message}");
                            }
                        }
                    }
                    #endregion

                    #region add or update  video

                    var existingEventVideo = _context.EventVideos.Where(x => x.EventId == existingEvent.EventId).FirstOrDefault();
                    if (existingEventVideo != null)
                    {
                        var selectedvedio = await _context.Videos.FindAsync(existingEventVideo.Video);
                        if (selectedvedio != null)
                        {
                            _context.Videos.Remove(selectedvedio);
                            _context.SaveChanges();
                        }
                        _context.EventVideos.Remove(existingEventVideo);
                        _context.SaveChanges();
                    }
                    if (!string.IsNullOrEmpty(EventDto.VideoUrl))
                    {
                        try
                        {
                            var video = new Video
                            {
                                VideoUrl = EventDto.VideoUrl,
                                VideoTypeId = 2,
                                IsActive = true,

                            };
                            _context.Videos.Add(video);
                            _context.SaveChanges();

                            var eventVideo = new EventVideo
                            {
                                VideoTypeId = 2,
                                VideoId = video.VideoId,
                                EventId = eventId,
                            };
                            _context.EventVideos.Add(eventVideo);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($"An error occurred while uploading the video: {ex.Message}");
                        }
                    }

                    #endregion
                    
                    transication.Commit();
                    return Ok("تمت تعديل الفعالية بنجاح");

                }
                catch (Exception ex)
                {
                    transication.Rollback();
                    return BadRequest($"An error occurred while Editing the Event: {ex.Message}");
                }
            }
        }

        [HttpPut( Name = "disActiveEvent")]
        public IActionResult disActiveEvent(long eventId)
        {
            try
            {
                var eventToUpdate = _context.Events.FirstOrDefault(e => e.EventId == eventId);

                if (eventToUpdate == null)
                {
                    return NotFound("لم يتم العثور على الفعالية");
                }
                if(eventToUpdate.IsActive==true)
                eventToUpdate.IsActive = false;
                else
                    eventToUpdate.IsActive = true;
                _context.SaveChanges();

                return Ok("تم التعديل بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating event with ID {eventId}: {ex.Message}");
            }
        }

        [HttpPut(Name = "ActiveEvent")]
        public IActionResult ActiveEvent(long eventId)
        {
            try
            {
                var eventToUpdate = _context.Events.FirstOrDefault(e => e.EventId == eventId);
                if (eventToUpdate == null)
                    return NotFound($"Event with ID {eventId} not found");
                
                eventToUpdate.IsActive = true;
                _context.SaveChanges();

                return Ok($"Event with ID {eventId} has been updated to inactive");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating event with ID {eventId}: {ex.Message}");
            }
        }

        [HttpDelete( Name = "DeleteEvent")]
        public async Task<IActionResult> DeleteEvent(long userprofileid, long eventId)
        {
            try
            {
                var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(userprofileid)).FirstOrDefault();
                if (entereduser == null) return NotFound("المستخدم الذى ادخلتة غير موجود");
               
                var user = _context.Users.Where(x => x.UserId == entereduser.UserId).FirstOrDefault();
                var UserPhone = user.Mobile;

                var existingEvent = await _context.Events.FindAsync(eventId);
                if (existingEvent == null) return NotFound("الفعالية التي تريد حذفها غير موجودة");
                

                // delete all associated images
                var eventImages = _context.EventImages.Where(e => e.EventId == eventId).ToList();
                foreach (var eventImage in eventImages)
                {
                    var image = _context.Images.Find(eventImage.ImageId);
                    if (image != null)
                    {
                        // delete image file from disk
                        var path = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\events\" + image.ImageFileName;
                                 //$@"h:\root\home\atmbank-001\www\site1\wwwroot\"
                        _context.EventImages.Remove(eventImage);
                        _context.Images.Remove(image);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        _context.SaveChanges();
                    }
                }

                    // delete all associated videos
                var eventVideos =  _context.EventVideos.Where(e => e.EventId == eventId).FirstOrDefault();
                if (eventVideos != null)
                {
                    var video = await _context.Videos.FindAsync(eventVideos.VideoId);
                    _context.EventVideos.Remove(eventVideos);
                    _context.Videos.Remove(video);
                    _context.SaveChanges();
                }

                // delete the event
                _context.Events.Remove(existingEvent);
                _context.SaveChanges();

                return Ok("تم حذف الفعالية بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the event: {ex.Message}");
            }
        }
    }
}

