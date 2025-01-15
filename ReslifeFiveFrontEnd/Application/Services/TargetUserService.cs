using ReslifeFiveBackEnd.Model;
using ReslifeFiveBusinessLayer.Service;

namespace ReslifeFiveFrontEnd.Application.Services
{
    public class TargetUserService: ITargetUserService
    {
        public User? targetUser { get; set; }
        private readonly IGenService _genService;
        public TargetUserService(IGenService genService) 
        {
            _genService = genService;
        }

        public void SetTargetUser(int id)
        {
            targetUser = _genService.GetModel<User>().FirstOrDefault(x => x.Id == id);
        }
        public void SetTargetUser(User user)
        {
            targetUser = user;
        }

    }
}
