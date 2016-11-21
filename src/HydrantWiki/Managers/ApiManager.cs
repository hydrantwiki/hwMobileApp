using System;
using System.IO;
using HydrantWiki.Network;
using HydrantWiki.Objects;
using HydrantWiki.ResponseObjects;
using Newtonsoft.Json;

namespace HydrantWiki.Managers
{
    public class ApiManager
    {
        private HWManager m_HWManager;

        public ApiManager(HWManager _manager)
        {
            m_HWManager = _manager;
        }

        /// <summary>
        /// Authenticates the user returning a user object with
        /// the username, display name, and an authorization token
        /// to be used on subsequent calls
        /// </summary>
        /// <param name="_email">Email.</param>
        /// <param name="_password">Password.</param>
        public User Authenticate(string _email, string _password)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/authorize";
            request.Headers.Add("Email", _email);
            request.Headers.Add("Password", _password);
            request.Timeout = 3000;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            AuthenticationResponse responseObject =
                JsonConvert.DeserializeObject<AuthenticationResponse>(response.Body);

            if (responseObject != null
                && responseObject.Success)
            {
                return responseObject.User;
            }

            return null;
        }

        public bool UsernameAvailable(string _requestedUsername)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/user/isavailable/" + _requestedUsername;
            request.Timeout = 2000;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            AvailableResponse responseObject =
                JsonConvert.DeserializeObject<AvailableResponse>(response.Body);

            if (responseObject != null
                && responseObject.Success)
            {
                return responseObject.Available;
            }

            return false;
        }

        public bool EmailInUse(string _emailAddress)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/user/inuse/" + _emailAddress;
            request.Timeout = 2000;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            AvailableResponse responseObject =
                JsonConvert.DeserializeObject<AvailableResponse>(response.Body);

            if (responseObject != null
                && responseObject.Success)
            {
                return responseObject.Available;
            }

            return false;
        }

        public ChangePasswordResponse ChangePassword(
            User _user,
            string _existingPassword,
            string _newPassword)
        {
            ChangePasswordBody cpb = new ChangePasswordBody
            {
                Username = _user.Username,
                ExistingPassword = _existingPassword,
                NewPassword = _newPassword
            };

            string body = JsonConvert.SerializeObject(cpb);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/user/password";
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);
            request.Body = body;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);

            if (response.Status == HWResponseStatus.Completed)
            {
                ChangePasswordResponse responseObject =
                    JsonConvert.DeserializeObject<ChangePasswordResponse>(response.Body);

                return responseObject;
            } else {
                throw new Exception(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Saves the tag tot he server
        /// </summary>
        /// <returns>A tag response object.</returns>
        /// <param name="_user">User.</param>
        /// <param name="_tag">Tag.</param>
        public TagResponse SaveTag(User _user, Tag _tag)
        {
            string body = JsonConvert.SerializeObject(_tag);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/tag";
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);
            request.Body = body;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);

            if (response.Status == HWResponseStatus.Completed)
            {
                TagResponse responseObject =
                    JsonConvert.DeserializeObject<TagResponse>(response.Body);

                return responseObject;
            } else {
                throw new Exception(response.ErrorMessage);
            }
        }

        /// <summary>
        /// Sends the image to the server.  Should be called after the Tag
        /// </summary>
        /// <returns>The tag image.</returns>
        /// <param name="_user">User.</param>
        /// <param name="_fileName">File name.</param>
        public TagResponse SaveTagImage(User _user, string _fileName)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/image";
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            HWFile file = new HWFile
            {
                Filename = Path.GetFileName(_fileName),
                FullPathFilename = _fileName
            };
            request.File = file;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            TagResponse responseObject =
                JsonConvert.DeserializeObject<TagResponse>(response.Body);

            return responseObject;
        }

        /// <summary>
        /// Returns the number of tags that the current user has made
        /// </summary>
        /// <returns>The my tag count.</returns>
        /// <param name="_user">User.</param>
        public TagCountResponse GetMyTagCount(User _user)
        {
            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = "/api/tags/mine/count";
            request.Timeout = 5000;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            TagCountResponse responseObject =
                JsonConvert.DeserializeObject<TagCountResponse>(response.Body);

            return responseObject;
        }

        public HydrantQueryResponse GetHydrantsInCirle(
            User _user, double _latitude, double _longitude, double _radius)
        {
            string url = string.Format("/api/hydrants/circle/{0}/{1}/{2}", _latitude, _longitude, _radius);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            HydrantQueryResponse responseObject =
                JsonConvert.DeserializeObject<HydrantQueryResponse>(response.Body);

            return responseObject;
        }

        public HydrantQueryResponse GetHydrantsInBox(
            User _user,
            double _minLatitude,
            double _maxLatitude,
            double _minLongitude,
            double _maxLongitude)
        {

            string url = string.Format("/api/hydrants/box/{0}/{1}/{2}/{3}", _maxLatitude, _minLatitude, _maxLongitude, _minLongitude);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            HydrantQueryResponse responseObject =
                JsonConvert.DeserializeObject<HydrantQueryResponse>(response.Body);

            return responseObject;
        }

        public TagsToReviewResponse GetTagsToReview(
            User _user)
        {

            string url = string.Format("/api/review/tags");

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Get;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            TagsToReviewResponse responseObject =
                JsonConvert.DeserializeObject<TagsToReviewResponse>(response.Body);

            return responseObject;
        }

        public RejectTagResponse RejectTag(
            User _user,
            Guid _tagId)
        {
            string url = string.Format("/api/review/tag/{0}/reject", _tagId);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            RejectTagResponse responseObject =
                JsonConvert.DeserializeObject<RejectTagResponse>(response.Body);

            return responseObject;
        }

        public ApproveTagResponse ApproveTag(
            User _user,
            Guid _tagId)
        {
            string url = string.Format("/api/review/tag/{0}/approve", _tagId);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            ApproveTagResponse responseObject =
                JsonConvert.DeserializeObject<ApproveTagResponse>(response.Body);

            return responseObject;
        }

        public MatchTagResponse MatchTag(
            User _user,
            Guid _tagId,
            Guid _hydrantId)
        {
            string url = string.Format("/api/review/tag/{0}/match/{1}", _tagId, _hydrantId);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Headers.Add("Username", _user.Username);
            request.Headers.Add("AuthorizationToken", _user.AuthorizationToken);

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            MatchTagResponse responseObject =
                JsonConvert.DeserializeObject<MatchTagResponse>(response.Body);

            return responseObject;
        }

        public CreateAccountResponse CreateAccount(
            string _username,
            string _email,
            string _password)
        {
            string url = string.Format("/api/user/create");

            CreateAccount newAccount = new CreateAccount
            {
                Username = _username,
                Email = _email,
                Password = _password
            };
            string json = JsonConvert.SerializeObject(newAccount);

            HWRestRequest request = new HWRestRequest();
            request.Method = HWRestMethods.Post;
            request.Host = m_HWManager.PlatformManager.ApiHost;
            request.Path = url;
            request.Timeout = 3000;
            request.Body = json;

            var response = m_HWManager.PlatformManager.SendRestRequest(request);
            CreateAccountResponse responseObject =
                JsonConvert.DeserializeObject<CreateAccountResponse>(response.Body);

            return responseObject;
        }

        public void Log(string prefix, string _message)
        {
            try
            {
                Guid installId = m_HWManager.SettingManager.GetInstallId();
                var config = AppConfiguration.GetInstance();

                LogglyMessage lm = new LogglyMessage
                {
                    from = installId.ToString(),
                    message = prefix + ": " + _message
                };
                string json = JsonConvert.SerializeObject(lm);


                HWRestRequest request = new HWRestRequest();
                request.Method = HWRestMethods.Post;
                request.Host = config.LogglyHost;
                request.Path = string.Format(config.LogglyResource, config.LogglyToken);
                request.Timeout = 3000;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Body = json;

                m_HWManager.PlatformManager.SendRestRequest(request);
            }
            catch
            {
                //Eat error
            }

        }
    }
}
