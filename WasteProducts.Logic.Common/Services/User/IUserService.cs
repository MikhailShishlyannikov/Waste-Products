﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users.WebUsers;

namespace WasteProducts.Logic.Common.Services.Users
{
    /// <summary>
    /// Standart BL level interface provides standart methods of working with User model.
    /// </summary>
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="user">Specific claim will be added to the user.</param>
        /// <param name="claim">Specific claim to add to the user.</param>
        /// <returns></returns>
        Task AddClaimAsync(string userId, Claim claim);

        /// <summary>
        /// Adds a specific new friend to the specific user's friend list.
        /// </summary>
        /// <param name="userId">List of friends of the user with this ID will be expanded by the newFriend user.</param>
        /// <param name="newFriend">ID of the new friend to add to the user's friend list.</param>
        Task AddFriendAsync(string userId, string newFriendId);

        /// <summary>
        ///  Add a login to the user.
        /// </summary>
        /// <param name="user">Specific login will be added to the user.</param>
        /// <param name="login">Specific login to add to the user.</param>
        /// <returns></returns>
        Task AddLoginAsync(string userId, UserLogin login);

        /// <summary>
        /// Adds specific new product to the specific user's list of products.
        /// </summary>
        /// <param name="userId">ID of user to whose list of products the specific product will be added.</param>
        /// <param name="productId">Specific product's ID to add to the user's list of products.</param>
        /// <param name="rating">Rating from 0 to 10 of this product given by the user.</param>
        /// <param name="description">Textual description of the product given by the user.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        Task AddProductAsync(string userId, string productId, int rating, string description);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="user">User will be added to this specific role.</param>
        /// <param name="roleName">Name of the specific role to add to the user.</param>
        /// <returns></returns>
        Task AddToRoleAsync(string userId, string roleName);

        /// <summary>
        /// Tries to reset a password of the specific user to the new password and returns whether resetting succeed or not.
        /// </summary>
        /// <param name="userId">ID of user to change its password.</param>
        /// <param name="oldPassword">Old password of the specific user.</param>
        /// <param name="newPassword">New password of the specific user.</param>
        /// <returns>Boolean representing whether resetting password succeed or not.</returns>
        Task ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        /// <summary>
        /// Confirms user's email by the confirmation token given by RegisterAsync method.
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
        /// Deletes a specific friend from the specific user's friend list.
        /// </summary>
        /// <param name="userId">From the list of friends of the user with this ID the deletingFriend user will be deleted.</param>
        /// <param name="deletingFriendId">ID of the specific friend to delete from the user's friend list.</param>
        Task DeleteFriendAsync(string userId, string deletingFriendId);

        /// <summary>
        /// Deletes specific product from the specific user's list of products.
        /// </summary>
        /// <param name="userId">ID of user frow whose list of products the specific product will be deleted.</param>
        /// <param name="productId">Specific product's ID to delete from the user's list of products.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        Task DeleteProductAsync(string userId, string productId);

        /// <summary>
        /// Deletes user from the WasteProducts.
        /// </summary>
        /// <param name="userId">Deleting user's ID.</param>
        /// <returns></returns>
        Task DeleteUserAsync(string userId);

        /// <summary>
        /// Generates email changing token.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="newEmail">New email of the user.</param>
        /// <returns>Email confirmation token.</returns>
        Task GenerateEmailChangingTokenAsync(string userId, string newEmail);

        /// <summary>
        /// Gets info about all Users in the database.
        /// </summary>
        /// <returns>List of all Users in dataase.</returns>
        Task<IEnumerable<User>> GetAllAsync();

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
        Task<IList<Friend>> GetFriendsAsync(string userId);

        /// <summary>
        /// Gets all user's groups.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<GroupOfUser>> GetGroupsAsync(string userId);

        /// <summary>
        /// Gets the logins for a user.
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <returns>User's logins.</returns>
        Task<IList<UserLogin>> GetLoginsAsync(string userId);

        /// <summary>
        /// Gets all product descriptions of a user.
        /// </summary>
        /// <param name="userId">ID of a user.</param>
        /// <returns>User's product descriptions.</returns>
        Task<IList<UserProduct>> GetProductsAsync(string userId);

        /// <summary>
        /// Get the names of the roles a user is a member of.
        /// </summary>
        /// <param name="userId">Method will return roles of user with this ID.</param>
        /// <returns>IList of roles of user with the specific ID.</returns>
        Task<IList<string>> GetRolesAsync(string userId);

        /// <summary>
        /// Requests a User by its id. User is returned without PasswordHash.
        /// </summary>
        /// <param name="id">Id of requested User.</param>
        /// <returns>Instance of User that has requested Id. Returns WITHOUT PasswordHash.</returns>
        Task<User> GetAsync(string id);

        /// <summary>
        /// Deletes user from the group.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="groupId">ID of the group.</param>
        /// <returns></returns>
        Task LeaveGroupAsync(string userId, string groupId);

        /// <summary>
        /// Tries to login as a user with the specific email and password.
        /// </summary>
        /// <param name="email">Email of the logging in user.</param>
        /// <param name="password">Password of the logging in user.</param>
        /// <returns>Logged in user.</returns>
        Task<User> LogInByEmailAsync(string email, string password);

        /// <summary>
        /// Tries to login as a user with the specific user name and password.
        /// </summary>
        /// <param name="userName">Name of the logging in user.</param>
        /// <param name="password">Password of the logging in user.</param>
        /// <returns>Logged in user.</returns>
        Task<User> LogInByNameAsync(string userName, string password);

        /// <summary>
        /// Tries to register a new user with a specific parameters
        /// and if succeed sends email with email confirmation token to confirm user's email.
        /// </summary>
        /// <param name="email">Email of the new user.</param>
        /// <param name="userName">UserName of the new user.</param>
        /// <param name="password">Password of the new user.</param>
        /// <returns>Data containing in the email, returns for test purposes.</returns>
        Task<(string id, string token)> RegisterAsync(string email, string userName, string password);

        /// <summary>
        /// Remove a claim from a user.
        /// </summary>
        /// <param name="user">Specific claim will be removed from the user.</param>
        /// <param name="claim">Specific claim to remove from the user.</param>
        /// <returns></returns>
        Task RemoveClaimAsync(string userId, Claim claim);

        /// <summary>
        /// Remove a user from a role.
        /// </summary>
        /// <param name="user">User will be removed from this specific role.</param>
        /// <param name="roleName">Name of the specific role to remove from the user.</param>
        /// <returns></returns>
        Task RemoveFromRoleAsync(string userId, string roleName);

        /// <summary>
        /// Remove a login from a user.
        /// </summary>
        /// <param name="user">Specific login will be removed from the user.</param>
        /// <param name="login">Specific login to remove from the user.</param>
        /// <returns></returns>
        Task RemoveLoginAsync(string userId, UserLogin login);

        /// <summary>
        /// Resets password of the user with the userId ID by the reset password token.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="token">Reset password token</param>
        /// <param name="newPassword">New password of the user.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        Task ResetPasswordAsync(string userId, string token, string newPassword);

        /// <summary>
        /// Requests an email with the password reset roken for the user registered to this email.
        /// </summary>
        /// <param name="email">Email of the user forgot its password.</param>
        /// <returns>Tuple with item1 = ID of the user and item2 = password resetting token.</returns>
        Task<(string id, string token)> ResetPasswordRequestAsync(string email);

        /// <summary>
        /// Confirms group invitation if isConfirmed == true or deletes invite if isConfirmed == false.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="groupId">ID of the group.</param>
        /// <param name="isConfirmed">True if invitation accepted or false if not.</param>
        /// <returns></returns>
        Task RespondToGroupInvitationAsync(string userId, string groupId, bool isConfirmed);

        /// <summary>
        /// Updates the specific user in the Database. You can't update email, Id, UserName and Password by this method.
        /// </summary>
        /// <param name="user">The specific user to update.</param>
        /// <returns>Boolean representing whether updating the user was correct or not.</returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Updates user's Email. You cannot update email if newEmail is already used by another user.
        /// </summary>
        /// <param name="user">ID of User wanting to update its email.</param>
        /// <param name="newEmail">New unique email to update user's email.</param>
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
        /// Updates user's UserName. You cannot update UserName if newUserName is already used by another user.
        /// </summary>
        /// <param name="userId">Id of the user wanting to update its UserName.</param>
        /// <param name="newUserName">New unique UserName to update user's UserName.</param>
        /// <returns></returns>
        Task<bool> UpdateUserNameAsync(string userId, string newUserName);

        // TODO USER MANAGEMENT PENDING FUNCTIONAL TO ADD:
        // sharing my products with my friends after model "Product" is created
        // subscribing special users to watch their news (if this functional will be approved)
        // chatting between users
        // registering by Facebook and VK profiles
        // getting "Approved Representative of The Company" status and its unique functional like special tools for speed feedback
    }
}
