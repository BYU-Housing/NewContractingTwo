using ReslifeFiveBackEnd.Model; 

namespace ReslifeFiveFrontEnd.Application.Services
{
    public interface ITargetUserService
    {
        public User targetUser { get; set; }
        void SetTargetUser(int id);

        void SetTargetUser(User user);

    }
}
