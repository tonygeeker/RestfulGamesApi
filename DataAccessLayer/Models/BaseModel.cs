using System.ComponentModel.DataAnnotations;

namespace RestfulGamesApi.DataAccessLayer.Models
{
    public class BaseModel
    {
        private bool _isEnabled = true;

        [Key]
        public int Id { get; set; }
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; } }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
