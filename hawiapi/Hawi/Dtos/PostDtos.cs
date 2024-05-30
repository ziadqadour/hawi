using Hawi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hawi.Dtos
{
    
    public record CreatePostDto
    {
        public string? Title { get; init; } 
        public string? ArticleText { get; init; }
        public bool? IsActive { get; set; } = true;
        public List<IFormFile>? Images { get; set; }
        public IFormFile? Video { get; init; }
        public string? VideoUrlfullPathFromAnotherPlatform { get; init; }
}

    public record EditPostDto
    {
        public string? Title { get; init; }
        public string? ArticleText { get; init; }
        public bool? IsActive { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<string>? StillImagesAfterModification { get; set; }       
        public IFormFile? Video { get; init; }
        public string? StillVideoAfterModification { get; set; }
        public string? VideoUrlfullPathFromAnotherPlatform { get; set; }
    }
    public class GetPostsDto
    {
        public long ArticleId { get; set; }
        public long UserId { get; set; }
        public long UserProfileId { get; set; }
        public bool? IsActive { get; set; }
        public int? ImageCount { get; set; }
        public string? UserName { get; set; }
        public string? UserImg { get; set; }
        public string? Title { get; set; }
        public string? ArticleText { get; set; }
        public DateTime ArticleCreateDate { get; set; }
        public string? ImageUrlfullPath { get; set; }
        public string? VideoUrlfullPath { get; set; }
        public string? VideoUrlfullPathFromAnotherPlatform { get; set; }
        public string? UserRole { get; set; }
        public bool? UserLikeReact { get; set; } 
        public bool? UserCommentReact { get; set; }


    }
    public class GetPostByIdDto
    {
        
        public long ArticleId { get; set; }
        public long UserId { get; set; }
        public long UserProfileId { get; set; }
        public bool? IsActive { get; set; }
        public int? ImageCount { get; set; }
        public string? UserName { get; set; }
        public string? UserImg { get; set; }
        public string? Title { get; set; }
        public string? ArticleText { get; set; }
        public DateTime ArticleCreateDate { get; set; }
        public List<string>? ImagesUrl { get; set; }
        public string? VideoUrlfullPath { get; set; }
        public string? VideoUrlfullPathFromAnotherPlatform { get; set; }
        public string? UserRole { get; set; }
        public bool? UserLikeReact { get; set; }
        public bool? UserCommentReact { get; set; }
    }
    public class ReportArticlesDto
    {
        [Required(ErrorMessage = "!يجب اضافة سبب للابلاغ عن البوست")]
        public byte? NotificationReasonId { get; set; }
        public string? NotificationMemo { get; set; }
    }
    public class GetReportArticlestsDto
    {

        public long ArticleId { get; set; }
        public long userprofileid { get; set; }
       // public long Targetuserprofileid { get; set; }
        public bool? IsActive { get; set; }
        public int? ImageCount { get; set; }
        public string? UserName { get; set; }
        public string? UserImg { get; set; }
        public string? Title { get; set; }
        public string? ArticleText { get; set; }
      //  public DateTime ArticlNotificationDate { get; set; }
        public DateTime ArticleCreateDate { get; set; }
        public string? ImageUrlfullPath { get; set; }
        public string? VideoUrlfullPath { get; set; }
        public string? UserRole { get; set; }
       // public string? TargetUserRole { get; set; }
      //  public string? TargetUserName { get; set; }
       // public string? NotificationMemo { get; set; }
        public long? likesCount { get; set;}
        public long? commentCount { get; set;}
        public long? reportCount { get; set;}

    }
    // comment
    public record CreateCommentDto
    {
        [Required(ErrorMessage = "Comment is required")]
        public string? Comment { get; init; }

    }
    public class CommentDto
    {
        public long UserId { get; set; }
        public long UserProfileId { get; set; }
        public string? username { get; set; }
        public string? userRole { get; set; }
        public string? UserImage { get; set; }
        public string? Comment { get; set; }
        public long? CommentlikesCount { get; set; }
        public bool? UserLikeReact { get; set; }
        public  DateTime? CreateDate { get; set; }
        public long commentId { get; set; }
    };
    public class ReportCommentDto
    {
        [Required(ErrorMessage = "!يجب اضافة سبب للابلاغ عن البوست")]
        public byte? CommentNotificationReasonId { get; set; }
        public string? CommentNotificationMemo { get; set; }
    }
    public class GetReportCommentsDto
    {

        public long CommentId { get; set; }
        public long userprofileid { get; set; }
        public long Targetuserprofileid { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string? TargetUserRole { get; set; }
        public string? TargetUserName { get; set; }
        public string? UserImg { get; set; }
        public string? Commenttext { get; set; }
        public DateTime CommentNotificationDate { get; set; }
        public DateTime? CommentCreateDate { get; set; }
        public string? NotificationMemo { get; set; }
        public long? likesCount { get; set; }
    }
    //like 
    public class PostLikeDetailesDTO
    {
        public long UserId { get; set; }
        public long UserProfileId { get; set; }
        public string username { get; set; }
        public string userRole { get; set; }
        public string UserImage { get; set; }
    };
    
    public class CommentLikeDetailesDTO
    {   
        public long UserId { get; set; }
        public long UserProfileId { get; set; }
        public string username { get; set; }
        public string userRole { get; set; }
        public string UserImage { get; set; }

    };

    public class PostLikeANDCommentDetailesDTO
    {
        public long Commintcount { get; set; }
        public long likescount { get; set; }
       
    };
}
