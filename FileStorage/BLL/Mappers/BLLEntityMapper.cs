using BLL.Interfaces.Entities;
using DAL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappers
{
    public static class BLLEntityMapper
    {
        public static DalUser ToDalUser(this UserEntity userEntity)
        {
            return new DalUser()
            {
                Id = userEntity.Id,
                Login = userEntity.Login,
                Password = userEntity.Password,
                Email = userEntity.Email,
                CreatedDate = userEntity.CreatedDate,
                RoleId = userEntity.RoleId
            };
        }

        public static UserEntity ToBllUser(this DalUser dalUser)
        {
            return new UserEntity()
            {
                Id = dalUser.Id,
                Login = dalUser.Login,
                Password = dalUser.Password,
                Email = dalUser.Email,
                CreatedDate = dalUser.CreatedDate,
                RoleId = dalUser.RoleId
            };
        }

        public static RoleEntity ToBLLRole(this DalRole dalRole)
        {
            return new RoleEntity()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static DalRole ToDalRole(this RoleEntity role)
        {
            return new DalRole()
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static ProfileEntity ToBLLProfile(this DalProfile dalProfile)
        {
            return new ProfileEntity()
            {
                Id = dalProfile.Id,
                FirstName = dalProfile.FirstName,
                LastName = dalProfile.LastName,
                Age = dalProfile.Age,
                Country = dalProfile.Country,
                City = dalProfile.City,
                UserId = dalProfile.UserId
            };
        }
        public static DalProfile ToDalProfile(this ProfileEntity profile)
        {
            return new DalProfile()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Age = profile.Age,
                Country = profile.Country,
                City = profile.City,
                UserId = profile.UserId
            };
        }

        public static PostEntity ToBLLEntity(this DalPost dalPost)
        {
            var post= new PostEntity()
            {
                Id = dalPost.Id,
                Name = dalPost.Name,
                Description = dalPost.Description,
                FileName = dalPost.FileName,
                FileType = dalPost.FileType,
                FileSize = dalPost.FileSize,
                UserId = dalPost.UserId,
                Permit=dalPost.Permit,
                Categories=null
            };
            if(dalPost.Categories!=null)
                post.Categories=dalPost.Categories.Select(p=>p.ToBllCategory()).ToList();
            return post;
        }

        public static DalPost ToDalPost(this PostEntity post)
        {
            return new DalPost()
            {
                Id = post.Id,
                Name = post.Name,
                Description = post.Description,
                FileName = post.FileName,
                FileType = post.FileType,
                FileSize = post.FileSize,
                UserId = post.UserId,
                Permit=post.Permit,
                Categories=post.Categories.Select(c=>c.ToDalCategory()).ToList()
            };
        }

        public static DalVote ToDalVote(this VoteEntity vote)
        {
            return new DalVote()
            {
                Id = vote.Id,
                PostId = vote.PostId,
                UserId = vote.UserId,
                Score = vote.Score
            };
        }

        public static VoteEntity ToBLLVote(this DalVote dalVote)
        {
            return new VoteEntity()
            {
                Id = dalVote.Id,
                PostId = dalVote.PostId,
                UserId = dalVote.UserId,
                Score = dalVote.Score
            };
        }

        public static DalComment ToDalComment(this CommentEntity com)
        {
            return new DalComment()
            {
                Id = com.Id,
                Text=com.Text,
                PostId = com.PostId,
                UserId = com.UserId
            };
        }

        public static CommentEntity ToBLLComment(this DalComment dalCom)
        {
            return new CommentEntity()
            {
                Id = dalCom.Id,
                Text=dalCom.Text,
                PostId = dalCom.PostId,
                UserId = dalCom.UserId
            };
        }

        public static DalCategory ToDalCategory(this CategoryEntity category)
        {
            return new DalCategory()
            {
                Id = category.Id,
                Name = category.Name                
            };
            
        }

        public static CategoryEntity ToBllCategory(this DalCategory dalCategory)
        {
            return new CategoryEntity()
            {
                Id = dalCategory.Id,
                Name = dalCategory.Name               
            };
            
        }
    }
}
