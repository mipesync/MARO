using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Aggregate.Models.ResponseModels;

namespace MARO.Application.Repository.AuthRepo
{
    public interface IAuthRepository
    {
        Task<RegisterResponseModel> EmailRegister(string arg, string password, string UrlRaw, string returnUrl);
        Task<string[]> PhoneRegister(string arg, string password, string returnUrl);
        Task<ConfirmResponseModel> EmailConfirm(string userId, string code, string? returnUrl);
        Task<ConfirmResponseModel> PhoneConfirm(string userId, string code, string? returnUrl);
        Task EmailForgotPassword(string email, string UrlRaw);
        Task<string> PhoneForgotPassword(string phoneNumber);
        Task ResetPassword(ResetPasswordViewModel model);
        Task<string> LoginAsGuest();
    }
}
