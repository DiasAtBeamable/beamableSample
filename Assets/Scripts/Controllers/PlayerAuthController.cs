using System;
using System.Threading.Tasks;
using Beamable;
using Beamable.Api;
using Beamable.Common.Api.Auth;
using UnityEngine;

namespace Controllers
{
    public class PlayerAuthController : MonoBehaviour
    {
        /// <summary>
        /// Attempts to log in using a third-party authentication provider by recovering the account associated with the provided token.
        /// </summary>
        /// <param name="authThirdParty">The type of the third-party authentication provider (e.g., Google, Facebook).</param>
        /// <param name="token">The token associated with the third-party authentication provider.</param>
        /// <returns>Returns a boolean value indicating whether the login attempt was successful.</returns>
        public async Task<bool> AttemptToLoginUsingThirdParty(AuthThirdParty authThirdParty, string token)
        {
            // Ensure that BeamContext is initialized by awaiting it.
            var beamContext = await BeamContext.Default.Instance;
            try
            {
                // Let's try to recover the account using the token.
                var recoverOperation = await beamContext.Accounts.RecoverAccountWithThirdParty(authThirdParty, token);
                if (!recoverOperation.isSuccess)
                {
                    Debug.LogWarning("There is no account associated with this token.");
                    return false;
                }

                // If we got here, we have a valid account.
                await beamContext.Accounts.SwitchToAccount(recoverOperation.account);
            }
            catch (PlatformRequesterException exception)
            {
                Debug.LogError($"[AttemptToLoginUsingThirdParty] Error: {JsonUtility.ToJson(exception.Error)}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves the usage status of a third-party credential based on the provided authentication type and token.
        /// </summary>
        /// <param name="authThirdParty">The type of the third-party authentication provider (e.g., Google, Facebook).</param>
        /// <param name="token">The token associated with the third-party authentication provider.</param>
        /// <returns>Returns the <see cref="CredentialUsageStatus"/> indicating the usage status of the provided credential.</returns>
        public async Task<CredentialUsageStatus> GetThirdPartyCredentialUsageStatus(AuthThirdParty authThirdParty,
            string token)
        {
            // Ensure that BeamContext is initialized by awaiting it.
            var beamContext = await BeamContext.Default.Instance;
            return await beamContext.Accounts.GetCredentialStatus(authThirdParty, token);
            // Depending on the CredentialUsageStatus, you can take appropriate action.
            // For example, you can show a message to the user indicating that the credential is already in use.
            // or continue with adding the third-party credential.
            // In some third party providers, the token is set as invalid after being used, so you may need to regenerate it.
        }

        /// <summary>
        /// Attempts to add a third-party authentication provider to the current user's account using the provided token.
        /// It's recommended to call the IsThirdPartyAvailable before AttemptToAddThirdParty to check if that
        /// third party account is available
        /// </summary>
        /// <param name="authThirdParty">The type of the third-party authentication provider (e.g., Google, Facebook).</param>
        /// <param name="token">The token associated with the third-party authentication provider.</param>
        /// <returns>Returns a boolean value indicating whether the third-party addition was successful.</returns>
        public async Task<bool> AttemptToAddThirdParty(AuthThirdParty authThirdParty, string token)
        {
            // Ensure that BeamContext is initialized by awaiting it.
            var beamContext = await BeamContext.Default.Instance;
            var currentAccount = beamContext.Accounts.Current;

            if (currentAccount.HasThirdParty(authThirdParty))
            {
                // This means that this account already has this auth third party.
                return false;
            }

            try
            {
                var registrationResult = await currentAccount.AddThirdParty(authThirdParty, token);
                if (registrationResult.isSuccess)
                    return true;
                throw new Exception($"Could not add third party to current user. Error: {registrationResult.error}",
                    registrationResult.innerException);
            }
            catch (PlatformRequesterException exception)
            {
                Debug.LogError($"[AttemptToAddThirdParty] Error: {JsonUtility.ToJson(exception.Error)}");
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove a third-party authentication provider from the player's account if it is currently linked.
        /// </summary>
        /// <param name="authThirdParty">The type of the third-party authentication provider (e.g., Google, Facebook).</param>
        /// <param name="token">The token associated with the third-party authentication provider.</param>
        /// <returns>Returns a boolean value indicating whether the removal attempt was successful.</returns>
        public async Task<bool> AttemptToRemoveThirdParty(AuthThirdParty authThirdParty, string token)
        {
            // Ensure that BeamContext is initialized by awaiting it.
            var beamContext = await BeamContext.Default.Instance;
            var currentAccount = beamContext.Accounts.Current;
            if (currentAccount.HasThirdParty(authThirdParty))
            {
                // Remove only if the current account has this auth third party.
                await currentAccount.RemoveThirdParty(authThirdParty, token);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a new account within the Beamable platform.
        /// </summary>
        /// <returns>Returns a task that represents the asynchronous operation of creating a new account.</returns>
        public async Task CreateNewAccount()
        {
            var beamContext = await BeamContext.Default.Instance;
            await beamContext.Accounts.CreateNewAccount();
        }

        [ContextMenu("Create New Account")]
        private void CreateNewAccount_Debug()
        {
            _ = CreateNewAccount();
        }
    }
}
