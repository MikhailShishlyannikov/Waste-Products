﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Repositories.Users
{
    /// <summary>
    /// Provides a standart DAL methods to work with users.
    /// </summary>
    public interface IUserRepository : IDisposable
    {
        /// <summary>
        /// Tries to add a new registered user to the database and if succeed returns email confirmation token needed to confirm user's email.
        /// </summary>
        /// <param name="email">Email of the new user.</param>
        /// <param name="userName">Name of the new user.</param>
        /// <param name="password">Password of the new user.</param>
        /// <returns>ID of the user and email confirmation token needed to confurm user's email.</returns>
        Task<(string id, string token)> AddAsync(string email, string userName, string password);

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="userId">Specific claim will be added to the user with this Id.</param>
        /// <param name="claim">Specific claim to add to the user.</param>
        /// <returns></returns>
        Task AddClaimAsync(string userId, Claim claim);

        /// <summary>
        /// Adds user with id = friendId to the list of friends of user with Id = userId.
        /// </summary>
        /// <param name="userId">Id of user which friend list will be expanded.</param>
        /// <param name="friendId">Id of a new friend of the user with Id = userId</param>
        /// <returns></returns>
        Task AddFriendAsync(string userId, string friendId);

        /// <summary>
        ///  Add a login to the user.
        /// </summary>
        /// <param name="userId">Specific login will be added to the user with this Id.</param>
        /// <param name="login">Specific login to add to the user.</param>
        /// <returns></returns>
        Task AddLoginAsync(string userId, UserLoginDB login);

        /// <summary>
        /// Adds product with specific ID to the user's list of products.
        /// </summary>
        /// <param name="userId">ID of user who wants to add the specific product to his/her list of products.</param>
        /// <param name="productId">ID of specific product to add to the user's list of products.</param>
        /// <param name="rating">Rating from 0 to 10 of this product given by the user.</param>
        /// <param name="description">Textual description of the product given by the user.</param>
        /// <returns>Boolean represents whether operation succeed or not.</returns>
        Task AddProductAsync(string userId, string productId, int rating, string description);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="userId">User with this ID will be added to this specific role.</param>
        /// <param name="roleName">Name of the specific role to add to the user.</param>
        /// <returns></returns>
        Task AddToRoleAsync(string userId, string roleName);

        /// <summary>
        /// Resets password of a user.
        /// </summary>
        /// <param name="user">Password of this user will be reset.</param>
        /// <param name="newPassword">New password for a user.</param>
        /// <returns></returns>
        Task ChangePasswordAsync(string userId, string newPassword, string oldPassword);

        /// <summary>
        /// Confirms user's email by the confirmation token given by AddAsync method.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        Task<bool> ConfirmEmailAsync(string userId, string token);

        /// <summary>
        /// Confirms new email with email confirmation token.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns></returns>
        Task ConfirmEmailChangingAsync(string userId, string token);

        /// <summary>
        /// Deletes the record of the specific user.
        /// </summary>
        /// <param name="userId">ID of the specific user to delete.</param>
        Task DeleteAsync(string userId);

        /// <summary>
        /// Deletes a specific friend from the specific user's friend list.
        /// </summary>
        /// <param name="userId">From the list of friends of User with Id = userID the User with Id = deletingFriendId will be deleted.</param>
        /// <param name="deletingFriendId">Specific friend's Id to delete from the user's friend list.</param>
        Task DeleteFriendAsync(string userID, string deletingFriendId);

        /// <summary>
        /// Deletess product with specific ID to the user's list of products.
        /// </summary>
        /// <param name="userId">ID of user who wants to remove the specific product from his/her list of products.</param>
        /// <param name="productId">ID of specific product to remove from the user's list of products.</param>
        /// <returns>Boolean represents whether operation succeed or not.</returns>
        Task DeleteProductAsync(string userId, string productId);

        /// <summary>
        /// Generates email changing token.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="newEmail">New email of the user.</param>
        /// <returns>Email confirmation token.</returns>
        Task<string> GenerateEmailChangingTokenAsync(string userId, string newEmail);

        /// <summary>
        /// Generates a password reset token for the specific user and returns tuple where item1 is ID of the user and item2 is the token.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <returns>ID of the user and password reset token.</returns>
        Task<(string id, string token)> GeneratePasswordResetTokenAsync(string email);

        /// <summary>
        /// Gets all users from DB.
        /// </summary>
        /// <return>All users of the application.</returns>
        Task<IEnumerable<UserDAL>> GetAllAsync();

        /// <summary>
        /// Use this method to select user by its ID.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>DB User entity.</returns>
        Task<UserDAL> GetAsync(string userId);

        /// <summary>
        /// Return a user with the specified email and password or null if there is no match.
        /// </summary>
        /// <param name="email">User's Email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Specific user with this ID and password.</returns>
        Task<UserDAL> GetByEmailAndPasswordAsync(string email, string password);

        /// <summary>
        /// Return a user with the specified username and password or null if there is no match.
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Specific user with this ID and password.</returns>
        Task<UserDAL> GetByNameAndPasswordAsync(string userName, string password);

        /// <summary>
        /// Get a users's claims
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <returns>User's claims.</returns>
        Task<IList<Claim>> GetClaimsAsync(string userId);

        /// <summary>
        /// Gets all friends of a user.
        /// </summary>
        /// <param name="userId">ID of a user.</param>
        /// <returns>User's friends.</returns>
        Task<IList<UserDAL>> GetFriendsAsync(string userId);

        /// <summary>
        /// Gets all user's groups.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<GroupUserDB>> GetGroupsAsync(string userId);

        /// <summary>
        /// Gets the logins for a user.
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <returns>User's logins.</returns>
        Task<IList<UserLoginDB>> GetLoginsAsync(string userId);

        /// <summary>
        /// Gets all product descriptions of a user.
        /// </summary>
        /// <param name="userId">ID of a user.</param>
        /// <returns>User's product descriptions.</returns>
        Task<IList<UserProductDescriptionDB>> GetUserProductDescriptionsAsync(string userId);

        /// <summary>
        /// Get the names of the roles a user is a member of.
        /// </summary>
        /// <param name="userId">Method will return roles of this user.</param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(string userId);

        /// <summary>
        /// Checks if email wasn't used in registering any user. Returns true if not.
        /// </summary>
        /// <param name="email">Checking email.</param>
        /// <returns>True if email is available, false if it's not.</returns>
        Task<bool> IsEmailAvailableAsync(string email);

        /// <summary>
        /// Returns true if there us no User with such Username. Returns false is there is a User with the UserName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> IsUserNameAvailable(string userName);

        /// <summary>
        /// Remove a claim from a user.
        /// </summary>
        /// <param name="userId">Specific claim will be removed from the user.</param>
        /// <param name="claim">Specific claim to remove from the user.</param>
        /// <returns></returns>
        Task RemoveClaimAsync(string userId, Claim claim);

        /// <summary>
        /// Remove a user from a role.
        /// </summary>
        /// <param name="userId">The specific role will be removed from the user with this ID.</param>
        /// <param name="roleName">Name of the specific role to remove from the user.</param>
        /// <returns></returns>
        Task RemoveFromRoleAsync(string userId, string roleName);

        /// <summary>
        /// Remove a login from a user.
        /// </summary>
        /// <param name="userId">Specific login will be removed from the user with this ID.</param>
        /// <param name="login">Specific login to remove from the user.</param>
        /// <returns></returns>
        Task RemoveLoginAsync(string userId, UserLoginDB login);

        /// <summary>
        /// Resets password of the user with the userId ID by the reset password token.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="token">Reset password token</param>
        /// <param name="newPassword">New password of the user.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        Task ResetPasswordAsync(string userId, string token, string newPassword);

        /// <summary>
        /// Confirms group invitation if isConfirmed == true or deletes invite if isConfirmed == false.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="groupId">ID of the group.</param>
        /// <param name="isConfirmed">True if invitation accepted or false if not.</param>
        /// <returns></returns>
        Task ChangeGroupInvitationStatusAsync(string userId, string groupId, bool isConfirmed);

        /// <summary>
        /// Updates the record of the specific user.
        /// </summary>
        /// <param name="user">Specific user to update.</param>
        Task UpdateAsync(UserDAL user);

        /// <summary>
        /// Updates user's email if it isn't used by another user. Returns true if email was successfully updated.
        /// </summary>
        /// <param name="userId">ID of User wanting to update its Email.</param>
        /// <param name="newEmail">New unique email.</param>
        /// <returns></returns>
        Task<bool> UpdateEmailAsync(string userId, string newEmail);

        /// <summary>
        /// Updates rating and description of the product with the specific ID in the user's product list.
        /// </summary>
        /// <param name="userId">ID of the user updating its product description.</param>
        /// <param name="productId">ID of the product.</param>
        /// <param name="rating">New rating of the product.</param>
        /// <param name="description">New description of the product.</param>
        /// <returns>Boolean represents whether operation succeed or not.</returns>
        Task UpdateProductDescriptionAsync(string userId, string productId, int rating, string description);

        /// <summary>
        /// Updates user's UserName if it isn't used by another user. Returns true if UserName was successfully updated.
        /// </summary>
        /// <param name="userId">ID of the user wanting to update its UserName.</param>
        /// <param name="newUserName">New unique UserName</param>
        /// <returns></returns>
        Task<bool> UpdateUserNameAsync(string userId, string newUserName);
    }
}