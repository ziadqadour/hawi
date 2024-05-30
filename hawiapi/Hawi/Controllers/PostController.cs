using Hawi.Dtos;
using Hawi.Extensions;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hawi.Controllers
{
    [Route("hawi/Post/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly HawiContext _context;
        private readonly ImageFunctions _imageFunctions;
        private readonly UserFunctions _UserFunctions;
        private readonly ArticleFunctions _ArticleFunctions;
        public PostController(UserFunctions userFunctions, HawiContext context, ImageFunctions imageFunctions, ArticleFunctions articleFunctions)
        {
            _UserFunctions = userFunctions;
            _context = context;
            _imageFunctions = imageFunctions;
            _ArticleFunctions = articleFunctions;
        }


        //i start refactor in that but not complete yet 
        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> GetPosts([FromQuery] OwnerParameters ownerParameters, long userProfileId)
        {
            try
            {
                var postsQuery = _context.Articles
                    .Where(a =>
                        !a.ArticleHides.Any(h => h.UserProfileId == userProfileId) &&
                        (
                            (a.UserProfile.RoleId != 6 && a.UserProfileId == userProfileId) ||
                            (a.UserProfile.SubscriptionTargetUserProfiles.Any(sub => sub.SourceUserProfileId == userProfileId))
                        )
                    )
                    .OrderByDescending(x => x.ArticleCreateDate)
                    .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                    .Take(ownerParameters.PageSize);


                //check Activate of post and user before showing it 
                var posts = postsQuery.Where(x => x.UserProfile.User.IsActive == true && x.UserProfile.IsActive == true && x.IsActive == true).ToList();

                var result = posts.Select(post => _ArticleFunctions.MapToGetPostsDto(post, userProfileId)).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetPostsofAdmin")]
        public async Task<IActionResult> GetPostsofAdmin([FromQuery] OwnerParameters ownerParameters, long userProfileId)
        {
            try
            {
                var postsQuery = await _context.Articles
                .Where(x => x.UserProfile.RoleId == 6)
                .OrderByDescending(x => x.ArticleCreateDate)
                .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                .Take(ownerParameters.PageSize)
                .ToListAsync();

                //check Activate of post and user before showing it 
                var posts = postsQuery.Where(x => x.IsActive == true).ToList();

                var result = posts.Select(post => _ArticleFunctions.MapToGetPostsDto(post, userProfileId)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetSpecificUserPosts")]
        public async Task<IActionResult> GetSpecificUserPosts([FromQuery] OwnerParameters ownerParameters, long UserProfileIdofpost, long UserProfileIdThatshowPosts)
        {
            try
            {
                var userProfile = await _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileIdofpost)).FirstOrDefaultAsync();
                if (userProfile == null)
                    return BadRequest("!المستخدم صاحب البروفايل الذى ادخلتة غير موجود");

                var userThatShowProfile = await _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileIdThatshowPosts)).FirstOrDefaultAsync();

                var postsQuery = new List<Article>();

                if (userThatShowProfile != null)
                {
                    //if user not enter as visitor and return post without hidden posts
                    //get post for profile without heddienpost
                    postsQuery = await _context.Articles
                     .Where(a => !a.ArticleHides.Any(h => h.UserProfileId == UserProfileIdThatshowPosts) && a.UserProfileId == UserProfileIdofpost)
                     .OrderByDescending(x => x.ArticleCreateDate)
                     .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                     .Take(ownerParameters.PageSize)
                     .ToListAsync();
                }
                else
                {
                    postsQuery = await _context.Articles
                         .OrderByDescending(x => x.ArticleCreateDate)
                         .Where(a => a.UserProfileId == UserProfileIdofpost)
                         .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
                         .Take(ownerParameters.PageSize)
                         .ToListAsync();
                }

                var posts = postsQuery.Where(x => x.IsActive == true).ToList();

                var result = posts.Select(post => _ArticleFunctions.MapToGetPostsDto(post, UserProfileIdThatshowPosts)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetArchiveArticleSOfUser")]
        public IActionResult GetArchiveArticleSOfUser(long UserProfileId)
        {
            //   var posts = _context.ArticleUsersSaveds.Where(x => x.UserProfileId == UserProfileId).ToList();
            var savedArticles = _context.ArticleUsersSaveds
             .Include(au => au.Article)
             .Where(au => au.UserProfileId == UserProfileId)
             .Select(au => au.Article)
             .ToList();

            var result = savedArticles.Select(post => _ArticleFunctions.MapToGetPostsDto(post, UserProfileId)).ToList();
            return Ok(result);
        }

        [HttpGet(Name = "GetPostById")]
        public async Task<IActionResult> GetPostById(long ArticleId, long userProfilId)
        {
            try
            {
                var Article = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (Article == null) return BadRequest($"!رقم المقال الذى ادخلتة غير موجود");

                var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(userProfilId)).FirstOrDefault();
                if (entereduser == null) return BadRequest("!المستخدم الذى ادخلتة غير موجود");

                var postResult = await _ArticleFunctions.GetPostByID(Article, userProfilId);

                return Ok(postResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetCommentsOfPost")]
        public async Task<IActionResult> GetCommentsOfPost(long articleId, long userProfileId)
        {
            try
            {
                var article = await _context.Articles.FirstOrDefaultAsync(x => x.ArticleId == articleId);
                if (article == null)
                    return BadRequest("لم يتم العثور على المقال!");

                var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == userProfileId);
                if (user == null)
                    return BadRequest("لم يتم العثور على المستخدم!");

                var commentForPost = await _context.ArticleComments
                    .Where(x => x.ArticleId == articleId &&
                                x.CommentUserProfile.User.IsActive == true &&
                                x.CommentUserProfile.IsActive == true)
                    .ToListAsync();

                var commentDtos = commentForPost.Select(postComment =>
                {
                    var userProfile = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == postComment.CommentUserProfileId);
                    var role = userProfile?.RoleId;
                    var userId = userProfile?.UserId;

                    return new CommentDto
                    {
                        UserId = (long)userId,
                        UserProfileId = postComment.CommentUserProfileId,
                        username = _UserFunctions.GetUserName((byte)role, postComment.CommentUserProfileId, (long)userId),
                        userRole = _context.Roles.FirstOrDefault(x => x.RoleId == role)?.Role1,
                        UserImage = _UserFunctions.GetUserImage(postComment.CommentUserProfileId, (byte)role),
                        Comment = postComment.Comment,
                        CommentlikesCount = _context.ArticleCommentLikes.Count(x => x.CommentId == postComment.CommentId),
                        UserLikeReact = _context.ArticleCommentLikes.Any(l => l.UserProfileId == userProfileId && l.CommentId == postComment.CommentId),
                        CreateDate = postComment.CreateDate,
                        commentId = postComment.CommentId
                    };
                }).ToList();

                return Ok(commentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"حدث خطأ غير متوقع أثناء معالجة الطلب: {ex.Message}");
            }
        }

        [HttpGet(Name = "ShowCommentLikeDetails")]
        public async Task<IActionResult> ShowCommentLikeDetails(long CommentId)
        {
            var comment = await _context.ArticleComments
                .Where(x => x.CommentId == CommentId)
                .FirstOrDefaultAsync();

            if (comment == null)
                return BadRequest("لم يتم العثور على التعليق الذى ادخلتة!");

            var GetCommentLikeDetails = await _ArticleFunctions.GetCommentLikeDetails(CommentId);

            return Ok(GetCommentLikeDetails);
        }

        [HttpGet(Name = "LikesCommentsCounter")]
        public async Task<IActionResult> LikesCommentsCounter(long articleId)
        {
            var article = await _context.Articles.Where(x => x.ArticleId == articleId).FirstOrDefaultAsync();
            if (article == null)
                return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

            var likesCount = await _context.ArticleLikes.Where(x => x.ArticleId == articleId).CountAsync();
            var commentCount = await _context.ArticleComments.Where(x => x.ArticleId == articleId).CountAsync();

            var countDetails = new PostLikeANDCommentDetailesDTO
            {
                likescount = likesCount,
                Commintcount = commentCount,
            };

            return Ok(countDetails);
        }

        [HttpGet(Name = "ShowPostLikeDetails")]
        public async Task<IActionResult> ShowPostLikeDetails(long ArticleId)
        {

            var article = await _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefaultAsync();
            if (article == null)
                return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

            var GetPostLikeDetails = await _ArticleFunctions.GetPostLikeDetails(ArticleId);
            return Ok(GetPostLikeDetails);
        }

        [HttpGet(Name = "GetAllArticlesNotificationReasons")]
        public async Task<IActionResult> GetAllArticlesNotificationReasons()
        {
            var notificationReasons = await _context.ArticleNotificationNotificationReasons
                  .Select(nr => new
                  {
                      nr.NotificationReasonId,
                      nr.NotificationReason
                  }).ToListAsync();

            return Ok(notificationReasons);
        }

        [HttpGet(Name = "GetReportArticles")]
        public async Task<IActionResult> GetReportArticles()
        {
            try
            {
                var result = await _ArticleFunctions.GetReportArticles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetSpecieficArticleNotificationMemo")]
        public async Task<IActionResult> GetSpecieficArticleNotificationMemo(long ArticleId)
        {
            try
            {
                var article = await _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefaultAsync();
                if (article == null)
                    return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");


                var Notificationposts = _context.ArticleNotifications.Where(x => x.ArticleId == ArticleId)
                    .Select(x => new
                    {
                        Image = _UserFunctions.GetUserImage(x.UserProfileId, x.UserProfile.RoleId),
                        Targetuserprofileid = x.UserProfileId,

                        TargetUserRole = _context.Roles.FirstOrDefault(x => x.RoleId.Equals(x.RoleId)).Role1,

                        TargetUserName = _context.Users.FirstOrDefault(x => x.UserId.Equals(_context.UserProfiles.FirstOrDefault(x => x.UserProfileId.Equals(x.UserProfileId)).UserId)).Name,

                        ArticlNotificationDate = x.CreateDate,

                        NotificationMemo = (x.NotificationReasonId == 4) ? x.NotificationMemo :
                       (_context.ArticleNotificationNotificationReasons.FirstOrDefault(reason => reason.NotificationReasonId == x.NotificationReasonId) != null)
                        ? _context.ArticleNotificationNotificationReasons.FirstOrDefault(reason => reason.NotificationReasonId == x.NotificationReasonId).NotificationReason
                        : null,
                    }).ToList();

                return Ok(Notificationposts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetReportComments")]
        public async Task<IActionResult> GetReportComments()
        {
            var result = await _ArticleFunctions.GetReportComments();
            return Ok(result);
        }

        //
        [HttpPost(Name = "AddPost")]
        public async Task<IActionResult> AddPostAsync(long UserProfileId, [FromForm] CreatePostDto post)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validate and add article
                    var entereduser = await _context.UserProfiles.Where(x => x.UserProfileId == (UserProfileId)).FirstOrDefaultAsync();
                    if (entereduser == null) return BadRequest("!المستخدم الذى ادخلتة غير موجود");

                    var UserPhone = _context.Users.FirstOrDefault(x => x.UserId == entereduser.UserId).Mobile;

                    var ValidateImages = await _ArticleFunctions.CheckValidateImageOfPost(post.Images);
                    if (ValidateImages != null) return BadRequest($"{ValidateImages}");

                    var Newpost = await _ArticleFunctions.MapToAddArticle(UserProfileId, post);
                    await _context.SaveChangesAsync();
                    #endregion

                    #region add images

                    if (post.Images != null && post.Images.Count > 0)
                    {
                        foreach (var image in post.Images)
                        {
                            try
                            {
                                string GenerateNewName = _imageFunctions.ChangeImageName(image);
                                string userImageDirectory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}";
                                string imagePath = Path.Combine(userImageDirectory, GenerateNewName);

                                await _imageFunctions.CheckDirectoryExist(userImageDirectory);
                                var uploadImageResult = await _imageFunctions.UploadImageToServerAsync(image, imagePath);

                                if (uploadImageResult != null)
                                {
                                    return BadRequest(uploadImageResult);
                                }

                                var imageType = (byte)(post.Images.IndexOf(image) == 0 ? 2 : 3);

                                var img = new Models.Image
                                {
                                    ImageUrlfullPath = $"http://mobile.hawisports.com/image/Users/{UserPhone}/{GenerateNewName}",
                                    ImageFileName = GenerateNewName,
                                    ImageTypeId = imageType,
                                    IsActive = true,
                                };

                                await _context.Images.AddAsync(img);
                                await _context.SaveChangesAsync();

                                var articleImage = new ArticleImage
                                {
                                    ArticleId = Newpost,
                                    ImageId = img.ImageId,
                                    ImageTypeId = img.ImageTypeId,
                                };

                                await _context.ArticleImages.AddAsync(articleImage);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                return BadRequest($"حدث خطا اثناء اضافة الصورة {image.FileName}");
                            }
                        }
                    }

                    #endregion

                    #region add video
                    if (post.Video != null)
                    {
                        try
                        {
                            var videoValidationResult = _imageFunctions.ValidateVideo(post.Video);
                            if (videoValidationResult != null)
                                return BadRequest(videoValidationResult);

                            string newVideoName = _imageFunctions.ChangeImageName(post.Video);
                            string userVideoDirectory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}";
                            string videoPath = Path.Combine(userVideoDirectory, newVideoName);

                            await _imageFunctions.CheckDirectoryExist(userVideoDirectory);

                            var uploadVideoResult = await _imageFunctions.UploadImageToServerAsync(post.Video, videoPath);

                            if (uploadVideoResult != null)
                                return BadRequest("!حدث خطا اثناء رفع الفديو ");


                            var video = new Video
                            {
                                VideoFileName = newVideoName,
                                VideoUrl = $"http://mobile.hawisports.com/image/Users/{UserPhone}/{newVideoName}",
                                VideoTypeId = 1,
                                IsActive = true,
                            };

                            _context.Videos.Add(video);
                            await _context.SaveChangesAsync();

                            var articleVideo = new ArticleVideo
                            {
                                VideoTypeId = 1,
                                VideoId = video.VideoId,
                                ArticleId = Newpost,
                            };

                            _context.ArticleVideos.Add(articleVideo);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($"حدث خطا اثناء رفع الفديو: {ex.Message}");
                        }
                    }

                    #endregion

                    #region AddVideoLink

                    if (post.VideoUrlfullPathFromAnotherPlatform != null)
                    {
                        var video = new Video
                        {
                            VideoFileName = null,
                            VideoUrl = post.VideoUrlfullPathFromAnotherPlatform,
                            VideoTypeId = 5,
                            IsActive = true,
                        };

                        await _context.Videos.AddAsync(video);
                        await _context.SaveChangesAsync();

                        var articleVideo = new ArticleVideo
                        {
                            VideoTypeId = 5,
                            VideoId = video.VideoId,
                            ArticleId = Newpost,
                        };

                        await _context.ArticleVideos.AddAsync(articleVideo);
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region save
                    _context.SaveChanges();
                    transiction.Commit();
                    return Ok("تمت إضافة المقال بنجاح");
                    #endregion
                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest($"حدث خطا اثناء اضافة المقال: {ex.Message}");
                }
            }
        }

        [HttpPost(Name = "AddComment")]
        public async Task<IActionResult> AddComment(long ArticleId, long UserProfileId, [FromBody] CreateCommentDto comment)
        {
            try
            {
                var article = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (article != null) return BadRequest($"{article}");

                var user = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (user != null) return BadRequest($"{user}");

                ArticleComment articleComment = new ArticleComment
                {
                    Comment = comment.Comment,
                    ArticleId = ArticleId,
                    CommentUserProfileId = UserProfileId,
                };
                await _context.ArticleComments.AddAsync(articleComment);
                await _context.SaveChangesAsync();
                return Ok("تم اضافة تعليقك بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddArticleCommentLike")]
        public async Task<IActionResult> AddArticleCommentLike(long ArticleId, long CommentId, long UserProfileId)
        {
            try
            {//add and delete

                var article = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (article != null) return BadRequest($"{article}");


                var user = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (user != null) return BadRequest($"{user}");

                var comment = await _ArticleFunctions.CheckExistComment(CommentId);
                if (comment != null) return BadRequest($"{comment}");

                var userCommentlike = await _context.ArticleCommentLikes.Where(l => l.UserProfileId == UserProfileId && l.CommentId == CommentId).FirstOrDefaultAsync();

                if (userCommentlike != null)
                {//delete
                    if (userCommentlike.UserProfileId != UserProfileId)
                        return BadRequest("!لا يمكن حذف تفاعل غير مملوك لك");

                    _context.ArticleCommentLikes.Remove(userCommentlike);
                }
                else
                {//add
                    var newArticleCommentLike = new ArticleCommentLike
                    {
                        CommentId = CommentId,
                        UserProfileId = UserProfileId
                    };
                    await _context.ArticleCommentLikes.AddAsync(newArticleCommentLike);
                }
                await _context.SaveChangesAsync();
                return Ok("تمت  بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "AddUserLike")]
        public async Task<IActionResult> AddLike(long UserProfileId, long ArticleId)
        {
            try
            {//add and delete
                var article = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (article != null) return BadRequest($"{article}");

                var user = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (user != null) return BadRequest($"{user}");

                var checkExistLikeOrNot = await _context.ArticleLikes.Where(l => l.LikeUserProfileId == UserProfileId && l.ArticleId == ArticleId).FirstOrDefaultAsync();
                if (checkExistLikeOrNot != null)
                    _context.ArticleLikes.Remove(checkExistLikeOrNot);
                else
                {
                    var ArticleLike = new ArticleLike
                    {
                        LikeUserProfileId = UserProfileId,
                        ArticleId = ArticleId,
                    };
                    await _context.ArticleLikes.AddAsync(ArticleLike);
                }
                await _context.SaveChangesAsync();
                return Ok("تمت بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "ReportArticles")]
        public async Task<IActionResult> ReportArticles(long ArticleId, long UserProfileId, [FromBody] ReportArticlesDto ReportArticle)
        {
            try
            {
                var article = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (article != null) return BadRequest($"{article}");

                var user = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (user != null) return BadRequest($"{user}");//UserId who report the post 

                var NewReportArticle = new ArticleNotification
                {
                    ArticleId = ArticleId,
                    UserProfileId = UserProfileId,
                    NotificationReasonId = ReportArticle.NotificationReasonId,
                    NotificationMemo = ReportArticle.NotificationMemo,
                };
                await _context.ArticleNotifications.AddAsync(NewReportArticle);
                await _context.SaveChangesAsync();
                return Ok("تم الابلاغ عن المقال بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(Name = "ReportComment")]
        public async Task<IActionResult> ReportComment(long ArticleId, long CommentId, long UserProfileId, [FromBody] ReportCommentDto ReportComment)
        {
            try
            {
                var article = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (article != null) return BadRequest($"{article}");


                var user = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (user != null) return BadRequest($"{user}");

                var comment = await _ArticleFunctions.CheckExistComment(CommentId);
                if (comment != null) return BadRequest($"{comment}");


                var newReportedArticle = new ArticleCommentNotification
                {
                    CommentId = CommentId,
                    UserProfileId = UserProfileId,
                    CommentNotificationReasonId = ReportComment.CommentNotificationReasonId,
                    CommentNotificationMemo = ReportComment.CommentNotificationMemo,
                };
                await _context.ArticleCommentNotifications.AddAsync(newReportedArticle);
                await _context.SaveChangesAsync();
                return Ok("تم الابلاغ عن التعليق بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(Name = "ArchiveArticle")]
        public async Task<IActionResult> ArchiveArticle(long ArticleId, long UserProfileId)
        {
            try
            {
                var Checkarticle = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (Checkarticle != null) return BadRequest($"{Checkarticle}");

                var Checkuser = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (Checkuser != null) return BadRequest($"{Checkuser}");

                var selectedarchievearticle = await _ArticleFunctions.CheckExistSavedArticle(ArticleId, UserProfileId);
                if (selectedarchievearticle != null) return Ok($"{selectedarchievearticle}");

                var ArchiveArticle = new ArticleUsersSaved
                {
                    ArticleId = ArticleId,
                    UserProfileId = UserProfileId,
                };
                await _context.ArticleUsersSaveds.AddAsync(ArchiveArticle);
                await _context.SaveChangesAsync();
                return Ok("تم حفظ المقال بنجاح ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "ArticleHide")]
        public async Task<IActionResult> ArticleHide(long ArticleId, long UserProfileId)
        {
            try
            {
                var Checkarticle = await _ArticleFunctions.CheckExistArticle(ArticleId);
                if (Checkarticle != null) return BadRequest($"{Checkarticle}");

                var Checkuser = await _UserFunctions.CheckUserProfileExists(UserProfileId);
                if (Checkuser != null) return BadRequest($"{Checkuser}");

                var checkArticleHide = await _ArticleFunctions.CheckExistHideArticle(ArticleId, UserProfileId);
                if (checkArticleHide != null) return Ok($"{checkArticleHide}");

                var newArticleHide = new ArticleHide
                {
                    UserProfileId = UserProfileId,
                    ArticleId = ArticleId,
                };
                await _context.ArticleHides.AddAsync(newArticleHide);
                await _context.SaveChangesAsync();
                return Ok("تم تنفيذ طلبك بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //
        //complete refactor from here
        [HttpPut(Name = "EditPost")]
        public async Task<IActionResult> EditPost(long UserProfileId, long ArticleId, [FromForm] EditPostDto post)
        {
            using (var transiction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region validation
                    if (post.StillImagesAfterModification != null)
                    {
                        for (int i = 0; i < post.StillImagesAfterModification.Count; i++)
                        {
                            post.StillImagesAfterModification[i] = post.StillImagesAfterModification[i].Trim('"');
                        }
                    }
                    var article = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                    if (article == null)
                        return BadRequest("لم يتم العثور على المقال.");

                    var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileId)).FirstOrDefault();
                    if (entereduser == null)
                        return BadRequest("المستخدم الذى ادخلتة غير موجود");

                    var UserPhone = _context.Users.FirstOrDefault(x => x.UserId == entereduser.UserId).Mobile;

                    if (article.UserProfileId != UserProfileId)
                        return BadRequest("لا يمكن تعديل مقال غير ملكك");

                    int count = 0;
                    // if (post.mainformFileIfchange != null) count++;
                    if (post.StillImagesAfterModification != null) count += post.StillImagesAfterModification.Count;
                    if (post.Images != null) count += post.Images.Count;
                    if (count > 5)
                        return BadRequest("الحد الاقصى للصور الخاصة بالمقال 5 صور فقط!");


                    if (post.Images != null)
                    {
                        foreach (var image in post.Images)
                        {
                            if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(image.FileName).ToLower()))
                                return BadRequest($"يتم قبول صيغ الصور التالية فقط: JPG، JPEG، PNG, {image.FileName}غير معوم!");
                            if (image.Length > 200 * 1024)
                                return BadRequest($"حجم الملف لا يجب أن يتجاوز 200 كيلو بايت,{image.FileName}.");
                        }
                    }
                    #endregion

                    #region VideoUrlfullPathFromAnotherPlatform

                    if (post.VideoUrlfullPathFromAnotherPlatform != null)
                    {
                        //video link from another place
                        var VideoFromAnotherPlatform = _context.ArticleVideos.FirstOrDefault(x => x.ArticleId == ArticleId && x.VideoTypeId == 5);
                        if (VideoFromAnotherPlatform != null)
                        {
                            var VideoUrlfullPathFromAnotherPlatform = _context.Videos.Where(x => x.VideoId == VideoFromAnotherPlatform.VideoId && x.VideoTypeId == 5).FirstOrDefault();
                            if (VideoUrlfullPathFromAnotherPlatform != null)
                                VideoUrlfullPathFromAnotherPlatform.VideoUrl = post.VideoUrlfullPathFromAnotherPlatform;
                        }
                        else
                        {
                            var video = new Video
                            {
                                VideoFileName = null,
                                VideoUrl = $"{post.VideoUrlfullPathFromAnotherPlatform}",
                                VideoTypeId = 5,
                                IsActive = true,
                            };
                            _context.Videos.Add(video);
                            _context.SaveChanges();

                            var articleVideo = new ArticleVideo
                            {
                                VideoTypeId = 5,
                                VideoId = video.VideoId,
                                ArticleId = ArticleId,
                            };
                            _context.ArticleVideos.Add(articleVideo);
                        }
                        _context.SaveChanges();
                    }
                    #endregion

                    #region updateVideioUrl

                    if (post.Video != null || post.StillVideoAfterModification != null)
                    {
                        if (post.Video != null && post.StillVideoAfterModification != null)
                            return BadRequest("!الحد الاقصى فديو واحد فقط");


                        // if there is videio in db and no StillVideoAfterModification (mean that delete videio)
                        var articleVid = _context.ArticleVideos.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                        if (articleVid != null)
                        {
                            if (post.StillVideoAfterModification == null)
                            {
                                var selectedvideio = _context.Videos.Where(x => x.VideoId == articleVid.VideoId).FirstOrDefault();
                                if (selectedvideio != null)
                                {
                                    var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\" + selectedvideio.VideoFileName;

                                    _context.ArticleVideos.Remove(articleVid);
                                    _context.Videos.Remove(selectedvideio);
                                    _context.SaveChanges();
                                    if (System.IO.File.Exists(imagePath))
                                    {
                                        System.IO.File.Delete(imagePath);
                                    }
                                }
                            }
                        }

                        if (post.Video != null)
                        {
                            try
                            {
                                var validate = _imageFunctions.ValidateVideo(post.Video);
                                if (validate != null)
                                    return BadRequest(validate);

                                var newname = _imageFunctions.ChangeImageName(post.Video);


                                var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\";
                                await _imageFunctions.CheckDirectoryExist(directory);
                                var path = directory + newname;

                                var uploadtoserver = await _imageFunctions.UploadImageToServerAsync(post.Video, path);
                                if (uploadtoserver != null)
                                    return BadRequest("!حدث خطا اثناء رفع الفديو ");

                                var video = new Video
                                {
                                    VideoFileName = newname,
                                    VideoUrl = $"http://mobile.hawisports.com/image/Users/{UserPhone}/" + newname,
                                    VideoTypeId = 1,
                                    IsActive = true,

                                };
                                _context.Videos.Add(video);
                                _context.SaveChanges();


                                var articleVideo = new ArticleVideo
                                {
                                    VideoTypeId = 1,
                                    VideoId = video.VideoId,
                                    ArticleId = article.ArticleId,
                                };
                                _context.ArticleVideos.Add(articleVideo);
                                _context.SaveChanges();


                            }
                            catch (Exception ex)
                            {
                                return BadRequest($"An error occurred while uploading the video: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        var articleVid = _context.ArticleVideos.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                        if (articleVid != null)
                        {
                            var selectedvideio = _context.Videos.Where(x => x.VideoId == articleVid.VideoId).FirstOrDefault();
                            if (selectedvideio != null)
                            {
                                var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\" + selectedvideio.VideoFileName;
                                _context.ArticleVideos.Remove(articleVid);
                                _context.Videos.Remove(selectedvideio);
                                _context.SaveChanges();
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }
                        }
                    }
                    #endregion

                    #region updateImage

                    #region (delete) compare with stored image and still image after modification to delete 
                    var articleImage = _context.ArticleImages.Where(x => x.ArticleId.Equals(ArticleId)).ToList();
                    if (articleImage != null && articleImage.Count != 0)
                    {
                        // if it empaty and there is image for articleImage delete all image "mean user delete images that was select"
                        if (post.StillImagesAfterModification == null || post.StillImagesAfterModification.Count == 0)
                        {
                            if (articleImage != null)
                            {
                                foreach (var articleimg in articleImage)
                                {
                                    var images = _context.Images.Where(x => x.ImageId.Equals(articleimg.ImageId)).FirstOrDefault();
                                    var imagename = _context.Images.FirstOrDefault(x => x.ImageId.Equals(articleimg.ImageId)).ImageFileName;
                                    var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\" + imagename;

                                    _context.ArticleImages.Remove(articleimg);
                                    _context.Images.Remove(images);
                                    if (System.IO.File.Exists(imagePath))
                                    {
                                        System.IO.File.Delete(imagePath);
                                    }
                                    _context.SaveChanges();
                                }
                            }
                        }

                        // Delete images of the post that are not in the StillImagesAfterModification list
                        else
                        {

                            var articleImageUrls = _context.ArticleImages
                                 .Where(x => x.ArticleId == ArticleId)
                                 .Select(x => x.Image.ImageUrlfullPath).ToList();

                            if (post.StillImagesAfterModification != null && post.StillImagesAfterModification.Any())
                            {

                                var imagesToDelete = articleImageUrls
                                    .Where(url => !post.StillImagesAfterModification.Contains(url))
                                    .ToList();

                                foreach (var imageUrl in imagesToDelete)
                                {

                                    var image = _context.Images.FirstOrDefault(x => x.ImageUrlfullPath == imageUrl);

                                    if (image != null)
                                    {

                                        var imageName = image.ImageFileName;
                                        var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\" + imageName;

                                        _context.ArticleImages.Remove(_context.ArticleImages.Where(x => x.ImageId == image.ImageId).FirstOrDefault());
                                        _context.Images.Remove(image);

                                        if (System.IO.File.Exists(imagePath))
                                        {
                                            System.IO.File.Delete(imagePath);
                                        }

                                    }
                                }
                                _context.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region add new image as normal image 

                    if (post.Images != null && post.Images.Count != 0)
                    {

                        foreach (var image in post.Images)
                        {
                            string englishFileName = _imageFunctions.ChangeImageName(image);

                            var directory = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\";
                            await _imageFunctions.CheckDirectoryExist(directory);
                            var path = directory + englishFileName;

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }


                            var img = new Models.Image
                            {

                                ImageUrlfullPath = $"http://mobile.hawisports.com/image/Users/{UserPhone}/" + englishFileName,
                                ImageFileName = englishFileName,
                                ImageTypeId = 3,
                                IsActive = true,
                            };

                            _context.Images.Add(img);
                            _context.SaveChanges();

                            var ArticleImage = new ArticleImage
                            {
                                ArticleId = ArticleId,
                                ImageId = img.ImageId,
                                ImageTypeId = 3,
                            };

                            _context.ArticleImages.Add(ArticleImage);
                            _context.SaveChanges();

                        }
                    }
                    #endregion

                    #region select MainImage 
                    bool ISdeletedmainimage = false;
                    int counter = 0;
                    long selectedToBemainimageid = 0;
                    foreach (var articleimg in articleImage)
                    {
                        if (counter == 0 && articleimg.ImageTypeId != 2)
                        {
                            counter++;
                            selectedToBemainimageid = articleimg.ImageId;
                        }
                        var images = _context.Images.Where(x => x.ImageId.Equals(selectedToBemainimageid)).FirstOrDefault();

                        if (images != null)
                            if (images.ImageTypeId == 2)
                                ISdeletedmainimage = true;
                    }

                    if (ISdeletedmainimage == false)
                    {
                        if (selectedToBemainimageid != 0)
                        {
                            var images = _context.Images.Where(x => x.ImageId.Equals(selectedToBemainimageid)).FirstOrDefault();
                            if (images != null)
                                images.ImageTypeId = 2;

                            _context.SaveChanges();
                        }
                    }
                    #endregion

                    #endregion

                    #region update article
                    article.Title = post.Title;
                    article.ArticleText = post.ArticleText;
                    article.LastUpdate = DateTime.Now;
                    article.IsActive = post.IsActive;
                    _context.Articles.Update(article);
                    _context.SaveChanges();

                    transiction.Commit();
                    return Ok("تم تعديل المقال بنجاح");
                    #endregion

                }
                catch (Exception ex)
                {
                    transiction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut(Name = "EditSpecifiecComment")]
        public IActionResult EditSpecifiecComment(long UserProfileId, long CommentId, long ArticleId, [FromBody] CreateCommentDto comment)
        {
            try
            {
                var article = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (article == null)
                    return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

                var Selectedcomment = _context.ArticleComments.Where(x => x.CommentId.Equals(CommentId) && x.ArticleId == ArticleId).FirstOrDefault();
                if (Selectedcomment == null)
                    return BadRequest("لم يتم العثور على التعليق!!");

                var user = _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileId)).FirstOrDefault();
                if (user == null)
                    return BadRequest("لم يتم العثور على المستخم الذى ادخلتة !");


                if (Selectedcomment.CommentUserProfileId != UserProfileId)
                    return BadRequest("لا يمكن تعديل هذا التعليق!");

                Selectedcomment.Comment = comment.Comment;
                Selectedcomment.CreateDate = DateTime.Now;
                _context.SaveChanges();
                return Ok("تم تعديل تعليقك بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///////

        [HttpDelete(Name = "DeletePost")]
        public async Task<IActionResult> DeletePost(long UserProfileId, long ArticleId)
        {
            try
            {
                #region validation 
                Article SelectedArticle = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (SelectedArticle == null)
                {
                    return BadRequest("لم يتم العثور على المقال.");
                }

                var entereduser = _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileId)).FirstOrDefault();
                if (entereduser == null)
                    return BadRequest("المستخدم الذى ادخلتة غير موجود!");

                var UserPhone = _context.Users.FirstOrDefault(x => x.UserId == entereduser.UserId).Mobile;

                #endregion
                #region Delete image & videio
                var articleVideo = _context.ArticleVideos.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                var articleImage = _context.ArticleImages.Where(x => x.ArticleId.Equals(ArticleId)).ToList();
                if (articleImage != null)
                {
                    foreach (var articleimg in articleImage)
                    {
                        var images = _context.Images.Where(x => x.ImageId.Equals(articleimg.ImageId)).FirstOrDefault();
                        var imagename = _context.Images.FirstOrDefault(x => x.ImageId.Equals(articleimg.ImageId)).ImageFileName;
                        var imagePath = $@"h:\root\home\hattanfjh-001\www\mobile\wwwroot\image\Users\{UserPhone}\" + imagename;

                        _context.ArticleImages.Remove(articleimg);
                        _context.Images.Remove(images);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        _context.SaveChanges();
                    }
                }

                if (articleVideo != null)
                {
                    var Video = _context.Videos.Where(x => x.VideoId.Equals(articleVideo.VideoId)).FirstOrDefault();
                    _context.ArticleVideos.Remove(articleVideo);
                    _context.Videos.Remove(Video);
                    _context.SaveChanges();
                }
                _context.Articles.Remove(SelectedArticle);
                _context.SaveChanges();

                #endregion

                return Ok("تم حذف المقال بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteSpecifiecComment")]
        public async Task<IActionResult> DeleteSpecifiecComment(long CommentId, long ArticleId, long UserProfileIdofpost, long UserProfileIdofcomment)
        {
            try
            {

                var article = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (article == null)
                    return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

                var comment = _context.ArticleComments.Where(x => x.CommentId.Equals(CommentId)).FirstOrDefault();
                if (comment == null)
                    return BadRequest("لم يتم العثور على التعليق !");


                if (article.UserProfileId == UserProfileIdofpost || comment.CommentUserProfileId == UserProfileIdofcomment)
                {
                    _context.ArticleComments.Remove(comment);
                    _context.SaveChanges();
                    return Ok("تم حذف التعليق بنجاح");
                }
                return BadRequest($"لا يمكن حذف هذا التعليق");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(Name = "DeleteCommentsOfPost")]
        public async Task<IActionResult> DeleteCommentsOfPost(long ArticleId, long UserProfileId)
        {
            try
            {
                var article = _context.Articles.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (article == null)
                    return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

                if (article.UserProfileId != UserProfileId)
                    return BadRequest("لا يمكن حذف هذا التعليق");


                var comments = _context.ArticleComments.Where(x => x.ArticleId.Equals(ArticleId)).ToList();
                if (comments == null)
                    return Ok("لم يتم التعليق على هذا المقال بعد ");

                _context.ArticleComments.RemoveRange(comments);
                _context.SaveChanges();

                return Ok("تم حذف جميع التعليقات بنجاح");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteSpeceificArchiveArticle")]
        public async Task<IActionResult> DeleteSpeceificArchiveArticle(long UserProfileId, long ArticleId)
        {
            try
            {
                var user = _context.UserProfiles.Where(x => x.UserProfileId.Equals(UserProfileId)).FirstOrDefault();
                if (user == null) return BadRequest("لم يتم العثور على المستخم الذى ادخلتة !");

                var article = _context.ArticleUsersSaveds.Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();
                if (article == null) return BadRequest("لم يتم العثور على المقال الذى ادخلتة!");

                var selectedarticle = _context.ArticleUsersSaveds.Where(x => x.ArticleId.Equals(ArticleId) && x.UserProfileId == UserProfileId).FirstOrDefault();
                if (selectedarticle == null) return BadRequest("ليس لديك الصلاحية لازالة المقال من المقالات المحفوظة !");

                _context.ArticleUsersSaveds.Remove(selectedarticle);
                _context.SaveChanges();

                return Ok("تم اذالة المقال مقالاتك المحفوظة ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
