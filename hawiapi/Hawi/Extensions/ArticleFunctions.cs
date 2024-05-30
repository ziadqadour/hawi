using Hawi.Dtos;
using Hawi.Models;
using Hawi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace Hawi.Extensions
{
    public class ArticleFunctions
    {
        private readonly HawiContext _context;
        private readonly UserFunctions _UserFunctions;
        private readonly ImageFunctions _ImageFunctions;
        public ArticleFunctions(HawiContext context, UserFunctions userFunctions, ImageFunctions imageFunctions)
        {
            _context = context;
            _UserFunctions = userFunctions;
            _ImageFunctions = imageFunctions;
        }

        public async Task<String>CheckExistArticle(long ArticleId)
        {
            var article =await _context.Articles.Where(x => x.ArticleId==ArticleId&& x.IsActive==true).FirstOrDefaultAsync();
            if (article == null)
                return "لم يتم العثور على المقال الذى ادخلتة!";
            return null;
        }
        public async Task<String> CheckExistHideArticle(long ArticleId, long UserProfileId)
        {
            var checkArticleHide = await _context.ArticleHides
                .Where(x => x.ArticleId == ArticleId && x.UserProfileId == UserProfileId)
                .FirstOrDefaultAsync();
            if (checkArticleHide != null)  return "تم تنفيذ طلبك بنجاح";
            
            return null;
        }
        public async Task<String> CheckExistSavedArticle(long ArticleId, long UserProfileId)
        {
            var selectedarchievearticle =await _context.ArticleUsersSaveds
                .Where(x => x.ArticleId == ArticleId && x.UserProfileId == UserProfileId)
                .FirstOrDefaultAsync();
            
            if (selectedarchievearticle != null)
                return "تم الحفظ";

            return null;
        }
        public async Task<String> CheckExistComment(long CommentId)
        {
            var comment = await _context.ArticleComments.Where(x => x.CommentId == CommentId).FirstOrDefaultAsync();
            if (comment == null)
                return "لم يتم العثور على هذا التعليق !";
            return null;
        }

        public async Task<String> CheckValidateImageOfPost(List<IFormFile>? Images)
        {
            if (Images != null && Images.Count > 0)
            {
                if (Images.Count > 5) return "الحد الأقصى للصور الخاصة بالمقال 5 صور فقط!";

                foreach (var image in Images)
                {
                    var checkImage = _ImageFunctions.GetInvalidImageMessage(image);
                    if (checkImage != null) return $"{checkImage}";
                }
            }
            return null;
        }

        public async Task<long> MapToAddArticle(long UserProfileId,  CreatePostDto post)
        {
            try
            {
                var Newpost = new Article
                {
                    UserProfileId = UserProfileId,
                    Title = post.Title,
                    ArticleText = post.ArticleText,
                    LastUpdate = DateTime.Now,
                    IsActive = post.IsActive,
                };
                await _context.Articles.AddAsync(Newpost);
                await _context.SaveChangesAsync();
                return Newpost.ArticleId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
       
        public  GetPostsDto MapToGetPostsDto(Article post, long userProfileId)
        {

            //check Enter User Have like or comment for that post or not 
            var checkExistCommentOrNot = _context.ArticleComments
                .FirstOrDefault(x => x.CommentUserProfileId == userProfileId && x.ArticleId == post.ArticleId);

            var checkExistLikeOrNot = _context.ArticleLikes
                .FirstOrDefault(l => l.LikeUserProfileId == userProfileId && l.ArticleId == post.ArticleId);

            //get images of post and  order by ImageTypeId to get main image first and if not get normal image for post
            var articleImage = _context.ArticleImages
                .Where(x => x.ArticleId == post.ArticleId && (x.ImageTypeId == 2 || x.ImageTypeId == 3))
                .OrderBy(x => x.ImageTypeId)
            .FirstOrDefault();

            string? imagepath = articleImage != null
                ? _context.Images.FirstOrDefault(x => x.ImageId == articleImage.ImageId)?.ImageUrlfullPath
                : null;


            string? VideoPath = null;
            ArticleVideo? AricleVideo = null;

            //if there is no any images in article will show video url "to decide will show viedio or image scroll in home"
            if (articleImage == null)
            {//video that uploaded in our server
                AricleVideo = _context.ArticleVideos
                    .FirstOrDefault(x => x.ArticleId == post.ArticleId && x.VideoTypeId == 1);
                VideoPath = AricleVideo != null
                    ? _context.Videos.FirstOrDefault(x => x.VideoId == AricleVideo.VideoId && x.VideoTypeId == 1)?.VideoUrl
                    : null;
            }

            //video link from another place like "youtube ,. ..."
            var VideoFromAnotherPlatform = _context.ArticleVideos
                .FirstOrDefault(x => x.ArticleId == post.ArticleId && x.VideoTypeId == 5);
            var VideoUrlfullPathFromAnotherPlatform = VideoFromAnotherPlatform != null
                ? _context.Videos.FirstOrDefault(x => x.VideoId == VideoFromAnotherPlatform.VideoId && x.VideoTypeId == 5)?.VideoUrl
            : null;

            var userprofileid = post.UserProfileId;
            var userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserProfileId == userprofileid);
            var Role = _context.Roles.FirstOrDefault(x => x.RoleId == userProfile.RoleId);
            var roleid = Role.RoleId;

            var UserID = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == userprofileid).UserId;
            var lastloginrole = _context.Users.FirstOrDefault(x => x.UserId == UserID)?.LastLoginRoleId;

            bool checklike, checkcomment;
            if (userProfileId == -1)
            {
                checklike = false;
                checkcomment = false;
            }
            else
            {
                checklike = checkExistLikeOrNot != null;
                checkcomment = checkExistCommentOrNot != null;
            }

            return new GetPostsDto
            {
                ArticleId = post.ArticleId,
                ImageCount = _context.ArticleImages.Count(x => x.ArticleId == post.ArticleId),
                IsActive = post.IsActive,
                Title = post.Title,
                ArticleText = post.ArticleText,
                ArticleCreateDate = post.ArticleCreateDate,
                ImageUrlfullPath = imagepath,
                VideoUrlfullPath = VideoPath,
                VideoUrlfullPathFromAnotherPlatform = VideoUrlfullPathFromAnotherPlatform,
                UserId = UserID,
                UserProfileId = post.UserProfileId,
                UserName = _UserFunctions.GetUserName(roleid, userprofileid, UserID),
                UserRole = Role?.Role1,
                UserImg = _UserFunctions.GetUserImage(userprofileid, roleid),
                UserLikeReact = checklike,
                UserCommentReact = checkcomment,
            };
        }
       
        public async Task<GetPostByIdDto> GetPostByID(Article article, long userProfilId)
        {
            try
            {
                //userlike and comment 
                var checkExistCommentOrNotw = await _context.ArticleComments
                    .FirstOrDefaultAsync(x => x.CommentUserProfileId == userProfilId && x.ArticleId == article.ArticleId);

                var checkExistLikeOrNot = await _context.ArticleLikes
                    .FirstOrDefaultAsync(l => l.LikeUserProfileId == userProfilId && l.ArticleId == article.ArticleId);

                //get image and video for post
                var articleImagee = await _context.ArticleImages
                    .Where(x => x.ArticleId.Equals(article.ArticleId))
                    .OrderBy(x => x.ImageTypeId)
                    .ToListAsync();

                var numOfImages = await _context.ArticleImages
                    .Where(x => x.ArticleId.Equals(article.ArticleId))
                    .ToListAsync();

                var imagePaths = new List<string>();
                foreach (var postImage in articleImagee)
                {
                    var imageId = postImage.ImageId;
                    var imagePath = await _context.Images
                        .Where(x => x.ImageId.Equals(imageId))
                        .Select(x => x.ImageUrlfullPath)
                        .FirstOrDefaultAsync();

                    if (imagePath != null)
                    {
                        imagePaths.Add(imagePath);
                    }
                }

                var articleVideo = await _context.ArticleVideos
                    .Where(x => x.ArticleId.Equals(article.ArticleId) && x.VideoTypeId == 1)
                    .FirstOrDefaultAsync();

                string? videoPath = (articleVideo == null)
                    ? null
                    : await _context.Videos
                        .Where(x => x.VideoId == articleVideo.VideoId && x.VideoTypeId == 1)
                        .Select(x => x.VideoUrl)
                        .FirstOrDefaultAsync();

                //video link from another place
                var videoFromAnotherPlatform = await _context.ArticleVideos
                    .FirstOrDefaultAsync(x => x.ArticleId == article.ArticleId && x.VideoTypeId == 5);

                var videoUrlFullPathFromAnotherPlatform = (videoFromAnotherPlatform != null)
                    ? await _context.Videos
                        .Where(x => x.VideoId == videoFromAnotherPlatform.VideoId && x.VideoTypeId == 5)
                        .Select(x => x.VideoUrl)
                        .FirstOrDefaultAsync()
                    : null;

                var userProfileId = article.UserProfileId;
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserProfileId == userProfileId);

                var role = await _context.Roles
                    .Where(x => x.RoleId.Equals(userProfile.RoleId))
                    .FirstOrDefaultAsync();

                var imgPath =  _UserFunctions.GetUserImage(article.UserProfileId, article.UserProfile.RoleId);

                var userId = await _context.UserProfiles
                    .Where(x => x.UserProfileId.Equals(userProfileId))
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();

                // check if the user register or not to prevent post react
                bool checkLike, checkComment;
                if (userProfilId == -1)
                {
                    checkLike = false;
                    checkComment = false;
                }
                else
                {
                    checkLike = (checkExistLikeOrNot != null);
                    checkComment = (checkExistCommentOrNotw != null);
                }

                string name =  _UserFunctions.GetUserName(article.UserProfile.RoleId, article.UserProfileId, userId);

                return new GetPostByIdDto
                {
                    ImageCount = numOfImages.Count,
                    IsActive = article.IsActive,
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    ArticleText = article.ArticleText,
                    ArticleCreateDate = article.ArticleCreateDate,
                    ImagesUrl = imagePaths,
                    VideoUrlfullPath = videoPath,
                    VideoUrlfullPathFromAnotherPlatform = videoUrlFullPathFromAnotherPlatform,
                    UserId = userId,
                    UserProfileId = article.UserProfileId,
                    UserName = name,
                    UserRole = role?.Role1,
                    UserImg = imgPath,
                    UserLikeReact = checkLike,
                    UserCommentReact = checkComment,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public async Task<List<GetReportArticlestsDto>> GetReportArticles()
        {
            try
            {
                var notificationPosts = await _context.ArticleNotifications
                    .OrderByDescending(x => x.CreateDate)
                    .GroupBy(x => x.ArticleId)
                    .Select(group => group.First()) // DistinctBy ArticleId
                    .ToListAsync();

                notificationPosts.Reverse();

                var results = new List<GetReportArticlestsDto>();
                foreach (var notificationPost in notificationPosts)
                {
                        var post = await _context.Articles
                            .Where(x => x.ArticleId == notificationPost.ArticleId)
                            .FirstOrDefaultAsync();

                        string imagePath = null;
                        string videoPath = null;

                        // To check to return any URL (video URL or image URL)
                        var articleImage = await _context.ArticleImages
                            .Where(x => x.ArticleId == post.ArticleId && x.ImageTypeId == 2)
                            .FirstOrDefaultAsync();

                        if (articleImage != null)
                        {
                            imagePath = (await _context.Images.FirstOrDefaultAsync(x => x.ImageId == articleImage.ImageId))?.ImageUrlfullPath;
                        }

                        var articleVideo = await _context.ArticleVideos
                            .Where(x => x.ArticleId == post.ArticleId)
                            .FirstOrDefaultAsync();

                        if (articleVideo != null)
                        {
                            videoPath = (await _context.Videos.FirstOrDefaultAsync(x => x.VideoId == articleVideo.VideoId))?.VideoUrl;
                        }

                        var userProfile = await _context.UserProfiles
                            .Where(up => up.UserProfileId == post.UserProfileId)
                            .FirstOrDefaultAsync();

                        if (userProfile == null)
                        {
                            continue; 
                        }

                        var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == userProfile.RoleId);

                        if (role == null)
                        {
                            continue; 
                        }

                        var userId = (await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == userProfile.UserProfileId))?.UserId;

                        var likesCount = await _context.ArticleLikes.CountAsync(x => x.ArticleId == post.ArticleId);
                        var commentCount = await _context.ArticleComments.CountAsync(x => x.ArticleId == post.ArticleId);
                        var reportCount = await _context.ArticleNotifications.CountAsync(x => x.ArticleId == notificationPost.ArticleId);

                        results.Add(new GetReportArticlestsDto
                        {
                            userprofileid = userProfile.UserProfileId,
                            UserRole = role.Role1,
                            UserName = _UserFunctions.GetUserName(userProfile.RoleId, userProfile.UserProfileId, (long)userId),
                            ArticleId = post.ArticleId,
                            ImageCount = await _context.ArticleImages.CountAsync(x => x.ArticleId == post.ArticleId),
                            Title = post.Title,
                            ArticleText = post.ArticleText,
                            ArticleCreateDate = post.ArticleCreateDate,
                            ImageUrlfullPath = imagePath,
                            VideoUrlfullPath = videoPath,
                            UserImg = _UserFunctions.GetUserImage(userProfile.UserProfileId, userProfile.RoleId),
                            likesCount = likesCount,
                            commentCount = commentCount,
                            reportCount = reportCount,
                        });
                }


                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public async Task<List<CommentLikeDetailesDTO>> GetCommentLikeDetails(long CommentId)
        {
            var likesforComment = await _context.ArticleCommentLikes
             .Where(x => x.CommentId == CommentId &&
                         x.UserProfile.User.IsActive == true &&
                         x.UserProfile.IsActive == true)
             .ToListAsync();

            List<CommentLikeDetailesDTO> li = new List<CommentLikeDetailesDTO>();

            foreach (var CommentLike in likesforComment)
            {
                var userId = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == CommentLike.UserProfileId).UserId;
                var role = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == CommentLike.UserProfileId).RoleId;
                var RoleName = _context.Roles.FirstOrDefault(x => x.RoleId == role).Role1;

                li.Add(
                new CommentLikeDetailesDTO
                {
                    UserId = userId,
                    UserProfileId = CommentLike.UserProfileId,
                    username = _UserFunctions.GetUserName(role, CommentLike.UserProfileId, userId),
                    userRole = RoleName,
                    UserImage = _UserFunctions.GetUserImage(CommentLike.UserProfileId, role),
                });
            }
            return li;
        }
        
        public async Task<List<PostLikeDetailesDTO>> GetPostLikeDetails(long ArticleId)
        {
            var likesforpost = await _context.ArticleLikes.Where(x => x.ArticleId.Equals(ArticleId)
                                     && x.LikeUserProfile.User.IsActive == true
                                     && x.LikeUserProfile.IsActive == true)
                              .ToListAsync();

            List<PostLikeDetailesDTO> li = new List<PostLikeDetailesDTO>();
            foreach (var PostLike in likesforpost)
            {
                var role = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == PostLike.LikeUserProfileId).RoleId;
                var RoleName = _context.Roles.FirstOrDefault(x => x.RoleId == role).Role1;

                var userId = _context.UserProfiles.FirstOrDefault(x => x.UserProfileId == PostLike.LikeUserProfileId).UserId;

                li.Add(
                new PostLikeDetailesDTO
                {
                    UserId = userId,
                    UserProfileId = PostLike.LikeUserProfileId,
                    username = _UserFunctions.GetUserName(role, PostLike.LikeUserProfileId, userId),
                    userRole = RoleName,
                    UserImage = _UserFunctions.GetUserImage(PostLike.LikeUserProfileId, role),
                });
            }
            return li;
        }

        public async Task<List<GetReportCommentsDto>> GetReportComments()
        {
            try
            {
                var reportComments = await _context.ArticleCommentNotifications
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

                var result = reportComments.Select(async notificationComment =>
                {
                    var comment = await _context.ArticleComments.FirstOrDefaultAsync(x => x.CommentId == notificationComment.CommentId);

                    if (comment == null)
                        return null;

                    var userprofileid = comment.CommentUserProfileId;
                    var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == userprofileid);
                    var targetUserProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == notificationComment.UserProfileId);
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == userProfile.RoleId);
                    var targetUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == targetUserProfile.UserId);
                    var notificationReason = notificationComment.CommentNotificationReasonId == 4
                        ? notificationComment.CommentNotificationMemo
                        : (await _context.ArticleNotificationNotificationReasons
                            .FirstOrDefaultAsync(x => x.NotificationReasonId == notificationComment.CommentNotificationReasonId))?.NotificationReason;

                    var likesCount = await _context.ArticleCommentLikes.CountAsync(x => x.CommentId == notificationComment.CommentId);

                    return new GetReportCommentsDto
                    {
                        userprofileid = userprofileid,
                        UserRole = role?.Role1,
                        UserName = _UserFunctions.GetUserName(userProfile.RoleId, userProfile.UserProfileId, userProfile.UserId),
                        Targetuserprofileid = notificationComment.UserProfileId,
                        TargetUserRole = (await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == targetUserProfile.RoleId))?.Role1,
                        TargetUserName = targetUser?.Name,
                        CommentId = comment.CommentId,
                        CommentCreateDate = comment.CreateDate,
                        CommentNotificationDate = notificationComment.CreateDate,
                        UserImg = _UserFunctions.GetUserImage(userProfile.UserProfileId, userProfile.RoleId),
                        NotificationMemo = notificationReason,
                        likesCount = likesCount,
                        Commenttext = comment.Comment
                    };
                });

                var filteredResults = (await Task.WhenAll(result)).Where(resultItem => resultItem != null).ToList();
                return filteredResults;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
